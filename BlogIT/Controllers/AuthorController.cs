using BlogIT.Models.Domain;
using BlogIT.Models.Dto;
using BlogIT.Repositories.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Abstractions;

namespace BlogIT.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorController : ControllerBase
    {
        private readonly IAuthorInterface _authorInterface;

        public AuthorController(IAuthorInterface authorInterface)
        {
            this._authorInterface = authorInterface;
        }

        [HttpGet]
        [Route("authors")]
        public async Task<IActionResult> GetAllAuthorsAsync()
        {
            var response = await _authorInterface.GetAuthorsAsync();

            if(response is not null)
            {
                var authors = new List<AuthorDto>();
                foreach (var author in response)
                {
                    authors.Add(new AuthorDto
                    {
                        Id = author.Id,
                        Name = author.Name,
                        UrlHandle = author.UrlHandle
                    });
                }

                return Ok(authors);
            }

            return NotFound("No authors found");
        }

        [HttpGet]
        [Route("id:guid")]
        public async Task<IActionResult> GetAuthorbyIdAsync([FromHeader] Guid id)
        { 
            var response = await _authorInterface.GetAuthorByIdAsync(id);
            if(response is not null)
            {
                var author = new AuthorDto
                {  
                    Id = response.Id,
                    Name = response.Name,
                    UrlHandle = response.UrlHandle
                };

                return Ok(author);
            }

            return NotFound("Author not found");
        }

        [HttpPut]
        [Route("id:guid")]
        public async Task<IActionResult> UpdateAsync([FromHeader]Guid id, [FromBody]UpdateAuthorDto updateAuthorDto)
        {
            var author = new Author
            {
                Id = id,
                Name = updateAuthorDto.Name,
                UrlHandle = updateAuthorDto.UrlHandle
            };

            var response = await _authorInterface.UpdateAsync(author);

            if (response is not null)
            {
                var updatedAuthor = new AuthorDto
                {
                    Id = response.Id,
                    Name = response.Name,
                    UrlHandle = response.UrlHandle
                };

                return Ok(updatedAuthor);
            }

            else
            {
               return NotFound("Author not found");
            }
        }

        [HttpPost]
        [Route("author")]
        public async Task<IActionResult> CreateAsync([FromBody] CreateAuthorDto createAuthorDto)
        {
            var author = new Author
            {
                Id = Guid.NewGuid(),
                Name = createAuthorDto.Name,
                UrlHandle = createAuthorDto.UrlHandle
            };

            var response = await _authorInterface.CreateAsync(author);

            if(response is not null)
            {
                var createdAuthor = new AuthorDto
                {
                    Id = response.Id,
                    Name = response.Name,
                    UrlHandle = response.UrlHandle
                };

                return Ok(createdAuthor);
            }
            else
            {
                return BadRequest("Unable to create author");
            }
        }
        [HttpDelete]
        [Route("author")]
        public async Task<IActionResult> DeleteAsync([FromHeader] Guid id)
        {
            await _authorInterface.DeleteAsync(id);
            if (Response is not null)
            {
                return Ok("Author deleted successfully");

            }
            else
            {
                return NotFound("Author not found");
            }
        }
    }
}
