using ColabSpace.Application.Common.Models;
using ColabSpace.Application.TaskItems.Commands.CreateTaskItem;
using ColabSpace.Application.TaskItems.Models;
using ColabSpace.Application.Teams.Models;
using ColabSpace.WebAPI.IntegrationTests2.Common;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace ColabSpace.WebAPI.IntegrationTests2.Controllers.TaskItems
{
    public class CreateTaskItemWithAttachments : IClassFixture<CustomWebApplicationFactory<Startup>>
    {
        private readonly CustomWebApplicationFactory<Startup> _factory;

        public CreateTaskItemWithAttachments(CustomWebApplicationFactory<Startup> factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task GivenValidRequestWithAttachfile_ShouldCreateTaskWithAttachfile()
        {
            var client = await _factory.GetAuthenticatedClientAsync();
            // init DB for test
            var context = _factory.InitializeDbForTests();

            var listAttachFiles = new List<AttachFileModel>();

            // Mock Attachfile 1
            var formData = _factory.MockUploadFileAction("file1.txt", "data file 1");
            var response1 = await client.PostAsync($"/api/Upload/", formData);
            var fileAfterUpload = await IntegrationTestHelper.GetResponseContent<FileListDto>(response1);
            listAttachFiles.Add(new AttachFileModel()
            {
                FileName = fileAfterUpload.Files[0].FileName,
                FileStorageName = fileAfterUpload.Files[0].FileStorageName,
                BlobStorageUrl = fileAfterUpload.Files[0].BlobStorageUrl,
                FileSize = fileAfterUpload.Files[0].FileSize,
                ThumbnailImage = fileAfterUpload.Files[0].ThumbnailImage,
                LocalUrl = fileAfterUpload.Files[0].LocalUrl
            });

            CreateTaskItemCommand command = new CreateTaskItemCommand()
            {
                Name = "Task With AttachFiles",
                Description = "Description Test",
                TeamId = Guid.Parse("197d0438-e04b-453d-b5de-eca05960c6ae"),
                AttachFiles = listAttachFiles,
                Assignee = new UserModel() { UserId = Guid.NewGuid(), DisplayName = Guid.NewGuid().ToString()},
                CreatedBy = new UserModel() { UserId = Guid.NewGuid(), DisplayName = Guid.NewGuid().ToString()},
            };

            var content = IntegrationTestHelper.GetRequestContent(command);

            var response2 = await client.PostAsync($"/api/TaskItems/", content);

            response2.EnsureSuccessStatusCode();

            // release DB
            _factory.DisposeDbForTests(context);
        }

        [Fact]
        public async Task GivenValidRequestWithAttachfile_OnCreateException_ShouldRaiseBadRequest_and_DeletefileOnBlobStorage()
        {
            var client = await _factory.GetAuthenticatedClientAsync();
            // init DB for test
            var context = _factory.InitializeDbForTests();

            var listAttachFiles = new List<AttachFileModel>();

            // Mock Attachfile 1
            var formData = _factory.MockUploadFileAction("file1.txt", "data file 1");
            var response1 = await client.PostAsync($"/api/Upload/", formData);
            var fileAfterUpload = await IntegrationTestHelper.GetResponseContent<FileListDto>(response1);
            listAttachFiles.Add(new AttachFileModel()
            {
                FileName = fileAfterUpload.Files[0].FileName,
                FileStorageName = fileAfterUpload.Files[0].FileStorageName,
                BlobStorageUrl = fileAfterUpload.Files[0].BlobStorageUrl,
                FileSize = fileAfterUpload.Files[0].FileSize,
                ThumbnailImage = fileAfterUpload.Files[0].ThumbnailImage,
                LocalUrl = fileAfterUpload.Files[0].LocalUrl
            });

            CreateTaskItemCommand command = new CreateTaskItemCommand()
            {
                Name = "Task With AttachFiles",
                Description = "Description Test",
                TeamId = Guid.Parse("197d0438-e04b-453d-b5de-eca05960c6ae"),
                AttachFiles = listAttachFiles,
                Assignee = new UserModel() { UserId = Guid.NewGuid(), DisplayName = Guid.NewGuid().ToString() },
                //CreatedBy = new UserModel() { UserId = Guid.NewGuid(), DisplayName = Guid.NewGuid().ToString() }, empty filed to Reproduce error
            };

            var content = IntegrationTestHelper.GetRequestContent(command);

            var response2 = await client.PostAsync($"/api/TaskItems/", content);

            response2.StatusCode.ShouldBe(System.Net.HttpStatusCode.BadRequest);

            // release DB
            _factory.DisposeDbForTests(context);
        }
    }
}
