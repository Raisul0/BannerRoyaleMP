using BannerRoyalMPLib;
using BannerRoyalMPLib.NetworkMessages;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.CampaignSystem.ViewModelCollection.Encyclopedia;
using TaleWorlds.Engine;
using TaleWorlds.Engine.GauntletUI;
using TaleWorlds.GauntletUI.Data;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.View;
using TaleWorlds.MountAndBlade.View.MissionViews;
using TaleWorlds.TwoDimension;
using static TaleWorlds.MountAndBlade.SkinVoiceManager;

namespace BannerRoyalMPClient
{
    [DefaultView]
    public class BannerRoyalInventoryView : MissionView
    {
        GauntletLayer _gauntletLayer;
        IGauntletMovie _movie;
        BannerRoyalInventoryVM _dataSource;
        int ViewOrderPriority = 99;
        bool InventoryVisible=false;
        private LootChest _lootChest;
        public override void OnMissionScreenInitialize()
        {
            base.OnMissionScreenInitialize();
            Mission.Current.IsInventoryAccessible = false;
        }

        public BannerRoyalInventoryView()
        {

        }

        public BannerRoyalInventoryView(BannerRoyalInventoryVM dataSource)
        {
            _dataSource = dataSource;
        }

        public override void OnBehaviorInitialize()
        {
            base.OnBehaviorInitialize();
            _gauntletLayer = new GauntletLayer(ViewOrderPriority);
            if (_dataSource == null) _dataSource = new BannerRoyalInventoryVM(Mission);

        }

        public override void OnMissionScreenTick(float dt)
        {       
            if (Input.IsKeyPressed(TaleWorlds.InputSystem.InputKey.I))
            {
                ToggleUI(false);
            }
            else if (Input.IsKeyPressed(TaleWorlds.InputSystem.InputKey.L))
            {
                ToggleUI(true);
            }
        }

        public override void OnMissionScreenFinalize()
        {
            base.OnMissionScreenFinalize();
            base.MissionScreen.RemoveLayer(_gauntletLayer);
            _movie = null;
            _gauntletLayer = null;
            _dataSource = null;
        }

        public void ToggleUI(bool isLPressed)
        {
            if(InventoryVisible == false)
            {
                _dataSource = GetNearestChest();
            }

            if (!_dataSource.InventoryIsVisible)
            {
                if (isLPressed)
                {
                    if (_dataSource._isLootBoxFocused)
                    {
                        Show(true);
                    }
                }
                else
                {
                    Show(false);
                }
            }
            else
            {
                Hide();
            }
        }

        public void Show(bool showLootbox)
        {
            _dataSource.InventoryIsVisible = true;
            InventoryVisible = true;
            _dataSource.LootboxIsVisible = showLootbox;
            _movie = _gauntletLayer.LoadMovie(BannerRoyalMovies.BannerRoyalInventory, _dataSource);
            MissionScreen.AddLayer(_gauntletLayer);
            _gauntletLayer.InputRestrictions.SetInputRestrictions(true, InputUsageMask.All);
            _lootChest.StopSound();
            var voiceType = SkinVoiceManager.VoiceType.MpBarks[1]; //new SkinVoiceType("CustomSound");

            Agent.Main.MakeVoice(voiceType, SkinVoiceManager.CombatVoiceNetworkPredictionType.NoPrediction);
        }

        public void Hide()
        {
            _dataSource.InventoryIsVisible = false;
            InventoryVisible = false;
            _dataSource.LootboxIsVisible = false;
            _gauntletLayer.ReleaseMovie(_movie);
            _gauntletLayer.InputRestrictions.SetInputRestrictions(false, InputUsageMask.Invalid);
        }

        public BannerRoyalInventoryVM GetNearestChest()
        {
            var entities = Mission.Current.Scene.FindEntitiesWithTag("LootChest");
            var chests = new List<LootChest>();

            foreach (var entity in entities)
            {
                chests.AddRange(entity.CollectObjects<LootChest>());
            }


            var heroPos = GameNetwork.MyPeer.ControlledAgent.GetChestGlobalPosition();

            chests.Sort((e1, e2) =>
            {
                var distance1 = (e1.GameEntity.GlobalPosition - heroPos).LengthSquared;
                var distance2 = (e2.GameEntity.GlobalPosition - heroPos).LengthSquared;
                return distance1.CompareTo(distance2);
            });

            // Get the closest entity
            _lootChest = chests.FirstOrDefault();
            return _lootChest._inventoryVM;
        }

    }


}
