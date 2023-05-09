using Settings.Interfaces;
using UnityEngine;

namespace Settings
{
    public class SettingsDataHolder : ISettingsDataService
    {
        private string _language;

        public void SetLanguage(string language)
        {
            _language = language;
            PlayerPrefs.SetString("LanguageSettings", _language);
            I2.Loc.LocalizationManager.CurrentLanguageCode  = _language;
        }

        public void SetLanguage(ISettingsDataService.Language language)
        {
            _language = language.ToString();
            PlayerPrefs.SetString("LanguageSettings", _language);
            I2.Loc.LocalizationManager.CurrentLanguageCode  = _language;
        }

        public void LoadLanguage()
        {
            _language = PlayerPrefs.GetString("LanguageSettings", I2.Loc.LocalizationManager.CurrentLanguageCode );
            SetLanguage(_language);
        }

        public string GetLanguage()
        {
            return _language;
        }
    }
}