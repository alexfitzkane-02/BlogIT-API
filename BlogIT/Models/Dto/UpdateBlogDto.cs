using BlogIT.Models.Domain;

namespace BlogIT.Models.Dto
{
    public class UpdateBlogDto
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public Guid Author { get; set; }
        public string FeaturedImageUrl { get; set; }
        public string UrlHandle { get; set; }
        public Guid[] Categories { get; set; }
        public bool IsVisible { get; set; }
    }
}
