using System;
using System.Collections.Specialized;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interactivity;

namespace Zw.EliteExx.Ui.Behaviors
{
    public class ScrollToBottomBehavior : Behavior<ListBox>
    {
        private static readonly log4net.ILog log = global::log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public static readonly DependencyProperty IsScrollToBottomProperty = DependencyProperty.RegisterAttached("IsScrollToBottom", typeof(bool), typeof(ScrollToBottomBehavior), new PropertyMetadata(false));

        public static bool GetIsScrollToBottom(DependencyObject d)
        {
            return (bool)d.GetValue(IsScrollToBottomProperty);
        }

        public static void SetIsScrollToBottom(DependencyObject d, bool value)
        {
            d.SetValue(IsScrollToBottomProperty, value);
        }

        protected override void OnAttached()
        {
            ListBox listBox = AssociatedObject;
            if (listBox == null)
            {
                log.Error($"Used on incompatible object '{AssociatedObject}'");
                return;
            }
            INotifyCollectionChanged collection = listBox.Items as INotifyCollectionChanged;
            if (collection == null)
            {
                log.Error($"Used on incompatible itemssource '{listBox.Items}'");
                return;
            }
            collection.CollectionChanged += ScrollToBottomBehavior_CollectionChanged;
        }

        protected override void OnDetaching()
        {
            ListBox listBox = AssociatedObject;
            if (listBox == null)
            {
                log.Error($"Used on incompatible object '{AssociatedObject}'");
                return;
            }
            INotifyCollectionChanged collection = listBox.Items as INotifyCollectionChanged;
            if (collection == null)
            {
                log.Error($"Used on incompatible itemssource '{listBox.Items}'");
                return;
            }
            collection.CollectionChanged -= ScrollToBottomBehavior_CollectionChanged;
        }

        private void ScrollToBottomBehavior_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            ListBox listBox = AssociatedObject;
            bool isb = GetIsScrollToBottom(listBox as DependencyObject);
            if ((isb) && (e.Action == NotifyCollectionChangedAction.Add))
            {
                listBox.ScrollIntoView(e.NewItems[0]);
            }
        }
    }
}
