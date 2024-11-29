using System;
using System.Collections.Generic;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;
using System.Linq;
using TaleWorlds.MountAndBlade.GauntletUI.Widgets.Inventory;

namespace BannerRoyalMPClient
{
    // Token: 0x02000133 RID: 307
    public class BannerRoyalItemTuppleWidget : InventoryItemButtonWidget
    {
        // Token: 0x17000595 RID: 1429
        // (get) Token: 0x06000FD9 RID: 4057 RVA: 0x0002BA16 File Offset: 0x00029C16
        // (set) Token: 0x06000FDA RID: 4058 RVA: 0x0002BA1E File Offset: 0x00029C1E
        public InventoryImageIdentifierWidget ItemImageIdentifier { get; set; }

        // Token: 0x06000FDB RID: 4059 RVA: 0x0002BA28 File Offset: 0x00029C28
        public BannerRoyalItemTuppleWidget(UIContext context) : base(context)
        {
            this.AcceptDrag = true;
            this.AcceptDrop = true;
            base.AddState("Selected");
        }

        protected override void OnDragBegin()
        {
            base.OnDragBegin();
        }

        protected override void OnDragEnd()
        {
            var draggedWidget = (BannerRoyalItemTuppleWidget)this.EventManager.DraggedWidget;
            if (draggedWidget != null)
            {
                draggedWidget.DropItem();
            }
            base.OnDragEnd();
        }

        public void DropItem()
        {
            EventFired("DropItem");
        }
        protected override bool OnDrop()
        {
            
            var draggedWidget = (BannerRoyalItemTuppleWidget)this.EventManager.DraggedWidget;

            if (this.WidgetType == draggedWidget.WidgetType || this.WidgetType == "Inventory")
            {
                   
                var currentMainWidget = this.Children.FirstOrDefault(x => x.Id == "MainWidget");
                if (draggedWidget != null)
                {
                    var draggedMainWidget = draggedWidget.Children.FirstOrDefault(x => x.Id == "MainWidget");
                    var currentDragWidget = this.DragWidget;


                    this.DragWidget = draggedWidget.DragWidget;
                    this.RemoveChild(currentMainWidget);
                    draggedWidget.RemoveChild(draggedMainWidget);

                    this.AddChild(draggedMainWidget);
                    draggedWidget.AddChild(currentMainWidget);


                    draggedWidget.DragWidget = currentDragWidget;

                    var draggedWidgetParent = draggedWidget.ParentWidget;
                    draggedWidgetParent.RemoveChild(draggedWidget);
                    draggedWidgetParent.AddChild(draggedWidget);

                    if(this.WidgetType == "Armor" || this.WidgetType == "Weapon")
                    {
                        draggedWidget.EquipItem();
                    }

                    //base.OnDrop();
                    return true;
                }
                base.OnDrop();
                return false;
            }
            base.OnDrop();
            return false;
        }


        // Token: 0x06000FDC RID: 4060 RVA: 0x0002BAA4 File Offset: 0x00029CA4
        private void SetWidgetsState(string state)
        {
            this.SetState(state);
            string currentState = "Default";
            if (currentState == "Default" && base.IsSelected)
            {
                base.EventFired("Opened", Array.Empty<object>());
                return;
            }
            if (currentState == "Selected" && !base.IsSelected)
            {
                base.EventFired("Closed", Array.Empty<object>());

            }
        }


        // Token: 0x06000FDE RID: 4062 RVA: 0x0002BBCC File Offset: 0x00029DCC
        protected override void RefreshState()
        {
            base.RefreshState();
            bool isVisible = this.IsVisible;
            if (base.IsDisabled)
            {
                this.SetWidgetsState("Disabled");
                return;
            }
            if (base.IsPressed)
            {
                this.SetWidgetsState("Pressed");
                return;
            }
            if (base.IsHovered)
            {
                this.SetWidgetsState("Hovered");
                return;
            }
            if (base.IsSelected)
            {
                this.SetWidgetsState("Selected");
                return;
            }
            this.SetWidgetsState("Default");
        }


