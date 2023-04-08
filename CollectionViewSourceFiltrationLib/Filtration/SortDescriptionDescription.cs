using DevExpress.Mvvm;
using System;
using System.ComponentModel;

namespace CollectionViewSourceFiltrationLib
{
    public class SortDescriptionDescription : ViewModelBase
    {
        public string Title { get; set; } = string.Empty;

        public bool IsDescending { get; set; } = false;

        public string PropertyName { get; set; } = string.Empty;

        public SortDescription? GetDescription()
        {
            if (PropertyName == String.Empty)
                return null;
            if (IsDescending)
                return new SortDescription(PropertyName, ListSortDirection.Descending);
            return new SortDescription(PropertyName, ListSortDirection.Ascending);
        }

        public SortDescriptionDescription(string title, string propertyName) : this(title, propertyName, false) { }

        public SortDescriptionDescription(string title, string propertyName, bool isDescending)
        {
            Title = title;
            PropertyName = propertyName;
            IsDescending = isDescending;
        }

        public SortDescriptionDescription() { }
    }
}
