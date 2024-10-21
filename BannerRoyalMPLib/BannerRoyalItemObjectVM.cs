using System;
using BannerRoyalMPLib.Globals;
using TaleWorlds.Core;
using TaleWorlds.Core.ViewModelCollection.Information;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade;
using BannerRoyalMPLib.NetworkMessages;

namespace BannerRoyalMPLib
{
    public class BannerRoyalItemObjectVM : ViewModel
    {
        public BannerRoyalItemObjectVM()
        {
            this.Object = null;
            Name = "No Item";
            ImageIdentifier = null;
            this._tier = ItemTiers.Common;
            ItemWidgetType = "Inventory";
        }

        public BannerRoyalItemObjectVM(ItemObject item, ItemTiers tier)
        {
            this.Object = item;
            SetItemInfo(this.Object);
            this._tier = tier;
        }


        protected void SetItemInfo(ItemObject item)
        {
            Name = item.ToString();
            ImageIdentifier = new ImageIdentifierVM(new ImageIdentifier(item as ItemObject, ""));
            ItemWidgetType = "Inventory";
            if (ViewModelLib.WeaponFilter.Contains(item.ItemType))
            {
                ItemWidgetType = "Weapon";
            }
            else if (ViewModelLib.ArmorFilter.Contains(item.ItemType))
            {
                ItemWidgetType = "Armor";
            }
            else
            {
                ItemWidgetType = "Inventory";
            }
        }

        [DataSourceProperty]
        public string Name
        {
            get
            {
                return this._name;
            }
            set
            {
                this._name = value;
                base.OnPropertyChangedWithValue<string>(value, "Name");
            }
        }

        [DataSourceProperty]
        public string ItemWidgetType
        {
            get
            {
                return this._itemWidgetType;
            }
            set
            {
                this._itemWidgetType = value;
                base.OnPropertyChangedWithValue<string>(value, "ItemWidgetType");
            }

        }

        [DataSourceProperty]
        public virtual ImageIdentifierVM ImageIdentifier
        {
            get
            {
                return this._visual;
            }
            set
            {
                this._visual = value;
                base.OnPropertyChangedWithValue<ImageIdentifierVM>(value, "Visual");
            }
        }

        [DataSourceProperty]
        public string TierName
        {
            get
            {
                return this._tier.ToString();
            }
        }

        [DataSourceProperty]
        public string TierColor
        {
            get
            {
                ItemTierInfo.ItemTierColors.TryGetValue(_tier, out string value);
                return value;
            }
        }

        [DataSourceProperty]
        public virtual ItemObject Object
        {
            get
            {
                return this._object;
            }
            set
            {
                this._object = value;
                base.OnPropertyChangedWithValue<ItemObject>(value, "Object");
            }
        }

        [DataSourceProperty]
        public BasicTooltipViewModel ObjectStatTooltip
        {
            get
            {
                return this._objectStatTooltip;
            }
            set
            {
                if (value != this._objectStatTooltip)
                {
                    this._objectStatTooltip = value;
                    base.OnPropertyChangedWithValue<BasicTooltipViewModel>(value, "ObjectStatTooltip");
                }
            }
        }

        public string GetItemStats()
        {
            var result = "";

            if (this.Object != null)
            {
                result += @"Name   : " + this.Object.Name + "\n" +
                          @"Weight : " + this.Object.Weight + "\n" +
                          @"Type   : " + this.Object.ItemType;
            }
            else
            {
                result += @"No Item";
            }
            return result;
        }

        public void ExecuteEquipItem()
        {
            GameNetwork.BeginModuleEventAsClient();
            GameNetwork.WriteMessage(new StartEquipItem(this.Object, GameNetwork.MyPeer));
            GameNetwork.EndModuleEventAsClient();

        }

        public void ExecuteOnSelected()
        {
            bool flag = this.onObjectSelected != null;
            if (flag)
            {
                this.onObjectSelected(this);
            }
        }

        public void ExecuteBeginHint()
        {

            this.ObjectStatTooltip = new BasicTooltipViewModel(() => this.GetItemStats());
            this.ObjectStatTooltip.ExecuteBeginHint();

        }
        public void ExecuteEndHint()
        {
            this.ObjectStatTooltip.ExecuteEndHint();
        }

        private string _name;
        private int _serial;
        private ImageIdentifierVM _visual;
        private ItemObject _object;
        private Action<BannerRoyalItemObjectVM> onObjectSelected;
        private Action<BannerRoyalItemObjectVM> onObjectHover;
        private ItemTiers _tier;
        private string _objectStatTooltipString = "";
        private string _itemWidgetType;
        private BasicTooltipViewModel _objectStatTooltip;


    }
}
