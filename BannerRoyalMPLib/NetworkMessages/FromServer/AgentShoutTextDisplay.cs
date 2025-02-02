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
    public sealed class AgentShoutTextDisplay : GameNetworkMessage
    {
        public int AgentIndex { get; private set; }

        public string ShoutName { get; private set; }

        public AgentShoutTextDisplay(int agent, string shoutName)
        {
            AgentIndex = agent;
            ShoutName = shoutName;
        }

        public AgentShoutTextDisplay()
        {
        }

        protected override bool OnRead()
        {
            bool bufferReadValid = true;
            AgentIndex = ReadAgentIndexFromPacket(ref bufferReadValid);
            ShoutName = ReadStringFromPacket(ref bufferReadValid);
            return bufferReadValid;
        }

        protected override void OnWrite()
        {
            WriteAgentIndexToPacket(AgentIndex);
            WriteStringToPacket(ShoutName);
        }

        protected override MultiplayerMessageFilter OnGetLogFilter()
        {
            return MultiplayerMessageFilter.None;
        }

        protected override string OnGetLogFormat()
        {
            return "FromServer.AgentShoutTextDisplay agent-index: " + AgentIndex + ", ShoutName" + ShoutName;
        }
    }
}
