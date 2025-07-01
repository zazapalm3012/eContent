using eContentApp.Application.DTOs.Category;
using eContentApp.Application.Interfaces;
using eContentApp.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace eContentApp.Host.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoriesController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CategoryDto>>> GetAllCategories()
        {
            var categories = await _categoryService.GetAllCategoriesAsync();
            return Ok(categories);
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<CategoryDto>> GetCategoryById(Guid id) 
        {
            var category = await _categoryService.GetCategoryByIdAsync(id);
            if (category == null)
            {
                return NotFound();
            }
            return Ok(category);
        }

        [HttpPost]
        public async Task<IActionResult> CreateCategory([FromBody] CreateCategoryDto createCategoryDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var newCategoryId = await _categoryService.CreateCategoryAsync(createCategoryDto);

                var createdCategory = await _categoryService.GetCategoryByIdAsync(newCategoryId);

                if (createdCategory == null)
                {
                    return StatusCode(500, "Failed to retrieve the created category.");
                }

               
                return CreatedAtAction(nameof(GetCategoryById), new { id = newCategoryId }, createdCategory);
            }
            catch (ApplicationException ex)
            {
                return BadRequest(new { message = ex.Message }); 
            }
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> UpdateCategory(Guid id, [FromBody] UpdateCategoryDto updateCategoryDto)
        {
            if (id != updateCategoryDto.Id)
            {
                return BadRequest("ID in URL and body mismatch.");
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                await _categoryService.UpdateCategoryAsync(updateCategoryDto);
                return NoContent();
            }
            catch (ApplicationException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception) 
            {
                return StatusCode(500, "An error occurred while updating the category.");
            }
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteCategory(Guid id)
        {
            try
            {
                await _categoryService.DeleteCategoryAsync(id);
                return NoContent();
            }
            catch (ApplicationException ex)
            {
                
                return NotFound(new { message = ex.Message });
            }
            catch (Exception) 
            {
                return StatusCode(500, "An error occurred while deleting the category.");
            }
        }
    }
}
