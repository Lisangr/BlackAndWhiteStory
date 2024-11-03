using UnityEngine;

public class Akane_CharacterController : MonoBehaviour
{
    private CharacterController controller;
    private Vector3 playerVelocity;
    private bool groundedPlayer;
    private float playerSpeed = 1.4f;
    private float gravityValue = -9.81f;
    private Animator animator;
    private Transform sitPlace;
    private bool isSit = false;
    private bool sitPlaceCompleted = false;

    private Transform platform; // Для хранения текущей платформы
    private Vector3 lastPlatformPosition; // Для хранения предыдущей позиции платформы

    void Start()
    {
        animator = GetComponent<Animator>();
        controller = gameObject.GetComponent<CharacterController>();
    }

    void Update()
    {
        groundedPlayer = controller.isGrounded;
        if (groundedPlayer && playerVelocity.y < 0)
        {
            playerVelocity.y = 0f;
        }

        // Переключение состояния сидения по нажатию клавиши G (если персонаж в триггере объекта SitPlace)
        if (Input.GetKeyDown(KeyCode.G))
        {
            isSit = !isSit;

            if (isSit)
            {
                animator.SetBool("isSit", true);
                //transform.position = sitPlace.position; // Перемещаем персонажа на место сидения

                // Останавливаем движение, но оставляем контроллер активным
                playerVelocity = Vector3.zero;
            }
            else
            {
                animator.SetBool("isSit", false);
                sitPlaceCompleted = true; // Завершаем взаимодействие с SitPlace
            }
        }

        // Если персонаж не сидит, то он может передвигаться
        if (!IsSitting())
        {
            Vector3 move = new Vector3(Input.GetAxis("Horizontal"), 0.0f, Input.GetAxis("Vertical"));
            controller.Move(move * Time.deltaTime * playerSpeed);

            if (move != Vector3.zero)
            {
                gameObject.transform.forward = move;
                animator.SetBool("isWalk", true);
            }
            else
            {
                animator.SetBool("isWalk", false);
            }
        }

        playerVelocity.y += gravityValue * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);
    }

    private bool IsSitting()
    {
        return animator.GetBool("isSit");
    }

    // Используем OnControllerColliderHit для обнаружения платформы
    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        // Если персонаж сталкивается с платформой, проверяем тег "Around"
        if (hit.collider.CompareTag("Around"))
        {
            if (platform == null)
            {
                platform = hit.collider.transform; // Сохраняем ссылку на платформу
                transform.SetParent(platform); // Устанавливаем платформу как родителя персонажа
                lastPlatformPosition = platform.position; // Запоминаем текущую позицию платформы
            }
        }
        else if (platform != null) // Если столкновение с платформой завершилось
        {
            // Если больше нет столкновения с платформой, сбрасываем родителя
            transform.SetParent(null);
            platform = null;
        }
    }

    // При выходе из триггера мы снимаем родительскую связь
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Around") && platform != null)
        {
            transform.SetParent(null); // Снимаем родительскую связь
            platform = null; // Очищаем ссылку на платформу
        }
    }

    // Метод для перемещения персонажа вместе с платформой
    private void FollowPlatform()
    {
        // Вычисляем смещение платформы за последний кадр
        Vector3 platformMovement = platform.position - lastPlatformPosition;

        // Применяем это смещение к позиции персонажа
        controller.Move(platformMovement);

        // Обновляем последнюю позицию платформы
        lastPlatformPosition = platform.position;
    }
}
