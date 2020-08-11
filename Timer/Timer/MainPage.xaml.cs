using Microsoft.Gaming.XboxGameBar;
using System;
using System.Diagnostics;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace Timer
{
	/// <summary>
	/// An empty page that can be used on its own or navigated to within a Frame.
	/// </summary>
	public sealed partial class MainPage : Page
	{
		XboxGameBarWidget widget = null;

		DispatcherTimer dispatcherTimer;
		Stopwatch stopwatch;

		bool IsRunning = false;

		public MainPage()
		{
			this.InitializeComponent();

			dispatcherTimer = new DispatcherTimer();
			dispatcherTimer.Tick += DispatcherTimer_Tick;
			dispatcherTimer.Interval = TimeSpan.FromMilliseconds(50);

			stopwatch = new Stopwatch();
		}

		private void DispatcherTimer_Tick(object sender, object e)
		{
			TimeSpan ElapsedTime = stopwatch.Elapsed;
			MainText.Text = ElapsedTime.ToString(@"hh\:mm\:ss");
			MilliText.Text = (ElapsedTime.Milliseconds/10).ToString("00");
		}

		private void StartButton_Click(object sender, RoutedEventArgs e)
		{
			if(!IsRunning)
			{
				stopwatch.Start();
				dispatcherTimer.Start();
				StartButton.Content = "Pause";
				ResetButton.Visibility = Visibility.Collapsed;
				StartButton.Width = 156;
				IsRunning = true;
			}
			else
			{
				stopwatch.Stop();
				dispatcherTimer.Stop();
				StartButton.Content = "Start";
				ResetButton.Visibility = Visibility.Visible;
				StartButton.Width = 75;
				IsRunning = false;
			}
			
		}

		private void ResetButton_Click(object sender, RoutedEventArgs e)
		{
			MainText.Text = "00:00:00";
			MilliText.Text = "00";
			stopwatch.Reset();
		}

		protected override void OnNavigatedTo(NavigationEventArgs e)
		{
			widget = e.Parameter as XboxGameBarWidget;

			if(widget != null)
			{
				widget.RequestedOpacityChanged += Widget_RequestedOpacityChanged;
				widget.RequestedThemeChanged += Widget_RequestedThemeChanged;
			}
		}

		private async void Widget_RequestedThemeChanged(XboxGameBarWidget sender, object args)
		{
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>

             {
                 RequestedTheme = widget.RequestedTheme;
             });
		}

		private void Widget_RequestedOpacityChanged(XboxGameBarWidget sender, object args)
		{
			Background.Opacity = widget.RequestedOpacity;
		}

		
	}
}
