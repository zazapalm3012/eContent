namespace eContentApp.Application.DTOs.Media
{
    public class MediaUploadResponseDto
    {
        public string? FileName { get; set; }
        public string? FilePath { get; set; }
        public string? FileType { get; set; }
        public long FileSize { get; set; }
    }
}
