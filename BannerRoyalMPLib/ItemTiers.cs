
namespace BannerRoyalMPLib
{
    public static class ItemTierName
    {
        public static readonly string N = "N";
        public static readonly string A = "A";
        public static readonly string B = "B";
        public static readonly string S = "S";
    }

    public static class ItemTierColor
    {
        public static readonly string Black = "#FFFFFFFF";
        public static readonly string Green = "#2FCF1DFF";
        public static readonly string Blue = "#F7C9A5FF";
        public static readonly string Purple = "#D96202FF";
    }

    public class ItemTier
    {
        public ItemTier(string tierName)
        {
            if(tierName == ItemTierName.A)
            {
                TierName = ItemTierName.A;
                TierColor = ItemTierColor.Green;
            }
            else if(tierName == ItemTierName.B)
            {
                TierName = ItemTierName.B;
                TierColor = ItemTierColor.Blue;
            }
            else if(tierName == ItemTierName.S)
            {
                TierName = ItemTierName.S;
                TierColor = ItemTierColor.Purple;
            }
            else
            {
                TierName = ItemTierName.N;
                TierColor = ItemTierColor.Black;
            }
        }

        public ItemTier(int tierNo=0)
        {
            
            if(tierNo == 1)
            {
                TierName = ItemTierName.A;
                TierColor = ItemTierColor.Green;
            }
            else if(tierNo == 2)
            {
                TierName = ItemTierName.B;
                TierColor = ItemTierColor.Blue;
            }
            else if (tierNo == 3)
            {
                TierName = ItemTierName.S;
                TierColor = ItemTierColor.Purple;
            }
            else
            {
                TierName = ItemTierName.N;
                TierColor = ItemTierColor.Black;
            }
        }

        public string TierName { get; set; }
        public string TierColor { get; set; }
        

    }
}
