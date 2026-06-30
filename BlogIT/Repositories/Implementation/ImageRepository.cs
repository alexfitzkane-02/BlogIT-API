using BlogIT.Data;
using BlogIT.Models.Domain;
using BlogIT.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace BlogIT.Repositories.Implementation
{
    public class ImageRepository : IImageInterface
    {
        private readonly IWebHostEnvironment webHostEnvironment;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly ApplicationDbContext applicationDbContext;

        public ImageRepository(IWebHostEnvironment webHostEnvironment, IHttpContextAccessor httpContextAccessor, ApplicationDbContext applicationDbContext)
        {
            this.webHostEnvironment = webHostEnvironment;
            this.httpContextAccessor = httpContextAccessor;
            this.applicationDbContext = applicationDbContext;
        }
        public async Task<BlogImage> Upload(IFormFile file, BlogImage blogImage)
        {
            //1 - Upload image to API Folder/Images
            var localPath = Path.Combine(webHostEnvironment.WebRootPath, "Images", $"{blogImage.FileName}{blogImage.FileExtension}");
            using var stream = new FileStream(localPath, FileMode.Create);
            await file.CopyToAsync(stream);

            //2 - Update database
            //codepulse.com/images/somefilename.jpg
            var httpRequest = httpContextAccessor.HttpContext.Request;
            var urlPath = $"{httpRequest.Scheme}://{httpRequest.Host}{httpRequest.PathBase}/Images/{blogImage.FileName}{blogImage.FileExtension}";

            blogImage.Url = urlPath;

            await applicationDbContext.BlogImages.AddAsync(blogImage);
            await applicationDbContext.SaveChangesAsync();

            return blogImage;
        }

        public async Task<IEnumerable<BlogImage>> GetAll()
        {
            return await applicationDbContext.BlogImages.ToListAsync();

        }
    }
}
