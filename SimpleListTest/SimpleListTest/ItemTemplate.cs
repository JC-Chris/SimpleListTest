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
            var idLbl = new Label { HorizontalOptions = LayoutOptions.Start, VerticalOptions = LayoutOptions.Center};
            idLbl.SetBinding<Item>(Label.TextProperty, i => i.Id);
            Children.Add(idLbl);

            var nameLbl = new Label { HorizontalOptions = LayoutOptions.StartAndExpand, VerticalOptions = LayoutOptions.Center};
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

        protected override void OnParentSet()
        {
            base.OnParentSet();

            if (ParentView != null && ParentView.BindingContext != null)
            {
                var vm = (ViewModel) ParentView.BindingContext;
                _delBtn.Command = vm.DeleteItemCommand;
                _delBtn.CommandParameter = (Item) BindingContext;
            }
        }
    }
}
