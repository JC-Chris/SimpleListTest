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
        private Button _delBtn;

        public ItemTemplate()
        {
            // create labels to display the item id and name
            var idLbl = new Label { HorizontalOptions = LayoutOptions.Start };
            idLbl.SetBinding<Item>(Label.TextProperty, i => i.Id);
            Children.Add(idLbl);

            var nameLbl = new Label { HorizontalOptions = LayoutOptions.StartAndExpand };
            nameLbl.SetBinding<Item>(Label.TextProperty, i => i.Name);
            Children.Add(nameLbl);

            // create a delete button
            _delBtn = new Button
            {
                Text = "X",
                BackgroundColor = Color.Red,
                TextColor = Color.White,
                HorizontalOptions = LayoutOptions.End
            };
            Children.Add(_delBtn);
            
            Orientation = StackOrientation.Horizontal;
            HorizontalOptions = LayoutOptions.FillAndExpand;
        }

        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();
            var item = BindingContext as Item;
            if (item == null)
                return;

            _delBtn.Command = item.ParentVM.DeleteItemCommand;
            _delBtn.CommandParameter = item;
        }
    }
}
