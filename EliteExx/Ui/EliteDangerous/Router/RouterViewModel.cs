using System;
using System.ComponentModel;
using System.Linq;
using System.Windows.Data;
using Akka.Actor;
using Caliburn.Micro;
using Zw.EliteExx.Core;
using Zw.EliteExx.EliteDangerous.Journal;

namespace Zw.EliteExx.Ui.EliteDangerous.Router
{
    public class RouterViewModel : Screen, Caliburn.Micro.IHandle<Entry>
    {
        private static readonly log4net.ILog log = global::log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly IEventAggregator eventAggregator;
        private readonly ActorSystemManager actorSystemManager;
        private readonly IWindowManager windowManager;
        private readonly BindableCollection<Item> routeItems;
        private readonly ICollectionView routeItemsView;
        private readonly Configuration configuration;
        private bool hideDone;
        private Item selectedItem;

        public bool HideDone
        {
            get => this.hideDone;
            set
            {
                if (value == this.hideDone) return;
                this.hideDone = value;
                NotifyOfPropertyChange();
                this.routeItemsView.Refresh();
                PersistRouterSettings();
            }
        }

        public Item SelectedItem
        {
            get => this.selectedItem;
            set
            {
                if (Object.ReferenceEquals(value, this.selectedItem)) return;
                this.selectedItem = value;
                NotifyOfPropertyChange();
                NotifyOfPropertyChange(() => HasSelectedItem);
            }
        }

        public bool HasSelectedItem
        {
            get => (this.selectedItem != null);
        }

        public ICollectionView RouteItemsView => this.routeItemsView;

        public BindableCollection<Item> RouteItems => this.routeItems;

        public RouterViewModel(IEventAggregator eventAggregator, ActorSystemManager actorSystemManager, IWindowManager windowManager, Configuration configuration)
        {
            this.eventAggregator = eventAggregator;
            this.actorSystemManager = actorSystemManager;
            this.windowManager = windowManager;
            this.configuration = configuration;
            this.routeItems = new BindableCollection<Item>();
            this.routeItemsView = CollectionViewSource.GetDefaultView(this.routeItems);
            this.routeItemsView.SortDescriptions.Add(new SortDescription("Order", ListSortDirection.Ascending));
            this.routeItemsView.SortDescriptions.Add(new SortDescription("Name", ListSortDirection.Ascending));
            this.routeItemsView.Filter = FilterItems;
            LoadRouterSettings();
        }

        protected override void OnInitialize()
        {
            this.eventAggregator.Subscribe(this);
        }

        protected override void OnDeactivate(bool close)
        {
            if (close) this.eventAggregator.Unsubscribe(this);
        }

        public void AddWaypointToEnd()
        {
            CreateWaypoint(this.routeItems.Count);
        }

        public void AddWaypointAfterSelected()
        {
            if (this.selectedItem == null) return;
            CreateWaypoint(this.routeItems.IndexOf(this.selectedItem) + 1);
        }

        public void RemoveSelected()
        {
            if (this.selectedItem == null) return;
            this.routeItems.Remove(this.selectedItem);
            RearrangeOrderNumbers();
            PersistRouterSettings();
        }

        public void CopySelected()
        {
            if (this.selectedItem == null) return;
            System.Windows.Clipboard.SetText(this.selectedItem.Name);
        }

        public void EditSelected()
        {
            if (this.selectedItem == null) return;
            var vm = IoC.Get<WaypointViewModel>();
            vm.Name = this.selectedItem.Name;
            var result = this.windowManager.ShowDialog(vm);
            if (result.GetValueOrDefault(false))
            {
                this.selectedItem.Name = vm.Name;
                PersistRouterSettings();
            }
        }

        public void ToggleDone()
        {
            if (this.selectedItem == null) return;
            this.selectedItem.Done = (this.selectedItem.Done == DoneState.Done) ? DoneState.NotDone : DoneState.Done;
            PersistRouterSettings();
        }

        public void Handle(Entry entry)
        {
            EntryFsdJump jump = entry as EntryFsdJump;
            if (jump == null) return;
            // check if we reached waypoint
            var match = this.routeItems.FirstOrDefault(i => String.Equals(i.Name, jump.StarSystem));
            if (match == null) return;
            match.Done = DoneState.Done;
            this.actorSystemManager.UiProcessor.Tell(new UiMessengerMessage.Publish(new EliteExx.EliteDangerous.Journal.EntryMetaMessage(DateTime.UtcNow, Event.MetaMessage, $"Reached waypoint '{match.Name}'")));
        }

        private void PersistRouterSettings()
        {
            Core.Config.RouterSettings newRouterSettings = new Core.Config.RouterSettings(this.routeItems.Select(i => new Core.Config.RouterWaypoint(i.Order, i.Name, i.Done == DoneState.Done)), this.hideDone);
            this.configuration.SaveRouterSettings(newRouterSettings);
        }

        private void LoadRouterSettings()
        {
            var existingRouterSettings = this.configuration.Instance?.RouterSettings;
            if (existingRouterSettings == null) return;
            this.HideDone = existingRouterSettings.HideDone;
            var newVmItems = existingRouterSettings.Waypoints.Select(w => new Item() { Order = w.Order, Name = w.Name, Done = w.IsDone ? DoneState.Done : DoneState.NotDone });
            this.routeItems.AddRange(newVmItems);
        }

        private void CreateWaypoint(int position)
        {
            var vm = IoC.Get<WaypointViewModel>();
            vm.Name = String.Empty;
            var result = this.windowManager.ShowDialog(vm);
            if (result.GetValueOrDefault(false))
            {
                Item newItem = new Item() { Name = vm.Name, Done = DoneState.NotDone, Order = 1 };
                if (this.routeItems.Count > 0)
                {
                    var maxOrder = this.routeItems.Max(i => i.Order);
                    newItem.Order = maxOrder + 1;
                }
                this.routeItems.Insert(position, newItem);
                PersistRouterSettings();
            }
        }

        private bool FilterItems(object i)
        {
            Item item = i as Item;
            if (item == null) return false;
            if ((this.hideDone) && (item.Done == DoneState.Done)) return false;
            return true;
        }

        private void RearrangeOrderNumbers()
        {
            for (int i = 0; i < this.routeItems.Count; i++)
            {
                this.routeItems[i].Order = i + 1;
            }
        }
    }
}
