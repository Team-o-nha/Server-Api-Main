using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using ColabSpace.Application.Common.Interfaces;
using ColabSpace.Application.Common.Models;
using ColabSpace.WebAPI.Converter;
using ColabSpace.WebAPI.Utilities;
using ICSharpCode.SharpZipLib.Zip;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Configuration;
using Microsoft.Net.Http.Headers;
using Microsoft.WindowsAzure.Storage.Blob;

namespace ColabSpace.WebAPI.Controllers
{
    [Authorize]
    public class UploadController : ApiController
    {
        private readonly long _fileSizeLimit;
        private readonly string _targetFilePath;
        private readonly IBlobStorageService _blobStorageService;

        // Get the default form options so that we can use them to set the default 
        // limits for request body data.
        private static readonly FormOptions _defaultFormOptions = new FormOptions();

        public UploadController(IConfiguration config, IBlobStorageService blobStorageService)
        {
            _fileSizeLimit = config.GetValue<long>("FileSizeLimit");
            _blobStorageService = blobStorageService;

            // To save physical files to a path provided by configuration:
            _targetFilePath = config.GetValue<string>("UploadFolder");

            if (!Directory.Exists(_targetFilePath))
            {
                Directory.CreateDirectory(_targetFilePath);
            }
        }

        [HttpPost]
        public async Task<IActionResult> UploadPhysical()
        {
            if (!MultipartRequestHelper.IsMultipartContentType(Request.ContentType))
            {
                ModelState.AddModelError("File", "The request couldn't be processed (Error 1).");

                return BadRequest(ModelState);
            }

            var boundary = MultipartRequestHelper.GetBoundary(
                MediaTypeHeaderValue.Parse(Request.ContentType),
                _defaultFormOptions.MultipartBoundaryLengthLimit);
            var reader = new MultipartReader(boundary, HttpContext.Request.Body);
            var section = await reader.ReadNextSectionAsync();

            var filesDto = new FileListDto
            {
                TargetFilePath = _targetFilePath
            };

            while (section != null)
            {
                var hasContentDispositionHeader =
                    ContentDispositionHeaderValue.TryParse(
                        section.ContentDisposition, out var contentDisposition);

                if (hasContentDispositionHeader)
                {
                    // This check assumes that there's a file
                    // present without form data. If form data
                    // is present, this method immediately fails
                    // and returns the model error.
                    if (!MultipartRequestHelper.HasFileContentDisposition(contentDisposition))
                    {
                        ModelState.AddModelError("File", "The request couldn't be processed (Error 2).");
                        // Log error

                        return BadRequest(ModelState);
                    }
                    else
                    {
                        // Don't trust the file name sent by the client. To display
                        // the file name, HTML-encode the value.
                        var trustedFileNameForDisplay = WebUtility.HtmlEncode(
                                contentDisposition.FileName.Value);
                        var trustedFileNameForFileStorage = Path.GetRandomFileName()
                            + Path.GetExtension(trustedFileNameForDisplay).ToLower();

                        var streamedFileContent = await FileHelpers.ProcessStreamedFile(section, ModelState, _fileSizeLimit);

                        if (!ModelState.IsValid)
                        {
                            return BadRequest(ModelState);
                        }

                        using (var targetStream = System.IO.File.Create(
                            Path.Combine(_targetFilePath, trustedFileNameForFileStorage)))
                        {
                            await targetStream.WriteAsync(streamedFileContent);

                            var file = new FileDto
                            {
                                FileSize = streamedFileContent.Length,
                                FileName = trustedFileNameForDisplay,
                                FileStorageName = trustedFileNameForFileStorage,
                                LocalUrl = Path.Combine(_targetFilePath, trustedFileNameForFileStorage)
                            };
                            file.ThumbnailImage = ConverThumbnail(file.LocalUrl, targetStream);

                            filesDto.Files.Add(file);
                            filesDto.Size += streamedFileContent.Length;
                        }
                    }
                }

                // Drain any remaining section body that hasn't been consumed and
                // read the headers for the next section.
                section = await reader.ReadNextSectionAsync();
            }

            filesDto.Count = filesDto.Files.Count;
            return Ok(filesDto);
        }

        [HttpGet("DownloadFile")]
        public async Task<IActionResult> DownloadFile([FromQuery] string fileUrl, [FromQuery] string name)
        {
            MemoryStream ms = new MemoryStream();
            Uri uri = new Uri(fileUrl);
            string filename = Path.GetFileName(uri.LocalPath);
            CloudBlob file = _blobStorageService.BlobContainer.GetBlobReference(filename);

            if (await file.ExistsAsync())
            {
                await file.DownloadToStreamAsync(ms);
                Stream blobStream = file.OpenReadAsync().Result;
                return File(blobStream, file.Properties.ContentType ?? "application/octet-stream", name);
            }
            else
            {
                if (System.IO.File.Exists(fileUrl))
                {
                    FileStream fs = System.IO.File.OpenRead(fileUrl);
                    return File(fs, "application/octet-stream", name);
                }
                else
                {
                    return BadRequest();
                }
            }
        }

