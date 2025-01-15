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


            return true;
        }
    }
}
