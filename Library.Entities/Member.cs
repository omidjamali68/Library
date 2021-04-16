using System;
using System.Collections.Generic;
using System.Text;

namespace Library.Entities
{
    public class Member
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public Int16 Age { get; set; }
        public string Address { get; set; }
        public Entrust Entrust { get; set; }


    }
}
