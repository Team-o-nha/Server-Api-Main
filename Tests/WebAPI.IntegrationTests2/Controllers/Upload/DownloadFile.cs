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
    public class DownloadFile : IClassFixture<CustomWebApplicationFactory<Startup>>
    {
        private readonly CustomWebApplicationFactory<Startup> _factory;

        public DownloadFile(CustomWebApplicationFactory<Startup> factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task GivenExistFile_ReturnsOkCode()
        {
            var client = await _factory.GetAuthenticatedClientAsync();

            // Mock file
            var formData = _factory.MockUploadFileAction("file1.txt","Test dataaaaaaaaaaaa");

            var response = await client.PostAsync($"/api/Upload/", formData);

            response.StatusCode.ShouldBe(HttpStatusCode.OK);

            var vm = await IntegrationTestHelper.GetResponseContent<FileListDto>(response);

            var blobUri = await _factory.GetService<IBlobStorageService>().UploadFileToBlobAsync(vm.Files[0].LocalUrl, vm.Files[0].FileStorageName);

            var downloadResponse = await client.GetAsync($"/api/Upload/DownloadFile/?fileUrl={blobUri}&name={"test.txt"}");

            downloadResponse.StatusCode.ShouldBe(HttpStatusCode.OK);
            await _factory.GetService<IBlobStorageService>().DeleteBlobData(blobUri);
        }

        [Fact]
        public async Task GivenLocalFile_ReturnsOkCode()
        {
            var client = await _factory.GetAuthenticatedClientAsync();

            //Setup mock file using a memory stream
            var formData = _factory.MockUploadFileAction("file1.txt", "Test dataaaaaaaaaaaa");

            var response = await client.PostAsync($"/api/Upload/", formData);

            response.StatusCode.ShouldBe(HttpStatusCode.OK);

            var vm = await IntegrationTestHelper.GetResponseContent<FileListDto>(response);

            var downloadResponse = await client.GetAsync($"/api/Upload/DownloadFile/?fileUrl={vm.Files[0].LocalUrl}&name={"test.txt"}");

            downloadResponse.StatusCode.ShouldBe(HttpStatusCode.OK);
        }

        [Fact]
        public async Task GivenNotExistFile_ReturnBadRequestCode()
        {
            var client = await _factory.GetAuthenticatedClientAsync();

            var blobUri = "https://colabspacepocfrontend.blob.core.windows.net/blob-container/file-invalid.xml";

            var response = await client.GetAsync($"/api/Upload/DownloadFile/?fileUrl={blobUri}&name={"abc"}");

            response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
        }
    }
}
