using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using PropertyChanged;
using Xamarin.Forms;

namespace SimpleListTest
{
    [ImplementPropertyChanged]
    public class ViewModel
    {
        private Action<string, string> _popup;

        public string EntryText { get; set; }
        public ICommand AddItemCommand { get; protected set; }
        public ICommand UpdateItemCommand { get; protected set; }
        public ICommand SelectedItemCommand { get; protected set; }
        public ICommand DeleteItemCommand { get; protected set; }
        public ObservableCollection<Item> Items { get; set; }

        public ViewModel(Action<string, string> popup)
        {
            _popup = popup;

            Items = new ObservableCollection<Item>
            {
                new Item() {Id = 1, Name = "Initial", ParentVM = this},
                new Item() {Id = 2, Name = "Added", ParentVM = this}
            };
            AddItemCommand = new Command(() =>
            {
                var max = 0;
                if (Items.Count > 0)
                    Items.Max(i => i.Id);
                Items.Add(new Item { Id = ++max, Name = EntryText, ParentVM = this});
            });
            UpdateItemCommand = new Command(() => Items[0].Name = "Update: " + Items[0].Name);
            SelectedItemCommand = new Command<Item>((item) => _popup("Item Selected", string.Format("You selected {0}", item.Name)));
            DeleteItemCommand = new Command<Item>((item) => Items.Remove(item));
        }
    }
}
