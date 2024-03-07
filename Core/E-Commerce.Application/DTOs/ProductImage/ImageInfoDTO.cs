namespace E_Commerce.Application.DTOs.ProductImage
{
    public class ImageInfoDTO
    {
        public required string FileName { get; set; }
        public required string Path { get; set; }
        public string StorageServiceName { get; set; }
    }
}
