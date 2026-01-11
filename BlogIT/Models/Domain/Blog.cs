namespace BlogIT.Models.Domain
{
    public class Blog
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public Author Author { get; set; }
        public string FeaturedImageUrl { get; set; }
        public string UrlHandle { get; set; }
        public ICollection<Category> Categories { get; set; }
        public bool IsVisible { get; set; }
        public DateOnly CreatedTimeStamp { get; set; }
        public DateOnly LastEditTimeStamp { get; set; }

    }
}
