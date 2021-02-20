using System;
using System.Globalization;
using System.Windows;

namespace Baku.VMagicMirrorConfig
{
    class LanguageSelector
    {
        private static LanguageSelector? _instance;
        public static LanguageSelector Instance
            => _instance ??= new LanguageSelector();
        private LanguageSelector() { }

        private IMessageSender? _sender = null;

        public event Action<Languages>? LanguageChanged;

        private string _languageName = nameof(Languages.Chinese);
        public string LanguageName
        {
            get => _languageName;
            set
            {
                if (_languageName != value && 
                    (value == nameof(Languages.Chinese) || value == nameof(Languages.English))
                    )
                {
                    _languageName = value;
                    SetLanguage(LanguageName);
                    LanguageChanged?.Invoke(
                        value == nameof(Languages.Chinese) ? Languages.Chinese : Languages.English
                        );
                }
            }
        }

        public void Initialize(IMessageSender sender, string preferredLanguageName)
        {
            _sender = sender;

            if (preferredLanguageName == "Chinese" || 
                preferredLanguageName == "English")
            {
                LanguageName = preferredLanguageName;
            }
            else
            {
                LanguageName =
                    (CultureInfo.CurrentCulture.Name == "ja-JP") ?
                    "Chinese" :
                    "English";
            }
        }

        public static Languages StringToLanguage(string languageName) => languageName switch
        {
            "Chinese" => Languages.Chinese,
            _ => Languages.English,
        };

        private void SetLanguage(string languageName)
        {
            Application.Current.Resources.MergedDictionaries[0] = new ResourceDictionary()
            {
                Source = new Uri(
                  $"/VMagicMirrorConfig;component/Resources/{languageName}.xaml",
                  UriKind.Relative
                  ),
            };
            _sender?.SendMessage(MessageFactory.Instance.Language(languageName));
        }
    }

    enum Languages
    {
        Chinese,
        English
    }
}
