using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace CollectionViewSourceFiltrationLib
{
    public interface IFilterPredicateDescription<TItem> :INotifyPropertyChanged
    {
        public ObservableCollection<IFilter<TItem>> Filters { get; }

        public Predicate<TItem> GetPredicate();

        public Predicate<object> GetObjPredicate();
    }
}