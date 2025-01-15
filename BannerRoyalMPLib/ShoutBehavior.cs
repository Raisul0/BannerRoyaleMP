using System.Collections.Generic;
using BannerRoyalMPLib.Globals;
using BannerRoyalMPLib.ObjectClasses;
using TaleWorlds.MountAndBlade;

namespace BannerRoyalMPLib
{
    public class ShoutBehavior : MissionNetwork
    {
        public override MissionBehaviorType BehaviorType => MissionBehaviorType.Other;
        public List<BannerRoyalShout> Shouts = new();
        public override void OnBehaviorInitialize()
        {
            base.OnBehaviorInitialize();
            if (GameNetwork.IsClient)
            {
                Read();
            }
        }

        protected override void HandleNewClientAfterSynchronized(NetworkCommunicator networkPeer)
        {
            if (GameNetwork.IsClient)
            {
                Read();
            }
        }

        public void RefreshTaunts()
        {
            if (GameNetwork.IsClient)
            {
                Read();
            }
        }

        public void Read()
        {
            Shouts = BannerRoyalShoutWheel.Shouts;
        }


    }
}
