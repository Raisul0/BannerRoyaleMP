
using BannerRoyalMPServer;
using NetworkMessages.FromServer;
using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade;
using TaleWorlds.ObjectSystem;
using MathF = TaleWorlds.Library.MathF;

namespace Alliance.Server.GameModes.BattleRoyale.Behaviors
{
    public class BannerRoyalMPBehavior : MissionMultiplayerGameModeBase
    {
        BannerRoyalMPSpawningBehavior spawnBehavior;

        const float DAMAGE_TICK_DELAY = 2f;
        const float ZONE_RADIUS_LEEWAY = 4f;
        const float WARNING_INTERVAL = 10f;

        private Dictionary<MissionPeer, float> _playerWarningTimestamps = new Dictionary<MissionPeer, float>();
        private bool _zoneInitialized;
        private bool _gameEnded;
        private bool _spawnStarted;
        private float _damageTick;

        public override bool IsGameModeHidingAllAgentVisuals
        {
            get
            {
                return true;
            }
        }

        public override bool IsGameModeUsingOpposingTeams
        {
            get
            {
                return false;
            }
        }

        public override MultiplayerGameType GetMissionType()
        {
            return MultiplayerGameType.FreeForAll;
        }

        public override void OnBehaviorInitialize()
        {
            base.OnBehaviorInitialize();
            spawnBehavior = (BannerRoyalMPSpawningBehavior)SpawnComponent.SpawningBehavior;
        }

        public override void AfterStart()
        {
            BasicCultureObject @object = MBObjectManager.Instance.GetObject<BasicCultureObject>(MultiplayerOptions.OptionType.CultureTeam1.GetStrValue(MultiplayerOptions.MultiplayerOptionsAccessMode.CurrentMapOptions));
            Banner banner = new Banner(@object.BannerKey, @object.BackgroundColor1, @object.ForegroundColor1);
            Team team = Mission.Teams.Add(BattleSideEnum.Attacker, @object.BackgroundColor1, @object.ForegroundColor1, banner, isPlayerGeneral: false, isPlayerSergeant: true, true);
            team.SetIsEnemyOf(team, true);
        }

        protected override void HandleNewClientAfterSynchronized(NetworkCommunicator networkPeer)
        {
            MissionPeer component = networkPeer.GetComponent<MissionPeer>();
            component.Team = Mission.AttackerTeam;
            component.Culture = MBObjectManager.Instance.GetObject<BasicCultureObject>(MultiplayerOptions.OptionType.CultureTeam1.GetStrValue(MultiplayerOptions.MultiplayerOptionsAccessMode.CurrentMapOptions));
        }

        public override void OnMissionTick(float dt)
        {
            base.OnMissionTick(dt);
            if (!_spawnStarted && EnoughPlayersJoined())
            {
                _spawnStarted = true;
                spawnBehavior.RequestStartSpawnSession();
              
            }
            if (_spawnStarted && spawnBehavior.SpawnEnded && !_gameEnded)
            {
                if (!_zoneInitialized)
                {
                    InitShrinkingZone();
                    _zoneInitialized = true;
                }
                DamageAgentsOutsideZone(dt);
                CheckForGameEnd();
            }
        }

        private void CheckForGameEnd()
        {
            List<Agent> remainingAgents = Mission.Current.Agents.FindAll(agent => agent.IsHuman && agent.Health > 0);
            if (remainingAgents.Count <= 1)
            {
                if (remainingAgents.Count == 1)
                {
                    Agent winner = remainingAgents.FirstOrDefault();
                    string winMessage = $"{winner.Name} is the last surviving participant. GG !";
                }
                else
                {
                    string loseMessage = $"Nobody survived... How is that even possible ?";
                }
                GameModeStarter.Instance.StartLobby("Lobby", MultiplayerOptions.OptionType.CultureTeam1.GetStrValue(), MultiplayerOptions.OptionType.CultureTeam2.GetStrValue());
                _gameEnded = true;
            }
        }

        private void DamageAgentsOutsideZone(float dt)
        {
            _damageTick += dt;
            if (_damageTick >= DAMAGE_TICK_DELAY)
            {
                _damageTick = 0;

                List<Agent> agents = Mission.Current.Agents.FindAll(agent => agent.IsHuman && agent.Health > 0);
                foreach (Agent agent in agents)
                {
                }
            }
        }

