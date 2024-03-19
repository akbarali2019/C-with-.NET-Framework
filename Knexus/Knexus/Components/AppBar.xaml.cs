using Knexus.Configuration;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Knexus.Components
{
    /// <summary>
    /// Interaction logic for RTCAppBar.xaml
    /// </summary>
    public partial class AppBar : UserControl
    {
        private readonly DispatcherTimer _timer;
        public AppBar()
        {
            InitializeComponent();
            SetTime();
            
            _timer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(1)
            };
            _timer.Tick += Timer_Tick;
            _timer.Start();

            SetImageToNetworkImage();
        }
        //
        public void SetTimerState(bool start)
        {
            if (start)
            {
                SetTime();
                _timer.Start();
            }
            else
            {
                _timer.Stop();
            }
        }
        //
        private void SetTime() { DateTimeTextBlock.Text = DateTime.Now.ToString("yyyy-MM-dd  HH:mm:ss"); }

        private void Timer_Tick(object? sender, EventArgs e)
        {
            SetTime();

            if (DateTime.Now.Second % 5 == 0)
            {
                SetImageToNetworkImage();
            }
        }

        private async void SetImageToNetworkImage()
        {
            string imageUrl = await getInternetStatus();
            BitmapImage bitmapImage = new BitmapImage();
            bitmapImage.BeginInit();
            bitmapImage.UriSource = new Uri(imageUrl, UriKind.RelativeOrAbsolute);
            bitmapImage.EndInit();

            NetworkStatus.Source = bitmapImage;
        }

        public static readonly DependencyProperty FirstNameProperty = DependencyProperty.Register(
            "FirstName", typeof(string), typeof(AppBar), new PropertyMetadata("Page Title", OnTitlePropertyChanged));

        public string FirstName
        {
            get { return (string)GetValue(FirstNameProperty); }
            set { SetValue(FirstNameProperty, value); }
        }
        //
        private static void OnTitlePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var appBar = d as AppBar;
            if (appBar != null)
            {
                appBar.HeaderText.Text = e.NewValue as string;
            }
        }

        
        //
        public static readonly DependencyProperty StatePropertry = DependencyProperty.Register(
            "State", typeof(RecordState), typeof(AppBar), new PropertyMetadata(RecordState.NONE, OnStatePropertyChanged));

        private static void OnStatePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var appBar = d as AppBar;
            if (appBar == null) return;
            for (int i = appBar.MainGrid.Children.Count - 1; i >= 0; i--)
            {
                var child = appBar.MainGrid.Children[i];
                if (Grid.GetColumn(child) == 1) // change the column index to match yours
                {
                    appBar.MainGrid.Children.Remove(child);
                }
            }
            RecordState newState = (RecordState)e.NewValue;
            var source = AppConstants.Descriptors.Where(d => d.State == newState).ToList();
            Grid grid = new Grid { Visibility = Visibility.Visible, HorizontalAlignment = HorizontalAlignment.Center };
            PopulateStateGridWithData(ref grid, source);
            Grid.SetColumn(grid, 1);
            appBar.MainGrid.Children.Add(grid);
        }

        public RecordState State
        {
            set { SetValue(StatePropertry, value); }
        }
        //
        private static void PopulateStateGridWithData(ref Grid grid, List<ColumnDescriptor> source)
        {
            for (int i = 0; i < source.Count; i++)
            {
                grid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });

                StackPanel stackPanel = new StackPanel
                {
                    VerticalAlignment = VerticalAlignment.Center,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    Orientation = Orientation.Horizontal,
                };
                Grid.SetColumn(stackPanel, i);

                TextBlock colorBlock = new TextBlock
                {
                    Width = 15,
                    Margin = new Thickness(5),
                    Background = (SolidColorBrush) new BrushConverter().ConvertFromString(source[i].Color!)!, ////
                };
                stackPanel.Children.Add(colorBlock);

                TextBlock textBlock = new TextBlock
                {
                    Text = $": {source[i].Text}",
                    VerticalAlignment = VerticalAlignment.Center,
                    FontWeight = FontWeights.Bold,
                };
                stackPanel.Children.Add(textBlock);

                grid.Children.Add(stackPanel);
            }
        }
        //
        Task<string> getInternetStatus()
        {
           return Task.Run(() =>
            {
                bool status = false;
                using Ping ping = new();
                try
                {
                    string hostName = "stackoverflow.com";
                    PingReply reply = ping.Send(hostName, 1000);
                    if (reply.Status == IPStatus.Success) status = true;
                    //Trace.WriteLine($"Ping status for ({hostName}): {reply.Status}");
                }
                catch { }
                finally { ping.Dispose(); }
                
                if (status)
                {
                    Trace.WriteLine("MainPage Bar");
                    return "../Images/Icons/signal.png";
                }
                else
                {
                    //Trace.WriteLine("False");
                    return "../Images/Icons/no_signal.png";
                }
            });
        }
    }

    public class ColumnDescriptor
    {
        public RecordState State { get; set; }
        public string? Color { get; set; } //
        public string? Text { get; set; } //
        public int Code { get; set; }
    }

    public enum RecordState
    {
        NONE,
        DATA,
        OPERATION,
        PREVENTION,
        SEND
    }
}
