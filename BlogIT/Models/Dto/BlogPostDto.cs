using BlogIT.Models.Domain;

namespace BlogIT.Models.Dto
{
    public class BlogPostDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public Author Author { get; set; } = new Author();
        public string FeaturedImageUrl { get; set; }
        public string UrlHandle { get; set; }
        public List<CategoryDto> Categories { get; set; } = new List<CategoryDto>();
        public bool IsVisible { get; set; }
        public DateOnly CreatedTimeStamp { get; set; }
        public DateOnly LastEditTimeStamp { get; set; }
    }
}
