namespace CodePulse.API.Models.Domain
{
    public class Category
    {
        public Guid Id { get; set; }
        public string Name { get; set; }    
        public string UrlHandle { get; set; }
        // to have many to many relationship with blogpost

        public ICollection<BlogPost> BlogPosts { get; set; }
    }
}
