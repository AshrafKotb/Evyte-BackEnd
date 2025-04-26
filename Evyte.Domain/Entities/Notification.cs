using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evyte.Domain.Entities
{
    public class Notification : BaseEntity
    {
        public string Title { get; set; }
        public string Body { get; set; }
        public bool IsSeen { get; set; }
        public string NotificationLink { get; set; }

    }
}
