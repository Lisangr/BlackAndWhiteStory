using UnityEngine;
using UnityEngine.UI;
using YG;

public class MyGames : MonoBehaviour
{
    public Button button;

    void Start()
    {
        if (button != null)
        {
            button.onClick.AddListener(OpenURL);
        }
    }

    // Метод для открытия URL
    void OpenURL()
    {
        Application.OpenURL("https://yandex." + YandexGame.EnvironmentData.domain + "/games/developer/87388");        
    }
}
