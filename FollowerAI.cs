using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class FollowerAI : MonoBehaviour
{
    public Transform player;
    public float followSpeed = 2.0f;
    public float followDistance = 1.0f;
    public float followTolerance = 0.3f;
    public float eatDuration = 3.0f;

    private Animator animator;
    private Vector3 previousPosition;
    private float currentSpeed = 0f;
    private bool isRunning = false;
    private bool isEating = false;
    private float timeToEat;
    private float eatTimer = 0;
    // Новые переменные для Canvas и таймера
    public Canvas actionCanvas; // Канвас с кнопкой
    public Button actionButton; // Кнопка для перемещения фолловера
    private QuestStep nextQuestStep; // Следующий шаг квеста, на который нужно переместиться
    private float inactivityTimer = 0; // Таймер неактивности
    private bool isMovingToQuestStep = false;
    void Start()
    {
        animator = GetComponent<Animator>();
        previousPosition = transform.position;
        SetRandomEatTime();

        actionCanvas.gameObject.SetActive(false); // Отключаем Canvas по умолчанию
        actionButton.onClick.AddListener(MoveToNextQuestStep);
    }

    void Update()
    {
        if (isEating)
        {
            HandleEatState();
        }
        else if (isMovingToQuestStep)
        {
            return;
        }
        else
        {
            FollowPlayer();
            CalculateSpeed();
            HandleEatTimer();

            if (inactivityTimer >= 60.0f)
            {
                ActivateActionCanvas();
            }
            else
            {
                inactivityTimer += Time.deltaTime;
            }
        }
    }
    void ActivateActionCanvas()
    {
        actionCanvas.gameObject.SetActive(true);
        actionCanvas.transform.rotation = Quaternion.Euler(0, 0, 0); // Устанавливаем угол поворота в (0, 0, 0)
    }
    public void StartInactivityTimer()
    {
        inactivityTimer = 0f; // Сброс таймера
        actionCanvas.gameObject.SetActive(false); // Отключаем Canvas при активации нового этапа
    }
    void MoveToNextQuestStep()
    {
        if (nextQuestStep != null)
        {
            Vector3 targetPosition = nextQuestStep.targetObject != null
                ? nextQuestStep.targetObject.transform.position
                : nextQuestStep.sitPlace.transform.position;

            isMovingToQuestStep = true; // Активируем режим перемещения к точке квеста
            StartCoroutine(MoveToPosition(targetPosition));
            actionCanvas.gameObject.SetActive(false); // Отключаем Canvas после активации
            inactivityTimer = 0; // Сбрасываем таймер

            nextQuestStep = null; // Сбрасываем для возврата к стандартной логике
        }
    }

    IEnumerator MoveToPosition(Vector3 targetPosition)
    {
        isRunning = true;
        ResetAllTriggers();
        animator.SetTrigger("Run");

        while (Vector3.Distance(transform.position, targetPosition) > 0.1f)
        {
            // Поворачиваем фолловера к точке назначения
            Vector3 directionToTarget = (targetPosition - transform.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(directionToTarget);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * followSpeed);

            // Перемещаем фолловера к точке назначения
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, followSpeed * Time.deltaTime);
            yield return null;
        }

        isRunning = false;
        ResetAllTriggers();
        animator.SetTrigger("Idle");

        isMovingToQuestStep = false; // Сбрасываем флаг после завершения движения к точке
    }


    void FollowPlayer()
    {
        if (isEating) return;

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        LookAtPlayer();

        if (distanceToPlayer > followDistance + followTolerance)
        {
            Vector3 direction = (player.position - transform.position).normalized;
            transform.position += direction * followSpeed * Time.deltaTime;

            if (currentSpeed > 0.2f && !isRunning)
            {
                ResetAllTriggers();
                animator.SetTrigger("Run");
                isRunning = true;
            }
        }
        else if (distanceToPlayer <= followDistance || currentSpeed <= 0.2f)
        {
            if (isRunning)
            {
                ResetAllTriggers();
                animator.SetTrigger("Idle");
                isRunning = false;
            }
        }
    }

    void CalculateSpeed()
    {
        currentSpeed = (transform.position - previousPosition).magnitude / Time.deltaTime;
        previousPosition = transform.position;
    }

    void HandleEatTimer()
    {
        timeToEat -= Time.deltaTime;

        if (timeToEat <= 0)
        {
            ResetAllTriggers();
            animator.SetTrigger("Eat");
            isEating = true;
            eatTimer = Random.Range(3.0f, 5.0f);
            SetRandomEatTime();
        }
    }

    void HandleEatState()
    {
        eatTimer -= Time.deltaTime;

        if (eatTimer <= 0)
        {
            isEating = false;
            ResetAllTriggers();
            float distanceToPlayer = Vector3.Distance(transform.position, player.position);

            if (distanceToPlayer > followDistance + followTolerance)
            {
                animator.SetTrigger("Run");
                isRunning = true;
            }
            else
            {
                animator.SetTrigger("Idle");
                isRunning = false;
            }
        }
    }

    void LookAtPlayer()
    {
        Vector3 lookDirection = player.position - transform.position;
        lookDirection.y = 0;
        transform.rotation = Quaternion.LookRotation(lookDirection);
    }

    void ResetAllTriggers()
    {
        animator.ResetTrigger("Run");
        animator.ResetTrigger("Idle");
        animator.ResetTrigger("Eat");
    }

    void SetRandomEatTime()
    {
        timeToEat = Random.Range(20.0f, 40.0f);
    }

    public void SetNextQuestStep(QuestStep step)
    {
        nextQuestStep = step;
    }
}