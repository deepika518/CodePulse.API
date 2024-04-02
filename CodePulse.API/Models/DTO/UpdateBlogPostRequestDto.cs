namespace CodePulse.API.Models.DTO
{
    public class UpdateBlogPostRequestDto
    {
        public string Title { get; set; }
        public string ShortDescription { get; set; }
        public string Content { get; set; }
        public string FeaturedImageUrl { get; set; }
        public string UrlHandle { get; set; }
        public DateTime PublishedDate { get; set; }
        public string Author { get; set; }
        public bool IsVisible { get; set; }

        //multiple categories can be associated with a blogpost, this passing the list of ids of categories

        public List<Guid> Categories { get; set; } = new List<Guid>();
    }
}
