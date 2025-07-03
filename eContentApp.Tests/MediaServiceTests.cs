
using eContentApp.Application.DTOs.Media;
using eContentApp.Application.Interfaces;
using eContentApp.Application.Services;
using eContentApp.Domain.Entities;
using Moq;
using System;
using System.IO;
using System.Threading.Tasks;
using Xunit;

namespace eContentApp.Tests
{
    public class MediaServiceTests
    {
        private readonly Mock<IMediaRepository> _mockMediaRepository;
        private readonly MediaService _mediaService;

        public MediaServiceTests()
        {
            _mockMediaRepository = new Mock<IMediaRepository>();
            _mediaService = new MediaService(_mockMediaRepository.Object);
        }

        [Fact]
        public async Task UploadMediaAsync_ShouldUploadFileAndCreateMediaRecord()
        {
            // Arrange
            var fileUploadDto = new FileUploadDto
            {
                Content = new byte[] { 1, 2, 3 },
                FileName = "test.jpg",
                ContentType = "image/jpeg"
            };
            var uploadPath = Path.Combine(Path.GetTempPath(), "eContentApp_Tests");
            if (!Directory.Exists(uploadPath)) Directory.CreateDirectory(uploadPath);

            _mockMediaRepository.Setup(repo => repo.AddAsync(It.IsAny<Media>())).Returns(Task.FromResult(0));

            // Act
            var result = await _mediaService.UploadMediaAsync(fileUploadDto, uploadPath);

            // Assert
            Assert.NotNull(result);
            Assert.EndsWith(fileUploadDto.FileName, result.FileName);
            Assert.True(File.Exists(Path.Combine(uploadPath, result.FileName!)));

            // Cleanup
            Directory.Delete(uploadPath, true);
        }

        [Fact]
        public async Task UploadMediaAsync_ShouldThrowArgumentException_WhenFileIsEmptyOrNull()
        {
            // Arrange
            var fileUploadDto = new FileUploadDto { Content = Array.Empty<byte>() };
            var uploadPath = Path.Combine(Path.GetTempPath(), "eContentApp_Tests");

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => _mediaService.UploadMediaAsync(fileUploadDto, uploadPath));
        }
    }
}
