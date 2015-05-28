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
using System.Windows.Input;
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

        public static readonly BindableProperty ItemSelectedCommandProperty = BindableProperty
            .Create<SimpleList, ICommand>(
                p => p.ItemSelectedCommand,
                default(ICommand),
                BindingMode.Default);

        public ICommand ItemSelectedCommand
        {
            get { return (ICommand) GetValue(ItemSelectedCommandProperty); }
            set {  SetValue(ItemSelectedCommandProperty, value); }
        }

        public string DisplayMember { get; set; }

        private void Repopulate()
        {
            Children.Clear();
            if (ItemsSource == null)
                return;
            
            // build our own internal command so that we can respond to selected events
            // correctly even if the command was set after the items were rendered
            var selectedCommand = new Command<object>(o =>
            {
                if (ItemSelectedCommand != null && ItemSelectedCommand.CanExecute(o))
                    ItemSelectedCommand.Execute(o);
            });

            View child;
            foreach (var item in ItemsSource)
            {
                if (ItemTemplate != null)
                {
                    child = ItemTemplate.CreateContent() as View;
                    if (child == null)
                        continue;
                    child.BindingContext = item;
                }
                else if (!string.IsNullOrEmpty(DisplayMember))
                {
                    var type = item.GetType();
                    var prop = type.GetRuntimeProperty(DisplayMember);
                    child = new Label {Text = prop.GetValue(item).ToString()};
                }
                else
                    child = new Label { Text = item.ToString() };
                
                // add an internal tapped handler
                var itemTapped = new TapGestureRecognizer {Command = selectedCommand, CommandParameter = item};
                child.GestureRecognizers.Add(itemTapped);

                Children.Add(child);
            }
        }

        void colChange_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            Repopulate();
        }
    }
}
