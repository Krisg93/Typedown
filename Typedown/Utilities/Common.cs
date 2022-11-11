﻿using Microsoft.Extensions.DependencyInjection;
using System;
using System.Diagnostics;
using System.Linq;
using Typedown.Universal.Utilities;
using Typedown.Universal.Enums;
using Typedown.Universal.ViewModels;
using Typedown.Windows;
using Windows.UI.ViewManagement;
using Windows.Foundation;
using System.Reactive.Linq;
using Windows.UI;
using Typedown.Universal;

namespace Typedown.Utilities
{
    public static class Common
    {
        public static Point MakePoint(this IntPtr p) => new(p.ToInt32() & 0xFFFF, p.ToInt32() >> 16);

        public static nint PackPoint(this Point point) => ((int)point.X) | (((int)point.Y) << 16);

        public static int GetHighWord(IntPtr p) => (int)(p.ToInt64() >> 16);

        public static int GetLowWord(IntPtr p) => (int)(p.ToInt64() & 0xFFFF);

        static public void OpenUrl(string url)
        {
            try
            {
                Process.Start(url);
            }
            catch
            {
                url = url.Replace("&", "^&");
                Process.Start(new ProcessStartInfo("cmd", $"/c start {url}") { CreateNoWindow = true });
            }
        }

        public static object GetCurrentTheme(this IServiceProvider provider)
        {
            var settings = provider.GetService<SettingsViewModel>();
            var themeSetting = settings.AppTheme;
            var isDarkMode = themeSetting switch
            {
                AppTheme.Light => false,
                AppTheme.Dark => true,
                _ => !GetUseLightTheme()
            };
            var color = new UISettings().GetColorValue(UIColorType.Accent);
            var background = isDarkMode ? Color.FromArgb(0xFF, 0x28, 0x28, 0x28) : Color.FromArgb(0xFF, 0xF9, 0xF9, 0xF9);
            return new { theme = isDarkMode ? "Dark" : "Light", color, background };
        }

        public static bool GetUseLightTheme()
        {
            return App.Current.RequestedTheme == global::Windows.UI.Xaml.ApplicationTheme.Light;
        }

        public static void SaveWindowPlacement(this MainWindow window, Point offset = default)
        {
            PInvoke.GetWindowPlacement(window.Handle, out var placement);
            window.AppViewModel.SettingsViewModel.StartupPlacement = placement;
        }

        public static bool TryRestoreWindowPlacement(this MainWindow window)
        {
            var placement = window.AppViewModel.SettingsViewModel.StartupPlacement;
            if (!placement.HasValue)
                return false;
            var value = placement.Value;
            if (value.showCmd != PInvoke.ShowWindowCommand.ShowMaximized)
                value.showCmd = PInvoke.ShowWindowCommand.Normal;
            PInvoke.SetWindowPlacement(window.Handle, ref value);
            return true;
        }

        public static IntPtr OpenNewWindow(string[] args)
        {
            var filePath = CommandLine.GetOpenFilePath(args);
            if (string.IsNullOrEmpty(filePath) || !FileViewModel.TryGetOpenedWindow(filePath, out var windowHWnd))
            {
                var newWindow = new MainWindow();
                newWindow.Show();
                newWindow.AppViewModel.CommandLineArgs = args;
                return newWindow.Handle;
            }
            else
            {
                var appWindows = FrameWindow.Windows.OfType<AppWindow>();
                var window = appWindows.Where(x => x.Handle == windowHWnd).FirstOrDefault();
                if (window != null && PInvoke.IsIconic(window.Handle))
                    PInvoke.ShowWindow(window.Handle, PInvoke.ShowWindowCommand.Restore);
                return windowHWnd;
            }
        }
    }
}
