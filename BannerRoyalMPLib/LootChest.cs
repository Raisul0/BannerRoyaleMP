using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade;

namespace BannerRoyalMPLib
{
    public class LootChest : UsableMachine
    {
        public LootChest() {

        }
        public override TextObject GetActionTextForStandingPoint(UsableMissionObject usableGameObject)
        {
            TextObject textObject = new TextObject("{=bl2aRW8f}{KEY} Open Chest ", null);
            textObject.SetTextVariable("KEY", HyperlinkTexts.GetKeyHyperlinkText("L"));
            return textObject;
        }

        public override string GetDescriptionText(GameEntity gameEntity = null)
        {
            return "Loot Chest";
        }

        protected override void OnInit()
        {
            base.OnInit();
            base.Id = new MissionObjectId(1010101,true);
            _inventoryVM = new BannerRoyalInventoryVM(Mission.Current);
            base.GameEntity.Name = "LootChest";
        }

        public override ScriptComponentBehavior.TickRequirement GetTickRequirement()
        {
            return ScriptComponentBehavior.TickRequirement.Tick | base.GetTickRequirement();
        }
        

        public override void OnFocusGain(Agent userAgent)
        {
            //base.OnFocusGain(userAgent);
            _inventoryVM._isLootBoxFocused = true;
        }

        // Token: 0x06002FBD RID: 12221 RVA: 0x000C57F0 File Offset: 0x000C39F0
        public override void OnFocusLose(Agent userAgent)
        {
            //base.OnFocusLose(userAgent);
            _inventoryVM._isLootBoxFocused = false;
        }

        public void SetViewModel(BannerRoyalInventoryVM inventoryVM)
        {
            _inventoryVM = inventoryVM;
        }

        public BannerRoyalInventoryVM _inventoryVM;
    }
}
