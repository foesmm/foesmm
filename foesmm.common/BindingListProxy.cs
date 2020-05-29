using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;

namespace foesmm
{
    public class BindingListProxy<T> : BindingList<T>, INotifyCollectionChanged
    {
        public BindingListProxy() : base()
        {
            
        }
        
        public BindingListProxy(IList<T> backedList) : base(backedList)
        {
        }
        
        public event NotifyCollectionChangedEventHandler CollectionChanged;

        public void NotifyCollectionChanged(NotifyCollectionChangedEventArgs args)
        {
            CollectionChanged?.Invoke(this, args);
        } 
    }
}