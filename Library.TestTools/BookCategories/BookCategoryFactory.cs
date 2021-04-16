using System;
using System.Collections.Generic;
using System.Text;
using Library.Entities;
using Library.Services.BookCategories.Contracts;

namespace Library.TestTools.BookCategories
{
    public static class BookCategoryFactory
    {
        public static BookCategory GenerateBookCategory()
        {
            return new BookCategory()
            {
                Title = "dummy-title"
            };
        }

        public static AddBookCategoryDto GenerateAddBookCategoryDto(string title)
        {
            return new AddBookCategoryDto
            {
                Title = title
            };
        }

        public static BookCategory GenerateBookCategory(string title)
        {
            return new BookCategory
            {
                Title = title
            };
        }
    }
}
