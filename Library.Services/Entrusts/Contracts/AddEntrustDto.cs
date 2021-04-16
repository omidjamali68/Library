using System;
using System.Collections.Generic;
using System.Text;

namespace Library.Services.Entrusts.Contracts
{
    public class AddEntrustDto
    {
        public int BookId { get; set; }
        public int MemberId { get; set; }
        public DateTime BookReturnDate { get; set; }
    }
}
