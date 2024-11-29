using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.Library;

namespace BannerRoyalMPLib
{
    public class CircleMenuItemVM : ViewModel
    {
        private readonly Action<CircleMenuItemVM> _onSelection;

        public string _text;
        public int _tauntIndex;
        private bool _isSelected;
        private string _typeAsString;

        [DataSourceProperty]
        public string Text
        {
            get
            {
                return _text;
            }
            set
            {
                if (value != _text)
                {
                    _text = value;
                    OnPropertyChangedWithValue(value, "Text");
                }
            }
        }

        [DataSourceProperty]
        public int TauntIndex
        {
            get
            {
                return _tauntIndex;
            }
            set
            {
                if (value != _tauntIndex)
                {
                    _tauntIndex = value;
                    OnPropertyChangedWithValue(value, "TauntIndex");
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
                        _onSelection(this);
                    }
                }
            }
        }

        [DataSourceProperty]
        public string TypeAsString
        {
            get
            {
                return _typeAsString;
            }
            set
            {
                if (value != _typeAsString)
                {
                    _typeAsString = value;
                    OnPropertyChangedWithValue(value, "TypeAsString");
                }
            }
        }

        public CircleMenuItemVM(string text, string itemTypeAsString, Action<CircleMenuItemVM> onSelection)
        {
            Text = text;
            TypeAsString = itemTypeAsString;
            _onSelection = onSelection;
        }
    }
}
