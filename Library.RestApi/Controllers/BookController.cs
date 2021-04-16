using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Library.Entities;
using Library.Services.Books.Contracts;

namespace Library.RestApi.Controllers
{
    [ApiController,Route("api/books")]
    public class BookController : Controller
    {
        private readonly BookService _service;

        public BookController(BookService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<int> Add(AddBookDto dto)
        {
            return await _service.AddBook(dto);
        }

        [HttpGet("{id}/")]
        public async Task<IEnumerable<Book>> GetBooksByCategoryId([FromRoute,Required] int id)
        {
            var books = await _service.GetBooksByCategoryId(id);
            return books;
        }

        [HttpPut("{id}")]
        public async Task<int> Update([FromRoute,Required] int id , UpdateBookDto dto)
        {
            int updatedId = await _service.UpdateBook(id, dto);
            return updatedId;
        }
    }
}
