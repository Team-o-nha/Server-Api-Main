using ColabSpace.Application.Common.Models;
using ColabSpace.WebAPI.IntegrationTests2.Common;
using Microsoft.AspNetCore.Http;
using Moq;
using Shouldly;
using Spire.Doc;
using Spire.Pdf;
using Spire.Pdf.Graphics;
using Spire.Presentation;
using Spire.Xls;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace ColabSpace.WebAPI.IntegrationTests2.Controllers.Upload
{
    public class UploadFile : IClassFixture<CustomWebApplicationFactory<Startup>>
    {
        private readonly CustomWebApplicationFactory<Startup> _factory;

        public UploadFile(CustomWebApplicationFactory<Startup> factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task GivenEmptyFileTxt_ReturnBadRequest()
        {
            var client = await _factory.GetAuthenticatedClientAsync();
            var fileMock = new Mock<IFormFile>();
            //Setup mock file using a memory stream
            var data = "";
            var fileName = "test.txt";
            var ms = new MemoryStream();
            var writer = new StreamWriter(ms);
            writer.Write(data);
            writer.Flush();
            ms.Position = 0;

            fileMock.Setup(_ => _.OpenReadStream()).Returns(ms);
            fileMock.Setup(_ => _.FileName).Returns(fileName);
            fileMock.Setup(_ => _.Length).Returns(ms.Length);
            var file = fileMock.Object;
            var formData = new MultipartFormDataContent();
            // Add file (file, field name, file name)
            formData.Add(new StreamContent(ms), "files", fileName);

            var response = await client.PostAsync($"/api/Upload/", formData);

            response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task GivenMaxFileSize_ReturnBadRequest()
        {
            var client = await _factory.GetAuthenticatedClientAsync();
            var fileMock = new Mock<IFormFile>();
            //Setup mock file using a memory stream
            var data = "0123456789";
            var fileName = "test.txt";
            var ms = new MemoryStream();
            var writer = new StreamWriter(ms);
            for (int i = 0; i < 204800; i++)
            {
                writer.Write(new byte[1024]);
            }
            writer.Flush();
            ms.Position = 0;

            fileMock.Setup(_ => _.OpenReadStream()).Returns(ms);
            fileMock.Setup(_ => _.FileName).Returns(fileName);
            fileMock.Setup(_ => _.Length).Returns(ms.Length);
            var file = fileMock.Object;
            var formData = new MultipartFormDataContent();
            // Add file (file, field name, file name)
            formData.Add(new StreamContent(ms), "files", fileName);

            var response = await client.PostAsync($"/api/Upload/", formData);

            response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task GivenFileTxt_UploadSuccess()
        {
            var client = await _factory.GetAuthenticatedClientAsync();
            //Setup mock file using a memory stream
            var data = "Hello World from a Fake File";
            var fileName = "test.txt";
            // Mock file
            var formData = _factory.MockUploadFileAction(fileName, data);

            var response = await client.PostAsync($"/api/Upload/", formData);

            response.StatusCode.ShouldBe(HttpStatusCode.OK);

            var vm = await IntegrationTestHelper.GetResponseContent<FileListDto>(response);

            vm.Count.ShouldBe(1);
            vm.Files[0].FileName.ShouldBe(fileName);
        }

        [Fact]
        public async Task GivenFileDocx_UploadSuccess()
        {
            var client = await _factory.GetAuthenticatedClientAsync();
            var fileMock = new Mock<IFormFile>();
            //Setup mock file using a memory stream
            var data = "Hello World from a Fake File";
            var fileName = "test.docx";
            var ms = new MemoryStream();
            var writer = new StreamWriter(ms);
            writer.Write(data);
            writer.Flush();
            ms.Position = 0;

            //Load Document
            Document document = new Document();
            document.LoadFromStream(ms, Spire.Doc.FileFormat.Txt);
            MemoryStream stream = new MemoryStream();
            document.SaveToStream(stream, Spire.Doc.FileFormat.Docx);

            fileMock.Setup(_ => _.OpenReadStream()).Returns(stream);
            fileMock.Setup(_ => _.FileName).Returns(fileName);
            fileMock.Setup(_ => _.Length).Returns(stream.Length);
            var file = fileMock.Object;
            var formData = new MultipartFormDataContent();
            // Add file (file, field name, file name)
            formData.Add(new StreamContent(stream), "files", fileName);

            var response = await client.PostAsync($"/api/Upload/", formData);

            response.StatusCode.ShouldBe(HttpStatusCode.OK);

            var vm = await IntegrationTestHelper.GetResponseContent<FileListDto>(response);

            vm.Count.ShouldBe(1);
            vm.Files[0].FileName.ShouldBe(fileName);
        }

        [Fact]
        public async Task GivenFileXlsx_UploadSuccess()
        {
            var client = await _factory.GetAuthenticatedClientAsync();
            var fileMock = new Mock<IFormFile>();
            var fileName = "test.xlsx";
            //Load Workbook
            Workbook workbook = new Workbook();
            workbook.CreateEmptySheet();
            MemoryStream stream = new MemoryStream();
            workbook.SaveToStream(stream);

            fileMock.Setup(_ => _.OpenReadStream()).Returns(stream);
            fileMock.Setup(_ => _.FileName).Returns(fileName);
            fileMock.Setup(_ => _.Length).Returns(stream.Length);
            var file = fileMock.Object;
            var formData = new MultipartFormDataContent();
            // Add file (file, field name, file name)
            formData.Add(new StreamContent(stream), "files", fileName);

            var response = await client.PostAsync($"/api/Upload/", formData);

            response.StatusCode.ShouldBe(HttpStatusCode.OK);

            var vm = await IntegrationTestHelper.GetResponseContent<FileListDto>(response);

            vm.Count.ShouldBe(1);
            vm.Files[0].FileName.ShouldBe(fileName);
        }

        [Fact]
        public async Task GivenFilePng_UploadSuccess()
        {
            var client = await _factory.GetAuthenticatedClientAsync();
            var fileMock = new Mock<IFormFile>();
            var fileName = "test.png";

            var memoryStream = new MemoryStream();
            Image image = new Bitmap(1, 1);
            image.Save(memoryStream, ImageFormat.Png);

            fileMock.Setup(_ => _.OpenReadStream()).Returns(memoryStream);
            fileMock.Setup(_ => _.FileName).Returns(fileName);
            fileMock.Setup(_ => _.Length).Returns(memoryStream.Length);
            var formData = new MultipartFormDataContent();
            // Add file (file, field name, file name)
            formData.Add(new StreamContent(memoryStream), "files", fileName);

            var response = await client.PostAsync($"/api/Upload/", formData);

            response.StatusCode.ShouldBe(HttpStatusCode.OK);

            var vm = await IntegrationTestHelper.GetResponseContent<FileListDto>(response);

            vm.Count.ShouldBe(1);
            vm.Files[0].FileName.ShouldBe(fileName);
        }

        [Fact]
        public async Task GivenFilePptx_UploadSuccess()
        {
            var client = await _factory.GetAuthenticatedClientAsync();
            var fileMock = new Mock<IFormFile>();
            var fileName = "test.pptx";

            var memoryStream = new MemoryStream();
            Presentation presentation = new Presentation();
            presentation.Slides.Append();
            presentation.SaveToFile(memoryStream, Spire.Presentation.FileFormat.Pptx2013);

            fileMock.Setup(_ => _.OpenReadStream()).Returns(memoryStream);
            fileMock.Setup(_ => _.FileName).Returns(fileName);
            fileMock.Setup(_ => _.Length).Returns(memoryStream.Length);
            var formData = new MultipartFormDataContent();
            // Add file (file, field name, file name)
            formData.Add(new StreamContent(memoryStream), "files", fileName);

            var response = await client.PostAsync($"/api/Upload/", formData);

            response.StatusCode.ShouldBe(HttpStatusCode.OK);

            var vm = await IntegrationTestHelper.GetResponseContent<FileListDto>(response);

            vm.Count.ShouldBe(1);
            vm.Files[0].FileName.ShouldBe(fileName);
        }
    }
}
