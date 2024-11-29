using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.MountAndBlade;

namespace BannerRoyalMPServer.Extensions.DropItem
{
    public class DropItemBehavior : MissionNetwork
    {
        public override void OnBehaviorInitialize()
        {
            base.OnBehaviorInitialize();

        }

        protected override void HandleNewClientAfterSynchronized(NetworkCommunicator networkPeer)
        {

        }
    }
}
