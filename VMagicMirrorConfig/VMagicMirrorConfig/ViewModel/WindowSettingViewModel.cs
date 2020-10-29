﻿using System;
using System.Windows;
using System.Windows.Media;
using System.Xml.Serialization;

namespace Baku.VMagicMirrorConfig
{
    //モデルが小さいのでVMと完全癒着してる点に注意(VMというよりNotifiableなモデル)
    public class WindowSettingViewModel : SettingViewModelBase
    {
        public WindowSettingViewModel() : base() { }
        internal WindowSettingViewModel(IMessageSender sender) : base(sender)
        {
        }

        public void Initialize()
        {
            UpdateBackgroundColor();
        }

        private int _r = 0;
        public int R
        {
            get => _r;
            set
            {
                if (SetValue(ref _r, value))
                {
                    UpdateBackgroundColor();
                    RaisePropertyChanged(nameof(Color));
                }
            }
        }

        private int _g = 255;
        public int G
        {
            get => _g;
            set
            {
                if (SetValue(ref _g, value))
                {
                    UpdateBackgroundColor();
                    RaisePropertyChanged(nameof(Color));
                }
            }
        }

        private int _b = 0;
        public int B
        {
            get => _b;
            set
            {
                if (SetValue(ref _b, value))
                {
                    UpdateBackgroundColor();
                    RaisePropertyChanged(nameof(Color));
                }
            }
        }

        [XmlIgnore]
        public Color Color { get; private set; }
        
        private void UpdateBackgroundColor()
        {
            Color = IsTransparent ?
                Color.FromArgb(0, 0, 0, 0) :
                Color.FromArgb(255, (byte)R, (byte)G, (byte)B);

            if (IsTransparent)
            {
                SendMessage(MessageFactory.Instance.Chromakey(0, 0, 0, 0));
            }
            else
            {
                SendMessage(MessageFactory.Instance.Chromakey(255, R, G, B));
            }
        }

        private bool _isTransparent = false;
        public bool IsTransparent
        {
            get => _isTransparent;
            set
            {
                if (SetValue(ref _isTransparent, value))
                {
                    HideWindowFrame = IsTransparent;
                    UpdateBackgroundColor();
                    if (IsTransparent)
                    {
                        HideWindowFrame = true;
                        if (IgnoreMouseWhenTransparent)
                        {
                            SendMessage(MessageFactory.Instance.IgnoreMouse(IgnoreMouseWhenTransparent));
                        }
                    }
                    else
                    {
                        SendMessage(MessageFactory.Instance.IgnoreMouse(false));
                    }
                }
            }
        }

        private bool _windowDraggable = true;
        public bool WindowDraggable
        {
            get => _windowDraggable;
            set
            {
                if (SetValue(ref _windowDraggable, value))
                {
                    IgnoreMouseWhenTransparent = !WindowDraggable;
                    SendMessage(MessageFactory.Instance.WindowDraggable(WindowDraggable));
                }
            }
        }

        private bool _topMost = true;
        public bool TopMost
        {
            get => _topMost;
            set
            {
                if (SetValue(ref _topMost, value))
                {
                    SendMessage(MessageFactory.Instance.TopMost(TopMost));
                }
            }
        }

        private bool _virtualCamEnabled = false;
        public bool VirtualCamEnabled
        {
            get => _virtualCamEnabled;
            set
            {
                if (SetValue(ref _virtualCamEnabled, value))
                {
                    SendMessage(MessageFactory.Instance.SetVirtualCamEnable(VirtualCamEnabled));
                }
            }
        }

        private int _virtualCamWidth = 640;
        public int VirtualCamWidth
        {
            get => _virtualCamWidth;
            set
            {
                if (_virtualCamWidth == value)
                {
                    return;
                }
                else if (value < 80 || value > 1920 || _virtualCamWidth == value - value % 4)
                {
                    //4の倍数になるよう調整したら元と同じになるケースや、単に値が極端な場合: 変化前の値に戻りました、という体裁にする
                    RaisePropertyChanged();
                    return;
                }
                else
                {
                    _virtualCamWidth = value - value % 4;
                    RaisePropertyChanged();
                    SendMessage(MessageFactory.Instance.SetVirtualCamWidth(VirtualCamWidth));
                }
            }
        }

        private int _virtualCamHeight = 480;
        public int VirtualCamHeight
        {
            get => _virtualCamHeight;
            set
            {
                if (_virtualCamHeight == value)
                {
                    return;
                }
                else if (value < 80 || value > 1920 ||  _virtualCamHeight == value - value % 4)
                {
                    RaisePropertyChanged();
                    return;
                }
                else
                {
                    _virtualCamHeight = value - value % 4;
                    RaisePropertyChanged();
                    SendMessage(MessageFactory.Instance.SetVirtualCamHeight(VirtualCamHeight));
                }
            }
        }

        private ActionCommand? _virtualCamResizeCommand;
        public ActionCommand VirtualCamResizeCommand
            => _virtualCamResizeCommand ??= new ActionCommand(VirtualCamResize);
        private void VirtualCamResize()
        {
            SendMessage(MessageFactory.Instance.SetVirtualCamBasedWindowSize(VirtualCamWidth, VirtualCamHeight));
        }

