using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IslahiTohfa.Application.DTOs
{
    public class AuthenticationResponse
    {
        public bool Success { get; set; }
        public string Token { get; set; }
        public UserDto User { get; set; }
        public DateTime TokenExpires { get; set; }
        public string RefreshToken { get; set; }
    }
}
