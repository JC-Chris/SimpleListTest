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
    public class SimpleList : StackLayout
    {
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

            var colChange = list.ItemsSource as INotifyCollectionChanged;
            if (colChange != null)
                colChange.CollectionChanged += list.colChange_CollectionChanged;
        }

        public static readonly BindableProperty ItemTemplateProperty = BindableProperty.Create<SimpleList, DataTemplate>(
            p => p.ItemTemplate,
            default(DataTemplate),
            BindingMode.Default,
            propertyChanged: ItemTemplateChanged);

        public DataTemplate ItemTemplate
        {
            get { return (DataTemplate)GetValue(ItemTemplateProperty); }
            set { SetValue(ItemTemplateProperty, value); }
        }

        private static void ItemTemplateChanged(BindableObject bindable, DataTemplate oldvalue, DataTemplate newvalue)
        {
            var list = (SimpleList)bindable;
            list.Repopulate();
        }

        public string DisplayMember { get; set; }

        private void Repopulate()
        {
            Children.Clear();
            if (ItemsSource == null)
                return;
            foreach (var item in ItemsSource)
            {
                if (ItemTemplate != null)
                {
                    var layout = ItemTemplate.CreateContent() as View;
                    if (layout == null)
                        continue;
                    layout.BindingContext = item;
                    Children.Add(layout);   
                }
                else if (!string.IsNullOrEmpty(DisplayMember))
                {
                    var type = item.GetType();
                    var prop = type.GetRuntimeProperty(DisplayMember);
                    Children.Add(new Label { Text = prop.GetValue(item).ToString() });
                }
                else
                    Children.Add(new Label { Text = item.ToString() });
            }
        }

        

        void colChange_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            Repopulate();
        }
    }
}
