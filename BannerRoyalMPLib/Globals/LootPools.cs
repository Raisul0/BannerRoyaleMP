using System;
using System.Collections.Generic;
using TaleWorlds.Core;

namespace BannerRoyalMPLib.Globals;

public static class LootPools
{
    public static List<Tuple<string,ItemObject, ItemTiers>> TestPool()
    {
        var items = AllItems.GetAnyNoOfWeaponAndArmor(12);

        var LootItems = new List<Tuple<string, ItemObject, ItemTiers>>();

        LootItems.Add(new Tuple<string, ItemObject, ItemTiers>(items[0].ToString(), items[0] ,ItemTiers.Common));
        LootItems.Add(new Tuple<string, ItemObject, ItemTiers>(items[1].ToString(), items[1] ,ItemTiers.Uncommon));
        LootItems.Add(new Tuple<string, ItemObject, ItemTiers>(items[2].ToString(), items[2] ,ItemTiers.Rare));
        LootItems.Add(new Tuple<string, ItemObject, ItemTiers>(items[3].ToString(), items[3] ,ItemTiers.Epic));
        LootItems.Add(new Tuple<string, ItemObject, ItemTiers>(items[4].ToString(), items[4] ,ItemTiers.Legendary));
        LootItems.Add(new Tuple<string, ItemObject, ItemTiers>(items[5].ToString(), items[5] ,ItemTiers.Common));
        LootItems.Add(new Tuple<string, ItemObject, ItemTiers>(items[6].ToString(), items[6] ,ItemTiers.Common));
        LootItems.Add(new Tuple<string, ItemObject, ItemTiers>(items[7].ToString(), items[7] ,ItemTiers.Uncommon));
        LootItems.Add(new Tuple<string, ItemObject, ItemTiers>(items[8].ToString(), items[8] ,ItemTiers.Epic));
        LootItems.Add(new Tuple<string, ItemObject, ItemTiers>(items[9].ToString(), items[9] ,ItemTiers.Rare));
        LootItems.Add(new Tuple<string, ItemObject, ItemTiers>(items[10].ToString(),items[10] ,ItemTiers.Common));
        LootItems.Add(new Tuple<string, ItemObject, ItemTiers>(items[11].ToString(), items[11] , ItemTiers.Legendary));

        return LootItems;
    }

    //public static List<Tuple<ItemObject, ItemTiers>> TestPool = new()
    //{
        
    //    new Tuple<string, ItemTiers>("mp_pilgrim_hood", ItemTiers.Common),
    //    new Tuple<string, ItemTiers>("mp_ragged_robes", ItemTiers.Uncommon),
    //    new Tuple<string, ItemTiers>("mp_ragged_armwraps", ItemTiers.Rare),
    //    new Tuple<string, ItemTiers>("mp_ragged_boots", ItemTiers.Epic),
    //    new Tuple<string, ItemTiers>("mp_default_dagger", ItemTiers.Legendary),
    //    new Tuple<string, ItemTiers>("mp_throwing_stone", ItemTiers.Common),
    //    new Tuple<string, ItemTiers>("mp_vlandian_short_sword", ItemTiers.Common),
    //    new Tuple<string, ItemTiers>("mp_vlandian_throwing_axe", ItemTiers.Uncommon),
    //    new Tuple<string, ItemTiers>("mp_strapped_round_shield", ItemTiers.Epic),
    //    new Tuple<string, ItemTiers>("mp_vlandian_mace", ItemTiers.Rare),
    //    new Tuple<string, ItemTiers>("mp_hatchet_axe", ItemTiers.Common),
    //    new Tuple<string, ItemTiers>("mp_khuzait_sichel", ItemTiers.Legendary),
    //};
}