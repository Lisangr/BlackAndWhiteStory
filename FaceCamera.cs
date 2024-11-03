using UnityEngine;

public class FaceCamera : MonoBehaviour
{
    public Transform faceTarget; // ������ �� ������-����� �� ���� ���������
    public float smoothSpeed = 5.0f; // �������� �������� �������� ������

    void Update()
    {
        // ���������, ��� ����� �� ���� ���������
        if (faceTarget != null)
        {
            // ���������� ������ �� ����� �� ����
            Vector3 direction = (faceTarget.position - transform.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(direction);

            // ������� �������� ������ � ����������� �����
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, smoothSpeed * Time.deltaTime);
        }
    }
}
