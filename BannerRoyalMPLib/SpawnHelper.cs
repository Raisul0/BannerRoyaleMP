using System;
using System.Collections.Generic;
using System.Linq;
using BannerRoyalMPLib.Globals;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Diamond;
using static TaleWorlds.MountAndBlade.MPPerkObject;
using Debug = TaleWorlds.Library.Debug;

namespace BannerRoyalMPLib
{
	public static class SpawnHelper
	{
		public enum Difficulty
		{
			PlayerChoice = -1,
			Easy = 0,
			Normal = 1,
			Hard = 2,
			VeryHard = 3,
			Bannerlord = 4
		}

		public static int TotalBots = 0;
		public const int MaxBotsPerSpawn = 200;

		static SpawnComponent SpawnComponent => Mission.Current.GetMissionBehavior<SpawnComponent>();
		static MissionLobbyComponent MissionLobbyComponent => Mission.Current.GetMissionBehavior<MissionLobbyComponent>();
		
		public static void SpawnPlayer(NetworkCommunicator networkPeer, MPOnSpawnPerkHandler onSpawnPerkHandler, BasicCharacterObject character, MatrixFrame? origin = null, int selectedFormation = -1, IEnumerable<(EquipmentIndex, EquipmentElement)> alternativeEquipment = null, Agent.MortalityState mortalityState = Agent.MortalityState.Mortal, BasicCultureObject customCulture = null)
		{
			try
			{
				MissionPeer component = networkPeer.GetComponent<MissionPeer>();
				SpawnComponent spawnComponent = Mission.Current.GetMissionBehavior<SpawnComponent>();
				MissionLobbyComponent missionLobbyComponent = Mission.Current.GetMissionBehavior<MissionLobbyComponent>();
				MissionMultiplayerGameModeBase gameMode = Mission.Current.GetMissionBehavior<MissionMultiplayerGameModeBase>();

				Formation form;
				if (selectedFormation > -1)
				{
					form = component.Team.GetFormation((FormationClass)selectedFormation);
					if (form == null) form = new Formation(component.Team, selectedFormation);
				}
				else
				{
					form = component.Team.GetFormation(character.GetFormationClass());
					if (form == null) form = new Formation(component.Team, character.DefaultFormationGroup);
				}

				BasicCultureObject culture = customCulture != null ? customCulture : component.Culture;
				uint color = component.Team == Mission.Current.AttackerTeam ? culture.Color : culture.ClothAlternativeColor;
				uint color2 = component.Team == Mission.Current.AttackerTeam ? culture.Color2 : culture.ClothAlternativeColor2;
				uint color3 = component.Team == Mission.Current.AttackerTeam ? culture.BackgroundColor1 : culture.BackgroundColor2;
				uint color4 = component.Team == Mission.Current.AttackerTeam ? culture.ForegroundColor1 : culture.ForegroundColor2;

				Banner banner = component.Team == Mission.Current.AttackerTeam ? Mission.Current.AttackerTeam.Banner : Mission.Current.DefenderTeam.Banner;
				int randomSeed = MBRandom.RandomInt();
				AgentBuildData agentBuildData = new AgentBuildData(character)
					.VisualsIndex(randomSeed)
					.Team(component.Team)
					.TroopOrigin(new BasicBattleAgentOrigin(character))
					.Formation(form)
					.ClothingColor1(component.Team == Mission.Current.AttackerTeam ? culture.Color : culture.ClothAlternativeColor)
					.ClothingColor2(component.Team == Mission.Current.AttackerTeam ? culture.Color2 : culture.ClothAlternativeColor2)
					.Banner(component.Team == Mission.Current.AttackerTeam ? Mission.Current.AttackerTeam.Banner : Mission.Current.DefenderTeam.Banner);
				agentBuildData.MissionPeer(component);
				bool randomEquipement = true;
				Equipment equipment = randomEquipement ? Equipment.GetRandomEquipmentElements(character, randomEquipmentModifier: false, isCivilianEquipment: false, MBRandom.RandomInt()) : character.Equipment.Clone();
				IEnumerable<(EquipmentIndex, EquipmentElement)> perkAlternativeEquipment = onSpawnPerkHandler?.GetAlternativeEquipments(isPlayer: true);
				if (perkAlternativeEquipment != null)
				{
					foreach ((EquipmentIndex, EquipmentElement) item in perkAlternativeEquipment)
					{
						equipment[item.Item1] = item.Item2;
					}
				}

				if (alternativeEquipment != null)
				{
					foreach ((EquipmentIndex, EquipmentElement) item in alternativeEquipment)
					{
						if (item.Item1 == EquipmentIndex.Horse &&
							!agentBuildData.AgentMonster.Flags.HasFlag(AgentFlag.CanRide)) continue;
						equipment[item.Item1] = item.Item2;
					}
				}

				equipment[EquipmentIndex.Head] = CustomAgent.Head;
                equipment[EquipmentIndex.Body] = CustomAgent.Body;
                equipment[EquipmentIndex.Leg] = CustomAgent.Leg;
                equipment[EquipmentIndex.Gloves] = CustomAgent.Gloves;
                equipment[EquipmentIndex.Cape] = CustomAgent.Cape;

                agentBuildData.Equipment(equipment);

				// Use player custom bodyproperties only if allowed
				agentBuildData.EquipmentSeed(missionLobbyComponent.GetRandomFaceSeedForCharacter(character, agentBuildData.AgentVisualsIndex));
				agentBuildData.BodyProperties(BodyProperties.GetRandomBodyProperties(agentBuildData.AgentRace, agentBuildData.AgentIsFemale, character.GetBodyProperties(agentBuildData.AgentOverridenSpawnEquipment), character.GetBodyPropertiesMax(), (int)agentBuildData.AgentOverridenSpawnEquipment.HairCoverType, agentBuildData.AgentEquipmentSeed, character.HairTags, character.BeardTags, character.TattooTags));
				agentBuildData.Age((int)agentBuildData.AgentBodyProperties.Age);
				agentBuildData.IsFemale(character.IsFemale);

				if (component.ControlledFormation != null && component.ControlledFormation.Banner == null)
				{
					component.ControlledFormation.Banner = banner;
				}

				MatrixFrame spawnFrame = (MatrixFrame)(origin != null ? origin : spawnComponent.GetSpawnFrame(component.Team, character.HasMount(), component.SpawnCountThisRound == 0));

				agentBuildData.InitialPosition(spawnFrame.origin);
				Vec2 vec = spawnFrame.rotation.f.AsVec2;
				vec = vec.Normalized();
				agentBuildData.InitialDirection(vec);
				Agent agent = Mission.Current.SpawnAgent(agentBuildData, spawnFromAgentVisuals: true);
				agent.AddComponent(new MPPerksAgentComponent(agent));
				agent.MountAgent?.UpdateAgentProperties();
				float bonusHealth = onSpawnPerkHandler?.GetHitpoints(true) ?? 0f;
				// Additional health for officers
				agent.HealthLimit += bonusHealth;
				agent.Health = agent.HealthLimit;

				agent.WieldInitialWeapons();

				if (mortalityState != Agent.MortalityState.Mortal)
				{
					agent.SetMortalityState(mortalityState);
					agent.MountAgent?.SetMortalityState(mortalityState);
				}


				if (agent.Formation != null)
				{
					MissionPeer temp = agent.MissionPeer;
					agent.MissionPeer = null;
					agent.Formation.OnUndetachableNonPlayerUnitAdded(agent);
					agent.MissionPeer = temp;
				}

				component.SpawnCountThisRound++;

            }
			catch (Exception ex)
			{
			}
		}

