using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Library.Infrastructure.Application;

namespace Library.Services.BookCategories.Contracts
{
    public interface BookCategoryService : Service
    {
        Task<int> AddCategory(AddBookCategoryDto dto);
    }
}
