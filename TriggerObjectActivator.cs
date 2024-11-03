using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
[System.Serializable]
public class Quest
{
    public string questName; // �������� ������
    public List<QuestStep> questSteps; // ������ ����� ������
}
public class TriggerObjectActivator : MonoBehaviour
{
    public List<Quest> allQuests; // ������ ���� �������
    private int currentQuestIndex = 0; // ������ �������� ������
    private int currentStepIndex = 0; // ������ �������� ����� � ������
    public GameObject nextLevel;
    public Image fButton; // ������ F
    public Image gButton; // ������ G

    private bool isPlayerNearObject = false; // ����, ��� ����� ����� � ��������
    private bool textShown = false; // ����, ��� ����� ��� �������
    private bool stepCompleted = false; // ����, ��� ��� ��������

    public Akane_CharacterController characterController; // ������ �� ������ ���������� ����������
    private FollowerAI followerAI;

    void Start()
    {
        characterController = GetComponent<Akane_CharacterController>();
        followerAI = FindObjectOfType<FollowerAI>();

        foreach (Quest quest in allQuests)
        {
            foreach (QuestStep step in quest.questSteps)
            {
                if (step.textToActivate != null)
                {
                    step.textToActivate.gameObject.SetActive(false);
                }
                if (step.dialogueBackground != null)
                {
                    step.dialogueBackground.SetActive(false);
                }
            }
        }
        nextLevel.gameObject.SetActive(false);
        fButton.gameObject.SetActive(false);
        gButton.gameObject.SetActive(false);
    }

    private void ActivateCurrentStepUI(QuestStep currentStep)
    {
        if (currentStep.textToActivate != null)
        {
            currentStep.textToActivate.gameObject.SetActive(true);
        }

        if (currentStep.dialogueBackground != null)
        {
            currentStep.dialogueBackground.SetActive(true);
        }

        fButton.gameObject.SetActive(false);
        followerAI.StartInactivityTimer(); // ������ ������� ���������� ��� FollowerAI ��� ������ �����
    }

    void Update()
    {
        if (currentQuestIndex >= allQuests.Count)
            return; // ��� ������ ���������

        Quest currentQuest = allQuests[currentQuestIndex];

        if (currentStepIndex >= currentQuest.questSteps.Count)
            return; // ������� ����� ��������

        QuestStep currentStep = currentQuest.questSteps[currentStepIndex];

        isPlayerNearObject = IsPlayerNearObject(currentStep);

        if (isPlayerNearObject && !stepCompleted)
        {
            if (currentStep.stepType == QuestStep.StepType.ActivateObject)
            {
                fButton.gameObject.SetActive(true);

                if (Input.GetKeyDown(KeyCode.F) && !textShown)
                {
                    ActivateCurrentStepUI(currentStep);
                    textShown = true;
                    Animator animator = GetComponent<Animator>();
                    animator.SetBool("isWalk", false);
                    characterController.enabled = false;
                }
                else if (Input.GetKeyDown(KeyCode.F) && textShown)
                {
                    DeactivateCurrentStepUI(currentStep);
                    stepCompleted = true;
                    characterController.enabled = true;
                    AdvanceToNextStep();
                }
            }
            else if (currentStep.stepType == QuestStep.StepType.SitPlace)
            {
                gButton.gameObject.SetActive(true);

                if (Input.GetKeyDown(KeyCode.G) && !textShown)
                {
                    ActivateCurrentStepUI(currentStep);
                    textShown = true;
                }
                else if (Input.GetKeyDown(KeyCode.G) && textShown)
                {
                    DeactivateCurrentStepUI(currentStep);
                    stepCompleted = true;
                    AdvanceToNextStep();
                }
            }
        }

        if ((!isPlayerNearObject && !stepCompleted) || stepCompleted)
        {
            fButton.gameObject.SetActive(false);
            gButton.gameObject.SetActive(false);
        }
    }

    private bool IsPlayerNearObject(QuestStep step)
    {
        if (step.stepType == QuestStep.StepType.ActivateObject && step.targetObject != null)
        {
            return Vector3.Distance(transform.position, step.targetObject.transform.position) < 2.0f;
        }
        else if (step.stepType == QuestStep.StepType.SitPlace && step.sitPlace != null)
        {
            return Vector3.Distance(transform.position, step.sitPlace.transform.position) < 2.0f;
        }
        return false;
    }
   
    private void AdvanceToNextStep()
    {
        textShown = false;
        stepCompleted = false;
        currentStepIndex++;

        Quest currentQuest = allQuests[currentQuestIndex];
        if (currentStepIndex >= currentQuest.questSteps.Count)
        {
            Debug.Log("����� ��������!");

            currentStepIndex = 0;
            currentQuestIndex++;

            if (currentQuestIndex >= allQuests.Count)
            {
                Debug.Log("��� ������ ���������!");
                fButton.gameObject.SetActive(false);
                gButton.gameObject.SetActive(false);

                nextLevel.gameObject.SetActive(true);
            }
        }

        // �������� ��������� ��������� ��� ������
        if (currentQuestIndex < allQuests.Count)
        {
            FollowerAI followerAI = FindObjectOfType<FollowerAI>();
            followerAI.SetNextQuestStep(allQuests[currentQuestIndex].questSteps[currentStepIndex]);
        }
    }
    private void DeactivateCurrentStepUI(QuestStep currentStep)
    {
        if (currentStep.textToActivate != null)
        {
            currentStep.textToActivate.gameObject.SetActive(false);
        }

        if (currentStep.dialogueBackground != null)
        {
            currentStep.dialogueBackground.SetActive(false);
        }
    }
}