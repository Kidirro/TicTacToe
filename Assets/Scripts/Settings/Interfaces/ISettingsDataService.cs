namespace Settings.Interfaces
{
    public interface ISettingsDataService
    {
        public void SetLanguage(string language);
        public void SetLanguage(ISettingsDataService.Language language);
        public void LoadLanguage();
        public string GetLanguage();
        
        public enum Language
        {
            ru,
            en
        }
    }
}