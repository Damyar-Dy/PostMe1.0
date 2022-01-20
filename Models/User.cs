using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PostMe.Models
{
    public class User : IdentityUser
    {
        public List<Post> Posts { get; set; }
    }
}
