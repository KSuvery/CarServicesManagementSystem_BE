using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarServ.Domain.Entities
{
   
    public class PromotionServicePackage
    {
        public int PromotionID { get; set; }
        public int PackageID { get; set; }
        
        public virtual Promotions Promotion { get; set; }
        public virtual ServicePackages ServicePackage { get; set; }
    }
}
