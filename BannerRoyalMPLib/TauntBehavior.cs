using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using BannerRoyalMPLib.Globals;
using BannerRoyalMPLib.ObjectClasses;
using TaleWorlds.Core;
using TaleWorlds.ModuleManager;
using TaleWorlds.MountAndBlade;
using static TaleWorlds.MountAndBlade.Diamond.TauntUsageManager;

namespace BannerRoyalMPLib
{
    public class TauntBehavior : MissionNetwork
    {
        public override MissionBehaviorType BehaviorType => MissionBehaviorType.Other;
        public List<BannerRoyalTaunt> Taunts = new();
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
            if(GameNetwork.IsClient)
            {
                Read();
            }
        }

        public void Read()
        {
            Taunts = BannerRoyalTauntWheel.Taunts;
        }


    }
}
