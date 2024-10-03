using System.Collections.Generic;
using System.Linq;
using BannerRoyalMPLib;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Multiplayer;
using TaleWorlds.MountAndBlade.Source.Missions;


namespace BannerRoyalMPClient
{
    public static class BannerRoyalMpMissionBehaviours
    {
        [MissionMethod]
        public static void OpenBannerRoyalMpClientBehaviours(string scene)
        {
            MissionState.OpenNew("BannerRoyalMP", new MissionInitializerRecord(scene), delegate (Mission missionController)
            {
                return new MissionBehavior[]
                {
                    MissionLobbyComponent.CreateBehavior(),

                    // Custom behaviors
                    new BannerRoyalMPCommonBehavior(),
                    new BannerRoyalMPVisualSpawnComponent(),


                    // Native behaviors
                    new MultiplayerTimerComponent(),
                    new MultiplayerMissionAgentVisualSpawnComponent(),
                    new MissionLobbyEquipmentNetworkComponent(),
                    new MissionHardBorderPlacer(),
                    new MissionBoundaryPlacer(),
                    new MissionBoundaryCrossingHandler(),
                    new MultiplayerPollComponent(),
                    new MultiplayerGameNotificationsComponent(),
                    new MissionOptionsComponent(),
                    new MissionScoreboardComponent(new TDMScoreboardData()),

                };
            }, true, true);
        }

        

    }
}