using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace SimpleListTest
{
    public class ItemTemplate : StackLayout
    {
        public ItemTemplate()
        {
            var idLbl = new Label { HorizontalOptions = LayoutOptions.Start };
            idLbl.SetBinding<Item>(Label.TextProperty, i => i.Id);
            var nameLbl = new Label { HorizontalOptions = LayoutOptions.StartAndExpand };
            nameLbl.SetBinding<Item>(Label.TextProperty, i => i.Name);
            
            Orientation = StackOrientation.Horizontal;
            HorizontalOptions = LayoutOptions.FillAndExpand;
            Children.Add(idLbl);
            Children.Add(nameLbl);
        }
    }
}
