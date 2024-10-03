using System;
using System.Collections.Generic;
using TaleWorlds.Core.ViewModelCollection.Information;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.ObjectSystem;

namespace BannerRoyalMPLib
{
    public class ButtonObjectVM<T> : ViewModel
    {
        // Token: 0x1700021E RID: 542
        // (get) Token: 0x06000887 RID: 2183 RVA: 0x00026380 File Offset: 0x00024580
        // (set) Token: 0x06000888 RID: 2184 RVA: 0x00026398 File Offset: 0x00024598
        [DataSourceProperty]
        public virtual ImageIdentifierVM Visual
        {
            get
            {
                return this._visual;
            }
            set
            {
                this._visual = value;
                base.OnPropertyChangedWithValue<ImageIdentifierVM>(value, "Visual");
            }
        }

        // Token: 0x1700021F RID: 543
        // (get) Token: 0x06000889 RID: 2185 RVA: 0x000263B0 File Offset: 0x000245B0
        // (set) Token: 0x0600088A RID: 2186 RVA: 0x000263C8 File Offset: 0x000245C8
        public virtual List<TooltipProperty> ToolTips
        {
            get
            {
                return this._toolTips;
            }
            set
            {
                this._toolTips = value;
                bool flag = this.ToolTips != null;
                if (flag)
                {
                    this.HintToolTip = new BasicTooltipViewModel(() => this.ToolTips);
                }
            }
        }

        // Token: 0x17000220 RID: 544
        // (get) Token: 0x0600088B RID: 2187 RVA: 0x00026404 File Offset: 0x00024604
        // (set) Token: 0x0600088C RID: 2188 RVA: 0x0002641C File Offset: 0x0002461C
        [DataSourceProperty]
        public virtual BasicTooltipViewModel HintToolTip
        {
            get
            {
                return this._hintToolTip;
            }
            private set
            {
                this._hintToolTip = value;
                base.OnPropertyChangedWithValue<BasicTooltipViewModel>(value, "HintToolTip");
            }
        }

        // Token: 0x17000221 RID: 545
        // (get) Token: 0x0600088D RID: 2189 RVA: 0x00026434 File Offset: 0x00024634
        // (set) Token: 0x0600088E RID: 2190 RVA: 0x0002644C File Offset: 0x0002464C
        [DataSourceProperty]
        public virtual bool IsDisabled
        {
            get
            {
                return this._isDisabled;
            }
            set
            {
                this._isDisabled = value;
                base.OnPropertyChangedWithValue(value, "IsDisabled");
            }
        }

        // Token: 0x17000222 RID: 546
        // (get) Token: 0x0600088F RID: 2191 RVA: 0x00026464 File Offset: 0x00024664
        // (set) Token: 0x06000890 RID: 2192 RVA: 0x0002647C File Offset: 0x0002467C
        [DataSourceProperty]
        public string Name
        {
            get
            {
                return this._name;
            }
            set
            {
                this._name = value;
                this.SetBaseToolTip();
                base.OnPropertyChangedWithValue<string>(value, "Name");
            }
        }

        // Token: 0x17000223 RID: 547
        // (get) Token: 0x06000891 RID: 2193 RVA: 0x0002649C File Offset: 0x0002469C
        // (set) Token: 0x06000892 RID: 2194 RVA: 0x000264B4 File Offset: 0x000246B4
        [DataSourceProperty]
        public MBBindingList<GenericVM<string>> NameLines
        {
            get
            {
                return this._nameLines;
            }
            set
            {
                this._nameLines = value;
                this.SetBaseToolTip();
                base.OnPropertyChangedWithValue<MBBindingList<GenericVM<string>>>(value, "NameLines");
            }
        }

        // Token: 0x17000224 RID: 548
        // (get) Token: 0x06000893 RID: 2195 RVA: 0x000264D4 File Offset: 0x000246D4
        // (set) Token: 0x06000894 RID: 2196 RVA: 0x000264EC File Offset: 0x000246EC
        [DataSourceProperty]
        public Color Color
        {
            get
            {
                return this._color;
            }
            set
            {
                this._color = value;
                base.OnPropertyChangedWithValue(value, "Color");
            }
        }

        // Token: 0x17000225 RID: 549
        // (get) Token: 0x06000895 RID: 2197 RVA: 0x00026504 File Offset: 0x00024704
        // (set) Token: 0x06000896 RID: 2198 RVA: 0x0002651C File Offset: 0x0002471C
        [DataSourceProperty]
        public string SpriteName
        {
            get
            {
                return this._spriteName;
            }
            set
            {
                this._spriteName = value;
                base.OnPropertyChangedWithValue<string>(value, "SpriteName");
            }
        }

