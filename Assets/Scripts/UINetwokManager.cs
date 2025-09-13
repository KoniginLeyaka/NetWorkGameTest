using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using TMPro;
using Unity.Netcode.Components;
using System.Collections;

public class UINetwokManager : MonoBehaviour
{
    [Header("UI Elements")]
    public TMP_InputField ipInputField;
    public Button connectButton;
    public Button hostButton;
    public TMP_Text statusText;

    [Header("Scene Names")]
    public string menuScene = "MenuScene";
    public string gameScene = "GameScene";

    private bool isChangingScene = false;

    [Header("OnLoadParameters")]
    [SerializeField] private TMP_InputField PlayerNameInputfield;
    public string PlayerName = null;

    void Start()
    {
        // Автозаполнение IP
        ipInputField.text = "127.0.0.1";

        // Подписываем кнопки
        connectButton.onClick.AddListener(ConnectAsClient);
        hostButton.onClick.AddListener(ConnectAsHost);

        statusText.text = "Введите IP сервера";

        // Подписываемся на сетевые события
        NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnected;
        NetworkManager.Singleton.OnServerStarted += OnServerStarted;
    }

    void OnDestroy()
    {
        // Отписываемся от событий
        if (NetworkManager.Singleton != null)
        {
            NetworkManager.Singleton.OnClientConnectedCallback -= OnClientConnected;
            NetworkManager.Singleton.OnServerStarted -= OnServerStarted;
        }
    }

    // Подключение как клиент
    public void ConnectAsClient()
    {
        PlayerData.PlayerName = PlayerNameInputfield.text;
        PlayerName = PlayerNameInputfield.text;
        if (isChangingScene || NetworkManager.Singleton == null) return;

        string ip = ipInputField != null ? ipInputField.text : "127.0.0.1";
        var transport = NetworkManager.Singleton.GetComponent<UnityTransport>();

        if (transport != null)
        {
            transport.SetConnectionData(ip, 7777);

            if (NetworkManager.Singleton.StartClient())
            {
                statusText.text = $"Подключаемся к {ip}...";
                isChangingScene = true;
            }
        }
    }

    // Создание хоста
    public void ConnectAsHost()
    {
        PlayerData.PlayerName = PlayerNameInputfield.text;
        if (isChangingScene) return;

        if (NetworkManager.Singleton == null)
        {
            Debug.LogError("NetworkManager is null!");
            return;
        }

        string ip = "127.0.0.1";
        if (ipInputField != null) ip = ipInputField.text;

        var transport = NetworkManager.Singleton.GetComponent<UnityTransport>();
        if (transport == null) return;

        transport.SetConnectionData(ip, 7777, "0.0.0.0");

        if (NetworkManager.Singleton.StartHost())
        {
            statusText.text = $"Хост создан на {ip}";

            // 🔥 ПРАВИЛЬНЫЙ способ: используем NetworkSceneManager
            NetworkManager.Singleton.SceneManager.LoadScene(gameScene, LoadSceneMode.Single);
            isChangingScene = true;
        }
    }

    // Когда клиент подключился
    private void OnClientConnected(ulong clientId)
    {
        // Клиенты НЕ загружают сцену самостоятельно - сервер синхронизирует сцену
        if (NetworkManager.Singleton.IsServer)
        {
            statusText.text = $"Игрок {clientId} подключился";
        }
    }


    // Когда сервер запущен
    private void OnServerStarted()
    {
        statusText.text = "Сервер запущен. Ожидаем игроков...";
    }

    // Кнопка возврата в меню
    public void ReturnToMenu()
    {
        if (NetworkManager.Singleton.IsServer)
        {
            // Сервер возвращает всех в меню
            NetworkManager.Singleton.SceneManager.LoadScene(menuScene, LoadSceneMode.Single);
        }
        else
        {
            // Клиент просто отключается и возвращается
            NetworkManager.Singleton.Shutdown();
            SceneManager.LoadScene(menuScene);
        }
    }
}