using System.Collections.Generic;

namespace BannerRoyalMPLib.Globals
{
    public enum ItemTiers
    {
        Common,
        Uncommon,
        Rare,
        Epic,
        Legendary
    }

    public static class ItemTierInfo
    {
        public static Dictionary<ItemTiers, string> ItemTierColors = new Dictionary<ItemTiers, string>
        {
            { ItemTiers.Common, "#FFFFFFFF" },
            { ItemTiers.Uncommon, "#2FCF1DFF" },
            { ItemTiers.Rare, "#F7C9A5FF" },
            { ItemTiers.Epic, "#D96202FF" },
            { ItemTiers.Legendary, "#FFFFFFFF" }
        };

        public static Dictionary<ItemTiers, int> ItemTierWeights = new Dictionary<ItemTiers, int>
        {
            { ItemTiers.Common, 10 },
            { ItemTiers.Uncommon, 5 },
            { ItemTiers.Rare, 3 },
            { ItemTiers.Epic, 2 },
            { ItemTiers.Legendary, 1 }
        };

        
    }
}
