using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.Library;

namespace BannerRoyalMPLib
{
    public class BannerRoyalShoutSlotVM : ViewModel
    {
        private int _shoutIndex;
        private string _shoutName;
        private bool _isSelected;
        private readonly Action<BannerRoyalShoutSlotVM> _onFocus;


        [DataSourceProperty]
        public string ShoutName
        {
            get
            {
                return _shoutName;
            }
            set
            {
                if (value != _shoutName)
                {
                    _shoutName = value;
                    OnPropertyChangedWithValue(value, "ShoutName");
                }
            }
        }

        [DataSourceProperty]
        public int ShoutIndex
        {
            get
            {
                return _shoutIndex;
            }
            set
            {
                if (value != _shoutIndex)
                {
                    _shoutIndex = value;
                    OnPropertyChangedWithValue(value, "ShoutIndex");
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



        public BannerRoyalShoutSlotVM(int shoutIndex, string shoutName, Action<BannerRoyalShoutSlotVM> onFocus)
        {
            ShoutIndex = shoutIndex;
            ShoutName = shoutName;
            _onFocus = onFocus;
        }
    }
}
