using BannerRoyalMPLib;
using NetworkMessages.FromClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.Core;
using TaleWorlds.Engine.GauntletUI;
using TaleWorlds.GauntletUI.Data;
using TaleWorlds.InputSystem;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.View;
using TaleWorlds.MountAndBlade.View.MissionViews;
using TaleWorlds.MountAndBlade.View.Screens;
using TaleWorlds.MountAndBlade.ViewModelCollection;
using TaleWorlds.MountAndBlade.ViewModelCollection.HUD;

namespace BannerRoyal
{
    [DefaultView]
    public class BannerRoyalTauntMenuView : MissionView
    {
        private bool complete = false;
        private bool _visiable = false;

        int ViewOrderPriority = 100;

        public override void OnMissionScreenInitialize()
        {
            base.OnMissionScreenInitialize();
        }

        public override void OnBehaviorInitialize()
        {
            base.OnBehaviorInitialize();
            _gauntletLayer = new GauntletLayer(ViewOrderPriority);
            this._dataSource = new BannerRoyalTauntMenuVM();

        }

        public void Show()
        {
            _movie = _gauntletLayer.LoadMovie(BannerRoyalMovies.BannerRoyalTaunt, this._dataSource);
            MissionScreen.AddLayer(this._gauntletLayer);
            _gauntletLayer.InputRestrictions.SetInputRestrictions(false, InputUsageMask.Invalid);
            _visiable = true;
        }

        public void Hide()
        {
            _gauntletLayer.ReleaseMovie(_movie);
            _dataSource.ExecuteTaunt();

            _gauntletLayer.InputRestrictions.SetInputRestrictions(false, InputUsageMask.Invalid);
            _visiable = false;
        }

        public override void OnMissionScreenTick(float dt)
        {
            base.OnMissionScreenTick(dt);
            if (this.IsMainAgentAvailable() && base.Mission.IsMainAgentItemInteractionEnabled)
            {
                if (Input.IsKeyDown(TaleWorlds.InputSystem.InputKey.M))
                {
                    if (!_visiable)
                    {
                        Show();
                    }
                }
                else
                {
                    if (_visiable)
                    {
                        Hide();
                    }
                    
                }

                return;
            }
        }


        private bool IsMainAgentAvailable()
        {
            Agent main = GameNetwork.MyPeer.ControlledAgent;
            return main != null && main.IsActive();
        }


        private GauntletLayer _gauntletLayer;
        private IGauntletMovie _movie;
        private BannerRoyalTauntMenuVM _dataSource;
    }
}
