using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VV.Web.Models
{
    public class UsersDTO
    {
        private static UsersDTO userDto = new UsersDTO();

        public string UserName { get; set; }

        private UsersDTO()
        {
        }

        public static UsersDTO Instance
        {
            get { return userDto == null ? new UsersDTO() : userDto; }
        }
    }
}