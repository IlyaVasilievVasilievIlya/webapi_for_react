using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearnProject.Shared.Common.Settings
{
    public class ExternalProviders
    {
        public Google google { get; set; } = new Google();


        public class Google
        {
            public string ClientId { get; set; } = string.Empty;
        }
    }
}
