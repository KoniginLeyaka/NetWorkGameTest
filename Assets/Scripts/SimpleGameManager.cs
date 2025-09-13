using UnityEngine;
using Unity.Netcode;
using TMPro;

public class SimpleGameManager : MonoBehaviour
{
    public TMP_Text connectionStatusText;

    void Start()
    {
        // ������������� �� ������� ����
        NetworkManager.Singleton.OnClientConnectedCallback += OnConnected;
        NetworkManager.Singleton.OnClientDisconnectCallback += OnDisconnected;
    }

    void OnDestroy()
    {
        // ������������ �� �������
        if (NetworkManager.Singleton != null)
        {
            NetworkManager.Singleton.OnClientConnectedCallback -= OnConnected;
            NetworkManager.Singleton.OnClientDisconnectCallback -= OnDisconnected;
        }
    }

    void Update()
    {
        // ��������� ������ �����������
        UpdateConnectionStatus();
    }

    void UpdateConnectionStatus()
    {
        if (NetworkManager.Singleton.IsClient)
        {
            connectionStatusText.text = "��������� � �������";
        }
        else if (NetworkManager.Singleton.IsHost)
        {
            connectionStatusText.text = "�� ����";
        }
        else
        {
            connectionStatusText.text = "�� ���������";
        }
    }

    void OnConnected(ulong clientId)
    {
        Debug.Log($"����������� ������: {clientId}");
    }

    void OnDisconnected(ulong clientId)
    {
        Debug.Log($"���������� ������: {clientId}");
    }

    // ������ ����������
    public void Disconnect()
    {
        NetworkManager.Singleton.Shutdown();
        connectionStatusText.text = "��������";
    }
}