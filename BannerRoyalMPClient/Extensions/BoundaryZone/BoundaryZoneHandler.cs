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
    public class BoundaryZoneHandler : IHandlerRegister
    {
        public void Register(GameNetwork.NetworkMessageHandlerRegisterer reg)
        {
            reg.Register<SetBoundaryZone>(SetBoundary);
        }

        public void SetBoundary(SetBoundaryZone message)
        {
            BoundaryZoneBehavior boundaryZoneBehavior = Mission.Current.GetMissionBehavior<BoundaryZoneBehavior>();
            boundaryZoneBehavior.InitZone(message.Origin, message.Radius, message.LifeTime, message.Visible);
        }

    }
}
