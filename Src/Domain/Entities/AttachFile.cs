namespace ColabSpace.Domain.Entities
{
    public class AttachFile
    {
        public string FileName { get; set; }

        public string FileStorageName { get; set; }

        public long FileSize { get; set; }

        public string BlobStorageUrl { get; set; }

        public string ThumbnailImage { get; set; }

        public bool? IsPin { get; set; }
    }
}
