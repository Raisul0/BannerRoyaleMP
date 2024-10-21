using BannerRoyalMPLib;
using BannerRoyalMPLib.Globals;
using BannerRoyalMPLib.NetworkMessages;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Network.Messages;

namespace BannerRoyalMPServer.Extensions
{
    public class SpawnChestBehavior : MissionNetwork
    {
        public bool spawnComplete = false;

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
        public override void OnMissionTick(float dt)
        {
            base.OnMissionTick(dt);
            foreach (var peer in GameNetwork.NetworkPeers)
            {
                if (peer.ControlledAgent != null)
                {
                    if (!spawnComplete)
                    {
                        this.SpawnChest();
                    }

                    //GameNetwork.BeginModuleEventAsServer(peer);
                    //string x = peer.ControlledAgent.Position.x.ToString();
                    //string y = peer.ControlledAgent.Position.y.ToString();
                    //string z = peer.ControlledAgent.Position.z.ToString();
                    //string w = peer.ControlledAgent.Position.w.ToString();
                    //GameNetwork.WriteMessage(new ServerMessage("[!]: [" + x + "," + y + "," + z + "," + w + "]", false));
                    //GameNetwork.EndModuleEventAsServer();

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

        public void SpawnChest()
        {
            if(GameNetwork.IsServer)
            {
                foreach (var location in LootLocations.Locations)
                {
                    var frame = new MatrixFrame(Mat3.Identity, new Vec3(location.Item1, location.Item2, location.Item3, location.Item4));
                    //var chest = GameEntity.Instantiate(this.Mission.Scene, "loot_chest", frame);
                    //GameEntity gameEntity = GameEntity.Instantiate(this.Mission.Scene, "loot_chest", frame);
                    var chest = base.Mission.CreateMissionObjectFromPrefab("loot_chest", frame);
                }
                spawnComplete = true;
            }
            
        }

    }
}