        // Token: 0x06000FE0 RID: 4064 RVA: 0x0002BDAF File Offset: 0x00029FAF
        private void OnViewClick(Widget widget)
        {
            if (base.ScreenWidget != null)
            {
                base.ScreenWidget.ItemPreviewWidget.SetLastFocusedItem(this);
            }
        }

        // Token: 0x06000FE1 RID: 4065 RVA: 0x0002BDCA File Offset: 0x00029FCA
        private void OnEquipClick(Widget widget)
        {
            base.EquipItem();
        }

        // Token: 0x06000FE2 RID: 4066 RVA: 0x0002BDD4 File Offset: 0x00029FD4
        private void OnTransferClick(Widget widget, int count)
        {
            foreach (Action<BannerRoyalItemTuppleWidget> action in this.TransferRequestHandlers)
            {
                action(this);
            }
            if (base.IsRightSide)
            {
                this.ProcessBuyItem(true, count);
                return;
            }
            this.ProcessSellItem(true, count);
        }

        // Token: 0x06000FE3 RID: 4067 RVA: 0x0002BE40 File Offset: 0x0002A040
        private void OnSliderTransferClick(int count)
        {
        }

        // Token: 0x06000FE4 RID: 4068 RVA: 0x0002BE42 File Offset: 0x0002A042
        public void ProcessBuyItem(bool playSound, int count = -1)
        {
            if (count == -1)
            {
                count = this.TransactionCount;
            }
            this.TransactionCount = count;
            base.ScreenWidget.TransactionCount = count;
            base.ScreenWidget.TargetEquipmentIndex = -1;
            this.TransferButton.FireClickEvent();
        }

        // Token: 0x06000FE5 RID: 4069 RVA: 0x0002BE7C File Offset: 0x0002A07C
        public void ProcessSellItem(bool playSound, int count = -1)
        {
            if (count == -1)
            {
                count = this.TransactionCount;
            }
            this.TransactionCount = count;
            base.ScreenWidget.TransactionCount = count;
            base.ScreenWidget.TargetEquipmentIndex = -1;
            this.TransferButton.FireClickEvent();
        }

        // Token: 0x06000FE6 RID: 4070 RVA: 0x0002BEB6 File Offset: 0x0002A0B6
        private void ProcessSelectItem()
        {
            if (base.ScreenWidget != null)
            {
                base.IsSelected = true;
                base.ScreenWidget.SetCurrentTuple(this, !base.IsRightSide);
            }
        }

        // Token: 0x06000FE7 RID: 4071 RVA: 0x0002BEDC File Offset: 0x0002A0DC
        protected override void OnMouseReleased()
        {
            base.OnMouseReleased();
            this.ProcessSelectItem();
        }

        // Token: 0x06000FE8 RID: 4072 RVA: 0x0002BEEA File Offset: 0x0002A0EA
        protected override void OnMouseAlternateReleased()
        {
            base.OnMouseAlternateReleased();
            base.EventFired("OnAlternateRelease", Array.Empty<object>());
        }
        // Token: 0x06000FEB RID: 4075 RVA: 0x0002BF4F File Offset: 0x0002A14F
        private void SliderIntPropertyChanged(PropertyOwnerObject owner, string propertyName, int value)
        {
            if (propertyName == "ValueInt")
            {
                this.TransactionCount = this._slider.ValueInt;
            }
        }

        // Token: 0x06000FEC RID: 4076 RVA: 0x0002BF70 File Offset: 0x0002A170
        private void SliderValuePropertyChanged(PropertyOwnerObject owner, string propertyName, object value)
        {
            if (propertyName == "OnMousePressed")
            {
                foreach (Action<BannerRoyalItemTuppleWidget> action in this.TransferRequestHandlers)
                {
                    action(this);
                }
            }
        }

        // Token: 0x06000FED RID: 4077 RVA: 0x0002BFD0 File Offset: 0x0002A1D0
        private void CountTextWidgetOnPropertyChanged(PropertyOwnerObject owner, string propertyName, int value)
        {
            if (propertyName == "IntText")
            {
                this.UpdateCountText();
            }
        }

