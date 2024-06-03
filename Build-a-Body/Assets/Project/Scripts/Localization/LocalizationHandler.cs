using UnityEngine.Localization.Settings;

public static class LocalizationHandler
{
    public enum AvailableLocales
    {
        en,
        nl
    }

    private const string TABLE_COLLECTION_NAME = "LocaleTable";

    public static void PassLocalizationToFMOD()
    {
        FMODUnity.RuntimeManager.StudioSystem.setParameterByNameWithLabel("Language", LocalizationSettings.SelectedLocale.Identifier.Code);
    }

    public static void SetCurrentLocale(AvailableLocales setToLocale)
    {
        SetCurrentLocale(setToLocale.ToString());
    }

    public static void SetCurrentLocale(string setToLocale)
    {
        LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.GetLocale(setToLocale);
        PassLocalizationToFMOD();
    }

    public static string GetLocalizedString(string key)
    {
        return LocalizationSettings.StringDatabase.GetLocalizedString(TABLE_COLLECTION_NAME, key);
    }
}
