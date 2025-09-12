using UnityEngine;
using Unity.Netcode;
using TMPro;

public class SimpleGameManager : MonoBehaviour
{
    public TMP_Text connectionStatusText;

    void Start()
    {
        // Подписываемся на события сети
        NetworkManager.Singleton.OnClientConnectedCallback += OnConnected;
        NetworkManager.Singleton.OnClientDisconnectCallback += OnDisconnected;
    }

    void OnDestroy()
    {
        // Отписываемся от событий
        if (NetworkManager.Singleton != null)
        {
            NetworkManager.Singleton.OnClientConnectedCallback -= OnConnected;
            NetworkManager.Singleton.OnClientDisconnectCallback -= OnDisconnected;
        }
    }

    void Update()
    {
        // Обновляем статус подключения
        UpdateConnectionStatus();
    }

    void UpdateConnectionStatus()
    {
        if (NetworkManager.Singleton.IsClient)
        {
            connectionStatusText.text = "Подключен к серверу";
        }
        else if (NetworkManager.Singleton.IsHost)
        {
            connectionStatusText.text = "Вы хост";
        }
        else
        {
            connectionStatusText.text = "Не подключен";
        }
    }

    void OnConnected(ulong clientId)
    {
        Debug.Log($"Подключился клиент: {clientId}");
    }

    void OnDisconnected(ulong clientId)
    {
        Debug.Log($"Отключился клиент: {clientId}");
    }

    // Кнопка отключения
    public void Disconnect()
    {
        NetworkManager.Singleton.Shutdown();
        connectionStatusText.text = "Отключен";
    }
}