        // Token: 0x06000FEF RID: 4079 RVA: 0x0002BFFA File Offset: 0x0002A1FA
        private void UpdateCountText()
        {
            if (this.SliderTextWidget != null)
            {
                this.SliderTextWidget.IsHidden = this.CountTextWidget.IsHidden;
            }
        }

        // Token: 0x06000FF0 RID: 4080 RVA: 0x0002C01C File Offset: 0x0002A21C
        private void UpdateCostText()
        {
            if (this.CostTextWidget == null)
            {
                return;
            }
            switch (this.ProfitState)
            {
                case -2:
                    this.CostTextWidget.SetState("VeryBad");
                    return;
                case -1:
                    this.CostTextWidget.SetState("Bad");
                    return;
                case 0:
                    this.CostTextWidget.SetState("Default");
                    return;
                case 1:
                    this.CostTextWidget.SetState("Good");
                    return;
                case 2:
                    this.CostTextWidget.SetState("VeryGood");
                    return;
                default:
                    return;
            }
        }

        // Token: 0x06000FF1 RID: 4081 RVA: 0x0002C0AB File Offset: 0x0002A2AB
        private void UpdateDragAvailability()
        {
            base.AcceptDrag = true;
        }

        // Token: 0x17000596 RID: 1430
        // (get) Token: 0x06000FF2 RID: 4082 RVA: 0x0002C0D0 File Offset: 0x0002A2D0
        // (set) Token: 0x06000FF3 RID: 4083 RVA: 0x0002C0D8 File Offset: 0x0002A2D


        // Token: 0x17000598 RID: 1432
        // (get) Token: 0x06000FF6 RID: 4086 RVA: 0x0002C131 File Offset: 0x0002A331
        // (set) Token: 0x06000FF7 RID: 4087 RVA: 0x0002C13C File Offset: 0x0002A33C
        [Editor(false)]
        public TextWidget CountTextWidget
        {
            get
            {
                return this._countTextWidget;
            }
            set
            {
                if (this._countTextWidget != value)
                {
                    if (this._countTextWidget != null)
                    {
                        this._countTextWidget.intPropertyChanged -= this.CountTextWidgetOnPropertyChanged;
                    }
                    this._countTextWidget = value;
                    if (this._countTextWidget != null)
                    {
                        this._countTextWidget.intPropertyChanged += this.CountTextWidgetOnPropertyChanged;
                    }
                    base.OnPropertyChanged<TextWidget>(value, "CountTextWidget");
                    this.UpdateCountText();
                }
            }
        }

        // Token: 0x17000599 RID: 1433
        // (get) Token: 0x06000FF8 RID: 4088 RVA: 0x0002C1A9 File Offset: 0x0002A3A9
        // (set) Token: 0x06000FF9 RID: 4089 RVA: 0x0002C1B1 File Offset: 0x0002A3B1
        [Editor(false)]
        public TextWidget CostTextWidget
        {
            get
            {
                return this._costTextWidget;
            }
            set
            {
                if (this._costTextWidget != value)
                {
                    this._costTextWidget = value;
                    this.UpdateCostText();
                    base.OnPropertyChanged<TextWidget>(value, "CostTextWidget");
                }
            }
        }

        // Token: 0x1700059A RID: 1434
        // (get) Token: 0x06000FFA RID: 4090 RVA: 0x0002C1D5 File Offset: 0x0002A3D5
        // (set) Token: 0x06000FFB RID: 4091 RVA: 0x0002C1DD File Offset: 0x0002A3DD
        public int ProfitState
        {
            get
            {
                return this._profitState;
            }
            set
            {
                if (value != this._profitState)
                {
                    this._profitState = value;
                    this.UpdateCostText();
                    base.OnPropertyChanged(value, "ProfitState");
                }
            }
        }
       

