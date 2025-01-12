﻿using Microsoft.Extensions.DependencyInjection;
using Typedown.Controls;
using Typedown.Core.Controls.FloatControls;
using Typedown.Core.Interfaces;
using Typedown.Core.Services;
using Typedown.Core.ViewModels;
using Typedown.Services;

namespace Typedown
{
    public static class Injection
    {
        public static ServiceProvider ServiceProvider { get; }

        static Injection()
        {
            if (ServiceProvider == null)
            {
                var builder = new ServiceCollection();
                RegisterViewModel(builder);
                RegisterService(builder);
                RegisterComponent(builder);
                ServiceProvider = builder.BuildServiceProvider();
            }
        }

        private static void RegisterViewModel(ServiceCollection builder)
        {
            builder.AddScoped<AppViewModel>();
            builder.AddScoped<EditorViewModel>();
            builder.AddScoped<FileViewModel>();
            builder.AddScoped<FloatViewModel>();
            builder.AddScoped<FormatViewModel>();
            builder.AddScoped<ParagraphViewModel>();
            builder.AddScoped<SettingsViewModel>();
            builder.AddScoped<UIViewModel>();
        }

        private static void RegisterService(ServiceCollection builder)
        {
            builder.AddScoped<IClipboard, Clipboard>();
            builder.AddScoped<IFileConverter, FileConverter>();
            builder.AddScoped<IFileExport, FileExport>();
            builder.AddScoped<IFileOperation, FileOperation>();
            builder.AddScoped<IKeyboardAccelerator, KeyboardAccelerator>();
            builder.AddScoped<IPowerShellService, PowerShellService>();
            builder.AddScoped<IWindowService, WindowService>();
            builder.AddScoped<AutoBackup>();
            builder.AddScoped<EventCenter>();
            builder.AddScoped<ImageAction>();
            builder.AddScoped<ImageUpload>();
            builder.AddScoped<RemoteInvoke>();
            builder.AddScoped<Transport>();
            builder.AddSingleton<AccessHistory>();
        }

        private static void RegisterComponent(ServiceCollection builder)
        {
            builder.AddScoped<IMarkdownEditor, MarkdownEditor>();
            builder.AddTransient<FrontMenu>();
            builder.AddTransient<TableTools>();
            builder.AddTransient<ImageSelector>();
            builder.AddTransient<ImageToolbar>();
            builder.AddTransient<ToolTip>();
        }
    }
}
