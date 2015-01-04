﻿using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Resources;
using Toasts.Forms.Plugin.Abstractions;

namespace Toasts.Forms.Plugin.WindowsPhone
{
    public class DefaultToastLayoutRenderer : IToastLayoutCustomRenderer
    {
        public virtual UIElement Render(ToastNotificationType type, string title, string description, out Brush brush)
        {
            string iconFile;

            switch (type)
            {
                case ToastNotificationType.Info:
                    brush = new SolidColorBrush(Color.FromArgb(255, 42, 112, 153));
                    iconFile = "info.png";
                    break;
                case ToastNotificationType.Success:
                    brush = new SolidColorBrush(Color.FromArgb(255, 69, 145, 34));
                    iconFile = "success.png";
                    break;
                case ToastNotificationType.Warning:
                    brush = new SolidColorBrush(Color.FromArgb(255, 180, 125, 1));
                    iconFile = "warning.png";
                    break;
                case ToastNotificationType.Error:
                    brush = new SolidColorBrush(Color.FromArgb(255, 206, 24, 24));
                    iconFile = "error.png";
                    break;
                default:
                    throw new ArgumentOutOfRangeException("type");
            }

            //this actually could be done in XAML, but I decided not to waste performance on XAML parsing
            //since UI is simple and if you want - you can override it by implementing IToastLayoutCustomRenderer

            var titleTb = new TextBlock
            {
                Text = title,
                VerticalAlignment = VerticalAlignment.Center,
                Margin = new Thickness(12, 6, 0, 0),
                TextTrimming = TextTrimming.None,
                TextWrapping = TextWrapping.NoWrap,
                Foreground = new SolidColorBrush(Colors.White),
                FontSize = 22,
                FontWeight = FontWeights.Bold
            };
            Grid.SetColumn(titleTb, 1);
            Grid.SetRow(titleTb, 0);

            var descTb = new TextBlock
            {
                Text = description,
                VerticalAlignment = VerticalAlignment.Center,
                Margin = new Thickness(12, -4, 0, 0),
                TextTrimming = TextTrimming.WordEllipsis,
                TextWrapping = TextWrapping.NoWrap,
                Foreground = new SolidColorBrush(Colors.White),
                FontSize = 18
            };
            Grid.SetColumn(descTb, 1);
            Grid.SetRow(descTb, 1);

            Image image = new Image
            {
                Width = 42,
                Height = 42,
                Margin = new Thickness(10, 0, 0, 0),
                VerticalAlignment = VerticalAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Center,
                Source = LoadBitmapImage(iconFile)
            };
            Grid.SetRowSpan(image, 2);

            Grid layout = new Grid();
            layout.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Auto) });
            layout.ColumnDefinitions.Add(new ColumnDefinition());
            layout.RowDefinitions.Add(new RowDefinition());
            layout.RowDefinitions.Add(new RowDefinition());

            layout.Children.Add(image);
            layout.Children.Add(titleTb);
            layout.Children.Add(descTb);

            return layout;
        }

        public bool IsTappable
        {
            get { return true; }
        }

        public bool HasCloseButton
        {
            get { return true; }
        }

        private static BitmapImage LoadBitmapImage(string fileName)
        {
            Uri imgUri = new Uri("/Toasts.Forms.Plugin.WindowsPhone;component/Icons/" + fileName, UriKind.Relative);
            StreamResourceInfo imageResource = Application.GetResourceStream(imgUri);
            BitmapImage image = new BitmapImage();
            image.SetSource(imageResource.Stream);
            return image;
        }
    }
}
