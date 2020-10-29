namespace Baku.VMagicMirrorConfig
{
    //メッセージボックスで表示するテキストの言語別対応。
    //リソースに書くほどでもないのでベタに書く
    class MessageIndication
    {
        private MessageIndication(string title, string content)
        {
            Title = title;
            Content = content;
        }

        public string Title { get; }
        public string Content { get; }

        public static MessageIndication LoadVrmConfirmation(string languageName)
            => LoadVrmConfirmation(LanguageSelector.StringToLanguage(languageName));

        public static MessageIndication ResetSettingConfirmation(string languageName)
            => ResetSettingConfirmation(LanguageSelector.StringToLanguage(languageName));

        public static MessageIndication ResetSingleCategoryConfirmation(string languageName)
            => ResetSingleCategoryConfirmation(LanguageSelector.StringToLanguage(languageName));
        public static MessageIndication ShowVRoidSdkUi(string languageName)
            => ShowVRoidSdkUi(LanguageSelector.StringToLanguage(languageName));

        public static MessageIndication ShowLoadingPreviousVRoid(string languageName)
            => ShowLoadingPreviousVRoid(LanguageSelector.StringToLanguage(languageName));

        /// <summary>
        /// NOTE: Contentのほうがフォーマット文字列なのでstring.Formatで消すアイテムの名前を指定して完成させること！
        /// string.Format(res.Content, "itemName")
        /// みたいな。
        /// </summary>
        /// <param name="languageName"></param>
        /// <returns></returns>
        public static MessageIndication ErrorLoadSetting(string languageName)
            => ErrorLoadSetting(LanguageSelector.StringToLanguage(languageName));

        /// <summary>
        /// NOTE: Contentのほうがフォーマット文字列なのでstring.Formatで消すアイテムの名前を指定して完成させること！
        /// ex: string.Format(res.Content, "itemName")
        /// </summary>
        /// <param name="languageName"></param>
        /// <returns></returns>
        public static MessageIndication DeleteWordToMotionItem(string languageName)
            => DeleteWordToMotionItem(LanguageSelector.StringToLanguage(languageName));

        /// <summary>
        /// NOTE: Contentがフォーマット文字列なため、削除予定のブレンドシェイプ名を指定して完成させること
        /// ex: string.Format(res.Content, "clipName")
        /// </summary>
        /// <param name="languageName"></param>
        /// <returns></returns>
        public static MessageIndication ForgetBlendShapeClip(string languageName)
            => ForgetBlendShapeClip(LanguageSelector.StringToLanguage(languageName));

        /// <summary>
        /// 無効なIPアドレスを指定したときに怒る文言です。
        /// </summary>
        /// <param name="languageName"></param>
        /// <returns></returns>
        public static MessageIndication InvalidIpAddress(string languageName)
            => InvalidIpAddress(LanguageSelector.StringToLanguage(languageName));

        /// <summary>
        /// モデルでExTrackerのパーフェクトシンクに必要なブレンドシェイプクリップが未定義だったときのエラーのヘッダー部
        /// </summary>
        /// <param name="languageName"></param>
        /// <returns></returns>
        public static MessageIndication ExTrackerMissingBlendShapeNames(string languageName)
            => ExTrackerMissingBlendShapeNames(LanguageSelector.StringToLanguage(languageName));

        /// <summary>
        /// webカメラのトラッキングを使うために外部トラッキングを切ろうとしてる人向けの確認ダイアログ
        /// </summary>
        /// <param name="languageName"></param>
        /// <returns></returns>
        public static MessageIndication ExTrackerCheckTurnOff(string languageName)
            => ExTrackerCheckTurnOff(LanguageSelector.StringToLanguage(languageName));

        public static MessageIndication LoadVrmConfirmation(Languages lang) => lang switch
        {
            Languages.Japanese => new MessageIndication(
                "VRMの読み込み",
                "ビューアー画面のライセンスを確認してください。読み込みますか？"
                ),
            _ => new MessageIndication(
                "加载VRM文件",
                "请确认窗口画面的许可协议。继续读取吗？"
                ),
        };

        public static MessageIndication ResetSettingConfirmation(Languages lang) => lang switch
        {
            Languages.Japanese => new MessageIndication(
                "設定のリセット",
                "リセットを実行すると全ての設定が初期状態に戻ります。リセットしますか？"
                ),
            _ => new MessageIndication(
                "重置",
                "确认要把所有设置还原到初始状态？"
                ),
        };

        public static MessageIndication ResetSingleCategoryConfirmation(Languages lang) => lang switch
        {
            Languages.Japanese => new MessageIndication(
                "設定のリセット",
                "選択したカテゴリの設定をリセットしますか？"
                ),
            _ => new MessageIndication(
                "重置",
                "确认要把所选类别的设置重置吗?"
                ),
        };

        public static MessageIndication ErrorLoadSetting(Languages lang) => lang switch
        {
            Languages.Japanese => new MessageIndication(
                "ロード失敗",
                "設定ファイルのロードに失敗しました。エラー: "
                ),
            _ => new MessageIndication(
                "加载失败",
                "无法加载设置文件。 Error: "
                ),
        };

        public static MessageIndication DeleteWordToMotionItem(Languages lang) => lang switch
        {
            Languages.Japanese => new MessageIndication(
                "モーションの削除",
                "このモーション'{0}'を削除しますか？"
                ),
            _ => new MessageIndication(
                "删除动作",
                "确定要删除 '{0}'吗?"
                ),
        };

        public static MessageIndication ForgetBlendShapeClip(Languages lang) => lang switch
        {
            Languages.Japanese => new MessageIndication(
                "ブレンドシェイプ情報のクリア",
                "このブレンドシェイプ'{0}'の設定を削除しますか？"
                ),
            _ => new MessageIndication(
                "删除混合图形设定",
                "确定要删除 '{0}'吗?"
                ),
        };

        public static MessageIndication InvalidIpAddress(Languages lang) => lang switch
        {
            Languages.Japanese => new MessageIndication(
                "無効なIPアドレス",
                "無効なIPアドレスが指定されています。入力を確認して下さい。"
                ),
            _ => new MessageIndication(
                "无效的IP地址",
                "设置了无效的IP地址，请检查输入。 "
                ),
        };

        public static MessageIndication ShowVRoidSdkUi(Languages lang) => lang switch
        {
            Languages.Japanese => new MessageIndication(
                "VRoid Hubに接続中",
                "キャラクターウィンドウ上でモデルを選択するか、またはキャンセルしてください。"),
            _ => new MessageIndication(
                "连接到VRoid Hub",
                "选择要加载的模型，或取消操作。 "),
        };

        public static MessageIndication ShowLoadingPreviousVRoid(Languages lang) => lang switch
        {
            Languages.Japanese => new MessageIndication(
                "VRoid Hubに接続中",
                "前回使用したモデルのロードを試みています。モデルをロードするか、またはキャンセルしてください。"),
            _ => new MessageIndication(
                "连接到VRoid Hub",
                "正在尝试加载以前使用的形象。选择模型或取消操作。"),
        };

        public static MessageIndication ExTrackerMissingBlendShapeNames(Languages lang) => lang switch
        {
            Languages.Japanese => new MessageIndication(
                "未定義のBlendShapeClipがあります",
                @"パーフェクトシンクに必要なBlendShapeClipの一部が未定義です。

モデルの見た目が正常であれば、この警告を無視して構いません。
モデルの見た目がおかしい場合はこのダイアログを閉じてから
「パーフェクトシンクとは？」をクリックし、
モデルのセットアップ手順を確認して下さい。

---
定義されていないBlendShapeClipの名称: 
"),
            _ => new MessageIndication(
                "缺少 BlendShapeClip",
                @"检测缺失BlendShapeClips无法实现完美同步。

如果你的头像看起来正常，忽略这条信息。
否则，关闭此对话框并查看“关于完美同步”
检查如何设置模型。

---

未定义的blendshapeclip名称：
"),
        };

        public static MessageIndication ExTrackerCheckTurnOff(Languages lang) => lang switch
        {
            Languages.Japanese => new MessageIndication(
                "外部トラッキング機能をオフ",
                @"外部トラッキング機能をオフにしますか？
webカメラでの顔トラッキングを有効にする場合は
'OK'を選択し、iPhone/iPadとの連携を終了してください。"),
            _ => new MessageIndication(
                "关闭外部跟踪功能",
                @"关闭外部跟踪功能吗？

选择“OK”，启用web摄像机的面部跟踪"),
        };
    }
}
