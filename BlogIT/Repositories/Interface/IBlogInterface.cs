using BlogIT.Models.Domain;

namespace BlogIT.Repositories.Interface
{
    public interface IBlogInterface
    {
        Task<IEnumerable<Blog>> GetAllBlogsAsync();
        Task<Blog?> GetBlogByIdAsync(Guid blogId);
        Task<Blog> CreateBlogAsync(Blog newBlog);
        Task<Blog?> UpdateBlogAsync(Blog updatedBlog);
        Task<Blog?> DeleteBlogAsync(Guid blogId);
    }
}
