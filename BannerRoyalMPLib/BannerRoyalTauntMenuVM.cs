using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BannerRoyalMPLib.NetworkMessages.FromClient;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade;

namespace BannerRoyalMPLib
{
    public class BannerRoyalTauntMenuVM : ViewModel
    {
        [DataSourceProperty]
        public MBBindingList<BannerRoyalTauntSlotVM> TauntSlots
        {
            get
            {
                return _tauntSlot;
            }
            set
            {
                if (value != _tauntSlot)
                {
                    _tauntSlot = value;
                    OnPropertyChangedWithValue(value, "TauntSlots");
                }
            }
        }


        public BannerRoyalTauntMenuVM()
        {
            TauntSlots = PopulateTauntSlots();
            RefreshValues();
        }
        private MBBindingList<BannerRoyalTauntSlotVM> PopulateTauntSlots()
        {
            var _tauntSlots = new MBBindingList<BannerRoyalTauntSlotVM>();

            var tauntBehaviour = Mission.Current.GetMissionBehavior<TauntBehavior>();

            foreach (var item in tauntBehaviour.Taunts)
            {
                _tauntSlots.Add(new BannerRoyalTauntSlotVM(item.TauntId, item.TauntName,item.TauntAction, OnSlotFocused));
            }

            return _tauntSlots;
        }

        private void OnSlotFocused(BannerRoyalTauntSlotVM tauntSlot)
        {
            _selectedSlot = tauntSlot;
        }

        public void ExecuteTaunt()
        {
            if (_selectedSlot != null) {

                var tauntIndex = _selectedSlot.TauntSlot;

                if (GameNetwork.IsClient)
                {
                    GameNetwork.BeginModuleEventAsClient();
                    GameNetwork.WriteMessage(new StartTaunt(tauntIndex, GameNetwork.MyPeer));
                    GameNetwork.EndModuleEventAsClient();
                }
            }

        }

        private MBBindingList<BannerRoyalTauntSlotVM> _tauntSlot;
        private BannerRoyalTauntSlotVM _selectedSlot;
    }
}
