using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraDistance : MonoBehaviour
{
    [SerializeField]
    private float maxViewDistance = 50.0f; // ������������ ��������� ���������

    private Camera cam;

    void Start()
    {
        // �������� ��������� ������
        cam = GetComponent<Camera>();

        // ���������, ���� �� ��������� ������, � ���� �� - ������������� ��������� ���������
        if (cam != null)
        {
            cam.farClipPlane = maxViewDistance;
        }
        else
        {
            Debug.LogWarning("��������� Camera �� ������!");
        }
    }

    // ����� ��� ������������� ��������� ��������� ��������� � �������� �������
    public void SetMaxViewDistance(float distance)
    {
        if (cam != null)
        {
            cam.farClipPlane = distance;
        }
    }
}
