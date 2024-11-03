using UnityEngine;

public class Akane_CameraScript : MonoBehaviour
{
    public Transform Playerposition;
    public float smoothSpeed = 0.125f;  // Скорость сглаживания
    private Vector3 cameraoffset;

    void Start()
    {
        cameraoffset = transform.position - Playerposition.position; // Вычисляем начальный отступ
    }

    void LateUpdate()
    {
        Vector3 desiredPosition = Playerposition.position + cameraoffset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed); // Плавно перемещаем камеру
        transform.position = smoothedPosition;
    }
}
