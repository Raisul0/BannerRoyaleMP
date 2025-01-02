using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BannerRoyalMPLib.ObjectClasses;

namespace BannerRoyalMPLib.Globals
{
    public static class BannerRoyalTauntWheel
    {
        public static List<BannerRoyalTaunt> Taunts = new() {
            new BannerRoyalTaunt(1,"First",""),
            new BannerRoyalTaunt(2,"Second",""),
            new BannerRoyalTaunt(3,"Third",""),
            new BannerRoyalTaunt(4,"Fourth","act_taunt_salsa"),
            new BannerRoyalTaunt(5,"Fifth","act_taunt_flexing"),
            new BannerRoyalTaunt(6,"Six","act_taunt_congo"),
        };
    }
}
