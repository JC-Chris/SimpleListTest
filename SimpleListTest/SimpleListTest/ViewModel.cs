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
        public ObservableCollection<string> Items { get; set; }

        public ViewModel()
        {
            Items = new ObservableCollection<string>(new string[] {"Initial", "Items", "Added"});
            AddItemCommand = new Command(() =>
            {    
                Items.Add(EntryText);
            });
        }
    }
}
