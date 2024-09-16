using System.Collections.Generic;
using System.Linq;
using Alliance.Server.GameModes.BattleRoyale.Behaviors;
using Alliance.Server.Patch.Behaviors;
using BannerRoyalMPLib;
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
                        new MissionScoreboardComponent(new TDMScoreboardData())
                    };
                }, true, true);

        }
    }
}