using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Registry.Models
{
    //Simple data class for a Service
    public class Service
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public string Endpoint { get; set; }
        [Required]
        public int Operands { get; set; }
        [Required]
        public string OperandType { get; set; }
    }
}