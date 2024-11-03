using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class QuestStep
{
    public enum StepType { ActivateObject, SitPlace }

    public StepType stepType; // ��� ����: ��������� ������� ��� SitPlace
    public GameObject targetObject; // ������ ��� ��������� (��� ���� ActivateObject)
    public Text textToActivate; // �����, ������� ������ �������������� (���� ��� ��� � ���������� �������)
    public GameObject sitPlace; // ������ SitPlace (��� ���� SitPlace)
    public string actionDescription; // �������� ��������
    public GameObject dialogueBackground; // ��� ��� �������
}
