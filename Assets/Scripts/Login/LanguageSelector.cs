using UnityEngine;
using UnityEngine.UI;

public class LanguageSelector : MonoBehaviour
{
    public Toggle toggleEnglish;
    public Toggle toggleHindi;

    private void Start()
    {
        // Set initial state from PlayerPrefs
        int isEnglish = PlayerPrefs.GetInt("isEnglish", 1); // Default to English
        ToggleLanguage(isEnglish == 1);

        // Add listeners
        toggleEnglish.onValueChanged.AddListener((state) =>
        {
            if (state) ToggleLanguage(true);
        });

        toggleHindi.onValueChanged.AddListener((state) =>
        {
            if (state) ToggleLanguage(false);
        });
    }

    public void ToggleLanguage(bool isEnglish)
    {
        toggleEnglish.isOn = isEnglish;
        toggleHindi.isOn = !isEnglish;

        PlayerPrefs.SetInt("isEnglish", isEnglish ? 0 : 1);

        Debug.Log("Language set to: " + (isEnglish ? "English" : "Hindi"));
    }
}
