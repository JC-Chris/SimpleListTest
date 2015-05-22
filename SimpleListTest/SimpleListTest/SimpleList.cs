using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;

using Xamarin.Forms;

namespace SimpleListTest
{
    public class SimpleList : ContentView
    {
        private ScrollView _scroll;
        private StackLayout _stack;

        public SimpleList()
        {
            _stack = new StackLayout
            {
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.FillAndExpand
            };
            _scroll = new ScrollView
            {
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.FillAndExpand,
                Content = _stack
            };
            Content = _scroll;
        }

        public static readonly BindableProperty ItemsSourceProperty = BindableProperty.Create<SimpleList, IEnumerable>(
            p => p.ItemsSource,
            default(IEnumerable),
            BindingMode.TwoWay,
            propertyChanged: ItemsSourceChanged);

        public IEnumerable ItemsSource 
        {
            get { return (IEnumerable) GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); } 
        }

        private static void ItemsSourceChanged(BindableObject bindable, IEnumerable oldvalue, IEnumerable newvalue)
        {
            var list = (SimpleList) bindable;
            list.Repopulate();

            var propChange = list.ItemsSource as INotifyPropertyChanged;
            if (propChange != null)
                propChange.PropertyChanged += list.propChange_PropertyChanged;
            var colChange = list.ItemsSource as INotifyCollectionChanged;
            if (colChange != null)
                colChange.CollectionChanged += list.colChange_CollectionChanged;
        }

        public string DisplayMember { get; set; }

        private void Repopulate()
        {
            _stack.Children.Clear();
            if (ItemsSource == null)
                return;
            foreach (var item in ItemsSource)
            {
                if (string.IsNullOrEmpty(DisplayMember))
                    _stack.Children.Add(new Label {Text = item.ToString()});
                else
                {
                    var type = item.GetType();
                    var prop = type.GetRuntimeProperty(DisplayMember);
                    _stack.Children.Add(new Label() {Text = prop.GetValue(item).ToString()});
                }
            }
        }

        

        void colChange_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            Repopulate();
        }

        void propChange_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            //Repopulate();
        }
    }
}
