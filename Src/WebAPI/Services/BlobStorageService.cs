using ColabSpace.Application.Common.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.IO;
using System.Threading.Tasks;

namespace ColabSpace.WebAPI.Services
{
    public class BlobStorageService : IBlobStorageService
    {
        public CloudBlobContainer BlobContainer { get; }

        public BlobStorageService(IConfiguration config)
        {
            // Retrieve storage account from connection string.
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(config["BlobStorage:ConnectionString"]);
            // Create the blob client.
            var blobClient = storageAccount.CreateCloudBlobClient();
            // Retrieve reference to a previously created container.
            BlobContainer = blobClient.GetContainerReference(config["BlobStorage:ContainerName"]);
            BlobContainer.CreateIfNotExistsAsync();
        }

        public async Task<string> UploadFileToBlobAsync(string filePath, string fileName)
        {
            CloudBlockBlob blockBlob = BlobContainer.GetBlockBlobReference(fileName);

            await blockBlob.UploadFromFileAsync(filePath);

            return blockBlob.Uri.AbsoluteUri;
        }

        public async Task DeleteBlobData(string fileUrl)
        {
            Uri uri = new Uri(fileUrl);
            string filename = Path.GetFileName(uri.LocalPath);

            var blob = BlobContainer.GetBlockBlobReference(filename);
            await blob.DeleteIfExistsAsync();
        }
    }
}
