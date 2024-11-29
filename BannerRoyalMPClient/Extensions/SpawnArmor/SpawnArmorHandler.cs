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
using TaleWorlds.ObjectSystem;

namespace BannerRoyalMPClient.Extensions.SpawnChest
{
    public class SpawnArmorHandler : IHandlerRegister
    {
        public void Register(GameNetwork.NetworkMessageHandlerRegisterer reg)
        {
            reg.Register<StartSpawnArmor>(SpawnArmor);
        }

        public void SpawnArmor(StartSpawnArmor message)
        {
            MissionWeapon armorMissionWeapon = new MissionWeapon(message.Armor, null, null);
            WeaponData weaponData = armorMissionWeapon.GetWeaponData(true);
            MatrixFrame frame = message.SpawnLocation;
            GameEntity armorEntity = GameEntity.Instantiate(Mission.Current.Scene, "spawned_armor", frame);
            SpawnedArmor spawnedArmor = armorEntity.GetFirstScriptOfType<SpawnedArmor>();
            spawnedArmor.SetArmorItem(message.Armor);
            armorEntity.AddPhysics(weaponData.BaseWeight, weaponData.CenterOfMassShift, weaponData.Shape, Vec3.One, Vec3.Zero, PhysicsMaterial.GetFromIndex(weaponData.PhysicsMaterialIndex), false, 0);
            armorEntity.SetPhysicsState(true, true);
            armorEntity.SetMobility(GameEntity.Mobility.dynamic);

        }
       
    }
}
