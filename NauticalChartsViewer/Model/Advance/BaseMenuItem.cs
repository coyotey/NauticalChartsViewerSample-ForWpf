using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using System.Collections.Generic;
using System.Windows.Input;

namespace NauticalChartsViewer
{
    public abstract class BaseMenuItem : ObservableObject
    {
        private ICommand checkedCommand;
        private ICommand selectedCommand;
        private bool isChecked = false;

        protected BaseMenuItem()
        {
        }

        protected BaseMenuItem(string header)
            : this(header, string.Empty)
        {
        }

        protected BaseMenuItem(string header, string action)
            : this(string.Empty, header, action, string.Empty, false, false, false, null)
        {
        }

        protected BaseMenuItem(string icon, string header, string action, string groupName, bool isCheckEnabled, bool isChecked, bool canToggle, CompositeMenuItem parent)
        {
            Icon = icon;
            Header = header;
            Action = action;
            GroupName = groupName;
            IsCheckable = isCheckEnabled;
            IsChecked = isChecked;
            CanToggle = canToggle;
            Parent = parent;
        }

        public string Action { get; private set; }

        public bool CanToggle { get; private set; }

        public ICommand CheckedCommand
        {
            get { return checkedCommand ?? (checkedCommand = new RelayCommand(HandleCheckedCommand)); }
        }

        public string GroupName { get; private set; }

        public string Header { get; private set; }

        public string Icon { get; set; }

        public bool IsCheckable { get; private set; }

        public bool IsChecked
        {
            get { return isChecked; }
            set
            {
                if (isChecked != value)
                {
                    isChecked = value;
                    RaisePropertyChanged("IsChecked");
                }
            }
        }

        public CompositeMenuItem Parent { get; internal set; }

        public ICommand SelectedCommand
        {
            get { return selectedCommand ?? (selectedCommand = new RelayCommand(HandleSelectedCommand)); }
        }

        public override string ToString()
        {
            return Header;
        }

        private bool CanCommandExcute()
        {
            return (!IsChecked && !CanToggle && IsCheckable);
        }

        private void HandleCheckedCommand()
        {
            if (!CanCommandExcute())
            {
                if (Parent != null)
                {
                    SwitchCheckedState(this, Parent.Children);
                }

                var message = new MenuItemMessage(this);
                Messenger.Default.Send<MenuItemMessage>(message);
            }
            else
            {
                IsChecked = true;
            }
        }

        private void HandleSelectedCommand()
        {
            SwitchCheckedState(this, Parent.Children);
            var message = new MenuItemMessage(this);
            Messenger.Default.Send<MenuItemMessage>(message);
        }

        private void SwitchCheckedState(BaseMenuItem menuItem, IEnumerable<object> menuItems)
        {
            if (menuItems != null)
            {
                IEnumerator<object> enumerator = menuItems.GetEnumerator();
                while (enumerator.MoveNext())
                {
                    object current = enumerator.Current;
                    if (current is SingleMenuItem)
                    {
                        SingleMenuItem singleMenuItem = (SingleMenuItem)current;
                        if (singleMenuItem.GroupName == menuItem.GroupName &&
                            singleMenuItem.IsCheckable &&
                            !singleMenuItem.CanToggle)
                        {
                            singleMenuItem.IsChecked = singleMenuItem == menuItem;
                        }
                    }
                }
            }
        }
    }
}