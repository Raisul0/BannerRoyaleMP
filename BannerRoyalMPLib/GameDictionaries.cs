using System.Collections.Generic;

namespace BannerRoyalMPLib;

public static class GameDictionaries
{
    public static Dictionary<string, List<string>> playerInventory = new Dictionary<string, List<string>>();
    public static Dictionary<string, List<string>> equippingInventory = new Dictionary<string, List<string>>();
}

public class PlayerEquipment
{
    public Armors armors;
    public Weapons weapons;
    public Inventory inventory;


}

public class Armors
{
    public Item
}