        // Token: 0x1700059D RID: 1437
        // (get) Token: 0x06001000 RID: 4096 RVA: 0x0002C24D File Offset: 0x0002A44D
        // (set) Token: 0x06001001 RID: 4097 RVA: 0x0002C258 File Offset: 0x0002A458
        [Editor(false)]
        public InventoryTwoWaySliderWidget Slider
        {
            get
            {
                return this._slider;
            }
            set
            {
                if (this._slider != value)
                {
                    if (this._slider != null)
                    {
                        this._slider.intPropertyChanged -= this.SliderIntPropertyChanged;
                        this._slider.PropertyChanged -= this.SliderValuePropertyChanged;
                    }
                    this._slider = value;
                    if (this._slider != null)
                    {
                        this._slider.intPropertyChanged += this.SliderIntPropertyChanged;
                        this._slider.PropertyChanged += this.SliderValuePropertyChanged;
                    }
                    base.OnPropertyChanged<InventoryTwoWaySliderWidget>(value, "Slider");
                    this.Slider.AddState("Selected");
                    this.Slider.OverrideDefaultStateSwitchingEnabled = true;
                }
            }
        }

        // Token: 0x1700059E RID: 1438
        // (get) Token: 0x06001002 RID: 4098 RVA: 0x0002C30C File Offset: 0x0002A50C
        // (set) Token: 0x06001003 RID: 4099 RVA: 0x0002C314 File Offset: 0x0002A514
        [Editor(false)]
        public Widget SliderParent
        {
            get
            {
                return this._sliderParent;
            }
            set
            {
                if (this._sliderParent != value)
                {
                    this._sliderParent = value;
                    base.OnPropertyChanged<Widget>(value, "SliderParent");
                    this.SliderParent.AddState("Selected");
                }
            }
        }

        // Token: 0x1700059F RID: 1439
        // (get) Token: 0x06001004 RID: 4100 RVA: 0x0002C342 File Offset: 0x0002A542
        // (set) Token: 0x06001005 RID: 4101 RVA: 0x0002C34A File Offset: 0x0002A54A
        [Editor(false)]
        public TextWidget SliderTextWidget
        {
            get
            {
                return this._sliderTextWidget;
            }
            set
            {
                if (this._sliderTextWidget != value)
                {
                    this._sliderTextWidget = value;
                    base.OnPropertyChanged<TextWidget>(value, "SliderTextWidget");
                    this.SliderTextWidget.AddState("Selected");
                }
            }
        }
        public Widget MainWidget
        {
            get
            {
                return this._mainWidget;
            }
            set
            {
                if (this._mainWidget != value)
                {
                    this._mainWidget = value;
                    base.OnPropertyChanged<Widget>(value, "MainWidget");
                }
            }
        }

        // Token: 0x170005A0 RID: 1440
        // (get) Token: 0x06001006 RID: 4102 RVA: 0x0002C378 File Offset: 0x0002A578
        // (set) Token: 0x06001007 RID: 4103 RVA: 0x0002C380 File Offset: 0x0002A580
        [Editor(false)]
        public bool IsTransferable
        {
            get
            {
                return this._isTransferable;
            }
            set
            {
                if (this._isTransferable != value)
                {
                    this._isTransferable = value;
                    base.OnPropertyChanged(value, "IsTransferable");
                    this.UpdateDragAvailability();
                }
            }
        }

        // Token: 0x170005A1 RID: 1441
        // (get) Token: 0x06001008 RID: 4104 RVA: 0x0002C3A4 File Offset: 0x0002A5A4
        // (set) Token: 0x06001009 RID: 4105 RVA: 0x0002C3AC File Offset: 0x0002A5AC
        [Editor(false)]
        public ButtonWidget EquipButton
        {
            get
            {
                return this._equipButton;
            }
            set
            {
                if (this._equipButton != value)
                {
                    ButtonWidget equipButton = this._equipButton;
                    if (equipButton != null)
                    {
                        equipButton.ClickEventHandlers.Remove(this._equipClickHandler);
                    }
                    this._equipButton = value;
                    ButtonWidget equipButton2 = this._equipButton;
                    if (equipButton2 != null)
                    {
                        equipButton2.ClickEventHandlers.Add(this._equipClickHandler);
                    }
                    base.OnPropertyChanged<ButtonWidget>(value, "EquipButton");
                }
            }
        }

