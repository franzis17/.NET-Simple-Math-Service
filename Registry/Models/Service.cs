using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Registry.Models
{
    //Simple data class for a Service
    public class Service
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Endpoint { get; set; }
        public int Operands { get; set; }
        public string OperandType { get; set; }
    }
}