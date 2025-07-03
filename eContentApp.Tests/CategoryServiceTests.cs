
using AutoMapper;
using eContentApp.Application.DTOs.Category;
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
    public class CategoryServiceTests
    {
        private readonly Mock<ICategoryRepository> _mockCategoryRepository;
        private readonly Mock<IMapper> _mockMapper;
        private readonly CategoryService _categoryService;

        public CategoryServiceTests()
        {
            _mockCategoryRepository = new Mock<ICategoryRepository>();
            _mockMapper = new Mock<IMapper>();
            _categoryService = new CategoryService(_mockCategoryRepository.Object, _mockMapper.Object);
        }

        [Fact]
        public async Task GetAllCategoriesAsync_ShouldReturnAllCategories()
        {
            // Arrange
            var categories = new List<Category>
            {
                new Category { Id = Guid.NewGuid(), Name = "Category 1" },
                new Category { Id = Guid.NewGuid(), Name = "Category 2" }
            };
            var categoryDtos = new List<CategoryDto>
            {
                new CategoryDto { Id = categories[0].Id, Name = "Category 1" },
                new CategoryDto { Id = categories[1].Id, Name = "Category 2" }
            };

            _mockCategoryRepository.Setup(repo => repo.GetAllAsync()).ReturnsAsync(categories);
            _mockMapper.Setup(mapper => mapper.Map<IEnumerable<CategoryDto>>(categories)).Returns(categoryDtos);

            // Act
            var result = await _categoryService.GetAllCategoriesAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
        }

        [Fact]
        public async Task GetCategoryByIdAsync_ShouldReturnCategory_WhenCategoryExists()
        {
            // Arrange
            var categoryId = Guid.NewGuid();
            var category = new Category { Id = categoryId, Name = "Test Category" };
            var categoryDto = new CategoryDto { Id = categoryId, Name = "Test Category" };

            _mockCategoryRepository.Setup(repo => repo.GetByIdAsync(categoryId)).ReturnsAsync(category);
            _mockMapper.Setup(mapper => mapper.Map<CategoryDto>(category)).Returns(categoryDto);

            // Act
            var result = await _categoryService.GetCategoryByIdAsync(categoryId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(categoryId, result.Id);
        }

        [Fact]
        public async Task GetCategoryByIdAsync_ShouldReturnNull_WhenCategoryDoesNotExist()
        {
            // Arrange
            var categoryId = Guid.NewGuid();
            _mockCategoryRepository.Setup(repo => repo.GetByIdAsync(categoryId)).ReturnsAsync((Category)null);

            // Act
            var result = await _categoryService.GetCategoryByIdAsync(categoryId);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task CreateCategoryAsync_ShouldReturnNewGuid()
        {
            // Arrange
            var createCategoryDto = new CreateCategoryDto { Name = "New Category" };
            var category = new Category { Id = Guid.NewGuid(), Name = "New Category" };

            _mockMapper.Setup(mapper => mapper.Map<Category>(createCategoryDto)).Returns(category);
            _mockCategoryRepository.Setup(repo => repo.AddAsync(category)).Returns(Task.FromResult(0));

            // Act
            var result = await _categoryService.CreateCategoryAsync(createCategoryDto);

            // Assert
            Assert.NotEqual(Guid.Empty, result);
        }

        [Fact]
        public async Task UpdateCategoryAsync_ShouldUpdateCategory()
        {
            // Arrange
            var categoryId = Guid.NewGuid();
            var updateCategoryDto = new UpdateCategoryDto { Id = categoryId, Name = "Updated Category" };
            var existingCategory = new Category { Id = categoryId, Name = "Old Category" };

            _mockCategoryRepository.Setup(repo => repo.GetByIdAsync(categoryId)).ReturnsAsync(existingCategory);
            _mockMapper.Setup(mapper => mapper.Map(updateCategoryDto, existingCategory)).Returns(existingCategory);
            _mockCategoryRepository.Setup(repo => repo.UpdateAsync(existingCategory)).Returns(Task.FromResult(0));

            // Act
            await _categoryService.UpdateCategoryAsync(updateCategoryDto);

            // Assert
            _mockCategoryRepository.Verify(repo => repo.UpdateAsync(existingCategory), Times.Once);
        }

        [Fact]
        public async Task DeleteCategoryAsync_ShouldDeleteCategory()
        {
            // Arrange
            var categoryId = Guid.NewGuid();
            var category = new Category { Id = categoryId, Name = "Test Category" };

            _mockCategoryRepository.Setup(repo => repo.GetByIdAsync(categoryId)).ReturnsAsync(category);
            _mockCategoryRepository.Setup(repo => repo.DeleteAsync(categoryId)).Returns(Task.FromResult(0));

            // Act
            await _categoryService.DeleteCategoryAsync(categoryId);

            // Assert
            _mockCategoryRepository.Verify(repo => repo.DeleteAsync(categoryId), Times.Once);
        }
    }
}
