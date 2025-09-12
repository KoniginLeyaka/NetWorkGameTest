using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestScript : MonoBehaviour
{
    [SerializeField] private float targetValue = 1.0f; // ������� ��������
    [SerializeField] private float currentValue = 0.0f; // ������� ��������
    [SerializeField] private float changeSpeed = 0.2f; // �������� ���������
    // Start is called before the first frame update

    // Update is called once per frame
    void Update()
    {
        // ���������, ������������ �� ������� ������
        if (Input.GetKey(KeyCode.Space))
        {
            // ����������� ������� ��������
            targetValue = 1.0f; // ������������� ������� �������� ��� ����������� �������
        }
        else
        {
            // ���������� ������� ��������
            targetValue = 0.0f; // ������������� ������� �������� ��� ���������� �������
        }

        // ������ �������� ������� �������� � ��������
        currentValue = Mathf.MoveTowards(currentValue, targetValue, Time.deltaTime * changeSpeed);
    }
}
