using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ServiceProvider.Models
{
    public class AuthenticationToken
    {
        [Required]
        public int Token { get; set; }
    }
}