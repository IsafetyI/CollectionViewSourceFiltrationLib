using DevExpress.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;

namespace CollectionViewSourceFiltrationLib
{
    public class ConjunctionFilterPredicateDescription<TItem> : ViewModelBase, IFilterPredicateDescription<TItem>
    {
        public ObservableCollection<IFilter<TItem>> Filters { get; private set; } = new ObservableCollection<IFilter<TItem>>();

        public Predicate<object> GetObjPredicate()
        {
            var predicate = GetPredicate();
            return (item) => 
            {
                if (item != null)
                    return predicate((TItem)item);
                else
                    throw new Exception();
            };
        }

        public Predicate<TItem> GetPredicate()
        {
            if (Filters.Any())
                return (item) =>
                {
                    foreach (var filter in Filters)
                        if(filter != null && !filter.Filter(item))
                            return false;
                    return true;
                };
            else
                return (item) => true;
        }

        public ConjunctionFilterPredicateDescription()
        {
            Filters.CollectionChanged += Filters_CollectionChanged;
        }

        public ConjunctionFilterPredicateDescription(ObservableCollection<IFilter<TItem>> filters)
        {
            Filters = filters;
            if(Filters.Any())
                foreach (var item in Filters)
                {
                    if (item != null && item.GetType().IsAssignableTo(typeof(INotifyPropertyChanged)))
                        ((INotifyPropertyChanged)item).PropertyChanged += FilterPropertyChanged;
                }
            Filters.CollectionChanged += Filters_CollectionChanged;
        }

        private void Filters_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            if(e != null && e.NewItems != null && e.NewItems.Count > 0)
                foreach (var item in e.NewItems)
                {
                    if(item != null && item.GetType().GetInterfaces().OfType<INotifyPropertyChanged>().Any())
                        ((INotifyPropertyChanged)item).PropertyChanged += FilterPropertyChanged;
                }
            if (e != null && e.OldItems != null && e.OldItems.Count > 0)
                foreach (var item in e.OldItems)
                {
                    if (item != null && item.GetType().GetInterfaces().OfType<INotifyPropertyChanged>().Any())
                        ((INotifyPropertyChanged)item).PropertyChanged -= FilterPropertyChanged;
                }
            RaisePropertiesChanged();
        }

        private void FilterPropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            RaisePropertiesChanged();
        }
    }
}
