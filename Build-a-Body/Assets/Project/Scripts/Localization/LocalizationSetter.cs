using UnityEngine;

public class LocalizationSetter : MonoBehaviour
{
    public void SetLocalization(string localeId)
    {
        LocalizationHandler.SetCurrentLocale(localeId);
    }
}
