﻿using System;
using System.Windows;

namespace Baku.VMagicMirrorConfig
{
    public partial class SettingWindow : Window
    {
        public SettingWindow() => InitializeComponent();

        /// <summary>現在設定ウィンドウがあればそれを取得し、なければnullを取得します。</summary>
        public static SettingWindow? CurrentWindow { get; private set; } = null;

        public static void OpenOrActivateExistingWindow(object dataContext)
        {
            if (CurrentWindow == null)
            {
                CurrentWindow = new SettingWindow()
                {
                    Owner = Application.Current.MainWindow,
                    WindowStartupLocation = WindowStartupLocation.CenterOwner,
                    DataContext = dataContext,
                };
                
                CurrentWindow.Closed += OnSettingWindowClosed;
                CurrentWindow.Show();
            }
            else
            {
                CurrentWindow.Activate();
            }
        }

        private static void OnSettingWindowClosed(object? sender, EventArgs e)
        {
            if (CurrentWindow != null)
            {
                CurrentWindow.Closed -= OnSettingWindowClosed;
                CurrentWindow = null;
            }
        }
    }
}
