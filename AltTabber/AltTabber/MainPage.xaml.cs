using Microsoft.Gaming.XboxGameBar;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;


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
    }
}
