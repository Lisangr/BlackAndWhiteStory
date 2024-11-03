using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
[System.Serializable]
public class Quest
{
    public string questName; // Название квеста
    public List<QuestStep> questSteps; // Список шагов квеста
}
public class TriggerObjectActivator : MonoBehaviour
{
    public List<Quest> allQuests; // Список всех квестов
    private int currentQuestIndex = 0; // Индекс текущего квеста
    private int currentStepIndex = 0; // Индекс текущего этапа в квесте
    public GameObject nextLevel;
    public Image fButton; // Кнопка F
    public Image gButton; // Кнопка G

    private bool isPlayerNearObject = false; // Флаг, что игрок рядом с объектом
    private bool textShown = false; // Флаг, что текст был показан
    private bool stepCompleted = false; // Флаг, что шаг завершён

    public Akane_CharacterController characterController; // Ссылка на скрипт управления персонажем
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
        followerAI.StartInactivityTimer(); // Запуск таймера активности для FollowerAI при начале этапа
    }

    void Update()
    {
        if (currentQuestIndex >= allQuests.Count)
            return; // Все квесты завершены

        Quest currentQuest = allQuests[currentQuestIndex];

        if (currentStepIndex >= currentQuest.questSteps.Count)
            return; // Текущий квест завершён

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
            Debug.Log("Квест завершён!");

            currentStepIndex = 0;
            currentQuestIndex++;

            if (currentQuestIndex >= allQuests.Count)
            {
                Debug.Log("Все квесты завершены!");
                fButton.gameObject.SetActive(false);
                gButton.gameObject.SetActive(false);

                nextLevel.gameObject.SetActive(true);
            }
        }

        // Передаем фолловеру следующий шаг квеста
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