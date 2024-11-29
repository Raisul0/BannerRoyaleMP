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
    public class RadialMenuView : MissionView
    {
        private bool complete = false;
        private bool _visiable = false;
        public event Action<bool> OnEquipmentDropInteractionViewToggled;

        // Token: 0x14000002 RID: 2
        // (add) Token: 0x060001B9 RID: 441 RVA: 0x0000AA64 File Offset: 0x00008C64
        // (remove) Token: 0x060001BA RID: 442 RVA: 0x0000AA9C File Offset: 0x00008C9C
        public event Action<bool> OnEquipmentEquipInteractionViewToggled;

        // Token: 0x17000057 RID: 87
        // (get) Token: 0x060001BB RID: 443 RVA: 0x0000AAD1 File Offset: 0x00008CD1
        private bool IsDisplayingADialog
        {
            get
            {
                IMissionScreen missionScreenAsInterface = this._missionScreenAsInterface;
                return missionScreenAsInterface != null && missionScreenAsInterface.GetDisplayDialog();
            }
        }

        // Token: 0x17000058 RID: 88
        // (get) Token: 0x060001BC RID: 444 RVA: 0x0000AAE4 File Offset: 0x00008CE4
        // (set) Token: 0x060001BD RID: 445 RVA: 0x0000AAEC File Offset: 0x00008CEC
        private bool EquipHoldHandled
        {
            get
            {
                return this._equipHoldHandled;
            }
            set
            {
                this._equipHoldHandled = value;
                MissionScreen missionScreen = base.MissionScreen;
                if (missionScreen == null)
                {
                    return;
                }
                missionScreen.SetRadialMenuActiveState(value);
            }
        }

        // Token: 0x17000059 RID: 89
        // (get) Token: 0x060001BE RID: 446 RVA: 0x0000AB06 File Offset: 0x00008D06
        // (set) Token: 0x060001BF RID: 447 RVA: 0x0000AB0E File Offset: 0x00008D0E
        private bool DropHoldHandled
        {
            get
            {
                return this._dropHoldHandled;
            }
            set
            {
                this._dropHoldHandled = value;
                MissionScreen missionScreen = base.MissionScreen;
                if (missionScreen == null)
                {
                    return;
                }
                missionScreen.SetRadialMenuActiveState(value);
            }
        }
        

        int ViewOrderPriority = 100;

        public override void OnMissionScreenInitialize()
        {
            base.OnMissionScreenInitialize();
        }

        public override void OnBehaviorInitialize()
        {
            base.OnBehaviorInitialize();
            _gauntletLayer = new GauntletLayer(ViewOrderPriority);
            this._dataSource = new RadialMenuVM(new Action<EquipmentIndex>(this.OnDropEquipment), new Action<SpawnedItemEntity, EquipmentIndex>(this.OnEquipItem));


        }

        public void Show()
        {
            _movie = _gauntletLayer.LoadMovie(BannerRoyalMovies.BannerRoyalTaunt, this._dataSource);
            MissionScreen.AddLayer(this._gauntletLayer);
            _gauntletLayer.InputRestrictions.SetInputRestrictions(false, InputUsageMask.Invalid);
            base.Mission.OnMainAgentChanged += this.OnMainAgentChanged;
            _visiable = true;
        }

        public void Hide()
        {
            _gauntletLayer.ReleaseMovie(_movie);
            _gauntletLayer.InputRestrictions.SetInputRestrictions(false, InputUsageMask.Invalid);
            _visiable = false;
        }

        public override void OnMissionScreenTick(float dt)
        {
            base.OnMissionScreenTick(dt);
            if (this.IsMainAgentAvailable() && base.Mission.IsMainAgentItemInteractionEnabled)
            {
                if (Input.IsKeyPressed(TaleWorlds.InputSystem.InputKey.M))
                {
                    if (!_visiable)
                    {
                        Show();
                        this.DropWeaponTick(dt);
                        this.EquipWeaponTick(dt);

                    }
                    else
                    {
                        Hide();
                    }
                    
                }
               

                return;
            }
            this._prevDropKeyDown = false;
            this._prevEquipKeyDown = false;
        }

        private void EquipWeaponTick(float dt)
        {
            if (base.MissionScreen.SceneLayer.Input.IsGameKeyDown((int)InputKey.K) && !this._prevDropKeyDown && !this.IsDisplayingADialog && this.IsMainAgentAvailable() && !base.MissionScreen.Mission.IsOrderMenuOpen)
            {
                if (!this._firstFrameOfEquipDownHandled)
                {
                    this._equipmentWasInFocusFirstFrameOfEquipDown = this._isFocusedOnEquipment;
                    this._firstFrameOfEquipDownHandled = true;
                }
                if (this._equipmentWasInFocusFirstFrameOfEquipDown)
                {
                    this._equipHoldTime += dt;
                    if (this._equipHoldTime > 0.5f && !this.EquipHoldHandled && this._isFocusedOnEquipment && this._isCurrentFocusedItemInteractable)
                    {
                        this.HandleOpeningHoldEquip();
                        this.EquipHoldHandled = true;
                    }
                }
                this._prevEquipKeyDown = true;
                return;
            }
            if (this._prevEquipKeyDown && !base.MissionScreen.SceneLayer.Input.IsGameKeyDown(13))
            {
                if (this._equipmentWasInFocusFirstFrameOfEquipDown)
                {
                    if (this._equipHoldTime < 0.5f)
                    {
                        if (this._focusedWeaponItem != null)
                        {
                            Agent main = GameNetwork.MyPeer.ControlledAgent;
                            if (main != null && main.CanQuickPickUp(this._focusedWeaponItem))
                            {
                                this.HandleQuickReleaseEquip();
                            }
                        }
                    }
                    else
                    {
                        this.HandleClosingHoldEquip();
                    }
                }
                if (this.EquipHoldHandled)
                {
                    this.EquipHoldHandled = false;
                }
                this._equipHoldTime = 0f;
                this._firstFrameOfEquipDownHandled = false;
                this._prevEquipKeyDown = false;
            }
        }

        // Token: 0x060001C8 RID: 456 RVA: 0x0000AF30 File Offset: 0x00009130
        private void DropWeaponTick(float dt)
        {
            if (base.MissionScreen.SceneLayer.Input.IsGameKeyDown(22) && !this._prevEquipKeyDown && !this.IsDisplayingADialog && this.IsMainAgentAvailable() && this.IsMainAgentHasAtLeastOneItem() && !base.MissionScreen.Mission.IsOrderMenuOpen)
            {
                this._dropHoldTime += dt;
                if (this._dropHoldTime > 0.5f && !this.DropHoldHandled)
                {
                    this.HandleOpeningHoldDrop();
                    this.DropHoldHandled = true;
                }
                this._prevDropKeyDown = true;
                return;
            }
            if (this._prevDropKeyDown && !base.MissionScreen.SceneLayer.Input.IsGameKeyDown(22))
            {
                if (this._dropHoldTime < 0.5f)
                {
                    this.HandleQuickReleaseDrop();
                }
                else
                {
                    this.HandleClosingHoldDrop();
                }
                this.DropHoldHandled = false;
                this._dropHoldTime = 0f;
                this._prevDropKeyDown = false;
            }
        }

        private void OnMainAgentChanged(object sender, PropertyChangedEventArgs e)
        {
            if (base.Mission.MainAgent == null)
            {
                //if (this.EquipHoldHandled)
                //{
                //    this.EquipHoldHandled = false;
                //    Action<bool> onEquipmentEquipInteractionViewToggled = this.OnEquipmentEquipInteractionViewToggled;
                //    if (onEquipmentEquipInteractionViewToggled != null)
                //    {
                //        onEquipmentEquipInteractionViewToggled(false);
                //    }
                //}
                //this._equipHoldTime = 0f;
                //this._dataSource.OnCancelEquipController();
                //if (this.DropHoldHandled)
                //{
                //    Action<bool> onEquipmentDropInteractionViewToggled = this.OnEquipmentDropInteractionViewToggled;
                //    if (onEquipmentDropInteractionViewToggled != null)
                //    {
                //        onEquipmentDropInteractionViewToggled(false);
                //    }
                //    this.DropHoldHandled = false;
                //}
                //this._dropHoldTime = 0f;
                //this._dataSource.OnCancelDropController();
            }
        }

        private void HandleOpeningHoldEquip()
        {
            //MissionMainAgentEquipmentControllerVM dataSource = this._dataSource;
            //if (dataSource != null)
            //{
            //    dataSource.OnEquipControllerToggle(true);
            //}
            //Action<bool> onEquipmentEquipInteractionViewToggled = this.OnEquipmentEquipInteractionViewToggled;
            //if (onEquipmentEquipInteractionViewToggled == null)
            //{
            //    return;
            //}
            //onEquipmentEquipInteractionViewToggled(true);
        }

        // Token: 0x060001CA RID: 458 RVA: 0x0000B03A File Offset: 0x0000923A
        private void HandleClosingHoldEquip()
        {
            //RadialMenuVM dataSource = this._dataSource;
            //if (dataSource != null)
            //{
            //    dataSource.OnEquipControllerToggle(false);
            //}
            //Action<bool> onEquipmentEquipInteractionViewToggled = this.OnEquipmentEquipInteractionViewToggled;
            //if (onEquipmentEquipInteractionViewToggled == null)
            //{
            //    return;
            //}
            //onEquipmentEquipInteractionViewToggled(false);
        }

        // Token: 0x060001CB RID: 459 RVA: 0x0000B05F File Offset: 0x0000925F
        private void HandleQuickReleaseEquip()
        {
            this.OnEquipItem(this._focusedWeaponItem, EquipmentIndex.None);
        }

        // Token: 0x060001CC RID: 460 RVA: 0x0000B06E File Offset: 0x0000926E
        private void HandleOpeningHoldDrop()
        {
            //MissionMainAgentEquipmentControllerVM dataSource = this._dataSource;
            //if (dataSource != null)
            //{
            //    dataSource.OnDropControllerToggle(true);
            //}
            //Action<bool> onEquipmentDropInteractionViewToggled = this.OnEquipmentDropInteractionViewToggled;
            //if (onEquipmentDropInteractionViewToggled == null)
            //{
            //    return;
            //}
            //onEquipmentDropInteractionViewToggled(true);
        }

        // Token: 0x060001CD RID: 461 RVA: 0x0000B093 File Offset: 0x00009293
        private void HandleClosingHoldDrop()
        {
            //MissionMainAgentEquipmentControllerVM dataSource = this._dataSource;
            //if (dataSource != null)
            //{
            //    dataSource.OnDropControllerToggle(false);
            //}
            //Action<bool> onEquipmentDropInteractionViewToggled = this.OnEquipmentDropInteractionViewToggled;
            //if (onEquipmentDropInteractionViewToggled == null)
            //{
            //    return;
            //}
            //onEquipmentDropInteractionViewToggled(false);
        }

        // Token: 0x060001CE RID: 462 RVA: 0x0000B0B8 File Offset: 0x000092B8
        private void HandleQuickReleaseDrop()
        {
            this.OnDropEquipment(EquipmentIndex.None);
        }

        private bool IsMainAgentAvailable()
        {
            Agent main = GameNetwork.MyPeer.ControlledAgent;
            return main != null && main.IsActive();
        }

        // Token: 0x060001D2 RID: 466 RVA: 0x0000B148 File Offset: 0x00009348
        private bool IsMainAgentHasAtLeastOneItem()
        {
            for (EquipmentIndex equipmentIndex = EquipmentIndex.WeaponItemBeginSlot; equipmentIndex < EquipmentIndex.NumAllWeaponSlots; equipmentIndex++)
            {
                if (!GameNetwork.MyPeer.ControlledAgent.Equipment[equipmentIndex].IsEmpty)
                {
                    return true;
                }
            }
            return false;
        }

        private void OnEquipItem(SpawnedItemEntity itemToEquip, EquipmentIndex indexToEquipItTo)
        {
            if (itemToEquip.GameEntity != null)
            {
                Agent main = GameNetwork.MyPeer.ControlledAgent;
                if (main == null)
                {
                    return;
                }
                main.HandleStartUsingAction(itemToEquip, (int)indexToEquipItTo);
            }
        }

        // Token: 0x060001D0 RID: 464 RVA: 0x0000B0E4 File Offset: 0x000092E4
        private void OnDropEquipment(EquipmentIndex indexToDrop)
        {
            if (GameNetwork.IsClient)
            {
                GameNetwork.BeginModuleEventAsClient();
                GameNetwork.WriteMessage(new DropWeapon(base.Input.IsGameKeyDown(10), indexToDrop));
                GameNetwork.EndModuleEventAsClient();
                return;
            }
            GameNetwork.MyPeer.ControlledAgent.HandleDropWeapon(base.Input.IsGameKeyDown(10), indexToDrop);
        }

        private const float _minHoldTime = 0.5f;

        // Token: 0x040000FC RID: 252
        private readonly IMissionScreen _missionScreenAsInterface;

        // Token: 0x040000FD RID: 253
        private bool _equipmentWasInFocusFirstFrameOfEquipDown;

        // Token: 0x040000FE RID: 254
        private bool _firstFrameOfEquipDownHandled;

        // Token: 0x040000FF RID: 255
        private bool _equipHoldHandled;

        // Token: 0x04000100 RID: 256
        private bool _isFocusedOnEquipment;

        // Token: 0x04000101 RID: 257
        private float _equipHoldTime;

        // Token: 0x04000102 RID: 258
        private bool _prevEquipKeyDown;

        // Token: 0x04000103 RID: 259
        private SpawnedItemEntity _focusedWeaponItem;

        // Token: 0x04000104 RID: 260
        private bool _dropHoldHandled;

        // Token: 0x04000105 RID: 261
        private float _dropHoldTime;

        // Token: 0x04000106 RID: 262
        private bool _prevDropKeyDown;

        // Token: 0x04000107 RID: 263
        private bool _isCurrentFocusedItemInteractable;

        // Token: 0x04000108 RID: 264
        private GauntletLayer _gauntletLayer;

        private IGauntletMovie _movie;

        // Token: 0x04000109 RID: 265
        private RadialMenuVM _dataSource;
    }
}
