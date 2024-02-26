using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearnProject.BLL.Contracts.Models.Identity
{
    public class ResetPasswordModel
    {
        public string Password { get; set; }

        public string Email { get; set; }

        public string Token { get; set; }
    }
}
