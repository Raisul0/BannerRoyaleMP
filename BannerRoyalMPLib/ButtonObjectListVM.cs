using System;
using TaleWorlds.Library;

namespace BannerRoyalMPLib
{
    public class ButtonObjectListVM<T> : ViewModel
    {
        // Token: 0x17000218 RID: 536
        // (get) Token: 0x06000875 RID: 2165 RVA: 0x00026180 File Offset: 0x00024380
        // (set) Token: 0x06000876 RID: 2166 RVA: 0x00026198 File Offset: 0x00024398
        [DataSourceProperty]
        public virtual bool IsVisible
        {
            get
            {
                return this._isVisible;
            }
            set
            {
                this._isVisible = value;
                base.OnPropertyChangedWithValue(value, "IsVisible");
            }
        }

        // Token: 0x17000219 RID: 537
        // (get) Token: 0x06000877 RID: 2167 RVA: 0x000261B0 File Offset: 0x000243B0
        // (set) Token: 0x06000878 RID: 2168 RVA: 0x000261C8 File Offset: 0x000243C8
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

        // Token: 0x1700021A RID: 538
        // (get) Token: 0x06000879 RID: 2169 RVA: 0x000261E0 File Offset: 0x000243E0
        // (set) Token: 0x0600087A RID: 2170 RVA: 0x000261F8 File Offset: 0x000243F8
        [DataSourceProperty]
        public virtual string ListName
        {
            get
            {
                return this._listName;
            }
            set
            {
                this._listName = value;
                base.OnPropertyChangedWithValue<string>(value, "ListName");
            }
        }

        // Token: 0x1700021B RID: 539
        // (get) Token: 0x0600087B RID: 2171 RVA: 0x00026210 File Offset: 0x00024410
        // (set) Token: 0x0600087C RID: 2172 RVA: 0x00026228 File Offset: 0x00024428
        [DataSourceProperty]
        public virtual string ListIconName
        {
            get
            {
                return this._listIconName;
            }
            set
            {
                this._listIconName = value;
                base.OnPropertyChangedWithValue<string>(value, "ListIconName");
            }
        }

        // Token: 0x1700021C RID: 540
        // (get) Token: 0x0600087D RID: 2173 RVA: 0x00026240 File Offset: 0x00024440
        // (set) Token: 0x0600087E RID: 2174 RVA: 0x00026258 File Offset: 0x00024458
        [DataSourceProperty]
        public virtual MBBindingList<ButtonObjectVM<T>> ObjectsList
        {
            get
            {
                return this._objectsList;
            }
            set
            {
                this._objectsList = value;
                base.OnPropertyChangedWithValue<MBBindingList<ButtonObjectVM<T>>>(value, "ObjectsList");
            }
        }

        // Token: 0x1700021D RID: 541
        // (get) Token: 0x0600087F RID: 2175 RVA: 0x00026270 File Offset: 0x00024470
        // (set) Token: 0x06000880 RID: 2176 RVA: 0x00026288 File Offset: 0x00024488
        [DataSourceProperty]
        public ButtonObjectVM<T> SelectedObjectVM
        {
            get
            {
                return this._selectedObjectVM;
            }
            set
            {
                this._selectedObjectVM = value;
                base.OnPropertyChangedWithValue<ButtonObjectVM<T>>(value, "SelectedObjectVM");
            }
        }

        // Token: 0x06000881 RID: 2177 RVA: 0x0002629F File Offset: 0x0002449F
        public ButtonObjectListVM()
        {
            this.ObjectsList = new MBBindingList<ButtonObjectVM<T>>();
        }

        // Token: 0x06000882 RID: 2178 RVA: 0x000262C3 File Offset: 0x000244C3
        public ButtonObjectListVM(Action<ButtonObjectListVM<T>> onOpened)
        {
            this.onOpened = (Action<ButtonObjectListVM<T>>)Delegate.Combine(this.onOpened, onOpened);
            this.ObjectsList = new MBBindingList<ButtonObjectVM<T>>();
        }

        // Token: 0x06000883 RID: 2179 RVA: 0x00026300 File Offset: 0x00024500
        public ButtonObjectVM<T> AddObject(T selectableObject, Action<ButtonObjectVM<T>> onObjectSelected = null, Action<ButtonObjectVM<T>> onObjectAlternateSelected = null, Action<ButtonObjectVM<T>> onObjectHoverBegin = null, Action<ButtonObjectVM<T>> onObjectHoverEnd = null)
        {
            bool flag = onObjectSelected != null;
            if (flag)
            {
                onObjectSelected = (Action<ButtonObjectVM<T>>)Delegate.Combine(onObjectSelected, new Action<ButtonObjectVM<T>>(this.OnObjectSelected));
            }
            ButtonObjectVM<T> objectVM = new ButtonObjectVM<T>(selectableObject, onObjectSelected, onObjectAlternateSelected, onObjectHoverBegin, onObjectHoverEnd, null);
            this.ObjectsList.Add(objectVM);
            return objectVM;
        }

        // Token: 0x06000884 RID: 2180 RVA: 0x0002634F File Offset: 0x0002454F
        public void ClearObjects()
        {
            this.ObjectsList.Clear();
        }

        // Token: 0x06000885 RID: 2181 RVA: 0x0002635E File Offset: 0x0002455E
        private void OnObjectSelected(ButtonObjectVM<T> objectSelected)
        {
            this.SelectedObjectVM = objectSelected;
        }

        // Token: 0x06000886 RID: 2182 RVA: 0x00026369 File Offset: 0x00024569
        private void ExecuteOnSelected()
        {
            Action<ButtonObjectListVM<T>> action = this.onOpened;
            if (action != null)
            {
                action(this);
            }
        }

        // Token: 0x040002DE RID: 734
        private bool _isVisible = false;

        // Token: 0x040002DF RID: 735
        private bool _isDisabled = false;

        // Token: 0x040002E0 RID: 736
        public string _listName;

        // Token: 0x040002E1 RID: 737
        public string _listIconName;

        // Token: 0x040002E2 RID: 738
        private MBBindingList<ButtonObjectVM<T>> _objectsList;

        // Token: 0x040002E3 RID: 739
        private ButtonObjectVM<T> _selectedObjectVM;

        // Token: 0x040002E4 RID: 740
        private Action<ButtonObjectListVM<T>> onOpened;
    }
}
