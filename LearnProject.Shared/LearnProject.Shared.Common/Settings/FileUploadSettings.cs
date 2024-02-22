using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearnProject.Shared.Common.Settings
{
    public class FileUploadSettings
    {
        public string MAX_SIZE { get; set; } = string.Empty;
        public string AllowedExtensions { get; set; } = string.Empty;
    }
}
