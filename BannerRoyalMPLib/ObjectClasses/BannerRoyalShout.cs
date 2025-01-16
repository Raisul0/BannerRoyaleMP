using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BannerRoyalMPLib.ObjectClasses
{
    public class BannerRoyalShout
    {
        public BannerRoyalShout(int shoutIndex, string shoutName, string voiceType)
        {
            ShoutIndex = shoutIndex;
            ShoutName = shoutName;
            VoiceType = voiceType;
        }

        public int ShoutIndex { get; set; }
        public string ShoutName { get; set; }
        public string VoiceType { get; set;}
    }
}
