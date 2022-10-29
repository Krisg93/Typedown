﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage.Pickers;
using Windows.UI.Xaml;

namespace Typedown.Universal.Utilities
{
    public static class FilePickersExtensions
    {
        [ComImport]
        [Guid("3E68D4BD-7135-4D10-8018-9FB6D9F33FA1")]
        [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        private interface IInitializeWithWindow
        {
            void Initialize(IntPtr hwnd);
        }

        public static void SetOwnerWindow(this FileOpenPicker picker, nint hWnd)
        {
            (picker as object as IInitializeWithWindow).Initialize(hWnd);
        }

        public static void SetOwnerWindow(this FileSavePicker picker, nint hWnd)
        {
            (picker as object as IInitializeWithWindow).Initialize(hWnd);
        }

        public static void SetOwnerWindow(this FolderPicker picker, nint hWnd)
        {
            (picker as object as IInitializeWithWindow).Initialize(hWnd);
        }
    }
}