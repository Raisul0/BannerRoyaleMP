using NetworkMessages.FromServer;
using System.Threading;
using TaleWorlds.Core;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.DedicatedCustomServer;
using static TaleWorlds.MountAndBlade.MultiplayerOptions;

namespace BannerRoyalMPServer
{
    /// <summary>
    /// Start custom game modes. Inspired by mentalrob's ChatCommands.
    /// </summary>
    public class GameModeStarter
    {
        private static readonly GameModeStarter instance = new GameModeStarter();
        public static GameModeStarter Instance { get { return instance; } }

        public bool MissionIsRunning
        {
            get
            {
                return Mission.Current != null;
            }
        }
        public bool EndingCurrentMissionThenStartingNewMission;

        public void SyncMultiplayerOptionsToClients()
        {
            GameNetwork.BeginBroadcastModuleEvent();
            GameNetwork.WriteMessage(new MultiplayerOptionsInitial());
            GameNetwork.EndBroadcastModuleEvent(GameNetwork.EventBroadcastFlags.IncludeUnsynchronizedClients, null);
            GameNetwork.BeginBroadcastModuleEvent();
            GameNetwork.WriteMessage(new MultiplayerOptionsImmediate());
            GameNetwork.EndBroadcastModuleEvent(GameNetwork.EventBroadcastFlags.IncludeUnsynchronizedClients, null);
        }

        public void StartMission()
        {
            if (!EndingCurrentMissionThenStartingNewMission)
            {
                if (!MissionIsRunning)
                {
                    StartMissionOnly();
                    return;
                }
                EndMissionThenStartMission();
            }
        }

        private void EndMissionThenStartMission()
        {
            // Try to stop everyone from using objects to prevent crash
            foreach (MissionObject missionObj in Mission.Current?.MissionObjects)
            {
                if (missionObj is UsableMachine machine)
                {
                    machine.Disable();
                }
            }
            foreach (Agent agent in Mission.Current?.AllAgents)
            {
                agent.SetMortalityState(Agent.MortalityState.Invulnerable);
                //UsableMissionObject missionObject = agent.CurrentlyUsedGameObject;
                //if (missionObject != null)
                //{
                //    Log("agent using " + missionObject?.GameEntity?.Name, 0, Debug.DebugColor.Blue);
                //    agent.StopUsingGameObject();
                //    Log("agent now using " + agent.CurrentlyUsedGameObject?.GameEntity?.Name, 0, Debug.DebugColor.Blue);
                //    missionObject.SetDisabled();
                //}
                //agent.AIStateFlags = Agent.AIStateFlag.Alarmed;
                //agent.SetScriptedCombatFlags(Agent.AISpecialCombatModeFlags.None);
                //agent.DisableScriptedMovement();
                //agent.ClearTargetFrame();
                //agent.Detachment?.RemoveAgent(agent);
            }

            MultiplayerIntermissionVotingManager.Instance.IsCultureVoteEnabled = false;
            MultiplayerIntermissionVotingManager.Instance.IsMapVoteEnabled = false;
            EndingCurrentMissionThenStartingNewMission = true;
            DedicatedCustomServerSubModule.Instance.ServerSideIntermissionManager.EndMission();
        }


        public bool StartMissionOnly()
        {
            if (!MissionIsRunning)
            {
                DedicatedCustomServerSubModule.Instance.ServerSideIntermissionManager.StartMission();
                return true;
            }
            return false;
        }

        public void StartLobby(string map, string culture1, string culture2, int nbBots = -1)
        {
            StartMission();
        }

        public GameModeStarter()
        {
        }
    }


    
}
