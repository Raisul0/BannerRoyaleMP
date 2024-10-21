using TaleWorlds.Core;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Network.Messages;
using TaleWorlds.ObjectSystem;
using TaleWorlds.PlayerServices;

namespace BannerRoyalMPLib.NetworkMessages
{
    [DefineGameNetworkMessageTypeForMod(GameNetworkMessageSendType.FromClient)]
    public sealed class StartEquipItem : GameNetworkMessage
    {
        public ItemObject Item { get; set; }
        public NetworkCommunicator Player { get; set; }
        public StartEquipItem() { 
        }
        public StartEquipItem(ItemObject item, NetworkCommunicator player)
        {
            Item = item;
            Player = player;
        }
        protected override MultiplayerMessageFilter OnGetLogFilter()
        {
            return MultiplayerMessageFilter.General;
        }

        protected override string OnGetLogFormat()
        {
            return "Checking";
        }

        protected override bool OnRead()
        {
            bool result = true;
            this.Item = (ItemObject)ReadObjectReferenceFromPacket(MBObjectManager.Instance, CompressionBasic.GUIDCompressionInfo, ref result);
            this.Player = ReadNetworkPeerReferenceFromPacket(ref result);
            return result;
        }

        protected override void OnWrite()
        {
            WriteObjectReferenceToPacket(this.Item, CompressionBasic.GUIDCompressionInfo);
            WriteNetworkPeerReferenceToPacket(this.Player);
        }
    }
}
