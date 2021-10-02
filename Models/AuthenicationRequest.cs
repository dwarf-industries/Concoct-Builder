using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Concoct_Builder.Models
{
    public class AuthenicationRequest
    {
        public string Token { get; set;}
        public string Username { get; set; }
        public string Password { get; set; }
        public string Instance { get; set; }
        public bool AuthType { get; set; }
    }
}