        // Token: 0x17000226 RID: 550
        // (get) Token: 0x06000897 RID: 2199 RVA: 0x00026533 File Offset: 0x00024733
        // (set) Token: 0x06000898 RID: 2200 RVA: 0x0002653B File Offset: 0x0002473B
        public T Object { get; private set; }

        // Token: 0x06000899 RID: 2201 RVA: 0x00026544 File Offset: 0x00024744
        public ButtonObjectVM(T selectableObject, Action<ButtonObjectVM<T>> onObjectSelected = null, Action<ButtonObjectVM<T>> onObjectAlternateSelected = null, Action<ButtonObjectVM<T>> onObjectHoverBegin = null, Action<ButtonObjectVM<T>> onObjectHoverEnd = null, object imageIdentifierArg = null)
        {
            this.Object = selectableObject;
            this.onObjectSelected = onObjectSelected;
            this.onObjectAlternateSelected = onObjectAlternateSelected;
            this.onObjectHoverBegin = onObjectHoverBegin;
            this.onObjectHoverEnd = onObjectHoverEnd;
            ImageIdentifier imageIdentifier = ViewModelLib.TryGetImageIdentifierType(selectableObject, imageIdentifierArg);
            bool flag = imageIdentifier != null;
            if (flag)
            {
                this.Visual = new ImageIdentifierVM(imageIdentifier);
            }
            this.IsDisabled = false;
            this.Name = selectableObject.ToString();
            this.SetBaseToolTip();
            this.NameLines = new MBBindingList<GenericVM<string>>();
        }

        // Token: 0x0600089A RID: 2202 RVA: 0x000265D4 File Offset: 0x000247D4
        public void SetBaseToolTip()
        {
            bool flag = this.Object is MBObjectBase;
            if (flag)
            {
                MBObjectBase objectBase = this.Object as MBObjectBase;
                this.HintToolTip = new BasicTooltipViewModel();
                this.ToolTips = new List<TooltipProperty>();
                this.ToolTips.TryAddToolTip("", this.Name, "", "", 1, TooltipProperty.TooltipPropertyFlags.Title);
            }
        }

        // Token: 0x0600089B RID: 2203 RVA: 0x0002664B File Offset: 0x0002484B
        public void ClearVisual()
        {
            this.Visual = new ImageIdentifierVM(ImageIdentifierType.Null);
        }

        // Token: 0x0600089C RID: 2204 RVA: 0x0002665C File Offset: 0x0002485C
        private void ExecuteOnSelected()
        {
            bool flag = this.onObjectSelected != null;
            if (flag)
            {
                this.onObjectSelected(this);
            }
        }

        // Token: 0x0600089D RID: 2205 RVA: 0x00026684 File Offset: 0x00024884
        private void ExecuteAlternateSelected()
        {
            bool flag = this.onObjectAlternateSelected != null;
            if (flag)
            {
                this.onObjectAlternateSelected(this);
            }
        }

        // Token: 0x0600089E RID: 2206 RVA: 0x000266AC File Offset: 0x000248AC
        private void ExecuteOnHoverBegin()
        {
            bool flag = this.onObjectHoverBegin != null;
            if (flag)
            {
                this.onObjectHoverBegin(this);
            }
        }

        // Token: 0x0600089F RID: 2207 RVA: 0x000266D4 File Offset: 0x000248D4
        private void ExecuteOnHoverEnd()
        {
            bool flag = this.onObjectHoverEnd != null;
            if (flag)
            {
                this.onObjectHoverEnd(this);
            }
        }

        // Token: 0x040002E5 RID: 741
        private ImageIdentifierVM _visual;

        // Token: 0x040002E6 RID: 742
        private List<TooltipProperty> _toolTips;

        // Token: 0x040002E7 RID: 743
        private BasicTooltipViewModel _hintToolTip;

        // Token: 0x040002E8 RID: 744
        private bool _isDisabled;

        // Token: 0x040002E9 RID: 745
        private string _name;

        // Token: 0x040002EA RID: 746
        private MBBindingList<GenericVM<string>> _nameLines;

        // Token: 0x040002EB RID: 747
        private Color _color;

        // Token: 0x040002EC RID: 748
        private string _spriteName;

        // Token: 0x040002EE RID: 750
        private Action<ButtonObjectVM<T>> onObjectSelected;

        // Token: 0x040002EF RID: 751
        private Action<ButtonObjectVM<T>> onObjectAlternateSelected;

        // Token: 0x040002F0 RID: 752
        private Action<ButtonObjectVM<T>> onObjectHoverBegin;

        // Token: 0x040002F1 RID: 753
        private Action<ButtonObjectVM<T>> onObjectHoverEnd;
    }
}
