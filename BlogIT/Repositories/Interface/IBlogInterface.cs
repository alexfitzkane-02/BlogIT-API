using BlogIT.Models.Domain;

namespace BlogIT.Repositories.Interface
{
    public interface IBlogInterface
    {
        Task<(IEnumerable<Blog> Blogs, int TotalCount)> GetAllBlogsAsync(int pageNumber, int pageSize, string? search = null, bool? isVisible = null);
        Task<Blog?> GetBlogByIdAsync(Guid blogId);
        Task<Blog?> GetBlogPostByUrlHandle (string urlHandle);
        Task<Blog> CreateBlogAsync(Blog newBlog);
        Task<Blog?> UpdateBlogAsync(Blog updatedBlog);
        Task<Blog?> DeleteBlogAsync(Guid blogId);
    }
}