        [HttpGet("DownloadAllFile")]
        public async Task<IActionResult> DownloadAllFile([FromQuery] List<string> fileUrls, [FromQuery] List<string> names)
        {
            MemoryStream ms = new MemoryStream();
            MemoryStream zipStream = new MemoryStream();
            ZipOutputStream zipOutputStream = new ZipOutputStream(zipStream);
            Dictionary<string, int> nameDupplicate = new Dictionary<string, int>();
            ZipStrings.UseUnicode = true;
            foreach (var fileUrl in fileUrls)
            {
                if (!string.IsNullOrWhiteSpace(fileUrl))
                {
                    zipOutputStream.SetLevel(0);
                    Uri uri = new Uri(fileUrl);
                    string filename = Path.GetFileName(uri.LocalPath);
                    // get file from blob
                    CloudBlob blob = _blobStorageService.BlobContainer.GetBlobReference(filename);
                    string downloadFileName = names[fileUrls.IndexOf(fileUrl)];
                    // rename duplicate file name
                    if (nameDupplicate.ContainsKey(downloadFileName))
                    {
                        // file name in zip file
                        var entry = new ZipEntry(downloadFileName.Insert(downloadFileName.LastIndexOf(".")
                            , " (" + nameDupplicate[downloadFileName] + ")"));
                        nameDupplicate[downloadFileName]++;
                        zipOutputStream.PutNextEntry(entry);
                    }
                    else
                    {
                        // file name in zip file
                        var entry = new ZipEntry(downloadFileName);
                        nameDupplicate.Add(downloadFileName, 2);
                        zipOutputStream.PutNextEntry(entry);
                    }
                    // save file to zip file
                    if (await blob.ExistsAsync())
                    {
                        // save file in blob storage
                        await blob.DownloadToStreamAsync(ms);
                        Stream blobStream = blob.OpenReadAsync().Result;
                        await blobStream.CopyToAsync(zipOutputStream);
                    }
                    else
                    {
                        if (System.IO.File.Exists(fileUrl))
                        {
                            // save file in local
                            FileStream fs = System.IO.File.OpenRead(fileUrl);
                            await fs.CopyToAsync(zipOutputStream);
                        }
                        else
                        {
                            return BadRequest();
                        }
                    }
                }
            }
            // False stops the Close also Closing the underlying stream.
            zipOutputStream.IsStreamOwner = false;
            // Must finish the ZipOutputStream before using outputMemStream.
            zipOutputStream.Close();
            zipStream.Position = 0;
            return File(zipStream.ToArray(), "application/octet-stream", "result.zip");
        }

        private string ConverThumbnail(string fileUrl, Stream inputFile)
        {
            var base64String = "";
            var pdfFileName = _targetFilePath + "/" + Path.GetRandomFileName();
            var ext = Path.GetExtension(fileUrl).ToLower();
            try
            {
                if (ext.Contains(".doc") || ext.Contains(".docx"))
                {
                    base64String = Convert.ToBase64String(SpireOfficeConverter.Word2Images(inputFile));
                }
                else if (ext.Contains(".xls") || ext.Contains(".xlsx"))
                {
                    base64String = Convert.ToBase64String(SpireOfficeConverter.Excel2Images(inputFile));
                }
                else if (ext.Contains(".ppt") || ext.Contains(".pptx"))
                {
                    base64String = Convert.ToBase64String(SpireOfficeConverter.PowerPoint2Images(inputFile, pdfFileName));
                }
                else if (ext.Contains(".txt") || ext.Contains(".xml"))
                {
                    base64String = Convert.ToBase64String(SpireOfficeConverter.Text2Images(inputFile, pdfFileName));
                }
                else if (ext.Contains(".gif") || ext.Contains(".jpg") || ext.Contains(".png") || ext.Contains(".jpeg")
                    || ext.Contains(".bmp") || ext.Contains(".tiff"))
                {
                    base64String = Convert.ToBase64String(SpireOfficeConverter.Image2Images(inputFile));
                }
                else if (ext.Contains(".pdf"))
                {
                    base64String = Convert.ToBase64String(SpireOfficeConverter.Pdf2Images(inputFile));
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Unrecognized Exception: " + e.Message);
            }

            return base64String;
        }
    }
}
