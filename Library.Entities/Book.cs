namespace Library.Entities
{
    public class Book
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Author { get; set; }
        public short MinAgeNeed { get; set; }
        public int BookCategoryId { get; set; }
        public BookCategory BookCategory { get; set; }
        public Entrust Entrust { get; set; }

    }

}
