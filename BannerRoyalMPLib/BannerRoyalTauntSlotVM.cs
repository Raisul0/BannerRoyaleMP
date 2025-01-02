using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.Library;

namespace BannerRoyalMPLib
{
    public class BannerRoyalTauntSlotVM : ViewModel
    {
        private int _tauntSlot;
        private string _tauntAction;
        private string _tauntName;
        private bool _isSelected;
        private readonly Action<BannerRoyalTauntSlotVM> _onFocus;

        [DataSourceProperty]
        public string TauntAction
        {
            get
            {
                return _tauntAction;
            }
            set
            {
                if (value != _tauntAction)
                {
                    _tauntAction = value;
                    OnPropertyChangedWithValue(value, "TauntAction");
                }
            }
        }

        [DataSourceProperty]
        public string TauntName
        {
            get
            {
                return _tauntName;
            }
            set
            {
                if (value != _tauntName)
                {
                    _tauntName = value;
                    OnPropertyChangedWithValue(value, "TauntName");
                }
            }
        }

        [DataSourceProperty]
        public int TauntSlot
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
                    OnPropertyChangedWithValue(value, "TauntSlot");
                }
            }
        }

        [DataSourceProperty]
        public bool IsSelected
        {
            get
            {
                return _isSelected;
            }
            set
            {
                if (value != _isSelected)
                {
                    _isSelected = value;
                    OnPropertyChangedWithValue(value, "IsSelected");
                    if (value)
                    {
                        _onFocus(this);
                    }
                }
            }
        }



        public BannerRoyalTauntSlotVM(int tauntSlot, string tauntName, string tauntAction, Action<BannerRoyalTauntSlotVM> onFocus)
        {
            TauntSlot = tauntSlot;
            TauntName = tauntName;
            TauntAction = tauntAction;
            _onFocus = onFocus;
        }
    }
}
