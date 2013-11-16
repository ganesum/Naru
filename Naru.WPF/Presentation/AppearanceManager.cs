using System;
using System.Linq;
using System.Windows;
using System.Windows.Media;

using Naru.WPF.ViewModel;

namespace Naru.WPF.Presentation
{
    /// <summary>
    /// Manages the theme, font size and accent colors for a Modern UI application.
    /// </summary>
    public class AppearanceManager : NotifyPropertyChanged
    {
        /// <summary>
        /// The location of the dark theme resource dictionary.
        /// </summary>
        public static readonly Uri DarkThemeSource = new Uri("/Naru.WPF;component/Assets/ModernUI.Dark.xaml", UriKind.Relative);
        
        /// <summary>
        /// The location of the light theme resource dictionary.
        /// </summary>
        public static readonly Uri LightThemeSource = new Uri("/Naru.WPF;component/Assets/ModernUI.Light.xaml", UriKind.Relative);

        /// <summary>
        /// The resource key for the accent color.
        /// </summary>
        public const string KeyAccentColor = "AccentColor";
        
        /// <summary>
        /// The resource key for the accent brush.
        /// </summary>
        public const string KeyAccent = "Accent";
        
        /// <summary>
        /// The resource key for the default font size.
        /// </summary>
        public const string KeyDefaultFontSize = "DefaultFontSize";
        
        /// <summary>
        /// The resource key for the fixed font size.
        /// </summary>
        public const string KeyFixedFontSize = "FixedFontSize";

        private static readonly AppearanceManager current = new AppearanceManager();

        /// <summary>
        /// Initializes a new instance of the <see cref="AppearanceManager"/> class.
        /// </summary>
        private AppearanceManager()
        {
        }

        private ResourceDictionary GetThemeDictionary()
        {
            // determine the current theme by looking at the app resources and return the first dictionary having the resource key 'WindowBackground' defined.
            return (from dict in Application.Current.Resources.MergedDictionaries
                    where dict.Contains("WindowBackground")
                    select dict).FirstOrDefault();
        }

        private Uri GetThemeSource()
        {
            var dict = GetThemeDictionary();
            if (dict != null)
            {
                return dict.Source;
            }

            // could not determine the theme dictionary
            return null;
        }

        private void SetThemeSource(Uri source, bool useThemeAccentColor)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }

            var oldThemeDict = GetThemeDictionary();
            var dictionaries = Application.Current.Resources.MergedDictionaries;
            var themeDict = new ResourceDictionary {Source = source};

            // if theme defines an accent color, use it
            var accentColor = themeDict[KeyAccentColor] as Color?;
            if (accentColor.HasValue)
            {
                // remove from the theme dictionary and apply globally if useThemeAccentColor is true
                themeDict.Remove(KeyAccentColor);

                if (useThemeAccentColor)
                {
                    ApplyAccentColor(accentColor.Value);
                }
            }

            // add new before removing old theme to avoid dynamicresource not found warnings
            dictionaries.Add(themeDict);

            // remove old theme
            if (oldThemeDict != null)
            {
                dictionaries.Remove(oldThemeDict);
            }

            RaisePropertyChanged(() => ThemeSource);
        }

        private void ApplyAccentColor(Color accentColor)
        {
            // set accent color and brush resources
            Application.Current.Resources[KeyAccentColor] = accentColor;
            Application.Current.Resources[KeyAccent] = new SolidColorBrush(accentColor);
        }

        private FontSize GetFontSize()
        {
            var defaultFontSize = Application.Current.Resources[KeyDefaultFontSize] as double?;

            if (defaultFontSize.HasValue)
            {
                return defaultFontSize.Value == 12D ? FontSize.Small : FontSize.Large;
            }

            // default large
            return FontSize.Large;
        }

        private void SetFontSize(FontSize fontSize)
        {
            if (GetFontSize() == fontSize)
            {
                return;
            }

            Application.Current.Resources[KeyDefaultFontSize] = fontSize == FontSize.Small ? 12D : 13D;
            Application.Current.Resources[KeyFixedFontSize] = fontSize == FontSize.Small ? 10.667D : 13.333D;

            RaisePropertyChanged(() => FontSize);
        }

        private Color GetAccentColor()
        {
            var accentColor = Application.Current.Resources[KeyAccentColor] as Color?;

            if (accentColor.HasValue)
            {
                return accentColor.Value;
            }

            // default color: teal
            return Color.FromArgb(0xff, 0x1b, 0xa1, 0xe2);
        }

        private void SetAccentColor(Color value)
        {
            ApplyAccentColor(value);

            // re-apply theme to ensure brushes referencing AccentColor are updated
            var themeSource = GetThemeSource();
            if (themeSource != null)
            {
                SetThemeSource(themeSource, false);
            }

            RaisePropertyChanged(() => AccentColor);
        }

        /// <summary>
        /// Gets the current <see cref="AppearanceManager"/> instance.
        /// </summary>
        public static AppearanceManager Current
        {
            get { return current; }
        }

        /// <summary>
        /// Gets or sets the current theme source.
        /// </summary>
        public Uri ThemeSource
        {
            get { return GetThemeSource(); }
            set { SetThemeSource(value, true); }
        }

        /// <summary>
        /// Gets or sets the font size.
        /// </summary>
        public FontSize FontSize
        {
            get { return GetFontSize(); }
            set { SetFontSize(value); }
        }

        /// <summary>
        /// Gets or sets the accent color.
        /// </summary>
        public Color AccentColor
        {
            get { return GetAccentColor(); }
            set { SetAccentColor(value); }
        }
    }
}