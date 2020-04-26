using ColabSpace.Application.Common.Models;
using ColabSpace.Application.TaskItems.Commands.UpdateTaskItem;
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
    public class UpdateTaskItemWithAttachment : IClassFixture<CustomWebApplicationFactory<Startup>>
    {
        private readonly CustomWebApplicationFactory<Startup> _factory;

        public UpdateTaskItemWithAttachment(CustomWebApplicationFactory<Startup> factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task GivenValidRequestWithAttachfile_ShouldUpdateTaskWithAttachfile()
        {
            var client = await _factory.GetAuthenticatedClientAsync();
            // init DB for test
            var context = _factory.InitializeDbForTests();

            var listAttachFiles = new List<AttachFileModel>();

            // Mock Attachfile 1
            var formData1 = _factory.MockUploadFileAction("file1.txt", "data file 1");
            var uploadResponse1 = await client.PostAsync($"/api/Upload/", formData1);
            var fileAfterUpload1 = await IntegrationTestHelper.GetResponseContent<FileListDto>(uploadResponse1);
            listAttachFiles.Add(new AttachFileModel()
            {
                FileName = fileAfterUpload1.Files[0].FileName,
                FileStorageName = fileAfterUpload1.Files[0].FileStorageName,
                BlobStorageUrl = fileAfterUpload1.Files[0].BlobStorageUrl,
                FileSize = fileAfterUpload1.Files[0].FileSize,
                ThumbnailImage = fileAfterUpload1.Files[0].ThumbnailImage,
                LocalUrl = fileAfterUpload1.Files[0].LocalUrl
            });
            string taskId = "197d0438-e04b-453d-b5de-eca05960c6ae";
            UpdateTaskItemCommand command1 = new UpdateTaskItemCommand()
            {
                Name = "Task With AttachFiles",
                Description = "Description Test",
                TeamId = Guid.Parse(taskId),
                AttachFiles = listAttachFiles,
                Assignee = new UserModel() { UserId = Guid.NewGuid(), DisplayName = Guid.NewGuid().ToString() },
                Deadline = DateTime.UtcNow,
                Id = new Guid(taskId),
                Status = '2',
                Tags = new List<TagModel>(),
                Relations = new List<RelatedObjectModel>()
            };

            var content1 = IntegrationTestHelper.GetRequestContent(command1);

            var response1 = await client.PutAsync($"/api/TaskItems/Update/{taskId}", content1);

            response1.EnsureSuccessStatusCode();

            // Mock Attachfile 2
            var formData2 = _factory.MockUploadFileAction("file2.txt", "data file 2");
            var uploadResponse2 = await client.PostAsync($"/api/Upload/", formData2);
            var fileAfterUpload2 = await IntegrationTestHelper.GetResponseContent<FileListDto>(uploadResponse2);
            listAttachFiles = new List<AttachFileModel>();
            listAttachFiles.Add(new AttachFileModel()
            {
                FileName = fileAfterUpload2.Files[0].FileName,
                FileStorageName = fileAfterUpload2.Files[0].FileStorageName,
                BlobStorageUrl = fileAfterUpload2.Files[0].BlobStorageUrl,
                FileSize = fileAfterUpload2.Files[0].FileSize,
                ThumbnailImage = fileAfterUpload2.Files[0].ThumbnailImage,
                LocalUrl = fileAfterUpload2.Files[0].LocalUrl
            });

            UpdateTaskItemCommand command2 = new UpdateTaskItemCommand()
            {
                Name = "Task With AttachFiles",
                Description = "Description Test",
                TeamId = Guid.Parse(taskId),
                AttachFiles = listAttachFiles,
                Assignee = new UserModel() { UserId = Guid.NewGuid(), DisplayName = Guid.NewGuid().ToString() },
                Deadline = DateTime.UtcNow,
                Id = new Guid(taskId),
                Status = '2',
                Tags = new List<TagModel>(),
                Relations = new List<RelatedObjectModel>()
            };

            var content2 = IntegrationTestHelper.GetRequestContent(command2);

            var response2 = await client.PutAsync($"/api/TaskItems/Update/{taskId}", content2);

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

            string taskId = "197d0438-e04b-453d-b5de-eca05960c6ae";
            UpdateTaskItemCommand command = new UpdateTaskItemCommand()
            {
                Name = "Task With AttachFiles",
                Description = "Description Test",
                TeamId = Guid.Parse(taskId),
                AttachFiles = listAttachFiles,
                Assignee = new UserModel() { UserId = Guid.NewGuid(), DisplayName = Guid.NewGuid().ToString() },
                //CreatedBy = new UserModel() { UserId = Guid.NewGuid(), DisplayName = Guid.NewGuid().ToString() }, empty filed to Reproduce error
                Deadline = DateTime.UtcNow,
                Id = new Guid(taskId),
                Status = '2',
                Tags = new List<TagModel>(),
                Relations = new List<RelatedObjectModel>()
            };

            var content = IntegrationTestHelper.GetRequestContent(command);

            var response2 = await client.PutAsync($"/api/TaskItems/Update/{taskId}", content);

            response2.StatusCode.ShouldBe(System.Net.HttpStatusCode.NoContent);

            // release DB
            _factory.DisposeDbForTests(context);
        }
    }
}
