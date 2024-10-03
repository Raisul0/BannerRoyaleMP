using BannerRoyalMPLib;
using System;
using TaleWorlds.Core;
using TaleWorlds.Core.ViewModelCollection.Information;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade;
using TaleWorlds.ObjectSystem;

namespace BannerRoyalMPLib
{
    public class BannerRoyalItemObjectVM : ViewModel
    {
        public BannerRoyalItemObjectVM()
        {
            this.Object = null;
            Name = "No Item";
            ImageIdentifier = null;
            this._tier = new ItemTier();
            ItemWidgetType = "Inventory";
        }

        public BannerRoyalItemObjectVM(string itemId,string tierName)
        {
            this.Object = AllItems.GetItemFromStringId(itemId);
            SetItemInfo(this.Object);
            this._tier = new ItemTier(tierName);
        }

        public BannerRoyalItemObjectVM(ItemObject itemObject,ItemTier tier)
        {
            this.Object = itemObject;
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
                return this._tier.TierName;
            }
            set
            {
                this._tier.TierName = value;
                base.OnPropertyChangedWithValue<string>(value, "TierName");
            }
        }

        [DataSourceProperty]
        public string TierColor
        {
            get
            {
                return this._tier.TierColor;
            }
            set
            {
                this._tier.TierColor = value;
                base.OnPropertyChangedWithValue<string>(value, "TierColor");
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

            if(this.Object != null)
            {
                result += @"Name   : " + this.Object.Name + "\n"+
                          @"Weight : " + this.Object.Weight + "\n"+
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

            if (this.Object != null)
            {
                var index = ViewModelLib.GetItemEquipmentIndex(Object);
                var missionPeer = GameNetwork.MyPeer.GetComponent<MissionPeer>();
                Equipment equipment = missionPeer.ControlledAgent.SpawnEquipment;
                equipment[index] = new EquipmentElement(this.Object);



            }

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

            this.ObjectStatTooltip = new BasicTooltipViewModel(() =>  this.GetItemStats());
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
        private ItemTier _tier; 
        private string _objectStatTooltipString = "";
        private string _itemWidgetType;
        private BasicTooltipViewModel _objectStatTooltip;


    }
}
