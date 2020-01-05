using System;
using System.Collections.Generic;
using System.Text;

namespace WikiLibs.Shared.Modules.Smtp
{
    public class Mail
    {
        public ICollection<Recipient> Recipients { get; set; }
        public ICollection<Recipient> CCRecipients { get; set; } = new HashSet<Recipient>();
        public string Subject { get; set; }
        public object Model { get; set; }
        public string Template { get; set; }
    }
}
