using Microsoft.Gaming.XboxGameBar;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace Timer
{

	/// <summary>
	/// An empty page that can be used on its own or navigated to within a Frame.
	/// </summary>
	public sealed partial class MainPage : Page
	{
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

		XboxGameBarWidget widget = null;

		protected override void OnNavigatedTo(NavigationEventArgs e)
		{
			widget = e.Parameter as XboxGameBarWidget;

			if(widget != null)
				widget.RequestedOpacityChanged += Widget_RequestedOpacityChanged;
		}

		private void Widget_RequestedOpacityChanged(XboxGameBarWidget sender, object args)
		{
			Background.Opacity = widget.RequestedOpacity;
		}
	}
}
