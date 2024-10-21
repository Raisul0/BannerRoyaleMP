using System.Diagnostics;
using BannerRoyalMPLib;
using TaleWorlds.Core;
using TaleWorlds.MountAndBlade;
using Debug = TaleWorlds.Library.Debug;

namespace BannerRoyalMPServer;

public class SubModule : MBSubModuleBase
{
    protected override void OnSubModuleLoad()
    {
        base.OnSubModuleLoad();
        Debug.Print("** BannerRoyalMP, OnSubModuleLoad BY RAISUL **", 0, Debug.DebugColor.Red);
        Console.WriteLine("hello_");
    }
    
    protected override void InitializeGameStarter(Game game, IGameStarter starterObject)
    {
        Debug.Print("** BannerRoyalMP, InitializeGameStarter BY RAISUL **", 0, Debug.DebugColor.Red);
        base.InitializeGameStarter(game, starterObject);
    }

    public override void OnMultiplayerGameStart(Game game, object starterObject)
    {
        Debug.Print("** BannerRoyalMP, OnMultiplayerGameStart **");
        BannerRoyalMPGameMode.OnStartMultiplayerGame += BannerRoyalMpMissionBehaviours.OpenBannerRoyalMpServerBehaviours;
        TaleWorlds.MountAndBlade.Module.CurrentModule.AddMultiplayerGameMode(new BannerRoyalMPGameMode("BannerRoyalMP"));

    }
    //public override void OnBeforeMissionBehaviorInitialize(Mission mission)
    //{

    //    AddCommonBehaviors(mission);

    //}
    //private void AddCommonBehaviors(Mission mission)
    //{
    //    mission.AddMissionBehavior(new ServerAutoHandler());
    //}
}