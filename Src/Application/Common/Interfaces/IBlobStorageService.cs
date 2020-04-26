using Microsoft.WindowsAzure.Storage.Blob;
using System.Threading.Tasks;

namespace ColabSpace.Application.Common.Interfaces
{
    public interface IBlobStorageService
    {
        Task<string> UploadFileToBlobAsync(string filePath, string fileName);

        Task DeleteBlobData(string fileUrl);

        CloudBlobContainer BlobContainer { get; }
    }
}
