using BannerRoyalMPLib.NetworkMessages.FromClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Diamond;
using static System.Collections.Specialized.BitVector32;

namespace BannerRoyalMPLib
{
    public class RadialMenuVM : ViewModel
    {
        [DataSourceProperty]
        public MBBindingList<CircleMenuItemVM> DropActions
        {
            get
            {
                return _dropActions;
            }
            set
            {
                if (value != _dropActions)
                {
                    _dropActions = value;
                    OnPropertyChangedWithValue(value, "DropActions");
                }
            }
        }

        [DataSourceProperty]
        public MBBindingList<CircleMenuItemVM> EquipActions
        {
            get
            {
                return _equipActions;
            }
            set
            {
                if (value != _equipActions)
                {
                    _equipActions = value;
                    OnPropertyChangedWithValue(value, "EquipActions");
                }
            }
        }

        public RadialMenuVM(Action<EquipmentIndex> onDropEquipment, Action<SpawnedItemEntity, EquipmentIndex> onEquipItem)
        {
            _onDropEquipment = onDropEquipment;
            _onEquipItem = onEquipItem;
            DropActions = GetDropActions();
            EquipActions = GetEquipActions();
            RefreshValues();
        }

        private MBBindingList<CircleMenuItemVM> GetDropActions()
        {
            var _dropActions = new MBBindingList<CircleMenuItemVM>();
            _dropActions.Add(new CircleMenuItemVM(GameTexts.FindText("str_cancel").ToString(), "None", OnItemSelected));
            _dropActions.Add(new CircleMenuItemVM(GameTexts.FindText("str_cancel").ToString(), "Crossbow", OnItemSelected));
            _dropActions.Add(new CircleMenuItemVM(GameTexts.FindText("str_cancel").ToString(), "Spear", OnItemSelected));
            _dropActions.Add(new CircleMenuItemVM(GameTexts.FindText("str_cancel").ToString(), "Sword", OnItemSelected));
            _dropActions.Add(new CircleMenuItemVM(GameTexts.FindText("str_cancel").ToString(), "Axe", OnItemSelected));

            //var item = AllItems.GetItemFromStringId("mp_vlandian_short_sword");
            //var equipmentIndex = EquipmentIndex.Weapon0;
            //if (item != null)
            //{
            //    string itemTypeAsString = MissionMainAgentEquipmentControllerVM.GetItemTypeAsString(item);
            //    bool isCurrentlyWielded = this.IsWieldedWeaponAtIndex(equipmentIndex);
            //    string weaponName = item.ToString();
            //    _dropActions.Add(new EquipmentActionItemVM(weaponName, itemTypeAsString, equipmentIndex, new Action<EquipmentActionItemVM>(this.OnItemSelected), isCurrentlyWielded));
            //}

            return _dropActions;
        }

        private MBBindingList<CircleMenuItemVM> GetEquipActions()
        {
            var _equipActions = new MBBindingList<CircleMenuItemVM>();
            _equipActions.Add(new CircleMenuItemVM("1", "None", OnItemSelected));
            _equipActions.Add(new CircleMenuItemVM("2", "1", OnItemSelected));
            _equipActions.Add(new CircleMenuItemVM("3", "2", OnItemSelected));
            _equipActions.Add(new CircleMenuItemVM("4", "3", OnItemSelected));
            _equipActions.Add(new CircleMenuItemVM("5", "4", OnItemSelected));

            //var item = AllItems.GetItemFromStringId("mp_vlandian_short_sword");
            //var equipmentIndex = EquipmentIndex.Weapon0;
            //if (item != null)
            //{
            //    string itemTypeAsString = MissionMainAgentEquipmentControllerVM.GetItemTypeAsString(item);
            //    bool isCurrentlyWielded = this.IsWieldedWeaponAtIndex(equipmentIndex);
            //    string weaponName = item.ToString();
            //    _equipActions.Add(new EquipmentActionItemVM(weaponName, itemTypeAsString, equipmentIndex, OnItemSelected, isCurrentlyWielded));
            //}

            return _equipActions;
        }

        private void OnItemSelected(CircleMenuItemVM item)
       {
            TauntUsageManager.TauntUsageSet usageSet = TauntUsageManager.GetUsageSet("taunt_17");
            var action = usageSet.GetUsages().FirstOrDefault().GetAction();

            

            if (GameNetwork.IsClient)
            {
                GameNetwork.BeginModuleEventAsClient();
                GameNetwork.WriteMessage(new StartTaunt(action, GameNetwork.MyPeer));
                GameNetwork.EndModuleEventAsClient();
            }


        }

        private bool IsWieldedWeaponAtIndex(EquipmentIndex index)
        {
            if (index != Agent.Main.GetWieldedItemIndex(Agent.HandIndex.MainHand))
            {
                return index == Agent.Main.GetWieldedItemIndex(Agent.HandIndex.OffHand);
            }

            return true;
        }

        private readonly Action<EquipmentIndex> _onDropEquipment;

        private readonly Action<SpawnedItemEntity, EquipmentIndex> _onEquipItem;

        private MBBindingList<CircleMenuItemVM> _dropActions;

        private MBBindingList<CircleMenuItemVM> _equipActions;
    }
}
