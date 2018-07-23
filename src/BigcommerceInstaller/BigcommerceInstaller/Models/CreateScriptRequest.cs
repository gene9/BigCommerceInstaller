using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BigcommerceInstaller.Models
{
    public class CreateScriptRequest
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Html { get; set; }
        public string Src { get; set; }
        public bool AutoUninstall { get; set; }
        public string LoadMethod { get; set; }
        public string Location { get; set; }
        public string visibility { get; set; }
        public string Kind { get; set; }
    }
}
