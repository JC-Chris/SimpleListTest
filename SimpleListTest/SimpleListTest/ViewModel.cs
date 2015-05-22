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
        public string EntryText { get; set; }
        public ICommand AddItemCommand { get; set; }
        public ObservableCollection<Item> Items { get; set; }

        public ViewModel()
        {
            Items = new ObservableCollection<Item>();
            Items.Add(new Item() {Id = 1, Name = "Initial"});
            Items.Add(new Item() {Id = 2, Name = "Added"});
            AddItemCommand = new Command(() =>
            {
                var max = Items.Max(i => i.Id);
                Items.Add(new Item { Id = ++max, Name = EntryText});
            });
        }
    }
}
