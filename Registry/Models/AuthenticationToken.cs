using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Registry.Models
{
    public class AuthenticationToken
    {
        [Required]
        public string Token { get; set; }
    }
}