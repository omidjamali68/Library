using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Library.Services.Members.Contracts;

namespace Library.RestApi.Controllers
{
    [ApiController,Route("api/members")]
    public class MemberController : Controller
    {
        private readonly MemberService _service;

        public MemberController(MemberService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<int> Add(AddMemberDto dto)
        {
            int addedId = await _service.AddMember(dto);
            return addedId;
        }
    }
}
