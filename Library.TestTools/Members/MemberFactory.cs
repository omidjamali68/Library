using System;
using System.Collections.Generic;
using System.Text;
using Library.Entities;
using Library.Services.Members.Contracts;

namespace Library.TestTools.Members
{
    public static class MemberFactory
    {
        public static Member GenerateMember(string address, short age, string fullName)
        {
            return new Member
            {
                Address = address,
                Age = age,
                FullName = fullName
            };
        }

        public static AddMemberDto GenerateAddMemberDto(string address, short age, string fullName)
        {
            return new AddMemberDto
            {
                Age = age,
                Address = address,
                FullName = fullName
            };
        }
    }
}
