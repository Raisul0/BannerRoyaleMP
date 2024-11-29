using System.Collections.Generic;
using System.Linq;
using BannerRoyalMPLib;
using BannerRoyalMPServer.Extensions;
using TaleWorlds.Core;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Multiplayer;
using TaleWorlds.MountAndBlade.Source.Missions;


namespace BannerRoyalMPServer
{
    public static class BannerRoyalMpMissionBehaviours
    {
        [MissionMethod]
        public static void OpenBannerRoyalMpServerBehaviours(string scene)
        {
            MissionState.OpenNew("BannerRoyalMP", new MissionInitializerRecord(scene),
                delegate(Mission missionController)
                {
                    return new MissionBehavior[]
                    {

                        new BannerRoyalMPLobbyComponent(),
                        new BannerRoyalMPBehavior(),
                        new BannerRoyalMPCommonBehavior(),
                        //new ShrinkingZoneBehavior(),

                        new MultiplayerTimerComponent(),
                        new SpawnComponent(new BannerRoyalMPSpawnFrameBehavior(),
                        new BannerRoyalMPSpawningBehavior()),
                        new AgentHumanAILogic(),
                        new MissionLobbyEquipmentNetworkComponent(),
                        new MissionHardBorderPlacer(),
                        new MissionBoundaryPlacer(),
                        new MissionBoundaryCrossingHandler(),
                        new MultiplayerPollComponent(),
                        new MultiplayerGameNotificationsComponent(),
                        new MissionOptionsComponent(),
                        new MissionScoreboardComponent(new FFAScoreboardData()),
                        new SpawnChestBehavior(),
                        new BoundaryZoneBehavior(),
                    };
                }, true, true);

        }
    }
}