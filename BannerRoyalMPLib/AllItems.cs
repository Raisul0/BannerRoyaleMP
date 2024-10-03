using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.Core;
using TaleWorlds.ObjectSystem;

namespace BannerRoyalMPLib
{
    public static class AllItems
    {
        private static List<ItemObject> _items = MBObjectManager.Instance.GetObjectTypeList<ItemObject>();
        private static Random rnd = new Random();

        #region ##Weapons##

        private static List<ItemObject> _weapons = (from itemObject in _items
                                                   where ViewModelLib.WeaponFilter.Contains(itemObject.ItemType)
                                                   select itemObject).ToList();
        public static List<ItemObject> Weapons
        {
            get { return _weapons; }
        }

        private static int _weaponCount = _weapons.Count;

        #endregion

        #region ##Armors##

        private static List<ItemObject> _armors = (from itemObject in _items
                                                   where ViewModelLib.ArmorFilter.Contains(itemObject.ItemType)
                                                   select itemObject).ToList();
        public static List<ItemObject> Armors
        {
            get { return _armors; }
        }

        private static int _armorCount = _armors.Count;

        #endregion

        #region ##WeaponsAndArmor##

        private static List<ItemObject> _weaponsAndArmors = (from itemObject in _items
                                                            where ViewModelLib.ArmorFilter.Contains(itemObject.ItemType) || ViewModelLib.WeaponFilter.Contains(itemObject.ItemType)
                                                            select itemObject).ToList();
        public static List<ItemObject> WeaponsAndArmors
        {
            get { return _weaponsAndArmors; }
        }

        private static int _weaponAndArmorCount = _weaponsAndArmors.Count;

        #endregion

        public static ItemObject GetItemFromStringId(string itemId)
        {
            if (!string.IsNullOrEmpty( itemId)) 
            { 
                return _weaponsAndArmors.FirstOrDefault(x=>x.StringId == itemId);
            }
            else
            {
                return new ItemObject();
            }
        }

        public static List<ItemObject> GetAnyNoWeapon(int count)
        {
            var allweaponNo = GetManyRandomNo(count,new List<int>(),_weaponCount-1);
            var weapons = new List<ItemObject>();

            allweaponNo.ForEach(x => {
                weapons.Add(_weapons[x]);
            });
            return weapons;
        }
        public static List<ItemObject> GetAnyNoArmor(int count)
        {
            var allArmorNo = GetManyRandomNo(count, new List<int>(), _armorCount - 1);
            var armors = new List<ItemObject>();

            allArmorNo.ForEach(x => {
                armors.Add(_armors[x]);
            });
            return armors;
        }
        public static List<ItemObject> GetAnyNoOfWeaponAndArmor(int count)
        {
            var allWeaponAndArmorNo = GetManyRandomNo(count, new List<int>(), _weaponAndArmorCount - 1);
            var weaponsAndArmors = new List<ItemObject>();

            allWeaponAndArmorNo.ForEach(x => {
                weaponsAndArmors.Add(_weaponsAndArmors[x]);
            });
            return weaponsAndArmors;
        }
        public static List<int> GetManyRandomNo(int count,List<int> numbers,int maxNo)
        {
            var number = rnd.Next(maxNo);
            for(int i = 0; i < count; i++)
            {
                if (numbers.Any(x => x == number))
                {
                    numbers = GetManyRandomNo(count - numbers.Count, numbers, maxNo);
                }
                else
                {
                    numbers.Add(number);
                }
            }
            return numbers;
        }

    }
}