		public static void SpawnPlayerPreview(NetworkCommunicator player, BasicCultureObject culture)
		{
			try
			{
				SpawnComponent spawnComponent = Mission.Current.GetMissionBehavior<SpawnComponent>();
				MissionMultiplayerGameModeBase gameMode = Mission.Current.GetMissionBehavior<MissionMultiplayerGameModeBase>();
				MissionPeer peer = player.GetComponent<MissionPeer>();
				if (peer != null && peer.ControlledAgent == null && !peer.HasSpawnedAgentVisuals && peer.Team != null && peer.Team != Mission.Current.SpectatorTeam && peer.TeamInitialPerkInfoReady && peer.SpawnTimer.Check(Mission.Current.CurrentTime))
				{
					int num = peer.SelectedTroopIndex;
					IEnumerable<MultiplayerClassDivisions.MPHeroClass> mpheroClasses = MultiplayerClassDivisions.GetMPHeroClasses(culture);
					MultiplayerClassDivisions.MPHeroClass mpheroClass = num < 0 ? null : mpheroClasses.ElementAt(num);
					if (mpheroClass == null && num < 0)
					{
						mpheroClass = mpheroClasses.First();
						num = 0;
					}

					BasicCharacterObject character = mpheroClass.TroopCharacter;

					// If player is officer, assign a HeroCharacter instea

					Equipment equipment = character.Equipment.Clone(false);
					MPPerkObject.MPOnSpawnPerkHandler onSpawnPerkHandler = MPPerkObject.GetOnSpawnPerkHandler(peer);
					IEnumerable<ValueTuple<EquipmentIndex, EquipmentElement>> enumerable = onSpawnPerkHandler?.GetAlternativeEquipments(true);
					if (enumerable != null)
					{
						foreach (ValueTuple<EquipmentIndex, EquipmentElement> valueTuple in enumerable)
						{
							equipment[valueTuple.Item1] = valueTuple.Item2;
						}
					}
					MatrixFrame matrixFrame;
					matrixFrame = spawnComponent.GetSpawnFrame(peer.Team, character.Equipment.Horse.Item != null, false);
					AgentBuildData agentBuildData = new AgentBuildData(character).MissionPeer(peer).Equipment(equipment).Team(peer.Team)
						.TroopOrigin(new BasicBattleAgentOrigin(character))
						.InitialPosition(matrixFrame.origin);
					Vec2 vec = matrixFrame.rotation.f.AsVec2;
					vec = vec.Normalized();
					AgentBuildData agentBuildData2 = agentBuildData.InitialDirection(vec);

					// If custom body is allowed, retrieve player's gender and body properti

					agentBuildData2.VisualsIndex(0)
						.ClothingColor1(peer.Team == Mission.Current.AttackerTeam ? culture.Color : culture.ClothAlternativeColor)
						.ClothingColor2(peer.Team == Mission.Current.AttackerTeam ? culture.Color2 : culture.ClothAlternativeColor2);
					gameMode.HandleAgentVisualSpawning(player, agentBuildData2, 0, false);

					
				}
			}
			catch (Exception ex)
			{

			}
		}

