using DevExpress.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reflection;

namespace CollectionViewSourceFiltrationLib
{
    public class EqualsFilter<TItem> : ViewModelBase, IFilter<TItem>
    {
        public PropertyInfo PropertyInfo { get; private set; }

        public bool IsEnabled { get; set; } = false;

        public bool IsInverted { get; set; }

        public ObservableCollection<object?> AvailableValues { get; set; } = new ObservableCollection<object?>();

        public object? ComparisonValue { get; set; }

        public string Title { get; set; } = string.Empty;

        public bool Filter(TItem item)
        {
            if (!IsEnabled)
                return true;
            if (item == null)
                return false;
            var valueObj = PropertyInfo.GetValue(item);
            if (valueObj == null)
                return ComparisonValue == null;
            if (ComparisonValue == null)
                return true;
            if (ComparisonValue.GetType() != PropertyInfo.PropertyType)
                return false;
            if (IsInverted)
                return !ComparisonValue.Equals((bool)valueObj);
            else
                return ComparisonValue.Equals((bool)valueObj);
        }

        public EqualsFilter(PropertyInfo propertyInfo, ObservableCollection<object?> availableValues, bool isEnabled, bool isInverted, string title, object? comparisonValue)
        {
            if (propertyInfo is null)
                throw new Exception("property Shouldn't be null");
            PropertyInfo = propertyInfo;
            AvailableValues = availableValues;
            IsEnabled = isEnabled;
            IsInverted = isInverted;
            Title = title;
            ComparisonValue = comparisonValue;
        }

        public EqualsFilter(PropertyInfo propertyInfo, bool isEnabled, bool isInverted, string title) : this(propertyInfo, new ObservableCollection<object>(), false, false, string.Empty, null) { }

        public EqualsFilter(PropertyInfo propertyInfo, bool isEnabled, bool isInverted) : this(propertyInfo, false, false, string.Empty) { }

        public EqualsFilter(PropertyInfo propertyInfo) : this(propertyInfo, false, false) { }
    }
}
