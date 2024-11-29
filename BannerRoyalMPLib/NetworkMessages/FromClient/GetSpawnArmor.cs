using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Network.Messages;
using TaleWorlds.ObjectSystem;
using TaleWorlds.PlayerServices;

namespace BannerRoyalMPLib.NetworkMessages.FromClient
{
    [DefineGameNetworkMessageTypeForMod(GameNetworkMessageSendType.FromClient)]
    public sealed class GetSpawnArmor : GameNetworkMessage
    {
        public ItemObject Armor { get; set; }
        public MatrixFrame Location { get; set; }
        public GetSpawnArmor() { 
        }
        public GetSpawnArmor(ItemObject armor,MatrixFrame location)
        {
            Armor = armor;
            Location = location;
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
            this.Armor = (ItemObject)ReadObjectReferenceFromPacket(MBObjectManager.Instance, CompressionBasic.GUIDCompressionInfo, ref result);
            this.Location = ReadMatrixFrameFromPacket(ref result);
            return result;
        }

        protected override void OnWrite()
        {
            WriteObjectReferenceToPacket(this.Armor, CompressionBasic.GUIDCompressionInfo);
            WriteMatrixFrameToPacket(this.Location);
        }
    }
}
