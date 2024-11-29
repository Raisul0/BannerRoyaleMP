using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Network.Messages;
using TaleWorlds.ObjectSystem;
using TaleWorlds.PlayerServices;

namespace BannerRoyalMPLib.NetworkMessages.FromServer
{
    [DefineGameNetworkMessageTypeForMod(GameNetworkMessageSendType.FromServer)]
    public sealed class SetBoundaryZone : GameNetworkMessage
    {
        public Vec3 Origin { get; private set; }
        public float Radius { get; private set; }
        public int LifeTime { get; private set; }
        public bool Visible { get; private set; }
        public SetBoundaryZone() { 
        }
        public SetBoundaryZone(Vec3 zoneOrigin, float zoneRadius, int zoneLifeTime, bool visible)
        {
            Origin = zoneOrigin;
            Radius = zoneRadius;
            LifeTime = zoneLifeTime;
            Visible = visible;
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
            bool bufferReadValid = true;
            Origin = ReadVec3FromPacket(CompressionBasic.PositionCompressionInfo, ref bufferReadValid);
            Radius = ReadFloatFromPacket(CompressionBasic.PositionCompressionInfo, ref bufferReadValid);
            LifeTime = ReadIntFromPacket(CompressionBasic.RoundTimeLimitCompressionInfo, ref bufferReadValid);
            Visible = ReadBoolFromPacket(ref bufferReadValid);
            return bufferReadValid;
        }

        protected override void OnWrite()
        {
            WriteVec3ToPacket(Origin, CompressionBasic.PositionCompressionInfo);
            WriteFloatToPacket(Radius, CompressionBasic.PositionCompressionInfo);
            WriteIntToPacket(LifeTime, CompressionBasic.RoundTimeLimitCompressionInfo);
            WriteBoolToPacket(Visible);
        }
    }
}
