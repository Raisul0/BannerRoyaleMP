using BannerRoyalMPLib;
using BannerRoyalMPLib.Globals;
using BannerRoyalMPLib.NetworkMessages.FromServer;
using NetworkMessages.FromServer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade;
using static TaleWorlds.MountAndBlade.SkinVoiceManager;

namespace BannerRoyalMPClient.Extensions.Shout
{
    public class AgentShoutTextDisplayHandler : IHandlerRegister
    {
        public void Register(GameNetwork.NetworkMessageHandlerRegisterer reg)
        {
            reg.Register<AgentShoutTextDisplay>(ShowShoutText);
        }

        public void ShowShoutText(AgentShoutTextDisplay baseMessage)
        {
            Agent agentFromIndex = Mission.MissionNetworkHelper.GetAgentFromIndex(baseMessage.AgentIndex, false);

            GameTexts.SetVariable("LEFT", agentFromIndex.Name);
            GameTexts.SetVariable("RIGHT", baseMessage.ShoutName);
            InformationManager.DisplayMessage(new InformationMessage(GameTexts.FindText("str_LEFT_colon_RIGHT_wSpaceAfterColon", null).ToString(), Color.White, "Bark"));
        }
       
    }
}
