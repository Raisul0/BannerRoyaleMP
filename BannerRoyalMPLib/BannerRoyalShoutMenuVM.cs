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
    public class BannerRoyalShoutMenuVM : ViewModel
    {
        [DataSourceProperty]
        public MBBindingList<BannerRoyalShoutSlotVM> ShoutSlots
        {
            get
            {
                return _shoutSlots;
            }
            set
            {
                if (value != _shoutSlots)
                {
                    _shoutSlots = value;
                    OnPropertyChangedWithValue(value, "ShoutSlots");
                }
            }
        }


        public BannerRoyalShoutMenuVM()
        {
            ShoutSlots = PopulateShoutSlots();
            RefreshValues();
        }
        private MBBindingList<BannerRoyalShoutSlotVM> PopulateShoutSlots()
        {
            var _shoutSlots = new MBBindingList<BannerRoyalShoutSlotVM>();

            var shoutBehavior = Mission.Current.GetMissionBehavior<ShoutBehavior>();

            foreach (var item in shoutBehavior.Shouts)
            {
                _shoutSlots.Add(new BannerRoyalShoutSlotVM(item.ShoutIndex, item.ShoutName, OnSlotFocused));
            }

            return _shoutSlots;
        }

        private void OnSlotFocused(BannerRoyalShoutSlotVM shoutSlot)
        {
            _selectedSlot = shoutSlot;
        }

        public void ExecuteShout()
        {
            if (_selectedSlot != null)
            {

                var shoutIndex = _selectedSlot.ShoutIndex;

                if (GameNetwork.IsClient)
                {
                    GameNetwork.BeginModuleEventAsClient();
                    GameNetwork.WriteMessage(new StartShout(shoutIndex, GameNetwork.MyPeer));
                    GameNetwork.EndModuleEventAsClient();
                }
            }

        }

        private MBBindingList<BannerRoyalShoutSlotVM> _shoutSlots;
        private BannerRoyalShoutSlotVM _selectedSlot;
    }
}
