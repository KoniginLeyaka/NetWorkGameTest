using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestScript : MonoBehaviour
{
    [SerializeField] private float targetValue = 1.0f; // Целевое значение
    [SerializeField] private float currentValue = 0.0f; // Текущее значение
    [SerializeField] private float changeSpeed = 0.2f; // Скорость изменения
    // Start is called before the first frame update

    // Update is called once per frame
    void Update()
    {
        // Проверяем, удерживается ли клавиша пробел
        if (Input.GetKey(KeyCode.Space))
        {
            // Увеличиваем целевое значение
            targetValue = 1.0f; // Устанавливаем целевое значение при удерживании пробела
        }
        else
        {
            // Сбрасываем целевое значение
            targetValue = 0.0f; // Устанавливаем целевое значение при отпускании пробела
        }

        // Плавно изменяем текущее значение к целевому
        currentValue = Mathf.MoveTowards(currentValue, targetValue, Time.deltaTime * changeSpeed);
    }
}
