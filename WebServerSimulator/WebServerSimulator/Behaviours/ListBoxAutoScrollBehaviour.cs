using System.Collections.Specialized;

using System.Windows.Controls;
using System.Windows.Interactivity;


namespace WebServerSimulator.Behaviours
{
    public class ListBoxAutoScrollBehaviour : Behavior<ListBox>
    {
        protected override void OnAttached()
        {
            ListBox listBox = AssociatedObject;
            ((INotifyCollectionChanged)listBox.Items).CollectionChanged += OnListBoxCollectionChanged;
        }

        protected override void OnDetaching()
        {
            ListBox listBox = AssociatedObject;
            ((INotifyCollectionChanged)listBox.Items).CollectionChanged -= OnListBoxCollectionChanged;
        }

        private void OnListBoxCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            ListBox listBox = AssociatedObject;
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                listBox.ScrollIntoView(e.NewItems[0]);
            }
        }
    }
}
