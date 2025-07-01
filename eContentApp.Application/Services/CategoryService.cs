using AutoMapper;
using eContentApp.Application.DTOs.Category;
using eContentApp.Application.Interfaces;
using eContentApp.Domain.Entities;

namespace eContentApp.Application.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;

        public CategoryService(
            ICategoryRepository categoryRepository,
            IMapper mapper)
        {
            _categoryRepository = categoryRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<CategoryDto>> GetAllCategoriesAsync()
        {
            var categories = await _categoryRepository.GetAllAsync(); 
            return _mapper.Map<IEnumerable<CategoryDto>>(categories);
        }

        public async Task<CategoryDto?> GetCategoryByIdAsync(Guid id)
        {
            var category = await _categoryRepository.GetByIdAsync(id); 
            if (category == null)
            {
                return null;
            }
            return _mapper.Map<CategoryDto>(category);
        }

        public async Task<Guid> CreateCategoryAsync(CreateCategoryDto categoryDto)
        {
            var category = _mapper.Map<Category>(categoryDto);
            category.Id = Guid.NewGuid(); 
            await _categoryRepository.AddAsync(category); 
            return category.Id;
        }

        public async Task UpdateCategoryAsync(UpdateCategoryDto categoryDto)
        {
            var existingCategory = await _categoryRepository.GetByIdAsync(categoryDto.Id);
            if (existingCategory == null)
            {
                throw new ApplicationException($"Category with ID {categoryDto.Id} not found.");
            }

            _mapper.Map(categoryDto, existingCategory); 
            await _categoryRepository.UpdateAsync(existingCategory); 
        }

        public async Task DeleteCategoryAsync(Guid id)
        {
            var category = await _categoryRepository.GetByIdAsync(id); 
            if (category == null)
            {
                throw new ApplicationException($"Category with ID {id} not found for deletion.");
            }
            await _categoryRepository.DeleteAsync(id); 
        }
    }
}