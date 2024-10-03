using BannerRoyalMPLib;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade;
using TaleWorlds.ObjectSystem;

namespace BannerRoyalMPServer
{
    public class SpawnChestLogic : MissionLogic
    {
        bool complete = false;
        public override void OnMissionTick(float dt)
        {
            foreach (var peer in GameNetwork.NetworkPeers)
            {
                if (peer.ControlledAgent != null)
                { 
                    if (!complete)
                    {
                        SpawnChest(peer.ControlledAgent.Frame);
                    }

                    var currentEquipment = peer.ControlledAgent.SpawnEquipment;
                    currentEquipment[EquipmentIndex.Head] = new EquipmentElement(MBObjectManager.Instance.GetObject<ItemObject>("mp_pilgrim_hood"));
                    peer.ControlledAgent.UpdateSpawnEquipmentAndRefreshVisuals(currentEquipment);
                }
            }            
        }

        private void SpawnChest(MatrixFrame frame)
        {
            var chest = base.Mission.CreateMissionObjectFromPrefab("loot_chest", frame);
            
            complete = true;
        }
    }
}