		public static BodyProperties GetBodyProperties(MissionPeer missionPeer, BasicCultureObject cultureLimit)
		{
			NetworkCommunicator networkPeer = missionPeer.GetNetworkPeer();
			MissionLobbyComponent missionLobbyComponent = Mission.Current.GetMissionBehavior<MissionLobbyComponent>();

			if (networkPeer != null)
			{
				return networkPeer.PlayerConnectionInfo.GetParameter<PlayerData>("PlayerData").BodyProperties;
			}

			SpawnComponent spawnComponent = Mission.Current.GetMissionBehavior<SpawnComponent>();
			Debug.FailedAssert("networkCommunicator != null", "C:\\Develop\\MB3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade\\Missions\\Multiplayer\\SpawnBehaviors\\SpawningBehaviors\\SpawningBehaviorBase.cs", "GetBodyProperties", 366);
			Team team = missionPeer.Team;
			BasicCharacterObject troopCharacter = MultiplayerClassDivisions.GetMPHeroClasses(cultureLimit).ToList().GetRandomElement()
				.TroopCharacter;
			MatrixFrame spawnFrame = spawnComponent.GetSpawnFrame(team, troopCharacter.HasMount(), isInitialSpawn: true);
			AgentBuildData agentBuildData = new AgentBuildData(troopCharacter).Team(team).InitialPosition(in spawnFrame.origin);
			Vec2 direction = spawnFrame.rotation.f.AsVec2.Normalized();
			AgentBuildData agentBuildData2 = agentBuildData.InitialDirection(in direction).TroopOrigin(new BasicBattleAgentOrigin(troopCharacter)).EquipmentSeed(missionLobbyComponent.GetRandomFaceSeedForCharacter(troopCharacter))
				.ClothingColor1(team.Side == BattleSideEnum.Attacker ? cultureLimit.Color : cultureLimit.ClothAlternativeColor)
				.ClothingColor2(team.Side == BattleSideEnum.Attacker ? cultureLimit.Color2 : cultureLimit.ClothAlternativeColor2)
				.IsFemale(troopCharacter.IsFemale);
			agentBuildData2.Equipment(Equipment.GetRandomEquipmentElements(troopCharacter, true, isCivilianEquipment: false, agentBuildData2.AgentEquipmentSeed));
			agentBuildData2.BodyProperties(BodyProperties.GetRandomBodyProperties(agentBuildData2.AgentRace, agentBuildData2.AgentIsFemale, troopCharacter.GetBodyPropertiesMin(), troopCharacter.GetBodyPropertiesMax(), (int)agentBuildData2.AgentOverridenSpawnEquipment.HairCoverType, agentBuildData2.AgentEquipmentSeed, troopCharacter.HairTags, troopCharacter.BeardTags, troopCharacter.TattooTags));
			return agentBuildData2.AgentBodyProperties;
		}
		
