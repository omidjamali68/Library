namespace Library.Services.Books.Contracts
{
    public class UpdateBookDto
    {
        public string Name { get; set; }
        public string Author { get; set; }
        public short MinAgeNeed { get; set; }
        public int CategoryId { get; set; }
    }
}
