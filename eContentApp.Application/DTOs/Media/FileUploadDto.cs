namespace eContentApp.Application.DTOs.Media
{
    public class FileUploadDto
    {
        public required byte[] Content { get; set; }
        public string? FileName { get; set; }
        public string? ContentType { get; set; }
    }
}
