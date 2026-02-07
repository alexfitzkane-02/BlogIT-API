using BlogIT.Models.Domain;
using BlogIT.Models.Dto;
using BlogIT.Repositories.Implementation;
using BlogIT.Repositories.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BlogIT.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlogController : ControllerBase
    {
        private readonly IBlogInterface _blogInterface;
        private readonly ICategoryInterface _categoryInterface;
        private readonly IAuthorInterface _authorInterface;

        public BlogController(IBlogInterface blogInterface, ICategoryInterface categoryInterface, IAuthorInterface authorInterface)
        {
            this._blogInterface = blogInterface;
            this._categoryInterface = categoryInterface;
            this._authorInterface = authorInterface;
        }

        //Get All Blogs
        [HttpGet]
        public async Task<IActionResult> GetAllBlogPosts()
        {
            var blogPosts = await _blogInterface.GetAllBlogsAsync();

            //convert model to dto
            var response = new List<BlogPostDto>();
            foreach (var blog in blogPosts)
            {
                response.Add(new BlogPostDto
                {
                    Id = blog.Id,
                    Title = blog.Title,
                    Description = blog.Description,
                    Author = blog.Author,
                    FeaturedImageUrl = blog.FeaturedImageUrl,
                    UrlHandle = blog.UrlHandle,
                    IsVisible = blog.IsVisible,
                    CreatedTimeStamp = blog.CreatedTimeStamp,
                    LastEditTimeStamp = blog.LastEditTimeStamp,
                    Categories = blog.Categories.Select(x => new CategoryDto
                    {
                        Id = x.Id,
                        Name = x.Name,
                        UrlHandle = x.UrlHandle
                    }).ToList()
                });
            }

            return Ok(response);
        }

        //Get Blog By Id
        [HttpGet]
        [Route("{id:guid}")]
        public async Task<IActionResult> GetBlogPostById([FromRoute] Guid id)
        {
            var blogPost = await _blogInterface.GetBlogByIdAsync(id);
            if (blogPost is not null)
            {
                //convert domain model to dto
                var response = new BlogPostDto
                {
                    Id = blogPost.Id,
                    Title = blogPost.Title,
                    Description = blogPost.Description,
                    Author = blogPost.Author,
                    FeaturedImageUrl = blogPost.FeaturedImageUrl,
                    UrlHandle = blogPost.UrlHandle,
                    IsVisible = blogPost.IsVisible,
                    CreatedTimeStamp = blogPost.CreatedTimeStamp,
                    LastEditTimeStamp = blogPost.LastEditTimeStamp,
                    Categories = blogPost.Categories.Select(x => new CategoryDto
                    {
                        Id = x.Id,
                        Name = x.Name,
                        UrlHandle = x.UrlHandle
                    }).ToList()
                };
                return Ok(response);
            }
            else
            {
                return NotFound("Blog post was not found");
            }
        }

        //Update Blog
        [HttpPut]
        [Route("{id:guid}")]
        public async Task<IActionResult> UpdateBlogById([FromRoute]Guid id, [FromBody]UpdateBlogDto updateBlogDto)
        {
            //check if author exists
            var existingAuthor = await _authorInterface.GetAuthorByIdAsync(updateBlogDto.Author);
            if (existingAuthor is null)
            {
                return NotFound("Author not found");
            }

            //convert DTO to Domain model
            var blog = new Blog
            {
                Id = id,
                Title = updateBlogDto.Title,
                Description = updateBlogDto.Description,
                Author = existingAuthor!,
                FeaturedImageUrl = updateBlogDto.FeaturedImageUrl,
                UrlHandle = updateBlogDto.UrlHandle,
                IsVisible = updateBlogDto.IsVisible,
                CreatedTimeStamp = DateOnly.FromDateTime(DateTime.Now),
                LastEditTimeStamp = DateOnly.FromDateTime(DateTime.Now),
                Categories = new List<Category>()
            };

            // loop through categories
            foreach (var categoryGuid in updateBlogDto.Categories)
            {
                var existingCategory = await _categoryInterface.GetCategoryByIdAsync(categoryGuid);
                if (existingCategory is not null)
                {
                    blog.Categories.Add(existingCategory);
                }
                else
                {
                    return NotFound($"Category with ID {categoryGuid} not found");
                }
            }

            //call repository to update blog post domain model
            var updateBlogPost = await _blogInterface.UpdateBlogAsync(blog);
            if (updateBlogPost is null)
            {
                return NotFound();
            }

            var response = new BlogPostDto
            {
                Id = blog.Id,
                Title = blog.Title,
                Description = blog.Description,
                Author = blog.Author,
                FeaturedImageUrl = blog.FeaturedImageUrl,
                UrlHandle = blog.UrlHandle,
                IsVisible = blog.IsVisible,
                CreatedTimeStamp = blog.CreatedTimeStamp,
                LastEditTimeStamp = blog.LastEditTimeStamp,
                Categories = blog.Categories.Select(x => new CategoryDto
                {
                    Id = x.Id,
                    Name = x.Name,
                    UrlHandle = x.UrlHandle
                }).ToList()

            };

            return Ok(response);
        }

        //Create Blog
        [HttpPost]
        public async Task<IActionResult> CreateBlogPost([FromBody]CreateBlogDto createBlogDto)
        {

            //check if author exists
            var resultAuthor = await _authorInterface.GetAuthorByIdAsync(createBlogDto.Author);
            if (Response is null)
            {
                return NotFound("Author not found");
            }

            //bind dto to model
            var blogPost = new Blog
            {
                Title = createBlogDto.Title,
                Description = createBlogDto.Description,
                Author = resultAuthor!,
                FeaturedImageUrl = createBlogDto.FeaturedImageUrl,
                UrlHandle = createBlogDto.UrlHandle,
                IsVisible = createBlogDto.IsVisible,
                CreatedTimeStamp = DateOnly.FromDateTime(DateTime.Now),
                LastEditTimeStamp = DateOnly.FromDateTime(DateTime.Now),
                Categories = new List<Category>()
            };

            //lookup if the guid exists and if so then add the record to the database
            foreach (var categoryGuid in createBlogDto.Categories)
            {
                var existingCategory = await _categoryInterface.GetCategoryByIdAsync(categoryGuid);
                if (existingCategory != null)
                {
                    blogPost.Categories.Add(existingCategory);
                }
            }

            //pass model to repository
            var result = await _blogInterface.CreateBlogAsync(blogPost);
            if (result != null)
            {
                var createdBlogPost = new BlogPostDto
                {
                    Id = result.Id,
                    Title = result.Title,
                    Description = result.Description,
                    Author = result.Author,
                    FeaturedImageUrl = result.FeaturedImageUrl,
                    UrlHandle = result.UrlHandle,
                    IsVisible = result.IsVisible,
                    CreatedTimeStamp = result.CreatedTimeStamp,
                    LastEditTimeStamp = result.LastEditTimeStamp,
                    Categories = result.Categories.Select(x => new CategoryDto
                    {
                        Id = x.Id,
                        Name = x.Name,
                        UrlHandle = x.UrlHandle
                    }).ToList()
                };

                return Ok(createdBlogPost);
            }
            else
            {
                return StatusCode(500, "An error occurred while creating the blog post.");
            }
        }

        //Delete Blog
        [HttpDelete]
        [Route("{id:guid}")]
        public async Task<IActionResult> DeleteBlogPostById(Guid id)
        {
            var deletedBlogPost = await _blogInterface.DeleteBlogAsync(id);
            if (deletedBlogPost is not null)
            {
                var response = new BlogPostDto
                {
                    Id = deletedBlogPost.Id,
                    Title = deletedBlogPost.Title,
                    Description = deletedBlogPost.Description,
                    Author = deletedBlogPost.Author,
                    FeaturedImageUrl = deletedBlogPost.FeaturedImageUrl,
                    UrlHandle = deletedBlogPost.UrlHandle,
                    CreatedTimeStamp = deletedBlogPost.CreatedTimeStamp,
                    IsVisible = deletedBlogPost.IsVisible,
                    Categories = deletedBlogPost.Categories.Select(x => new CategoryDto
                    {
                        Id = x.Id,
                        Name = x.Name,
                        UrlHandle = x.UrlHandle
                    }).ToList()
                };
                
                return Ok(response);
            }
            else
            {
                return NotFound("Blog not found");
            }
        }

    }
}
