﻿using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Linq;
using System.ComponentModel;
using System.IO;
using Typedown.Core.Models;
using Typedown.Core.Services;
using Typedown.Core.Utilities;
using Typedown.Core.ViewModels;
using Windows.UI.Xaml;

namespace Typedown.Core.Controls.EditorControls.ContextMenuItems
{
    public sealed partial class ImageItem : MenuItemCollection, INotifyPropertyChanged
    {
        public AppViewModel ViewModel { get; private set; }

        public ImageUpload ImageUpload => ViewModel.ServiceProvider.GetService<ImageUpload>();

        public JToken SelectedImage { get; private set; }

        public string ImageSrc => SelectedImage?["token"]?["src"]?.ToString() ?? "";

        public ImageItem()
        {
            this.InitializeComponent();
        }

        private void OnOpenImageLocationItemLoaded(object sender, RoutedEventArgs e)
        {
            ViewModel = (sender as FrameworkElement).GetService<AppViewModel>();
            SelectedImage = ViewModel.EditorViewModel.Selection["selectedImage"];
            UpdateMenuItemState();
            // UpdateImageUploadConfigItem();
        }

        private void UpdateMenuItemState()
        {
            var isLocalImage = UriHelper.TryGetLocalPath(ImageSrc, out _);
            OpenImageLocationItem.IsEnabled = isLocalImage;
            // MoveImageItem.IsEnabled = isLocalImage;
            // DeleteImageItem.IsEnabled = isLocalImage;
        }

        //private void UpdateImageUploadConfigItem()
        //{
        //    var configs = ImageUpload.ImageUploadConfigs.Where(x => x.IsEnable).ToList();
        //    while (UploadSubMenu.Items[1] is not MenuFlyoutSeparator)
        //        UploadSubMenu.Items.RemoveAt(1);
        //    foreach (var config in configs.Reverse<ImageUploadConfig>())
        //    {
        //        var item = new MenuFlyoutItem() { Text = config.Name, Tag = config };
        //        item.Click += (s, e) => OnUploadImageClick(config);
        //        UploadSubMenu.Items.Insert(1, item);
        //    }
        //    NoUploadConfigItem.Visibility = configs.Any() ? Visibility.Collapsed : Visibility.Visible;
        //}

        private void OnOpenImageLocationClick(object sender, RoutedEventArgs e)
        {
            try
            {
                UriHelper.TryGetLocalPath(ImageSrc, out var path);
                var fullPath = Path.GetFullPath(Path.Combine(ViewModel.FileViewModel.ImageBasePath, path));
                Common.OpenFileLocation(fullPath);
            }
            catch
            {

            }
        }

        private void OnCopyImageToClick(object sender, RoutedEventArgs e)
        {

        }

        private void OnMoveImageToClick(object sender, RoutedEventArgs e)
        {

        }

        private void OnUploadImageClick(ImageUploadConfig config)
        {

        }

        private void OnImageUploadSettingsClick(object sender, RoutedEventArgs e)
        {
            ViewModel.NavigateCommand.Execute("Settings/Image");
        }

        private void OnSaveImageClick(object sender, RoutedEventArgs e)
        {

        }

        private void OnDeleteImageFileClick(object sender, RoutedEventArgs e)
        {

        }
    }
}
