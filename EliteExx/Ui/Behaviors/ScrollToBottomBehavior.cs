using System;
using System.Collections.Specialized;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interactivity;
using Akka.Actor;
using Caliburn.Micro;
using Zw.EliteExx.Core;
using Zw.EliteExx.Ui.Events;

namespace Zw.EliteExx.Ui.Behaviors
{
    /// <summary>
    /// ListBoxes (and descendants like ListViews) with this behavior will scroll to the last inserted element using debounce method with the UiProcessorActor.
    /// </summary>
    public class ScrollToBottomBehavior : Behavior<ListBox>, Caliburn.Micro.IHandle<ScrollToItemCommand>
    {
        private static readonly log4net.ILog log = global::log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public static readonly DependencyProperty IsScrollToBottomProperty = DependencyProperty.RegisterAttached("IsScrollToBottom", typeof(bool), typeof(ScrollToBottomBehavior), new PropertyMetadata(false));

        private IEventAggregator eventAggregator;
        private ActorSystemManager actorSystemManager;

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
            this.eventAggregator = this.eventAggregator ?? IoC.Get<IEventAggregator>();
            this.eventAggregator.Subscribe(this);
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
            this.eventAggregator.Unsubscribe(this);
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
                this.actorSystemManager = this.actorSystemManager ?? IoC.Get<ActorSystemManager>();
                this.actorSystemManager.UiProcessor.Tell(new UiProcessorMessage.QueueBufferedScrollToBottom(listBox, e.NewItems[0]));
            }
        }

        public void Handle(ScrollToItemCommand message)
        {
            if (!Object.ReferenceEquals(message.ListBox, AssociatedObject)) return;
            AssociatedObject.ScrollIntoView(message.TargetItem);
        }
    }
}
