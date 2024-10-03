using TaleWorlds.Library;

namespace BannerRoyalMPLib
{
    public class GenericVM<T> : ViewModel
    {
        // Token: 0x17000232 RID: 562
        // (get) Token: 0x060008BA RID: 2234 RVA: 0x000269CC File Offset: 0x00024BCC
        // (set) Token: 0x060008BB RID: 2235 RVA: 0x000269E4 File Offset: 0x00024BE4
        [DataSourceProperty]
        public T DataSourceObject0
        {
            get
            {
                return this._dataSourceObject0;
            }
            set
            {
                this._dataSourceObject0 = value;
                base.OnPropertyChangedWithValue<object>(value, "DataSourceObject0");
            }
        }

        // Token: 0x060008BC RID: 2236 RVA: 0x00026A00 File Offset: 0x00024C00
        public GenericVM(T object0)
        {
            this.DataSourceObject0 = object0;
        }

        // Token: 0x040002FD RID: 765
        private T _dataSourceObject0;
    }
}
