using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evyte.Domain.Entities
{
    public class GeneralInformation : BaseEntity
    {
        #region Social Media Data
        public string FaceBook { get; set; }
        public string Instagram { get; set; }
        public string X { get; set; }
        public string Tiktok { get; set; }
        public string Youtube { get; set; }
        public string WhatsApp { get; set; }
        public string Snapchat { get; set; }
        public string Email { get; set; }

        #endregion
        public string TermsAndConditionsAr { get; set; }
        public string TermsAndConditionsEn { get; set; }

    }
}
