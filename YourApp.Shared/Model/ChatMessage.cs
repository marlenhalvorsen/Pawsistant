using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace Library.Shared.Model
{
    public class ChatMessage
    {
        public string Role { get; set; } = "user";
        public string Content {  get; set; }
        public DateTime DateTime { get; set; }
    }
}
