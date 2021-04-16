using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Library.Services.BookCategories.Contracts;

namespace Library.RestApi.Controllers
{
    [ApiController, Route("api/book-categories")]
    public class BookCategoryController : Controller
    {
        private readonly BookCategoryService _service;

        public BookCategoryController(BookCategoryService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<int> Add(AddBookCategoryDto dto)
        {
            int addedId= await _service.AddCategory(dto);
            return addedId;
        }
    }
}
