using TaleWorlds.Core;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Network.Messages;
using TaleWorlds.ObjectSystem;
using TaleWorlds.PlayerServices;

namespace BannerRoyalMPLib.NetworkMessages.FromClient
{
    [DefineGameNetworkMessageTypeForMod(GameNetworkMessageSendType.FromClient)]
    public sealed class StartTaunt : GameNetworkMessage
    {
        public int TauntIndex { get; set; }
        public NetworkCommunicator Player { get; set; }
        public StartTaunt() { 
        }
        public StartTaunt(int tauntIndex, NetworkCommunicator player)
        {
            TauntIndex = tauntIndex;
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
            this.TauntIndex = ReadIntFromPacket(new CompressionInfo.Integer(-1, 5000, true), ref result);
            this.Player = ReadNetworkPeerReferenceFromPacket(ref result);
            return result;
        }

        protected override void OnWrite()
        {
            WriteIntToPacket(TauntIndex, new CompressionInfo.Integer(-1, 5000, true));
            WriteNetworkPeerReferenceToPacket(this.Player);
        }
    }
}
