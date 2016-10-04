using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace GridRowBehaviorSample
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        private string itemValues;
        public IReadOnlyList<ItemViewModel> Items { get; }

        public string ItemValues
        {
            get { return this.itemValues; }
            private set
            {
                if (value == this.itemValues) return;
                this.itemValues = value;
                this.OnPropertyChanged();
            }
        }

        public MainWindowViewModel()
        {
            this.Items = new[] {
                    "First name",
                    "Last name",
                    "Age",
                }
                .Select(name => new ItemViewModel(name))
                .ToArray();

            foreach (var item in this.Items)
            {
                item.PropertyChanged += (sender, args) =>
                {
                    if (args.PropertyName == nameof(item.Value))
                    {
                        this.ItemValues = string.Join(",", this.Items.Select(item_ => item_.Value ?? ""));
                    }
                };
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
