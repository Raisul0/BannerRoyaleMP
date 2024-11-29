using BannerRoyalMPLib;
using BannerRoyalMPLib.NetworkMessages.FromClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.Core;
using TaleWorlds.MountAndBlade;

namespace BannerRoyalMPServer.Extensions.Taunt
{
    public class TauntHandler : IHandlerRegister
    {
        public void Register(GameNetwork.NetworkMessageHandlerRegisterer reg)
        {
            reg.Register<StartTaunt>(UseTaunt);
        }

        public bool UseTaunt(NetworkCommunicator networkPeer, StartTaunt baseMessage)
        {
            var peer = baseMessage.Player;
            var action = baseMessage.Action;

            ActionIndexCache suitableTauntAction = CosmeticsManagerHelper.GetSuitableTauntAction(networkPeer.ControlledAgent, 1);
            if (suitableTauntAction.Index >= 0)
            {
                networkPeer.ControlledAgent.SetActionChannel(1, suitableTauntAction, false, 0UL, 0f, 1f, -0.2f, 0.4f, 0f, false, -0.2f, 0, true);
            }

            return true;
        }
    }
}
