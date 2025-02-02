using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.MountAndBlade.Network.Messages;
using TaleWorlds.MountAndBlade;

namespace BannerRoyalMPLib.NetworkMessages.FromServer
{
    [DefineGameNetworkMessageTypeForMod(GameNetworkMessageSendType.FromServer)]
    public sealed class AgentShout : GameNetworkMessage
    {
        public string VoiceType { get; private set; }
        public int AgentIndex { get; private set; }

        public AgentShout(string voiceType, int agentIndex)
        {
            this.VoiceType = voiceType;
            AgentIndex = agentIndex;
        }

        // Token: 0x06000468 RID: 1128 RVA: 0x000084FD File Offset: 0x000066FD
        public AgentShout()
        {
        }

        // Token: 0x06000469 RID: 1129 RVA: 0x00008508 File Offset: 0x00006708
        protected override bool OnRead()
        {
            bool result = true;
            this.VoiceType = ReadStringFromPacket(ref result);
            this.AgentIndex = ReadAgentIndexFromPacket(ref result);
            return result;
        }

        // Token: 0x0600046A RID: 1130 RVA: 0x00008537 File Offset: 0x00006737
        protected override void OnWrite()
        {
            WriteAgentIndexToPacket(this.AgentIndex);
            WriteStringToPacket(this.VoiceType);
        }

        // Token: 0x0600046B RID: 1131 RVA: 0x00008554 File Offset: 0x00006754
        protected override MultiplayerMessageFilter OnGetLogFilter()
        {
            return MultiplayerMessageFilter.None;
        }

        // Token: 0x0600046C RID: 1132 RVA: 0x00008558 File Offset: 0x00006758
        protected override string OnGetLogFormat()
        {
            return "FromServer.AgentShout";
        }
    }
}
