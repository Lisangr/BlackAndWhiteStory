using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class QuestStep
{
    public enum StepType { ActivateObject, SitPlace }

    public StepType stepType; // Тип шага: активация объекта или SitPlace
    public GameObject targetObject; // Объект для активации (для типа ActivateObject)
    public Text textToActivate; // Текст, который должен активироваться (если это шаг с активацией объекта)
    public GameObject sitPlace; // Объект SitPlace (для типа SitPlace)
    public string actionDescription; // Описание действия
    public GameObject dialogueBackground; // Фон для диалога
}
