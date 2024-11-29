using BannerRoyalMPLib.Globals;

using BannerRoyalMPLib.NetworkMessages.FromClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.InputSystem;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade;

namespace BannerRoyalMPLib
{
    public class SpawnedArmor : UsableMachine
    {
        public ItemObject ArmorItem { get;set; }
        public bool isFocused { get; set; }
        public SpawnedArmor()
        {

        }

        public override TextObject GetActionTextForStandingPoint(UsableMissionObject usableGameObject)
        {
            TextObject textObject = new TextObject("{=bl2aRW8f}{KEY} Equip Armor", null);
            textObject.SetTextVariable("KEY", HyperlinkTexts.GetKeyHyperlinkText("L"));
            return textObject;
        }

        protected override void OnInit()
        {
            base.OnInit();
            isFocused = false;
            base.GameEntity.Name = "SpawnedArmor";
        }

        protected override void OnTick(float dt)
        {
            if (Input.IsKeyPressed(TaleWorlds.InputSystem.InputKey.L)){
                if (isFocused)
                {
                    EquipArmor();
                    
                }
            }
            base.OnTick(dt);
        }

        public override ScriptComponentBehavior.TickRequirement GetTickRequirement()
        {
            return ScriptComponentBehavior.TickRequirement.Tick | base.GetTickRequirement();
        }

        public override void OnFocusGain(Agent userAgent)
        {
            isFocused=true;
            base.OnFocusGain(userAgent);
        }

        // Token: 0x06002FBD RID: 12221 RVA: 0x000C57F0 File Offset: 0x000C39F0
        public override void OnFocusLose(Agent userAgent)
        {
            isFocused = false;
            base.OnFocusLose(userAgent);
        }

        public override string GetDescriptionText(GameEntity gameEntity = null)
        {
            return "Armor";
        }

        public void SetArmorItem(ItemObject armorItem)
        {
            ArmorItem = armorItem;
        }

        public void EquipArmor()
        {
            if (ArmorItem != null)
            {
                GameNetwork.BeginModuleEventAsClient();
                GameNetwork.WriteMessage(new StartEquipItem(ArmorItem, GameNetwork.MyPeer));
                GameNetwork.EndModuleEventAsClient();
            }
        }
    }
}
