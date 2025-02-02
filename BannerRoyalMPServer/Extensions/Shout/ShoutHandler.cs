using BannerRoyalMPLib;
using BannerRoyalMPLib.Globals;
using BannerRoyalMPLib.NetworkMessages.FromClient;
using NetworkMessages.FromServer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection.Lobby.Armory;
using static TaleWorlds.MountAndBlade.SkinVoiceManager;

namespace BannerRoyalMPServer.Extensions.Shout
{
    public class ShoutHandler : IHandlerRegister
    {
        public void Register(GameNetwork.NetworkMessageHandlerRegisterer reg)
        {
            reg.Register<StartShout>(MakeShout);
        }

        public bool MakeShout(NetworkCommunicator networkPeer, StartShout baseMessage)
        {
            var peer = baseMessage.Player;
            var shoutIndex = baseMessage.ShoutIndex;
            var voiceType = BannerRoyalShoutWheel.Shouts.FirstOrDefault(x => x.ShoutIndex == shoutIndex)?.VoiceType ?? "CustomShout";
            networkPeer.ControlledAgent.MakeVoice(new SkinVoiceType(voiceType), SkinVoiceManager.CombatVoiceNetworkPredictionType.OwnerPrediction);

            if (GameNetwork.IsMultiplayer)
            {
                GameNetwork.BeginBroadcastModuleEvent();
                GameNetwork.WriteMessage(new BarkAgent(networkPeer.ControlledAgent.Index, 1));
                GameNetwork.EndBroadcastModuleEvent(GameNetwork.EventBroadcastFlags.ExcludeOtherTeamPlayers, networkPeer);
            }

            return true;
        }
    }
}
