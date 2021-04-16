using System;
using System.Collections.Generic;
using System.Text;
using Library.Entities;

namespace Library.Services.Books.Contracts
{
    public class AddBookDto
    {
        public string Name { get; set; }
        public string Author { get; set; }
        public short MinAgeNeed { get; set; }
        public int CategoryId { get; set; }
    }
}
