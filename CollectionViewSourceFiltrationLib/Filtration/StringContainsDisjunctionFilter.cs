using DevExpress.Mvvm;
using System;
using System.Reflection;

namespace CollectionViewSourceFiltrationLib
{
    public class StringContainsDisjunctionFilter<TItem> : ViewModelBase, IFilter<TItem>
    {
        public PropertyInfo PropertyInfo { get; private set; }

        private bool isEnabled;

        public bool IsEnabled 
        { 
            get => isEnabled;
            set
            {
                isEnabled = value;
                RaisePropertiesChanged(nameof(IsEnabled));
            }
        }

        private string? comparisonValue = string.Empty;

        public string? ComparisonValue
        {
            get => comparisonValue;
            set
            {
                comparisonValue = value;
                RaisePropertiesChanged(nameof(ComparisonValue));
            }
        }

        public string Title { get; set; } = string.Empty;

        public bool Filter(TItem item)
        {
            if (!IsEnabled)
                return true;
            if(item == null)
                return false;
            var valueString = (string?)PropertyInfo.GetValue(item);
            if (valueString == null)
                return ComparisonValue == null || ComparisonValue == string.Empty;
            if (ComparisonValue == null)
                return true;
            if (ComparisonValue.GetType() != PropertyInfo.PropertyType)
                return false;
            string[] values = ComparisonValue.Split(new char[] { ' ', '\n' }, StringSplitOptions.RemoveEmptyEntries) ?? new string[] { };
            if (values.Length == 0)
                return true;
            foreach (var value in values)
                if (valueString.Contains(value))
                    return true;
            return false;
        }

        public StringContainsDisjunctionFilter(PropertyInfo propertyInfo, bool isEnabled, string title, string? comparisonValue)
        {
            if (!propertyInfo.PropertyType.IsAssignableTo(typeof(string)))
                throw new Exception("Property type should be a string");
            Title = title ?? Title;
            PropertyInfo = propertyInfo;
            IsEnabled = isEnabled;
            ComparisonValue = comparisonValue;
        }

        public StringContainsDisjunctionFilter(PropertyInfo propertyInfo, bool isEnabled) : this(propertyInfo, isEnabled, string.Empty, string.Empty) { }

        public StringContainsDisjunctionFilter(PropertyInfo propertyInfo) : this(propertyInfo, false) { }
    }
}