        // Token: 0x170005A2 RID: 1442
        // (get) Token: 0x0600100A RID: 4106 RVA: 0x0002C40E File Offset: 0x0002A60E
        // (set) Token: 0x0600100B RID: 4107 RVA: 0x0002C418 File Offset: 0x0002A618
        [Editor(false)]
        public ButtonWidget ViewButton
        {
            get
            {
                return this._viewButton;
            }
            set
            {
                if (this._viewButton != value)
                {
                    ButtonWidget viewButton = this._viewButton;
                    if (viewButton != null)
                    {
                        viewButton.ClickEventHandlers.Remove(this._viewClickHandler);
                    }
                    this._viewButton = value;
                    ButtonWidget viewButton2 = this._viewButton;
                    if (viewButton2 != null)
                    {
                        viewButton2.ClickEventHandlers.Add(this._viewClickHandler);
                    }
                    base.OnPropertyChanged<ButtonWidget>(value, "ViewButton");
                }
            }
        }

        // Token: 0x170005A3 RID: 1443
        // (get) Token: 0x0600100C RID: 4108 RVA: 0x0002C47A File Offset: 0x0002A67A
        // (set) Token: 0x0600100D RID: 4109 RVA: 0x0002C484 File Offset: 0x0002A684
        [Editor(false)]
        public InventoryTransferButtonWidget TransferButton
        {
            get
            {
                return this._transferButton;
            }
            set
            {
                if (this._transferButton != value)
                {
                    InventoryTransferButtonWidget transferButton = this._transferButton;
                    if (transferButton != null)
                    {
                        transferButton.ClickEventHandlers.Remove(this._transferClickHandler);
                    }
                    this._transferButton = value;
                    InventoryTransferButtonWidget transferButton2 = this._transferButton;
                    if (transferButton2 != null)
                    {
                        transferButton2.ClickEventHandlers.Add(this._transferClickHandler);
                    }
                    base.OnPropertyChanged<InventoryTransferButtonWidget>(value, "TransferButton");
                }
            }
        }

        // Token: 0x170005A4 RID: 1444
        // (get) Token: 0x0600100E RID: 4110 RVA: 0x0002C4E6 File Offset: 0x0002A6E6
        // (set) Token: 0x0600100F RID: 4111 RVA: 0x0002C4F0 File Offset: 0x0002A6F0
        [Editor(false)]
        public ButtonWidget SliderTransferButton
        {
            get
            {
                return this._sliderTransferButton;
            }
            set
            {
                if (this._sliderTransferButton != value)
                {
                    ButtonWidget sliderTransferButton = this._sliderTransferButton;
                    if (sliderTransferButton != null)
                    {
                        sliderTransferButton.ClickEventHandlers.Remove(this._sliderTransferClickHandler);
                    }
                    this._sliderTransferButton = value;
                    ButtonWidget sliderTransferButton2 = this._sliderTransferButton;
                    if (sliderTransferButton2 != null)
                    {
                        sliderTransferButton2.ClickEventHandlers.Add(this._sliderTransferClickHandler);
                    }
                    base.OnPropertyChanged<ButtonWidget>(value, "SliderTransferButton");
                }
            }
        }

        // Token: 0x170005A5 RID: 1445
        // (get) Token: 0x06001010 RID: 4112 RVA: 0x0002C552 File Offset: 0x0002A752
        // (set) Token: 0x06001011 RID: 4113 RVA: 0x0002C55A File Offset: 0x0002A75A
        [Editor(false)]
        public int TransactionCount
        {
            get
            {
                return this._transactionCount;
            }
            set
            {
                if (this._transactionCount != value)
                {
                    this._transactionCount = value;
                    base.OnPropertyChanged(value, "TransactionCount");
                }
            }
        }

