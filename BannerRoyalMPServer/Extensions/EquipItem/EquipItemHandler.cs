using BannerRoyalMPLib;
using BannerRoyalMPLib.NetworkMessages.FromClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.Core;
using TaleWorlds.MountAndBlade;

namespace BannerRoyalMPServer.Extensions.EquipItem
{
    public class EquipItemHandler : IHandlerRegister
    {
        public void Register(GameNetwork.NetworkMessageHandlerRegisterer reg)
        {
            reg.Register<StartEquipItem>(EquipSelectedItem);
        }

        public bool EquipSelectedItem(NetworkCommunicator networkPeer, StartEquipItem baseMessage)
        {
            var peer = baseMessage.Player;
            var item = baseMessage.Item;

            var currentEquipment = peer.ControlledAgent.SpawnEquipment;
            var index = ViewModelLib.GetItemEquipmentIndex(item);
            currentEquipment[index] = new EquipmentElement(item);
            peer.ControlledAgent.UpdateSpawnEquipmentAndRefreshVisuals(currentEquipment);

            return true;
        }
    }
}
