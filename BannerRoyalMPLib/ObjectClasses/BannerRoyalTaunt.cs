using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BannerRoyalMPLib.ObjectClasses
{
    public class BannerRoyalTaunt
    {
        public BannerRoyalTaunt(int tauntId, string tauntName,string tauntAction)
        {
            TauntId = tauntId;
            TauntName = tauntName;
            TauntAction = tauntAction;
        }

        public int TauntId { get; set; }
        public string TauntName { get; set; }
        public string TauntAction { get; set; }
    }
}
