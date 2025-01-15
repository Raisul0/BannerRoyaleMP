using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BannerRoyalMPLib.ObjectClasses
{
    public class BannerRoyalShout
    {
        public BannerRoyalShout(int shoutIndex, string shoutName)
        {
            ShoutIndex = shoutIndex;
            ShoutName = shoutName;
        }

        public int ShoutIndex { get; set; }
        public string ShoutName { get; set; }
    }
}
