using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class ScriptManagerUI : MonoBehaviour
{
    [SerializeField] private Button HostButton;
    [SerializeField] private Button ClientButton;

    private void Awake()
    {
        HostButton.onClick.AddListener(call: (() =>
        NetworkManager.Singleton.StartHost()));

        ClientButton.onClick.AddListener(call: (() =>
        NetworkManager.Singleton.StartClient()));
    }
}
