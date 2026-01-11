using BlogIT.Data;
using BlogIT.Models.Domain;
using BlogIT.Repositories.Interface;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace BlogIT.Repositories.Implementation
{
    public class AuthorRepository : IAuthorInterface
    {
        private readonly ApplicationDbContext _applicationDbContext;

        public AuthorRepository(ApplicationDbContext applicationDbContext)
        {
            this._applicationDbContext = applicationDbContext;
        }

        public async Task<Author> CreateAsync(Author author)
        {
           await _applicationDbContext.Authors.AddAsync(author);
           await _applicationDbContext.SaveChangesAsync();
            return author;

        }

        public async Task<Author?> DeleteAsync(Guid id)
        {
            var response = await _applicationDbContext.Authors.FirstOrDefaultAsync(x => x.Id == id);

            if (response is not null)
            {
                _applicationDbContext.Authors.Remove(response);
                await _applicationDbContext.SaveChangesAsync();

                return response;
            }

            return null;
        }

        public async Task<Author?> GetAuthorByIdAsync(Guid id)
        {
            var response = await _applicationDbContext.Authors.FirstOrDefaultAsync(x => x.Id == id);
            return response;
        }

        public async Task<IEnumerable<Author>> GetAuthorsAsync()
        {
            var response = await _applicationDbContext.Authors.ToListAsync();
            return response;
        }

        public async Task<Author?> UpdateAsync(Author author)
        {
            var response = await _applicationDbContext.Authors.FirstOrDefaultAsync(x => x.Id == author.Id);

            if (response is not null)
            {
                response.Name = author.Name;
                response.UrlHandle = author.UrlHandle;
                await _applicationDbContext.SaveChangesAsync();
                return response;
            }
            else
            {
                return null;
            }
        }
    }
}
