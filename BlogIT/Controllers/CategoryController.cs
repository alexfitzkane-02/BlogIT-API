using BlogIT.Data;
using BlogIT.Models.Domain;
using BlogIT.Models.Dto;
using BlogIT.Repositories.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BlogIT.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryInterface _categoryRepository;

        public CategoryController(ICategoryInterface categoryRepository)
        {
            this._categoryRepository = categoryRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllCategories()
        {
            //store results in variable
            var response = await _categoryRepository.GetAllCategoriesAsync();

            //map response to dto
            var categories = new List<CategoryDto>();
            foreach (var category in response)
            {
                categories.Add(new CategoryDto
                {
                    Id = category.Id,
                    Name = category.Name,
                    UrlHandle = category.UrlHandle
                });
            }

            return Ok(categories);
        }

        [HttpGet]
        [Route("{id:guid}")]
        public async Task<IActionResult> GetCategoryById([FromRoute] Guid id)
        {
            var response = await _categoryRepository.GetCategoryByIdAsync(id);

            if (response is not null)
            {
                var category = new CategoryDto
                {
                    Id = response.Id,
                    Name = response.Name,
                    UrlHandle = response.UrlHandle
                };

                return Ok(category);
            }

            return NotFound("Category not found");
        }

        [HttpPut]
        [Route("{id:guid}")]
        public async Task<IActionResult> UpdateCategory([FromRoute] Guid id, [FromBody] UpdateCateogryRequestDto request)
        {
            //convert request dto to data model
            var category = new Category
            {
                Id = id,
                Name = request.Name,
                UrlHandle = request.UrlHandle
            };

            var response = await _categoryRepository.UpdateAsync(category);

            //convert data model to dto if there is a response
            if (response is not null)
            {
                var categoryDto = new CategoryDto
                {
                    Id = response.Id,
                    Name = response.Name,
                    UrlHandle = response.UrlHandle
                };

                return Ok(categoryDto);
            }

            return NotFound("Category not found");
        }

        [HttpPost]
        public async Task<IActionResult> CreateCategory([FromBody] CreateCategoryDto createCategoryDto)
        {
            //convert dto to data model

            var category = new Category
            {
                Id = Guid.NewGuid(),
                Name = createCategoryDto.Name,
                UrlHandle = createCategoryDto.UrlHandle
            };

            var response = await _categoryRepository.CreateAsync(category);

            //convert response to dto
            if(response is not null)
            {
                var categoryDto = new CategoryDto
                {
                    Id = response.Id,
                    Name = response.Name,
                    UrlHandle = response.UrlHandle
                };

                return Ok(categoryDto);
            }

            return BadRequest("Unable to create category");
        }

        [HttpDelete]
        [Route("{id:guid}")]
        public async Task<IActionResult> DeleteCategory([FromHeader] Guid id)
        {
            
            var response = await _categoryRepository.DeleteAsync(id);
            
            if(response is not null)
            {
                return Ok("Category deleted successfully");
            }
            else
            {
              return NotFound("Category not found");

            }
        }
    }
}
