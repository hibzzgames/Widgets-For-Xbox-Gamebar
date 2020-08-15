using Microsoft.Gaming.XboxGameBar;
using System;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Windows.System;
using Windows.System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Diagnostics;


// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace AltTabber
{
	/// <summary>
	/// An empty page that can be used on its own or navigated to within a Frame.
	/// </summary>
	public sealed partial class MainPage : Page
	{
		XboxGameBarWidget widget;

		public MainPage()
		{
			this.InitializeComponent();
		}

		#region xbox gamebar code

		protected override void OnNavigatedTo(NavigationEventArgs e)
		{
			widget = e.Parameter as XboxGameBarWidget;

			if (widget != null)
			{
				widget.RequestedOpacityChanged += Widget_RequestedOpacityChanged;
				widget.RequestedThemeChanged += Widget_RequestedThemeChanged;
                widget.VisibleChanged += Widget_VisibleChanged;
			}
		}

        private async void Widget_VisibleChanged(XboxGameBarWidget sender, object args)
        {
			if (sender.Visible == true)
			{
				await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
				{
					ReloadWindows();
				});
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

		#endregion
		
		private async void ReloadWindows()
        {
			bool DiagnosticAccessStatusResult = await GetDiagnosticAccessStatusAsync();

			if(DiagnosticAccessStatusResult)
            {
				string s = "";

				var pricesses = Process.GetProcesses();

				var processes = ProcessDiagnosticInfo.GetForProcesses().Where(p => p.IsPackaged).ToArray();

				foreach(var process in processes)
                {
					s += process.ExecutableFileName + "\n";
                }
				
				MainText.Text = s;
				MainText.Visibility = Visibility.Visible;
			}
            else
            {
				MainText.Visibility = Visibility.Collapsed;
            }
        }

		private async System.Threading.Tasks.Task<bool> GetDiagnosticAccessStatusAsync()
        {
			DiagnosticAccessStatus diagnosticAccessStatus = await AppDiagnosticInfo.RequestAccessAsync();
			
			switch (diagnosticAccessStatus)
			{
				case DiagnosticAccessStatus.Allowed:
					DebugText.Visibility = Visibility.Collapsed;
					return true;
				case DiagnosticAccessStatus.Limited:
					DebugText.Visibility = Visibility.Visible;
					DebugText.Text = "Diagnostic info is a core feature requirement as we use it to get the list of open applications. " +
						"We want you to make an informed choice, and we hope you give us the permission. To do so, " +
						"\n \nGo to Settings > Apps > Apps and Features > AltTabber > Advanced Options, and turn on App Diagnostics. ";
					return false;
			}

			return false;
		}

	}
}