        // Token: 0x170005A6 RID: 1446
        // (get) Token: 0x06001012 RID: 4114 RVA: 0x0002C578 File Offset: 0x0002A778
        // (set) Token: 0x06001013 RID: 4115 RVA: 0x0002C580 File Offset: 0x0002A780
        [Editor(false)]
        public int ItemCount
        {
            get
            {
                return this._itemCount;
            }
            set
            {
                if (this._itemCount != value)
                {
                    this._itemCount = value;
                    base.OnPropertyChanged(value, "ItemCount");
                    this.UpdateDragAvailability();
                }
            }
        }

        // Token: 0x170005A7 RID: 1447
        // (get) Token: 0x06001014 RID: 4116 RVA: 0x0002C5A4 File Offset: 0x0002A7A4
        // (set) Token: 0x06001015 RID: 4117 RVA: 0x0002C5AC File Offset: 0x0002A7AC
        [Editor(false)]
        public bool IsCivilian
        {
            get
            {
                return this._isCivilian;
            }
            set
            {
                if (this._isCivilian != value || !this._isCivilianStateSet)
                {
                    this._isCivilian = value;
                    base.OnPropertyChanged(value, "IsCivilian");
                    this._isCivilianStateSet = true;
                }
            }
        }

        // Token: 0x170005A8 RID: 1448
        // (get) Token: 0x06001016 RID: 4118 RVA: 0x0002C5DF File Offset: 0x0002A7DF
        // (set) Token: 0x06001017 RID: 4119 RVA: 0x0002C5E7 File Offset: 0x0002A7E7
        [Editor(false)]
        public bool IsGenderDifferent
        {
            get
            {
                return this._isGenderDifferent;
            }
            set
            {
                if (this._isGenderDifferent != value)
                {
                    this._isGenderDifferent = value;
                    base.OnPropertyChanged(value, "IsGenderDifferent");
                }
            }
        }

        [Editor(false)]
        public string WidgetType
        {
            get
            {
                return this._widgetType;
            }
            set
            {
                if (this._widgetType != value)
                {
                    this._widgetType = value;
                    base.OnPropertyChanged<string>(value, "WidgetType");
                }
            }
        }

        // Token: 0x170005A9 RID: 1449
        // (get) Token: 0x06001018 RID: 4120 RVA: 0x0002C60B File Offset: 0x0002A80B
        // (set) Token: 0x06001019 RID: 4121 RVA: 0x0002C613 File Offset: 0x0002A813
        [Editor(false)]
        public bool IsEquipable
        {
            get
            {
                return this._isEquipable;
            }
            set
            {
                if (this._isEquipable != value)
                {
                    this._isEquipable = value;
                    base.OnPropertyChanged(value, "IsEquipable");
                    this.UpdateDragAvailability();
                }
            }
        }

        // Token: 0x170005AA RID: 1450
        // (get) Token: 0x0600101A RID: 4122 RVA: 0x0002C637 File Offset: 0x0002A837
        // (set) Token: 0x0600101B RID: 4123 RVA: 0x0002C63F File Offset: 0x0002A83F
        [Editor(false)]
        public bool IsNewlyAdded
        {
            get
            {
                return this._isNewlyAdded;
            }
            set
            {
                if (this._isNewlyAdded != value)
                {
                    this._isNewlyAdded = value;
                    base.OnPropertyChanged(value, "IsNewlyAdded");
                    this.ItemImageIdentifier.SetRenderRequestedPreviousFrame(value);
                }
            }
        }

        // Token: 0x170005AC RID: 1452
        // (get) Token: 0x0600101E RID: 4126 RVA: 0x0002C695 File Offset: 0x0002A895
        // (set) Token: 0x0600101F RID: 4127 RVA: 0x0002C69D File Offset: 0x0002A89D
        [Editor(false)]
        public Brush DefaultBrush
        {
            get
            {
                return this._defaultBrush;
            }
            set
            {
                if (this._defaultBrush != value)
                {
                    this._defaultBrush = value;
                    base.OnPropertyChanged<Brush>(value, "DefaultBrush");
                }
            }
        }

