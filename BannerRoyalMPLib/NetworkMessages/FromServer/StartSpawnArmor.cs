using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Network.Messages;
using TaleWorlds.ObjectSystem;
using TaleWorlds.PlayerServices;

namespace BannerRoyalMPLib.NetworkMessages.FromServer
{
    [DefineGameNetworkMessageTypeForMod(GameNetworkMessageSendType.FromServer)]
    public sealed class StartSpawnArmor : GameNetworkMessage
    {
        public ItemObject Armor { get; set; }
        public MatrixFrame SpawnLocation { get; set; }

        public StartSpawnArmor()
        {

        }
        public StartSpawnArmor(ItemObject armor,MatrixFrame spawnLocation) {
            Armor = armor;
            SpawnLocation = spawnLocation;
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
            this.SpawnLocation = ReadMatrixFrameFromPacket(ref result);
            return result;
        }

        protected override void OnWrite()
        {
            WriteObjectReferenceToPacket(this.Armor, CompressionBasic.GUIDCompressionInfo);
            WriteMatrixFrameToPacket(this.SpawnLocation);
        }
    }
}
