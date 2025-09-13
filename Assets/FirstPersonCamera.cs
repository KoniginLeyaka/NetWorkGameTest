using TMPro;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;

public class FirstPersonController : NetworkBehaviour
{
    [Header("��������� ������")]
    [SerializeField] private float mouseSensitivity = 100f;
    [SerializeField] private Transform playerBody;
    [SerializeField] private float minVerticalAngle = -90f;
    [SerializeField] private float maxVerticalAngle = 90f;

    [Header("��������� ��������")]
    [SerializeField] private float walkSpeed = 5f;
    [SerializeField] private float runSpeed = 10f;
    [SerializeField] private float gravity = -9.81f;
    [SerializeField] private float jumpHeight = 1f;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundDistance = 0.4f;
    [SerializeField] private LayerMask groundMask;
    [SerializeField] private Camera playerCamera;

    [Header("������ ��������")]
    [SerializeField] private TMP_Text PlayerNameDisplay;

    private CharacterController controller;
    private float xRotation = 0f;
    private Vector3 velocity;
    private bool isGrounded;
    private float currentSpeed;

    public NetworkVariable<FixedString64Bytes> networkPlayerName = new NetworkVariable<FixedString64Bytes>();

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();

        // �������� CharacterController � ����� ������ (playerBody ��������� �� ������ � ������������)
        controller = playerBody.GetComponent<CharacterController>();
        if (controller == null)
        {
            controller = playerBody.gameObject.AddComponent<CharacterController>();
        }

        if (IsOwner)
        {
            TeleportToSpawn(new Vector3(1.508f, 2.56f, -3.28f));
            SetupLocalPlayer();

            // �������� ������ ��� ���������� ������
            //TeleportToSpawn(new Vector3(1.508f, 2.56f, -3.28f));
            if (IsServer)
            {
                networkPlayerName.Value = PlayerData.PlayerName;
                PlayerNameDisplay.text = networkPlayerName.Value.ToString();
            }
            else
            {
                SubmitNameServerRpc(PlayerData.PlayerName);
            }
        }
        else
        {
            SetupRemotePlayer();
        }

        networkPlayerName.OnValueChanged += (oldValue, newValue) =>
        {
            PlayerNameDisplay.text = newValue.ToString();
        };

        PlayerNameDisplay.text = networkPlayerName.Value.ToString();
    }

    [ServerRpc]
    private void SubmitNameServerRpc(string name, ServerRpcParams rpcParams = default)
    {
        networkPlayerName.Value = name;
    }

    private void TeleportToSpawn(Vector3 position)
    {
        if (controller != null)
        {
            controller.enabled = false; // ���������, ����� �� ����� �����������
            playerBody.position = position; // ���������� �������� ������ ������
            controller.enabled = true;
        }
        else
        {
            playerBody.position = position;
        }
    }

    private void SetupLocalPlayer()
    {
        Debug.Log($"Setting up LOCAL player for client {OwnerClientId}");
        //PlayerNameDisplay.gameObject.SetActive(false);
        // ���������� ������ ��� ���������� ������
        if (playerCamera != null)
        {
            playerCamera.enabled = true;
            playerCamera.gameObject.SetActive(true);
        }

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        currentSpeed = walkSpeed;
    }

    private void SetupRemotePlayer()
    {
        Debug.Log($"Setting up REMOTE player for client {OwnerClientId}");
        //PlayerNameDisplay.text = PlayerData.PlayerName;

        // ��������� ������ ��� ��������� �������
        if (playerCamera != null)
        {
            playerCamera.enabled = false;
            playerCamera.gameObject.SetActive(false);
        }

        // ��������� �������� ���������� ��� ��������� �������
        // CharacterController ���������, ��� ��� �� ����� ��� ������
    }
    void Update()
    {
        if (!IsOwner) return;
        HandleMovement();
        HandleMouseLook();
        HandleJump();
        HandleRun();
    }

    private void HandleMouseLook()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, minVerticalAngle, maxVerticalAngle);

        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        playerBody.Rotate(Vector3.up * mouseX);
    }

    private void HandleMovement()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = playerBody.transform.right * x + playerBody.transform.forward * z;
        controller.Move(move * currentSpeed * Time.deltaTime);

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    private void HandleJump()
    {
        if (Input.GetKey(KeyCode.Space) && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }
    }

    private void HandleRun()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            currentSpeed = runSpeed;
        }
        else
        {
            currentSpeed = walkSpeed;
        }
    }
}