		// Return a linear troop cost from minCost to MaxCost, depending on TroopMultiplie

		/// <summary>
		/// Get total troop cost from a character, troop count and difficulty

		/// <summary>
		/// Get corresponding perks from a character and a list of perks indices.
		/// </summary>
		public static List<IReadOnlyPerkObject> GetPerks(BasicCharacterObject troop, List<int> indices)
		{
			MultiplayerClassDivisions.MPHeroClass heroClass = MultiplayerClassDivisions.GetMPHeroClassForCharacter(troop);
			List<List<IReadOnlyPerkObject>> allPerks = MultiplayerClassDivisions.GetAllPerksForHeroClass(heroClass);
			List<IReadOnlyPerkObject> selectedPerks = new List<IReadOnlyPerkObject>();
			int i = 0;
			foreach (List<IReadOnlyPerkObject> perkList in allPerks)
			{
				IReadOnlyPerkObject selectedPerk = perkList.ElementAtOrValue(indices.ElementAtOrValue(i, 0), null);
				if (selectedPerk != null)
				{
					selectedPerks.Add(selectedPerk);
				}
				i++;
			}
			return selectedPerks;
		}

		public static float DifficultyMultiplierFromLevel(int difficultyLevel)
		{
			switch (difficultyLevel)
			{
				case 0: return 0.5f;
				case 1: return 1f;
				case 2: return 1.5f;
				case 3: return 2f;
				case 4: return 2.5f;
				default: return 1f;
			}
		}

		public static float DifficultyMultiplierFromLevel(Difficulty difficultyLevel)
		{
			return DifficultyMultiplierFromLevel((int)difficultyLevel);
		}

		public static float DifficultyMultiplierFromLevel(string difficultyLevel)
		{
			if (Enum.TryParse(difficultyLevel, out Difficulty difficulty))
			{
				return DifficultyMultiplierFromLevel(difficulty);
			}
			return DifficultyMultiplierFromLevel(Difficulty.Normal);
		}

		public static int DifficultyLevelFromString(string difficultyString)
		{
			if (Enum.TryParse(difficultyString, out Difficulty difficulty))
			{
				return (int)difficulty;
			}
			return (int)Difficulty.Normal;
		}

		public static Difficulty DifficultyFromMultiplier(float multiplier)
		{
			switch (multiplier)
			{
				case 0.5f: return Difficulty.Easy;
				case 1f: return Difficulty.Normal;
				case 1.5f: return Difficulty.Hard;
				case 2f: return Difficulty.VeryHard;
				case 2.5f: return Difficulty.Bannerlord;
				default: return Difficulty.Normal;
			}
		}
	}
}
