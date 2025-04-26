using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Evyte.ApplicationCore.Models.ViewModels
{
    public class SideMenuVM
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string ProfileImage { get; set; }
        public int Complaints { get; set; }
    }
}