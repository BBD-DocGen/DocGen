using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocGen.Models
{
    internal class Content
    {
        public string content { get; set; }

        public Content()
        {
            this.content = string.Empty;
        }

        public Content(string content)
        {
            this.content = content;
        }
    }
}
