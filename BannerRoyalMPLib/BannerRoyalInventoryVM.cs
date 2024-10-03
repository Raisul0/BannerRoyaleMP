using System;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade;

namespace BannerRoyalMPLib
{
    public class BannerRoyalInventoryVM : ViewModel
    {
        public BannerRoyalInventoryVM(Mission mission)
        {
            InventoryIsVisible = false;
            _mission = mission;
            LootboxIsVisible = false;
            _isLootBoxFocused = false;

            SetInventoryItems();
            SetLootItems();
        }

        private void SetInventoryItems()
        {
            var _weaponItems = new MBBindingList<BannerRoyalItemObjectVM>();
            var _armorItems = new MBBindingList<BannerRoyalItemObjectVM>();
            var _inventoryItems = new MBBindingList<BannerRoyalItemObjectVM>();
            for (int i = 0; i < 5; i++)
            {
                var emptyItem = new BannerRoyalItemObjectVM();
                if (i < 4)
                {
                    _weaponItems.Add(emptyItem);
                    _armorItems.Add(emptyItem);
                }
                _inventoryItems.Add(emptyItem);
            }
            this.WeaponItems = _weaponItems;
            this.ArmorItems = _armorItems;
            this.InventoryItems = _inventoryItems;
        }

        private void SetLootItems()
        {

            TimeSpan t = (DateTime.UtcNow - new DateTime(1900, 1, 1));
            int timestamp = (int)t.TotalSeconds;
            Random ran = new Random(timestamp);


            var _topRowItems = new MBBindingList<BannerRoyalItemObjectVM>();
            foreach (var topRowItem in AllItems.GetAnyNoOfWeaponAndArmor(6))
            {
                var randomNo = ran.Next();
                var tierNo = randomNo % 3 + 1;
                _topRowItems.Add(new BannerRoyalItemObjectVM(topRowItem, new ItemTier(tierNo)));
            }
            this.ChestItemsTopRow = _topRowItems;

            var _bottomRowItems = new MBBindingList<BannerRoyalItemObjectVM>();
            foreach (var bottomRowItem in AllItems.GetAnyNoOfWeaponAndArmor(6))
            {
                var randomNo = ran.Next();
                var tierNo = randomNo % 3 + 1;
                _bottomRowItems.Add(new BannerRoyalItemObjectVM(bottomRowItem, new ItemTier(tierNo)));

            }
            this.ChestItemsBottomRow = _bottomRowItems;

        }

        private void OnItemHover(BannerRoyalItemObjectVM item)
        {

        }

        private void OnItemSelected(BannerRoyalItemObjectVM item)
        {

        }

        Mission _mission;
        public bool _isLootBoxFocused;
        bool _inventoryIsVisible;
        bool _lootboxIsVisible;
        private MBBindingList<BannerRoyalItemObjectVM> _weaponItems;
        private MBBindingList<BannerRoyalItemObjectVM> _armorItems;
        private MBBindingList<BannerRoyalItemObjectVM> _inventoryItems;
        private MBBindingList<BannerRoyalItemObjectVM> _chestItemsTopRow;
        private MBBindingList<BannerRoyalItemObjectVM> _chestItemsBottomRow;

        [DataSourceProperty]
        public bool InventoryIsVisible
        {
            get
            {
                return this._inventoryIsVisible;
            }
            set
            {
                if (value != this._inventoryIsVisible)
                {
                    this._inventoryIsVisible = value;
                    base.OnPropertyChangedWithValue(value, "InventoryIsVisible");
                }
            }
        }

        [DataSourceProperty]
        public bool LootboxIsVisible
        {
            get
            {
                return this._lootboxIsVisible;
            }
            set
            {
                if (value != this._lootboxIsVisible)
                {
                    this._lootboxIsVisible = value;
                    base.OnPropertyChangedWithValue(value, "LootboxIsVisible");
                }
            }
        }

        [DataSourceProperty]
        public MBBindingList<BannerRoyalItemObjectVM> WeaponItems
        {
            get
            {
                return this._weaponItems;
            }
            set
            {
                if (this._weaponItems != value)
                {
                    this._weaponItems = value;
                    base.OnPropertyChangedWithValue<MBBindingList<BannerRoyalItemObjectVM>>(value, "WeaponItems");
                }
            }
        }

        [DataSourceProperty]
        public MBBindingList<BannerRoyalItemObjectVM> ArmorItems
        {
            get
            {
                return this._armorItems;
            }
            set
            {
                if (this._armorItems != value)
                {
                    this._armorItems = value;
                    base.OnPropertyChangedWithValue<MBBindingList<BannerRoyalItemObjectVM>>(value, "ArmorItems");
                }
            }
        }

        [DataSourceProperty]
        public MBBindingList<BannerRoyalItemObjectVM> InventoryItems
        {
            get
            {
                return this._inventoryItems;
            }
            set
            {
                if (this._inventoryItems != value)
                {
                    this._inventoryItems = value;
                    base.OnPropertyChangedWithValue<MBBindingList<BannerRoyalItemObjectVM>>(value, "InventoryItems");
                }
            }
        }

        [DataSourceProperty]
        public MBBindingList<BannerRoyalItemObjectVM> ChestItemsTopRow
        {
            get
            {
                return this._chestItemsTopRow;
            }
            set
            {
                if (this._chestItemsTopRow != value)
                {
                    this._chestItemsTopRow = value;
                    base.OnPropertyChangedWithValue<MBBindingList<BannerRoyalItemObjectVM>>(value, "ChestItemsTopRow");
                }
            }
        }

        [DataSourceProperty]
        public MBBindingList<BannerRoyalItemObjectVM> ChestItemsBottomRow
        {
            get
            {
                return this._chestItemsBottomRow;
            }
            set
            {
                if (this._chestItemsBottomRow != value)
                {
                    this._chestItemsBottomRow = value;
                    base.OnPropertyChangedWithValue<MBBindingList<BannerRoyalItemObjectVM>>(value, "ChestItemsBottomRow");
                }
            }
        }


    }
}
