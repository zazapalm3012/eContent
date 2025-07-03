namespace eContentApp.Application.DTOs.Media
{
    public class FileUploadDto
    {
        public required byte[] Content { get; set; }
        public string FileName { get; set; } = string.Empty;
        public string ContentType { get; set; } = string.Empty;
    }
}
