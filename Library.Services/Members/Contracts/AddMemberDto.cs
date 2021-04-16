using System;
using System.Collections.Generic;
using System.Text;

namespace Library.Services.Members.Contracts
{
    public class AddMemberDto
    {
        public string FullName { get; set; }
        public Int16 Age { get; set; }
        public string Address { get; set; }
    }
}
