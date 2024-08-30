using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Media.Animation;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Devices.Input;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Microsoft.UI;
using Microsoft.UI.Xaml.Documents;



// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Test_App
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainWindow : Window
    {
        private DispatcherTimer increaseTimer;
        private DispatcherTimer decreaseTimer;
        private double accelerationHeight = 0;
        private const double MaxHeight = 200;

        public MainWindow()
        {
            this.InitializeComponent();
            this.AppWindow.MoveAndResize(new Windows.Graphics.RectInt32(500, 870, 500, 870));
            Congratulation.Visibility = Visibility.Collapsed;
            AddSessionHistory();
            this.Title = "Test App";

            increaseTimer = new DispatcherTimer();
            increaseTimer.Interval = TimeSpan.FromMilliseconds(50); 
            increaseTimer.Tick += IncreaseTimer_Tick;

            decreaseTimer = new DispatcherTimer();
            decreaseTimer.Interval = TimeSpan.FromMilliseconds(50);
            decreaseTimer.Tick += DecreaseTimer_Tick;

            AccelerateButton.AddHandler(UIElement.PointerPressedEvent, new PointerEventHandler(AccelerateButton_PointerPressed), true);
            AccelerateButton.AddHandler(UIElement.PointerReleasedEvent, new PointerEventHandler(AccelerateButton_PointerReleased), true);

        }

        private void AccelerateButton_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            //AddSessionHistory(); //for testing SessionHistory
            decreaseTimer.Stop(); 
            increaseTimer.Start(); 
        }

        private void AccelerateButton_PointerReleased(object sender, PointerRoutedEventArgs e)
        {
            increaseTimer.Stop(); 
            decreaseTimer.Start(); 
        }

        private void AccelerationTimer_Tick(object sender, object e)
        {
            if (accelerationHeight < 200) 
            {
                accelerationHeight += 2; 
                accelerationRectangle.Height = accelerationHeight;
                UpdateRectangleColor();
            }
            else
            {
                increaseTimer.Stop();
            }
        }
        private void IncreaseTimer_Tick(object sender, object e)
        {
            if (accelerationHeight < 200) 
            {
                accelerationHeight += 3; 
                accelerationRectangle.Height = accelerationHeight;
                UpdateRectangleColor();
            }
            else
            {
                increaseTimer.Stop();
            }
        }

        private void DecreaseTimer_Tick(object sender, object e)
        {
            if (accelerationHeight > 0) 
            {
                accelerationHeight -= 1;
                accelerationRectangle.Height = accelerationHeight;
                UpdateRectangleColor();
            }
            else
            {
                decreaseTimer.Stop();
            }
        }

        private bool isDialogOpen = false; 

        private async void UpdateRectangleColor()
        {
            if (accelerationHeight >= MaxHeight)
            {
                accelerationRectangle.Fill = new SolidColorBrush(Colors.Red);
                Congratulation.Visibility = Visibility.Visible;

                if (!isDialogOpen)
                {
                    isDialogOpen = true; 

                    ContentDialog dialog = new ContentDialog()
                    {
                        Title = "Congratulations🥳",
                        Content = "Nice Work✨. Adding New Session",
                        CloseButtonText = "OK",
                    };
                    dialog.XamlRoot = AccelerateButton.XamlRoot;

                    ContentDialogResult result = await dialog.ShowAsync();

                    if (result == ContentDialogResult.None) 
                    {
                        Congratulation.Visibility = Visibility.Collapsed;
                        AddSessionHistory(); // For Session History Checking
                    }

                    isDialogOpen = false; 
                }
            }
            else
            {
                accelerationRectangle.Fill = new SolidColorBrush(Colors.Blue);
                //Congratulation.Visibility = Visibility.Collapsed;
            }
        }

        private void AddSessionHistory()
        {
            string loginTimeText = $"Login time: {DateTime.Now:yyyy-MM-dd | hh:mm tt}";
            string sessionText = $" Session {sessionHistoryList.Items.Count + 1}";

            var loginTimeTextBlock = new TextBlock
            {
                Text = loginTimeText,
                Foreground = new SolidColorBrush(Colors.Gray),
                Margin = new Thickness(0, 5, 0, 0)
            };

            var sessionTextBlock = new TextBlock
            {
                Text = sessionText,
                Foreground = new SolidColorBrush(Colors.Black),
                Margin = new Thickness(-4, 0, 0, 0)
            };

            var stackPanel = new StackPanel
            {
                Orientation = Orientation.Vertical,
                Children = { loginTimeTextBlock, sessionTextBlock }
            };

            sessionHistoryList.Items.Add(new ListViewItem { Content = stackPanel });
        }

    }
}
