using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearnProject.Shared.Common.Settings
{
    public class FileUploadSettings
    {
        public int MAX_SIZE { get; set; }
        public string AllowedExtensions { get; set; } = string.Empty;
    }
}
