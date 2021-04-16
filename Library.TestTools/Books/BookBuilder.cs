using System;
using System.Collections.Generic;
using System.Text;
using Library.Entities;

namespace Library.TestTools.Books
{
    public class BookBuilder
    {
        private Book book=new Book
        {
            Name = "dummy-name",
            BookCategoryId = 1,
            Author = "dummy-author",
            MinAgeNeed = 15
        };
        public BookBuilder BuildBookWithName(string name)
        {
            book.Name = name;
            return this;
        }

        public BookBuilder BuildBookWithAuthor(string author)
        {
            book.Author = author;
            return this;
        }

        public BookBuilder BuildBookWithAgeRange(short minAgeNeed)
        {
            book.MinAgeNeed = minAgeNeed;
            return this;
        }

        public BookBuilder BuildBookWithCategoryId(int categoryId)
        {
            book.BookCategoryId = categoryId;
            return this;
        }

        public Book Build()
        {
            return book;
        }
    }
}
