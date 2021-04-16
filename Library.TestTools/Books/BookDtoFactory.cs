using System;
using System.Collections.Generic;
using System.Text;
using Library.Entities;
using Library.Services.Books.Contracts;

namespace Library.TestTools.Books
{
    public static class BookDtoFactory
    {

        public static AddBookDto GenerateAddBookDto(int categoryId, short minAgeNeed
            , string author, string name)
        {
            return new AddBookDto
            {
                CategoryId = categoryId,
                Author = author,
                MinAgeNeed = minAgeNeed,
                Name = name
            };
        }


        public static UpdateBookDto GenerateUpdateBookDto(int categoryId, short minAgeNeed,
            string author, string name)
        {
            return new UpdateBookDto
            {
                CategoryId = categoryId,
                MinAgeNeed = minAgeNeed,
                Author = author,
                Name = name
            };
        }
    }
}