        private void DamageAgent(Agent agent)
        {
            MissionPeer peer = agent.MissionPeer;
            if (peer != null)
            {
                // Check if enough time has passed since the last warning for this player
                bool warningAlreadySentRecently = _playerWarningTimestamps.ContainsKey(peer) && Mission.Current.CurrentTime - _playerWarningTimestamps[peer] < WARNING_INTERVAL;

                if (!warningAlreadySentRecently)
                {
                    GameNetwork.BeginModuleEventAsServer(peer.GetNetworkPeer());
                    GameNetwork.EndModuleEventAsServer();
                    _playerWarningTimestamps[peer] = Mission.Current.CurrentTime; // Update last warning time
                }
            }

            Blow blow = new Blow(agent.Index);
            blow.DamageType = DamageTypes.Blunt;
            blow.BoneIndex = agent.Monster.HeadLookDirectionBoneIndex;
            blow.GlobalPosition = agent.Position;
            blow.GlobalPosition.z = blow.GlobalPosition.z + agent.GetEyeGlobalHeight();
            blow.BaseMagnitude = 10f;
            blow.WeaponRecord.FillAsMeleeBlow(null, null, -1, -1);
            blow.InflictedDamage = 10;
            blow.SwingDirection = agent.LookDirection;
            MatrixFrame frame = agent.Frame;
            blow.SwingDirection = frame.rotation.TransformToParent(new Vec3(-1f, 0f, 0f, -1f));
            blow.SwingDirection.Normalize();
            blow.Direction = blow.SwingDirection;
            blow.DamageCalculated = true;
            sbyte mainHandItemBoneIndex = agent.Monster.MainHandItemBoneIndex;
            AttackCollisionData attackCollisionDataForDebugPurpose = AttackCollisionData.GetAttackCollisionDataForDebugPurpose(false, false, false, true, false, false, false, false, false, false, false, false, CombatCollisionResult.StrikeAgent, -1, 0, 2, blow.BoneIndex, BoneBodyPartType.Head, mainHandItemBoneIndex, Agent.UsageDirection.AttackLeft, -1, CombatHitResultFlags.NormalHit, 0.5f, 1f, 0f, 0f, 0f, 0f, 0f, 0f, Vec3.Up, blow.Direction, blow.GlobalPosition, Vec3.Zero, Vec3.Zero, agent.Velocity, Vec3.Up);
            agent.RegisterBlow(blow, attackCollisionDataForDebugPurpose);
        }

        private void InitShrinkingZone()
        {
            Vec3 zoneOrigin;
            GameEntity lastStand = Mission.Current.Scene.FindEntityWithTag("last_stand");
            if (lastStand != null)
            {
                zoneOrigin = lastStand.GlobalPosition;
            }
            else
            {
                MatrixFrame randomSpawnLocation = SpawnComponent.SpawnFrameBehavior.GetSpawnFrame(Mission.AttackerTeam, false, true);
                zoneOrigin = randomSpawnLocation.origin;
            }
            float zoneRadius = 10f;

            // Find the farthest agent
            List<Agent> agents = Mission.Current.Agents.FindAll(agent => agent.IsHuman && agent.Health > 0);
            foreach (Agent agent in agents)
            {
                float distAgentToZone = agent.Position.Distance(zoneOrigin) + 10f;
                if (distAgentToZone > zoneRadius) zoneRadius = distAgentToZone;
            }

        }

        private bool EnoughPlayersJoined()
        {
            int minPlayersForStart = (int)MathF.Clamp((float)Math.Round(GameNetwork.NetworkPeers.Count / 1.1), 1, Math.Max(GameNetwork.NetworkPeers.Count - 1, 1));
            int playersReady = 0;
            foreach (ICommunicator peer in GameNetwork.NetworkPeers)
            {
                if (peer.IsSynchronized) playersReady++;
            }
            string log = "Waiting for players to load... (" + playersReady + "/" + minPlayersForStart + ")";
            GameNetwork.BeginBroadcastModuleEvent();
            GameNetwork.WriteMessage(new ServerMessage(log));
            GameNetwork.EndBroadcastModuleEvent(GameNetwork.EventBroadcastFlags.None);
            return playersReady >= minPlayersForStart;
        }

        public BannerRoyalMPBehavior()
        {
        }
    }
}
