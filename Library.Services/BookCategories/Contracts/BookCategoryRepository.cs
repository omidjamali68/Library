using System;
using System.Collections.Generic;
using System.Text;
using Library.Entities;
using Library.Infrastructure.Application;

namespace Library.Services.BookCategories.Contracts
{
    public interface BookCategoryRepository : Repository
    {
        void Add(BookCategory bookCategory);
    }
}
