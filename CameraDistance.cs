using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraDistance : MonoBehaviour
{
    [SerializeField]
    private float maxViewDistance = 50.0f; // Максимальная дальность видимости

    private Camera cam;

    void Start()
    {
        // Получаем компонент камеры
        cam = GetComponent<Camera>();

        // Проверяем, есть ли компонент камеры, и если да - устанавливаем дальность видимости
        if (cam != null)
        {
            cam.farClipPlane = maxViewDistance;
        }
        else
        {
            Debug.LogWarning("Компонент Camera не найден!");
        }
    }

    // Метод для динамического изменения дальности видимости в реальном времени
    public void SetMaxViewDistance(float distance)
    {
        if (cam != null)
        {
            cam.farClipPlane = distance;
        }
    }
}
