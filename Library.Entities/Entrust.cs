using System;
using System.Collections.Generic;
using System.Text;

namespace Library.Entities
{
    public class Entrust
    {
        public int Id { get; set; }
        public int BookId { get; set; }
        public Book Book { get; set; }
        public int MemberId { get; set; }
        public Member Member { get; set; }
        public DateTime DeterminateReturnDate { get; set; }
        public DateTime? RealReturnDate { get; set; }


    }
}
