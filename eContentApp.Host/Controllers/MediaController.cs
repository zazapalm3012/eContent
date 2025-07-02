using eContentApp.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using eContentApp.Application.DTOs.Media;
using Microsoft.AspNetCore.Hosting;
using System.IO;

namespace eContentApp.Host.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MediaController : ControllerBase
    {
        private readonly IMediaService _mediaService;
        private readonly IWebHostEnvironment _hostEnvironment;

        public MediaController(IMediaService mediaService, IWebHostEnvironment hostEnvironment)
        {
            _mediaService = mediaService;
            _hostEnvironment = hostEnvironment;
        }

        [HttpPost("upload")]
        public async Task<IActionResult> UploadImage(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("No file uploaded.");
            }

            try
            {
                Console.WriteLine("MediaController: Received file upload request.");
                using var memoryStream = new MemoryStream();
                await file.CopyToAsync(memoryStream);
                var fileDto = new FileUploadDto
                {
                    Content = memoryStream.ToArray(),
                    FileName = file.FileName,
                    ContentType = file.ContentType
                };

                var uploadsFolder = Path.Combine(_hostEnvironment.WebRootPath, "images");
                Console.WriteLine($"MediaController: Uploads folder path: {uploadsFolder}");

                var result = await _mediaService.UploadMediaAsync(fileDto, uploadsFolder);
                Console.WriteLine($"MediaController: File uploaded successfully. URL: {result.FilePath}");
                return Ok(new { url = result.FilePath }); // Return the URL for Quill.js
            }
            catch (System.Exception ex)
            {
                Console.WriteLine($"MediaController: Error during file upload: {ex.Message}");
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
