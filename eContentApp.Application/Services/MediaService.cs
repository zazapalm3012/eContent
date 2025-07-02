using eContentApp.Application.DTOs.Media;
using eContentApp.Application.Interfaces;
using eContentApp.Domain.Entities;
using System;
using System.IO;
using System.Threading.Tasks;

namespace eContentApp.Application.Services
{
    public class MediaService : IMediaService
    {
        private readonly IMediaRepository _mediaRepository;

        public MediaService(IMediaRepository mediaRepository)
        {
            _mediaRepository = mediaRepository;
        }

        public async Task<MediaUploadResponseDto> UploadMediaAsync(FileUploadDto fileDto, string uploadPath)
        {
            if (fileDto == null || fileDto.Content == null || fileDto.Content.Length == 0)
            {
                Console.WriteLine("MediaService: File is empty or null.");
                throw new ArgumentException("File is empty or null.");
            }

            if (!Directory.Exists(uploadPath))
            {
                Console.WriteLine($"MediaService: Creating directory: {uploadPath}");
                Directory.CreateDirectory(uploadPath);
            }

            var uniqueFileName = Guid.NewGuid().ToString() + "_" + (fileDto.FileName ?? "");
            var filePath = Path.Combine(uploadPath, uniqueFileName);
            Console.WriteLine($"MediaService: Saving file to: {filePath}");

            await File.WriteAllBytesAsync(filePath, fileDto.Content);
            Console.WriteLine("MediaService: File saved to disk.");

            var media = new Media
            {
                FileName = uniqueFileName,
                FilePath = "http://localhost:5026/images/" + uniqueFileName, // Absolute path for web access
                FileType = fileDto.ContentType ?? string.Empty,
                FileSize = fileDto.Content!.Length,
                UploadedAt = DateTime.UtcNow
            };

            Console.WriteLine("MediaService: Adding media record to database.");
            await _mediaRepository.AddAsync(media);
            Console.WriteLine("MediaService: Media record added to database.");

            return new MediaUploadResponseDto
            {
                FileName = media.FileName,
                FilePath = media.FilePath,
                FileType = media.FileType,
                FileSize = media.FileSize
            };
        }
    }
}
