using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.DomainClasses
{
    public class Mail
    {
        public string From { get; set; }
        public string Destination { get; set; }

        public string Subject { get; set; }
        public string Body { get; set; }
    }
}
