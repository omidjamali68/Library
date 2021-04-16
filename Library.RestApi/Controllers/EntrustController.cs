using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Library.Services.Books.Contracts;
using Library.Services.Entrusts.Contracts;
using Library.Services.Members.Contracts;

namespace Library.RestApi.Controllers
{
    [ApiController,Route("api/entrusts")]
    public class EntrustController : Controller
    {
        private readonly EntrustService _entrustService;

        public EntrustController(EntrustService entrustService)
        {
            _entrustService = entrustService;
        }

        [HttpPost]
        public async Task<int> Add(AddEntrustDto dto)
        {
            int addedId = await _entrustService.AddEntrust(dto);
            return addedId;
        }

        [HttpPut("{id}")]
        public async Task<int> Update([FromRoute,Required] int id, UpdateEntrustRealReturnDateDto dto)
        {
            int updatedId = await _entrustService.UpdateEntrustRealReturnDate(id, dto);
            return updatedId;
        }
    }
}
