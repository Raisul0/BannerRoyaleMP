using BannerRoyalMPLib;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade;

namespace BannerRoyalMPClient
{
    public class SubModule : MBSubModuleBase
    {
        protected override void InitializeGameStarter(Game game, IGameStarter starterObject)
        {
            base.InitializeGameStarter(game, starterObject);
        }
        public override void OnMultiplayerGameStart(Game game, object starterObject)
        {
            InformationManager.DisplayMessage(new InformationMessage("** Banner Royal, Multiplayer Game Start Loading..."));
            TaleWorlds.Library.Debug.Print("** Banner Royal, Multiplayer Game Start Loading...");

            BannerRoyalMPGameMode.OnStartMultiplayerGame += BannerRoyalMpMissionBehaviours.OpenBannerRoyalMpClientBehaviours;
            TaleWorlds.MountAndBlade.Module.CurrentModule.AddMultiplayerGameMode(new BannerRoyalMPGameMode("BannerRoyalMP"));
        }
        protected override void OnSubModuleLoad()
        {
            base.OnSubModuleLoad();
        }

        //public override void OnBeforeMissionBehaviorInitialize(Mission mission)
        //{

        //    AddCommonBehaviors(mission);

        //}

        //public void AddCommonBehaviors(Mission mission)
        //{
        //    mission.AddMissionBehavior(new ClientAutoHandler());
        //    mission.AddMissionBehavior(new SpawnChestBehavior());
        //}
    }
}