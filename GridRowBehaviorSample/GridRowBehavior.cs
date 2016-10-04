using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interactivity;

namespace GridRowBehaviorSample
{

    public class GridRowBehavior : Behavior<Grid>
    {
        public static readonly DependencyProperty ItemsProperty = DependencyProperty.RegisterAttached(
            "Items", typeof(IEnumerable), typeof(GridRowBehavior), new PropertyMetadata(default(IEnumerable)));

        public static void SetItems(DependencyObject element, IEnumerable value)
        {
            element.SetValue(ItemsProperty, value);
        }

        public static IEnumerable GetItems(DependencyObject element)
        {
            return (IEnumerable) element.GetValue(ItemsProperty);
        }

        public static readonly DependencyProperty ColumnTemplateProperty = DependencyProperty.RegisterAttached(
            "ColumnTemplate", typeof(ControlTemplate), typeof(GridRowBehavior), new PropertyMetadata(default(ControlTemplate)));

        public static void SetColumnTemplate(DependencyObject element, ControlTemplate value)
        {
            element.SetValue(ColumnTemplateProperty, value);
        }

        public static ControlTemplate GetColumnTemplate(DependencyObject element)
        {
            return (ControlTemplate) element.GetValue(ColumnTemplateProperty);
        }
        
        protected override void OnAttached()
        {
            base.OnAttached();

            var grid = this.AssociatedObject;
            grid.RowDefinitions.Clear();
            grid.Children.Clear();

            var items = GetItems(grid);
            if (items == null)
            {
                return;
            }

            int rowIndex = 0;
            foreach (var item in items)
            {
                // Add RowDefinition
                var rowDefinition = new RowDefinition();
                rowDefinition.SetValue(RowDefinition.HeightProperty, GridLength.Auto);
                grid.RowDefinitions.Add(rowDefinition);

                // Add controls for each column.
                for(int columnIndex = 0; columnIndex < grid.ColumnDefinitions.Count; columnIndex++)
                {
                    var columnDefinition = grid.ColumnDefinitions[columnIndex];

                    // Create a control from corresponding template.
                    var template = GetColumnTemplate(columnDefinition) ?? new ControlTemplate(typeof(ContentControl));
                    var control = (FrameworkElement) template.LoadContent();
                    // Set data context
                    control.DataContext = item;
                    // Set Grid.Column and Grid.Row property
                    Grid.SetColumn(control, columnIndex);
                    Grid.SetRow(control, rowIndex);
                    // Add to the grid.
                    grid.Children.Add(control);
                }

                rowIndex++;
            }
        }
    }
}
