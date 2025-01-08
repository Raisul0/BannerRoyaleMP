using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.Core;

namespace BannerRoyalMPLib.Globals
{
    public static class CustomAgent
    {
        public static EquipmentElement Head    = new EquipmentElement(LootPools.GetItemObjectfromString("mp_aserai_civil_c_head"));
        public static EquipmentElement Body    = new EquipmentElement(LootPools.GetItemObjectfromString("mp_southern_lamellar_armor"));
        public static EquipmentElement Leg     = new EquipmentElement(LootPools.GetItemObjectfromString("mp_strapped_shoes"));
        public static EquipmentElement Gloves  = new EquipmentElement(LootPools.GetItemObjectfromString("mp_reinforced_mail_mitten"));
        public static EquipmentElement Cape    = new EquipmentElement(LootPools.GetItemObjectfromString("mp_brass_scale_shoulders"));
    }
}
