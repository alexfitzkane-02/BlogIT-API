using BlogIT.Data;
using BlogIT.Models.Domain;
using BlogIT.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace BlogIT.Repositories.Implementation
{
    public class BlogRepository : IBlogInterface
    {
        private readonly ApplicationDbContext _applicationDbContext;

        public BlogRepository(ApplicationDbContext applicationDbContext)
        {
            this._applicationDbContext = applicationDbContext;
        }

        public async Task<Blog> CreateBlogAsync(Blog newBlog)
        {
            await _applicationDbContext.Blogs.AddAsync(newBlog);
            await _applicationDbContext.SaveChangesAsync();
            return newBlog;
        }

        public async Task<Blog?> DeleteBlogAsync(Guid blogId)
        {
            var response = await _applicationDbContext.Blogs.FirstOrDefaultAsync(x => x.Id == blogId);
            if (response is not null)
            {
                _applicationDbContext.Blogs.Remove(response);
                await _applicationDbContext.SaveChangesAsync();
                return response;

            }
                return null;
        }

        public async Task<IEnumerable<Blog>> GetAllBlogsAsync()
        {
            return await _applicationDbContext.Blogs
                    .Include(x => x.Author)
                    .Include(x => x.Categories)
                        .ToListAsync();
        }

        public async Task<Blog?> GetBlogByIdAsync(Guid blogId)
        {
            var response = await _applicationDbContext.Blogs
                   .Include(x => x.Author)
                   .Include(x => x.Categories)
                     .FirstOrDefaultAsync(x => x.Id == blogId);

            return response;
        }

        public async Task<Blog?> UpdateBlogAsync(Blog updatedBlog)
        {
            var response = await _applicationDbContext.Blogs
                .Include(x => x.Author)
                .Include(x => x.Categories)
                .FirstOrDefaultAsync(x => x.Id == updatedBlog.Id);

            if (response is not null)
            {
                _applicationDbContext.Entry(response).CurrentValues.SetValues(updatedBlog);

                response.Categories = updatedBlog.Categories;
                response.Author = updatedBlog.Author;

                await _applicationDbContext.SaveChangesAsync();

                return response;
            }

            return null;
        }
    }
}
