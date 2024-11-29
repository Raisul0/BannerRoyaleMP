using BannerRoyalMPLib;
using BannerRoyalMPLib.NetworkMessages;
using BannerRoyalMPLib.NetworkMessages.FromClient;
using BannerRoyalMPLib.NetworkMessages.FromServer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.Core;
using TaleWorlds.MountAndBlade;

namespace BannerRoyalMPServer.Extensions.DropItem
{
    public class DropItemHandler : IHandlerRegister
    {
        public void Register(GameNetwork.NetworkMessageHandlerRegisterer reg)
        {
            reg.Register<GetSpawnArmor>(SpawnDroppedItem);
        }

        public bool SpawnDroppedItem(NetworkCommunicator networkPeer, GetSpawnArmor baseMessage)
        {
            var armor = baseMessage.Armor;
            var spawnLocation = baseMessage.Location;

            GameNetwork.BeginModuleEventAsServer(networkPeer);
            GameNetwork.WriteMessage(new StartSpawnArmor(armor,spawnLocation));
            GameNetwork.EndModuleEventAsServer();

            return true;
        }
    }
}