        // Token: 0x170005AD RID: 1453
        // (get) Token: 0x06001020 RID: 4128 RVA: 0x0002C6BB File Offset: 0x0002A8BB
        // (set) Token: 0x06001021 RID: 4129 RVA: 0x0002C6C3 File Offset: 0x0002A8C3
        [Editor(false)]
        public Brush CivilianDisabledBrush
        {
            get
            {
                return this._civilianDisabledBrush;
            }
            set
            {
                if (this._civilianDisabledBrush != value)
                {
                    this._civilianDisabledBrush = value;
                    base.OnPropertyChanged<Brush>(value, "CivilianDisabledBrush");
                }
            }
        }

        // Token: 0x170005AE RID: 1454
        // (get) Token: 0x06001022 RID: 4130 RVA: 0x0002C6E1 File Offset: 0x0002A8E1
        // (set) Token: 0x06001023 RID: 4131 RVA: 0x0002C6E9 File Offset: 0x0002A8E9
        [Editor(false)]
        public Brush CharacterCantUseBrush
        {
            get
            {
                return this._characterCantUseBrush;
            }
            set
            {
                if (this._characterCantUseBrush != value)
                {
                    this._characterCantUseBrush = value;
                    base.OnPropertyChanged<Brush>(value, "CharacterCantUseBrush");
                }
            }
        }

        

        // Token: 0x04000735 RID: 1845
        private readonly Action<Widget> _viewClickHandler;

        // Token: 0x04000736 RID: 1846
        private readonly Action<Widget> _equipClickHandler;

        // Token: 0x04000737 RID: 1847
        private readonly Action<Widget> _transferClickHandler;

        // Token: 0x04000738 RID: 1848
        private readonly Action<Widget> _sliderTransferClickHandler;

        // Token: 0x04000739 RID: 1849
        public List<Action<BannerRoyalItemTuppleWidget>> TransferRequestHandlers = new List<Action<BannerRoyalItemTuppleWidget>>();

        // Token: 0x0400073A RID: 1850
        private bool _isCivilianStateSet;

        // Token: 0x0400073B RID: 1851
        private float _extendedUpdateTimer;

        // Token: 0x0400073C RID: 1852
        private TextWidget _nameTextWidget;

        // Token: 0x0400073D RID: 1853
        private TextWidget _countTextWidget;

        // Token: 0x0400073E RID: 1854
        private TextWidget _costTextWidget;

        // Token: 0x0400073F RID: 1855
        private int _profitState;


        // Token: 0x04000741 RID: 1857
        private InventoryTupleExtensionControlsWidget _extendedControlsContainer;

        // Token: 0x04000742 RID: 1858
        private InventoryTwoWaySliderWidget _slider;

        // Token: 0x04000743 RID: 1859
        private Widget _sliderParent;

        // Token: 0x04000744 RID: 1860
        private TextWidget _sliderTextWidget;

        private Widget _mainWidget;

        // Token: 0x04000745 RID: 1861
        private bool _isTransferable;

        // Token: 0x04000746 RID: 1862
        private ButtonWidget _equipButton;

        // Token: 0x04000747 RID: 1863
        private ButtonWidget _viewButton;

        // Token: 0x04000748 RID: 1864
        private InventoryTransferButtonWidget _transferButton;

        // Token: 0x04000749 RID: 1865
        private ButtonWidget _sliderTransferButton;

        // Token: 0x0400074A RID: 1866
        private int _transactionCount;

        // Token: 0x0400074B RID: 1867
        private int _itemCount;

        // Token: 0x0400074C RID: 1868
        private bool _isCivilian;

        // Token: 0x0400074D RID: 1869
        private bool _isGenderDifferent;

        // Token: 0x0400074E RID: 1870
        private bool _isEquipable;

        // Token: 0x0400074F RID: 1871
        private bool _canCharacterUseItem;

        // Token: 0x04000750 RID: 1872
        private bool _isNewlyAdded;

        // Token: 0x04000751 RID: 1873
        private Brush _defaultBrush;

        // Token: 0x04000752 RID: 1874
        private Brush _civilianDisabledBrush;

        // Token: 0x04000753 RID: 1875
        private Brush _characterCantUseBrush;

        // Token: 0x04000754 RID: 1876
        private string _widgetType;



    }
}
