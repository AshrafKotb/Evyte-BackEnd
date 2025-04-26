using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Evyte.ApplicationCore.Models.ViewModels
{
    public class NotificationVM
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public bool IsSeen { get; set; }
        public long RequestId { get; set; }
        //public NotifiactionType NotificationType { get; set; }
        public string Link { get; set; }
        public string CreatedDate { get; set; }
    }
}