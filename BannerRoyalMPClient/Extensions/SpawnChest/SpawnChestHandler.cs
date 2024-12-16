using BannerRoyalMPLib;
using BannerRoyalMPLib.Globals;
using BannerRoyalMPLib.NetworkMessages.FromServer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade;

namespace BannerRoyalMPClient.Extensions.SpawnChest
{
    public class SpawnChestHandler : IHandlerRegister
    {
        public void Register(GameNetwork.NetworkMessageHandlerRegisterer reg)
        {
            reg.Register<StartSpawnChest>(SpawnChest);
        }

        public void SpawnChest(StartSpawnChest message)
        {
            foreach (var location in LootLocations.Locations)
            {
                var frame = new MatrixFrame(Mat3.Identity, new Vec3(location.Item1, location.Item2, location.Item3, location.Item4));
                GameEntity gameEntity = GameEntity.Instantiate(Mission.Current.Scene, "loot_chest", frame);
                gameEntity.SetFactorColor(Colors.Green.ToUnsignedInteger());
                LootChest chest = gameEntity.GetFirstScriptOfType<LootChest>();
                var inventoryVm = new BannerRoyalInventoryVM(Mission.Current);
                inventoryVm.SetChestItems(LootPools.TestPoolItems);
                chest.SetViewModel(inventoryVm);
            }
        }
       
    }
}
