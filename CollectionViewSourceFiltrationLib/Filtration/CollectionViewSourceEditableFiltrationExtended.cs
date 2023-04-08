using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Windows.Data;

namespace CollectionViewSourceFiltrationLib
{
    public class CollectionViewSourceEditableFiltrationExtended<TItem> : CollectionViewSource, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        public bool WasFiltered { get; protected set; } = false;

        private IFilterPredicateDescription<TItem> _filterDescription;

        public IFilterPredicateDescription<TItem> FilterDescription
        {
            get => _filterDescription;
            protected set
            {
                if(_filterDescription is not null)
                    _filterDescription.PropertyChanged -= OnFilterDescriptionPropertyChanged;
                _filterDescription = value;
                if (View != null)
                    View.Filter = _filterDescription.GetObjPredicate();
                FilterDescription.PropertyChanged += OnFilterDescriptionPropertyChanged;
            }
        }

        private ObservableCollection<SortDescriptionDescription> sortDescriptionDescriptions = new ObservableCollection<SortDescriptionDescription>();
        public ObservableCollection<SortDescriptionDescription> SortDescriptionDescriptions
        {
            get => sortDescriptionDescriptions;
            set
            {
                if(value is null)
                    return;
                sortDescriptionDescriptions.CollectionChanged -= OnSortDescriptionDescriptionsCollectionChanged;
                foreach (var description in sortDescriptionDescriptions)
                    description.PropertyChanged -= OnSortDescriptionPropertyChanged; 
                sortDescriptionDescriptions = value;
                sortDescriptionDescriptions.CollectionChanged += OnSortDescriptionDescriptionsCollectionChanged;
                foreach (var description in sortDescriptionDescriptions)
                    description.PropertyChanged += OnSortDescriptionPropertyChanged;
                OnSortDescriptionPropertyChangedBody();
            }
        }

        public CollectionViewSourceEditableFiltrationExtended(IFilterPredicateDescription<TItem> filterDescription, ObservableCollection<SortDescriptionDescription> descriptions) : base()
        {
            FilterDescription = filterDescription;
            SortDescriptionDescriptions = descriptions;
        }

        protected void OnFilterDescriptionPropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (View == null) 
                return;
            var filter = _filterDescription.GetObjPredicate();
            if (!WasFiltered)
            {
                //it's necessary due to some strange things inside of
                //collectionViewSource(Or inside of Listbox) internal logic
                //
                //Note: it is not necessary if you use only certain wpf elements such as
                //TextBox, button and toggle button
                View.Filter = (obj) => (true);
                WasFiltered = true;
            }
            View.Filter = filter;
        }

        protected void OnSortDescriptionPropertyChanged(object? sender, PropertyChangedEventArgs e) => OnSortDescriptionPropertyChangedBody();

        private void OnSortDescriptionDescriptionsCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e) => OnSortDescriptionPropertyChangedBody();

        protected void OnSortDescriptionPropertyChangedBody()
        {
            if (View == null)
                return;
            View.SortDescriptions.Clear();
            foreach (var description in SortDescriptionDescriptions)
            {
                var d = description.GetDescription();
                if (d != null)
                    View.SortDescriptions.Add(d.Value);
            }
        }
    }
}
