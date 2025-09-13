using UnityEngine;
using Unity.Netcode;

public class SwitcherLight : ItemClass
{
#pragma warning disable CS0108 // ���� �������� �������������� ����: ����������� ����� �������� �����
    [SerializeField] private Light light;
#pragma warning restore CS0108 // ���� �������� �������������� ����: ����������� ����� �������� �����
    [SerializeField] private GameObject switcher;

    public override void InteractItem()
    {
        // ������ NetworkObject � �������� � ���� ��������� �����
        var netObj = GetComponent<Unity.Netcode.NetworkObject>();
        if (netObj != null && netObj.IsSpawned)
        {
            ToggleLightServerRpc();
        }
    }

    [ServerRpc(RequireOwnership = false)]
    private void ToggleLightServerRpc()
    {
        light.enabled = !light.enabled;
        switcher.transform.Rotate(0, 180, 0);

        ToggleLightClientRpc(light.enabled);
    }

    [ClientRpc]
    private void ToggleLightClientRpc(bool newState)
    {
        light.enabled = newState;
        switcher.transform.Rotate(0, 180, 0);
    }
}
