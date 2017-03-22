using System;
using System.Windows;

namespace NauticalChartsViewer
{
    /// <summary>
    /// Interaction logic for NumericTextBox.xaml
    /// </summary>
    public partial class NumericTextBox
    {
        public static readonly DependencyProperty IncrementProperty =
            DependencyProperty.Register("Increment", typeof(double), typeof(NumericTextBox), new PropertyMetadata(1.0));

        public static readonly DependencyProperty MaxValueProperty =
            DependencyProperty.Register("MaxValue", typeof(double), typeof(NumericTextBox), new PropertyMetadata(100.0));

        public static readonly DependencyProperty MinValueProperty =
            DependencyProperty.Register("MinValue", typeof(double), typeof(NumericTextBox), new PropertyMetadata(0.0));

        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register("Value", typeof(double), typeof(NumericTextBox), new PropertyMetadata(0.0, null, new CoerceValueCallback(OnCoerceValueCallback)));

        public NumericTextBox()
        {
            this.InitializeComponent();
        }

        public double Increment
        {
            get { return (double)GetValue(IncrementProperty); }
            set { SetValue(IncrementProperty, value); }
        }

        public double MaxValue
        {
            get { return (double)GetValue(MaxValueProperty); }
            set { SetValue(MaxValueProperty, value); }
        }

        public double MinValue
        {
            get { return (double)GetValue(MinValueProperty); }
            set { SetValue(MinValueProperty, value); }
        }

        public double Value
        {
            get { return (double)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        private static object OnCoerceValueCallback(DependencyObject d, object baseValue)
        {
            var sender = d as NumericTextBox;
            double val = (double)baseValue;
            if (val > sender.MaxValue)
            {
                sender.ValueText.Text = string.Empty;
                sender.ValueText.Text = sender.MaxValue.ToString();
                return sender.MaxValue;
            }
            if (val < sender.MinValue)
            {
                sender.ValueText.Text = string.Empty;
                sender.ValueText.Text = sender.MinValue.ToString();
                return sender.MinValue;
            }
            return val;
        }

        private void DownButton_Click(object sender, RoutedEventArgs e)
        {
            double newValue = (Value - Increment);
            if (newValue < MinValue)
            {
                Value = MinValue;
            }
            else
            {
                Value = newValue;
            }
        }

        private void UpButton_Click(object sender, RoutedEventArgs e)
        {
            double newValue = (Value + Increment);
            if (newValue > MaxValue)
            {
                Value = MaxValue;
            }
            else
            {
                Value = newValue;
            }
        }

        private void ValueText_LostFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                Value = double.Parse(ValueText.Text);
            }
            catch (Exception)
            {
                Value = 0;
                ValueText.Text = Value.ToString();
            }
        }
    }
}