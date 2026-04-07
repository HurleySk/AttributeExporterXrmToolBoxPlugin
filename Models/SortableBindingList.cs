using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace AttributeExporterXrmToolBoxPlugin.Models
{
    /// <summary>
    /// Sortable BindingList implementation for DataGridView sorting support
    /// </summary>
    public class SortableBindingList<T> : BindingList<T>
    {
        private bool _isSorted;
        private ListSortDirection _sortDirection;
        private PropertyDescriptor _sortProperty;

        public SortableBindingList() : base() { }

        public SortableBindingList(IList<T> list) : base(list) { }

        protected override bool SupportsSortingCore => true;

        protected override bool IsSortedCore => _isSorted;

        protected override ListSortDirection SortDirectionCore => _sortDirection;

        protected override PropertyDescriptor SortPropertyCore => _sortProperty;

        protected override void ApplySortCore(PropertyDescriptor prop, ListSortDirection direction)
        {
            List<T> itemsList = (List<T>)this.Items;

            if (prop.PropertyType.GetInterface("IComparable") != null)
            {
                itemsList.Sort(delegate (T x, T y)
                {
                    object xValue = prop.GetValue(x);
                    object yValue = prop.GetValue(y);

                    // Handle nulls
                    if (xValue == null && yValue == null) return 0;
                    if (xValue == null) return direction == ListSortDirection.Ascending ? -1 : 1;
                    if (yValue == null) return direction == ListSortDirection.Ascending ? 1 : -1;

                    int result = ((IComparable)xValue).CompareTo(yValue);
                    return direction == ListSortDirection.Ascending ? result : -result;
                });

                _isSorted = true;
                _sortDirection = direction;
                _sortProperty = prop;

                // Notify that the list has been reset (re-sorted)
                this.OnListChanged(new ListChangedEventArgs(ListChangedType.Reset, -1));
            }
        }

        protected override void RemoveSortCore()
        {
            _isSorted = false;
            _sortProperty = null;
        }
    }
}
