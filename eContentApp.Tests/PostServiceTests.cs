
using AutoMapper;
using eContentApp.Application.DTOs.Post;
using eContentApp.Application.Interfaces;
using eContentApp.Application.Services;
using eContentApp.Domain.Entities;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace eContentApp.Tests
{
    public class PostServiceTests
    {
        private readonly Mock<IPostRepository> _mockPostRepository;
        private readonly Mock<ICategoryRepository> _mockCategoryRepository;
        private readonly Mock<IMapper> _mockMapper;
        private readonly PostService _postService;

        public PostServiceTests()
        {
            _mockPostRepository = new Mock<IPostRepository>();
            _mockCategoryRepository = new Mock<ICategoryRepository>();
            _mockMapper = new Mock<IMapper>();
            _postService = new PostService(_mockPostRepository.Object, _mockCategoryRepository.Object, _mockMapper.Object);
        }

        [Fact]
        public async Task GetAllPostsAsync_ShouldReturnAllPosts()
        {
            // Arrange
            var posts = new List<Post>
            {
                new Post { Id = Guid.NewGuid(), Title = "Post 1" },
                new Post { Id = Guid.NewGuid(), Title = "Post 2" }
            };
            var postListDtos = new List<PostListDto>
            {
                new PostListDto { Id = posts[0].Id, Title = "Post 1" },
                new PostListDto { Id = posts[1].Id, Title = "Post 2" }
            };

            _mockPostRepository.Setup(repo => repo.GetAllPostsSimpleAsync()).ReturnsAsync(posts);
            _mockMapper.Setup(mapper => mapper.Map<IEnumerable<PostListDto>>(posts)).Returns(postListDtos);

            // Act
            var result = await _postService.GetAllPostsAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
        }

        [Fact]
        public async Task GetPostDetailAsync_ShouldReturnPostDetail_WhenPostExists()
        {
            // Arrange
            var postId = Guid.NewGuid();
            var post = new Post { Id = postId, Title = "Test Post" };
            var postDetailDto = new PostDetailDto { Id = postId, Title = "Test Post" };

            _mockPostRepository.Setup(repo => repo.GetPostByIdWithCategoriesAsync(postId)).ReturnsAsync(post);
            _mockMapper.Setup(mapper => mapper.Map<PostDetailDto>(post)).Returns(postDetailDto);

            // Act
            var result = await _postService.GetPostDetailAsync(postId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(postId, result.Id);
        }

        [Fact]
        public async Task GetPostDetailAsync_ShouldReturnNull_WhenPostDoesNotExist()
        {
            // Arrange
            var postId = Guid.NewGuid();
            _mockPostRepository.Setup(repo => repo.GetPostByIdWithCategoriesAsync(postId)).ReturnsAsync((Post)null);

            // Act
            var result = await _postService.GetPostDetailAsync(postId);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task CreatePostAsync_ShouldReturnNewGuid()
        {
            // Arrange
            var createPostDto = new CreatePostDto { Title = "New Post" };
            var post = new Post { Id = Guid.NewGuid(), Title = "New Post" };

            _mockMapper.Setup(mapper => mapper.Map<Post>(createPostDto)).Returns(post);
            _mockPostRepository.Setup(repo => repo.AddAsync(post)).Returns(Task.FromResult(0));

            // Act
            var result = await _postService.CreatePostAsync(createPostDto);

            // Assert
            Assert.NotEqual(Guid.Empty, result);
        }

        [Fact]
        public async Task UpdatePostAsync_ShouldUpdatePost()
        {
            // Arrange
            var postId = Guid.NewGuid();
            var updatePostDto = new UpdatePostDto { Id = postId, Title = "Updated Post" };
            var existingPost = new Post { Id = postId, Title = "Old Post" };

            _mockPostRepository.Setup(repo => repo.GetPostByIdWithCategoriesAsync(postId)).ReturnsAsync(existingPost);
            _mockMapper.Setup(mapper => mapper.Map(updatePostDto, existingPost)).Returns(existingPost);
            _mockPostRepository.Setup(repo => repo.UpdateAsync(existingPost)).Returns(Task.FromResult(0));

            // Act
            await _postService.UpdatePostAsync(updatePostDto);

            // Assert
            _mockPostRepository.Verify(repo => repo.UpdateAsync(existingPost), Times.Once);
        }

        [Fact]
        public async Task DeletePostAsync_ShouldDeletePost()
        {
            // Arrange
            var postId = Guid.NewGuid();
            _mockPostRepository.Setup(repo => repo.DeleteAsync(postId)).Returns(Task.FromResult(0));

            // Act
            await _postService.DeletePostAsync(postId);

            // Assert
            _mockPostRepository.Verify(repo => repo.DeleteAsync(postId), Times.Once);
        }
    }
}
