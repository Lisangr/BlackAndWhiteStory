using UnityEngine;

public class FaceCamera : MonoBehaviour
{
    public Transform faceTarget; // Ссылка на объект-метку на лице персонажа
    public float smoothSpeed = 5.0f; // Скорость плавного вращения камеры

    void Update()
    {
        // Проверяем, что метка на лице назначена
        if (faceTarget != null)
        {
            // Направляем камеру на метку на лице
            Vector3 direction = (faceTarget.position - transform.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(direction);

            // Плавное вращение камеры в направлении метки
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, smoothSpeed * Time.deltaTime);
        }
    }
}
