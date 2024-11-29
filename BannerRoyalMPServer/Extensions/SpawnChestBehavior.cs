using BannerRoyalMPLib;
using BannerRoyalMPLib.Globals;
using BannerRoyalMPLib.NetworkMessages;
using BannerRoyalMPLib.NetworkMessages.FromClient;
using BannerRoyalMPLib.NetworkMessages.FromServer;
using NetworkMessages.FromServer;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.InputSystem;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Network.Messages;
using TaleWorlds.ObjectSystem;

namespace BannerRoyalMPServer.Extensions
{
    public class SpawnChestBehavior : MissionNetwork
    {
        public bool chestSpawnComplete = false;
        public bool armorSpawnComplete = false;

        public override MissionBehaviorType BehaviorType => MissionBehaviorType.Other;

        public override void OnBehaviorInitialize()
        {
            base.OnBehaviorInitialize();
            this.AddRemoveMessageHandlers(GameNetwork.NetworkMessageHandlerRegisterer.RegisterMode.Add);
            
        }
        public override void OnRemoveBehavior()
        {
            base.OnRemoveBehavior();
            this.AddRemoveMessageHandlers(GameNetwork.NetworkMessageHandlerRegisterer.RegisterMode.Remove);
        }
        public void AddRemoveMessageHandlers(GameNetwork.NetworkMessageHandlerRegisterer.RegisterMode mode)
        {
            GameNetwork.NetworkMessageHandlerRegisterer networkMessageHandlerRegisterer = new GameNetwork.NetworkMessageHandlerRegisterer(mode);
            if (GameNetwork.IsServer)
            {
                networkMessageHandlerRegisterer.Register<StartEquipItem>(this.EquipSelectedItem);
            }
        }

        public void Init()
        {

        }

        protected override void HandleNewClientAfterSynchronized(NetworkCommunicator networkPeer)
        {
            this.SpawnChest(networkPeer);
        }
        public override void OnMissionTick(float dt)
        {
            base.OnMissionTick(dt);

            if(GameNetwork.NetworkPeerCount > 0)
            {
                var peer = GameNetwork.NetworkPeers[0];
                if (peer.ControlledAgent != null)
                {
                    GameNetwork.BeginModuleEventAsServer(peer);
                    string x = peer.ControlledAgent.Position.x.ToString();
                    string y = peer.ControlledAgent.Position.y.ToString();
                    string z = peer.ControlledAgent.Position.z.ToString();
                    string w = peer.ControlledAgent.Position.w.ToString();
                    GameNetwork.WriteMessage(new ServerMessage("[!]: [" + x + "," + y + "," + z + "," + w + "]", false));
                    GameNetwork.EndModuleEventAsServer();
                }
            }
        }

        public override void AfterStart()
        {

        }

        public bool EquipSelectedItem(NetworkCommunicator networkPeer, StartEquipItem baseMessage)
        {
            var peer = baseMessage.Player;
            var item = baseMessage.Item;

            var currentEquipment = peer.ControlledAgent.SpawnEquipment;
            var index = ViewModelLib.GetItemEquipmentIndex(item);
            currentEquipment[index] = new EquipmentElement(item);
            peer.ControlledAgent.UpdateSpawnEquipmentAndRefreshVisuals(currentEquipment);

            Debug.Print("Equiping Selected Item" + item.Name, 0, Debug.DebugColor.Red);

            return true;
        }

        public void SpawnChest(NetworkCommunicator networkPeer)
        {
            if(GameNetwork.IsServer)
            {

                GameNetwork.BeginModuleEventAsServer(networkPeer);
                GameNetwork.WriteMessage(new StartSpawnChest());
                GameNetwork.EndModuleEventAsServer();

                //foreach (var location in LootLocations.Locations)
                //{
                //    var frame = new MatrixFrame(Mat3.Identity, new Vec3(location.Item1, location.Item2, location.Item3, location.Item4));
                //    GameEntity gameEntity = GameEntity.Instantiate(Mission.Current.Scene, "loot_chest", frame);
                //    LootChest chest = gameEntity.GetFirstScriptOfType<LootChest>();
                //    var inventoryVm = new BannerRoyalInventoryVM(Mission.Current);
                //    inventoryVm.SetChestItems(LootPools.TestPool());
                //    chest.SetViewModel(inventoryVm);
                //    chest.SetTag("loot_chest_server");
                //}
                chestSpawnComplete = true;
            }
            
        }

        public void SpawnArmor(NetworkCommunicator networkPeer)
        {
            if (GameNetwork.IsServer)
            {

                //GameNetwork.BeginModuleEventAsServer(networkPeer);
                //GameNetwork.WriteMessage(new StartSpawnArmor());
                //GameNetwork.EndModuleEventAsServer();


                //var armorItem = MBObjectManager.Instance.GetObject<ItemObject>("mp_desert_helmet");
                //MissionWeapon armorMissionWeapon = new MissionWeapon(armorItem, null, null);
                //WeaponData weaponData = armorMissionWeapon.GetWeaponData(true);


                //MatrixFrame frame = new MatrixFrame(Mat3.Identity, new Vec3(334.74f, 596.142f, 18.32f, position.w));
                //var armorObject = base.Mission.CreateMissionObjectFromPrefab("spawned_armor", frame);
                //var spawnedArmor = (BannerRoyalMPLib.SpawnedArmor)armorObject;
                //spawnedArmor.SetArmorItem(armorItem);
                //var armorEntity = spawnedArmor.GameEntity;
                //armorEntity.AddPhysics(weaponData.BaseWeight, weaponData.CenterOfMassShift, weaponData.Shape, Vec3.One, Vec3.Zero, PhysicsMaterial.GetFromIndex(weaponData.PhysicsMaterialIndex), false, 0);
                //armorEntity.SetPhysicsState(true, true);
                //armorEntity.SetMobility(GameEntity.Mobility.dynamic_forced);

                armorSpawnComplete = true;
            }

        }

    }
}
