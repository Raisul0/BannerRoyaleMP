using BannerRoyalMPLib;
using BannerRoyalMPLib.Globals;
using BannerRoyalMPLib.NetworkMessages.FromClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.Core;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection.Lobby.Armory;

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
            var tauntIndex = baseMessage.TauntIndex;

            var tauntAction = BannerRoyalTauntWheel.Taunts.FirstOrDefault(x=>x.TauntId == tauntIndex)?.TauntAction ?? "act_taunt_congo";

            ActionIndexCache suitableTauntAction = ActionIndexCache.Create(tauntAction);

            if (suitableTauntAction.Index >= 0)
            {
                networkPeer.ControlledAgent.SetActionChannel(1, suitableTauntAction, false, 0UL, 0f, 1f, -0.2f, 0.4f, 0f, false, -0.2f, 0, true);
            }

            return true;
        }
    }
}
