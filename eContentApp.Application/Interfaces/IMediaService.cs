using eContentApp.Application.DTOs.Media;

namespace eContentApp.Application.Interfaces
{
    public interface IMediaService
    {
        Task<MediaUploadResponseDto> UploadMediaAsync(FileUploadDto fileDto, string uploadPath);
    }
}
