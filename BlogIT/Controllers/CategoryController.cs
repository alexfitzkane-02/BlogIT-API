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
        private readonly ICategoryRepository _categoryRepository;

        public CategoryController(ICategoryRepository categoryRepository)
        {
            this._categoryRepository = categoryRepository;
        }

        [HttpGet]
        [Route("getcategories")]
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
        [Route("id:guid")]
        public async Task<IActionResult> GetCategoryById([FromHeader] Guid id)
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

            return NotFound();
        }

        [HttpPut]
        [Route("id:guid")]
        public async Task<IActionResult> UpdateCategory([FromHeader] Guid id, [FromBody] UpdateCateogryRequestDto request)
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

            return NotFound();
        }

        [HttpPost]
        [Route("createcategory")]
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

            var categoryDto = new CategoryDto
            {
                Id = response.Id,
                Name = response.Name,
                UrlHandle = response.UrlHandle
            };

            return Ok(categoryDto);

        }

        [HttpDelete]
        [Route("deletecategory")]
        public async Task<IActionResult> DeleteCategory([FromHeader] Guid id)
        {
            
            var response = await _categoryRepository.DeleteAsync(id);
            
            if(response is not null)
            {
                return Ok();
            }
            else
            {
              return NotFound();

            }
        }
    }
}
