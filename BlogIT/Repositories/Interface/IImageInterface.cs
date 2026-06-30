using BlogIT.Models.Domain;

namespace BlogIT.Repositories.Interface
{
    public interface IImageInterface
    {
        Task<BlogImage> Upload(IFormFile file, BlogImage blogImage);
        Task<IEnumerable<BlogImage>> GetAll();
    }
}
