
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
        public async Task GetAllPostsAsync_ShouldReturnPostsSortedByPublishedAtDescending()
        {
            // Arrange
            var post1 = new Post { Id = Guid.NewGuid(), Title = "Post 1", PublishedAt = new DateTime(2023, 1, 1) };
            var post2 = new Post { Id = Guid.NewGuid(), Title = "Post 2", PublishedAt = new DateTime(2023, 1, 3) };
            var post3 = new Post { Id = Guid.NewGuid(), Title = "Post 3", PublishedAt = new DateTime(2023, 1, 2) };

            var posts = new List<Post> { post1, post2, post3 };

            // Expected sorted order (descending by PublishedAt)
            var expectedSortedPosts = new List<Post> { post2, post3, post1 };

            var postListDtos = new List<PostListDto>
            {
                new PostListDto { Id = post2.Id, Title = "Post 2", PublishedAt = post2.PublishedAt },
                new PostListDto { Id = post3.Id, Title = "Post 3", PublishedAt = post3.PublishedAt },
                new PostListDto { Id = post1.Id, Title = "Post 1", PublishedAt = post1.PublishedAt }
            };

            _mockPostRepository.Setup(repo => repo.GetAllPostsSimpleAsync()).ReturnsAsync(posts);
            // Mapper should be set up to return the DTOs in the *expected sorted order*
            _mockMapper.Setup(mapper => mapper.Map<IEnumerable<PostListDto>>(It.IsAny<IEnumerable<Post>>()))
                       .Returns((IEnumerable<Post> source) => 
                           source.OrderByDescending(p => p.PublishedAt)
                                 .Select(p => new PostListDto { Id = p.Id, Title = p.Title, PublishedAt = p.PublishedAt }));

            // Act
            var result = await _postService.GetAllPostsAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(3, result.Count());
            Assert.Equal(postListDtos.Select(p => p.Id), result.Select(p => p.Id));
            Assert.Equal(postListDtos.Select(p => p.PublishedAt), result.Select(p => p.PublishedAt));
        }

        [Fact]
        public async Task GetPostDetailAsync_ShouldReturnPostDetail_WhenPostExists()
        {
            // Arrange
            var postId = Guid.NewGuid();
            var post = new Post { Id = postId, Title = "Test Post" };
            var postDetailDto = new PostDetailDto { Id = postId, Title = "Test Post" };

            _mockPostRepository.Setup(repo => repo.GetPostByIdWithCategoriesAsync(postId)).ReturnsAsync(post);
            #pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
            _mockMapper.Setup(mapper => mapper.Map<PostDetailDto>((Post)post)).Returns(postDetailDto);
            #pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.

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