        private ActionCommand? _resetVirtualCamSettingCommand;
        public ActionCommand ResetVirtualCamSettingCommand
            => _resetVirtualCamSettingCommand ??= new ActionCommand(ResetVirtualCamSetting);
        private void ResetVirtualCamSetting()
        {
            VirtualCamEnabled = false;
            VirtualCamWidth = 640;
            VirtualCamHeight = 480;
        }

        private ActionCommand? _openCameraInstallDialogCommand;
        public ActionCommand OpenCameraInstallDialogCommand
            => _openCameraInstallDialogCommand ??= new ActionCommand(OpenCameraInstallDialog);
        private void OpenCameraInstallDialog()
        {
            //NOTE: カメラのインストール/アンインストールは.batの実行で実現するためViewModelはIPCを行わない
            new CameraInstallWindow()
            {
                DataContext = new CameraInstallerViewModel(),
                Owner = SettingWindow.CurrentWindow ?? App.Current.MainWindow,
                WindowStartupLocation = WindowStartupLocation.CenterOwner,
            }.ShowDialog();
        }


        private ActionCommand? _resetWindowPositionCommand;
        public ActionCommand ResetWindowPositionCommand
            => _resetWindowPositionCommand ??= new ActionCommand(ResetWindowPosition);

        private void ResetWindowPosition()
        {
            //NOTE: ウィンドウが被ると困るのを踏まえ、すぐ上ではなく右わきに寄せる点にご注目
            var pos = WindowPositionUtil.GetThisWindowRightTopPosition();
            SendMessage(MessageFactory.Instance.MoveWindow(pos.X, pos.Y));
            SendMessage(MessageFactory.Instance.ResetWindowSize());
        }


        private int _wholeWindowTransparencyLevel = 2;
        public int WholeWindowTransparencyLevel
        {
            get => _wholeWindowTransparencyLevel;
            set
            {
                if (SetValue(ref _wholeWindowTransparencyLevel, value))
                {
                    SendMessage(
                        MessageFactory.Instance.SetWholeWindowTransparencyLevel(_wholeWindowTransparencyLevel)
                        );
                }
            }
        }

        private int _alphaValueOnTransparent = 128;
        public int AlphaValueOnTransparent
        {
            get => _alphaValueOnTransparent;
            set
            {
                if (SetValue(ref _alphaValueOnTransparent, value))
                {
                    SendMessage(
                        MessageFactory.Instance.SetAlphaValueOnTransparent(_alphaValueOnTransparent)
                        );
                }
            }
        }


        #region privateになったプロパティ

        private bool _hideWindowFrame = false;
        private bool HideWindowFrame
        {
            get => _hideWindowFrame;
            set
            {
                if (SetValue(ref _hideWindowFrame, value))
                {
                    SendMessage(MessageFactory.Instance.WindowFrameVisibility(!HideWindowFrame));
                }
            }
        }

        private bool _ignoreMouseWhenTransparent = false;
        private bool IgnoreMouseWhenTransparent
        {
            get => _ignoreMouseWhenTransparent;
            set
            {
                //すでに透明だったときだけ処理する(不透明時はIsTransparentが変わったタイミングでメッセージが飛ぶ)
                if (SetValue(ref _ignoreMouseWhenTransparent, value) &&
                    IsTransparent)
                {
                    SendMessage(MessageFactory.Instance.IgnoreMouse(IgnoreMouseWhenTransparent));
                }
            }
        }

        #endregion

        #region Reset API

        private ActionCommand? _resetBackgroundColorSettingCommand = null;
        public ActionCommand ResetBackgroundColorSettingCommand
            => _resetBackgroundColorSettingCommand ??= new ActionCommand(
                () => SettingResetUtils.ResetSingleCategorySettingAsync(ResetBackgroundColor)
                );

        private void ResetBackgroundColor()
        {
            R = 0;
            G = 255;
            B = 0;
        }

        private ActionCommand? _resetOpacitySettingCommand = null;
        public ActionCommand ResetOpacitySettingCommand
            => _resetOpacitySettingCommand ??= new ActionCommand(
                () => SettingResetUtils.ResetSingleCategorySettingAsync(ResetOpacity)
                );

        private void ResetOpacity()
        {
            WholeWindowTransparencyLevel = 2;
            AlphaValueOnTransparent = 128;
        }


        #endregion

        public override void ResetToDefault()
        {
            ResetBackgroundColor();

            IsTransparent = false;
            WindowDraggable = true;
            TopMost = true;

            ResetOpacity();

            ResetVirtualCamSetting();

            //このリセットはあまり定数的ではないことに注意！
            ResetWindowPosition();
        }

        #region Unity側にUIが出っぱなしの状態でexeを切ったときの復帰用API

        /// <summary>
        /// <see cref="TopMost"/>の値を変更しますが、プロセス間通信は行いません。
        /// </summary>
        /// <param name="topMost"></param>
        public void SilentSetTopMost(bool topMost)
            => SetValue(ref _topMost, topMost, nameof(TopMost));

        #endregion
    }
}
