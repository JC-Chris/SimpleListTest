using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using PropertyChanged;

namespace SimpleListTest
{
    [ImplementPropertyChanged]
    public class Item
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ViewModel ParentVM { get; set; }
    }
}
