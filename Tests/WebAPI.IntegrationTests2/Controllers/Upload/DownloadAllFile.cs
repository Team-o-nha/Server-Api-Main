using ColabSpace.Application.Common.Interfaces;
using ColabSpace.Application.Common.Models;
using ColabSpace.WebAPI.IntegrationTests2.Common;
using Microsoft.AspNetCore.Http;
using Moq;
using Shouldly;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace ColabSpace.WebAPI.IntegrationTests2.Controllers.Upload
{
    public class DownloadAllFile : IClassFixture<CustomWebApplicationFactory<Startup>>
    {
        private readonly CustomWebApplicationFactory<Startup> _factory;

        public DownloadAllFile(CustomWebApplicationFactory<Startup> factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task GivenExistFile_ReturnsOkCode()
        {
            var client = await _factory.GetAuthenticatedClientAsync();

            //Setup mock file using a memory stream
            var data = "Hello World from a Fake File";
            var fileName = "test.txt";
            var formData = _factory.MockUploadFileAction(fileName, data);

            var response = await client.PostAsync($"/api/Upload/", formData);

            response.StatusCode.ShouldBe(HttpStatusCode.OK);

            var vm = await IntegrationTestHelper.GetResponseContent<FileListDto>(response);

            var blobUri = await _factory.GetService<IBlobStorageService>().UploadFileToBlobAsync(vm.Files[0].LocalUrl,vm.Files[0].FileStorageName);

            var downloadResponse = await client.GetAsync($"/api/Upload/DownloadAllFile/?fileUrls={blobUri}&names={"test.txt"}");

            downloadResponse.StatusCode.ShouldBe(HttpStatusCode.OK);
            await _factory.GetService<IBlobStorageService>().DeleteBlobData(blobUri);
        }

        [Fact]
        public async Task GivenLocalFile_ReturnsOkCode()
        {
            var client = await _factory.GetAuthenticatedClientAsync();

            //Setup mock file using a memory stream
            var data = "Hello World from a Fake File";
            var fileName = "test.txt";
            var formData = _factory.MockUploadFileAction(fileName, data);

            var response = await client.PostAsync($"/api/Upload/", formData);

            response.StatusCode.ShouldBe(HttpStatusCode.OK);

            var vm = await IntegrationTestHelper.GetResponseContent<FileListDto>(response);

            var downloadResponse = await client.GetAsync($"/api/Upload/DownloadAllFile/?fileUrls={vm.Files[0].LocalUrl}&names={"test.txt"}");

            downloadResponse.StatusCode.ShouldBe(HttpStatusCode.OK);
        }

        [Fact]
        public async Task GivenDupplicateFile_ReturnsOkCode()
        {
            var client = await _factory.GetAuthenticatedClientAsync();

            //Setup mock file using a memory stream
            var data = "Hello World from a Fake File";
            var fileName = "test.txt";
            var formData = _factory.MockUploadFileAction(fileName, data);

            var response = await client.PostAsync($"/api/Upload/", formData);

            response.StatusCode.ShouldBe(HttpStatusCode.OK);

            var vm = await IntegrationTestHelper.GetResponseContent<FileListDto>(response);

            var blobUri = await _factory.GetService<IBlobStorageService>().UploadFileToBlobAsync(vm.Files[0].LocalUrl, vm.Files[0].FileStorageName);

            var downloadResponse = await client.GetAsync($"/api/Upload/DownloadAllFile/?fileUrls={blobUri}&fileUrls={blobUri}&names={"test.txt"}&names={"test.txt"}");

            downloadResponse.StatusCode.ShouldBe(HttpStatusCode.OK);
            await _factory.GetService<IBlobStorageService>().DeleteBlobData(blobUri);
        }
    }
}
