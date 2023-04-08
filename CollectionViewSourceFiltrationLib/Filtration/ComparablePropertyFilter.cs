using DevExpress.Mvvm;
using System;
using System.Reflection;

namespace CollectionViewSourceFiltrationLib
{
    public class ComparablePropertyFilter<TItem> : ViewModelBase, IFilter<TItem>
    {
        public PropertyInfo PropertyInfo { get; private set; }

        public bool IsEnabled { get; set; }

        public IComparable? ComparisonValue { get; set; }

        public ComparablePropertyFilterMode Mode { get; set; }

        public string Title { get; set; } = string.Empty;

        public bool Filter(TItem item)
        {
            if (!IsEnabled)
                return true;
            var valueObj = PropertyInfo.GetValue(item);
            if (valueObj == null)
                return ComparisonValue == null;
            if(ComparisonValue == null)
                return false;
            if(ComparisonValue.GetType() != PropertyInfo.PropertyType)
                return false;
            var comparisonResult = ((IComparable)valueObj).CompareTo(ComparisonValue);
            return Mode switch
            {
                ComparablePropertyFilterMode.Less => comparisonResult < 0,
                ComparablePropertyFilterMode.LessEqual => comparisonResult <= 0,
                ComparablePropertyFilterMode.Equal => comparisonResult == 0,
                ComparablePropertyFilterMode.NotEqual => comparisonResult != 0,
                ComparablePropertyFilterMode.GreaterEqual => comparisonResult >= 0,
                ComparablePropertyFilterMode.Greater => comparisonResult > 0,
                _ => false,
            };
        }

        public ComparablePropertyFilter(PropertyInfo propertyInfo, bool isEnabled, ComparablePropertyFilterMode mode, IComparable? comparisonValue)
        {
            if (!propertyInfo.PropertyType.IsAssignableTo(typeof(IComparable)))
                throw new Exception("Property type should implement IComparable");
            PropertyInfo = propertyInfo;
            IsEnabled = isEnabled;
            Mode = mode;
            ComparisonValue = comparisonValue;
        }

        public ComparablePropertyFilter(PropertyInfo propertyInfo) : this(propertyInfo, false, ComparablePropertyFilterMode.Equal, null!) { }
    }
}
