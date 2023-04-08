using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CollectionViewSourceFiltrationLib
{
    public interface IFilter<TItem> : IFilter
    {
        public bool Filter(TItem item);
    }

    public interface IFilter
    {
        public PropertyInfo PropertyInfo { get; }

        public string Title { get; set; }

        public bool IsEnabled { get; set; }
    }
}
