using System;
using System.Collections.Generic;
using System.Linq;
using BannerRoyalMPLib.Globals;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace BannerRoyalMPLib.ObjectClasses;

public class LootChestObject
{
    public float X { get; set; }
    public float Y { get; set; }
    public float Z { get; set; }
    public float W { get; set; }
    public List<Tuple<ItemObject, ItemTiers>> ItemPool { get; set; }
    
    public MatrixFrame ChestLocation { get; set; }
    public List<ItemObject> Contents { get; private set; }
    public bool IsUnopened { get; private set; }

    private Random random = new Random();

    public LootChestObject(float x, float y, float z,float w, List<Tuple<ItemObject, ItemTiers>> itemPool)
    {
        X = x;
        Y = y;
        Z = z;
        W = w;

        ChestLocation = new MatrixFrame(Mat3.Identity, new Vec3(X, Y, Z, W));
        ItemPool = itemPool;
        //IsUnopened = true;
        //Contents = GenerateContents();
    }

    public LootChestObject(MatrixFrame frame, List<Tuple<ItemObject, ItemTiers>> itemPool)
    {
        ChestLocation = frame;
        ItemPool = itemPool;
        //IsUnopened = true;
        //Contents = GenerateContents();
    }

    //private List<ItemObject> GenerateContents()
    //{
    //    List<ItemObject> generatedItems = new List<ItemObject>();
    //    int numberOfItems = random.Next(1, 13);

    //    for (int i = 0; i < numberOfItems; i++)
    //    {
    //        ItemObject selectedItem = GetRandomItem();
    //        if (selectedItem != null)
    //        {
    //            generatedItems.Add(selectedItem);
    //        }
    //    }

    //    return generatedItems;
    //}

    //private ItemObject GetRandomItem()
    //{
    //    ItemObject randomItem = AllItems.GetItemFromStringId(ItemPool.GetRandomElement().Item1);
    //    return randomItem;
    //    /*
    //    int totalTierWeight = ItemTierInfo.ItemTierWeights.Values.Sum();
    //    int randomValue = random.Next(0, totalTierWeight);

    //    foreach (var item in ItemPool)
    //    {
    //        if (randomValue < item.Value)
    //        {
    //            return item.Key;
    //        }
    //        randomValue -= item.Value;
    //    }
    //    return null;
    //    */
    //}

    public void OpenChest()
    {
        if (IsUnopened)
        {
            IsUnopened = false;
            // ... SOUND LOGIC HERE
        }
    }
}