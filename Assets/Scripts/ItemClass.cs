using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using Unity.Netcode;
using Unity.Netcode.Components;

public class ItemClass : NetworkBehaviour
{
    [SerializeField] private string nameItem = "Unknown item";
    [SerializeField] private string descriptionItem = "Unknown description";
    public bool isGrabbed = false;
    [SerializeField] private NetworkObject networkObject;

    private void Start()
    {
        networkObject = GetComponent<NetworkObject>();
    }

    public string getName()
    {
        return nameItem;
    }
    public string getDescription()
    {
        return descriptionItem;
    }
    public virtual bool IsGrab()
    {
        return false;
    }
    public virtual void InteractItem()
    {
        return;
    }
    public virtual void GrabItem(GameObject gameObject)
    {
        GameObject player = NetworkManager.Singleton.LocalClient.PlayerObject.gameObject;
        RequestOwnershipServerRpc(NetworkManager.Singleton.LocalClientId);

        if (!IsServer)
        {
            GetComponent<NetworkTransform>().enabled = false;
        }
        KinematicOnServerRpc(true);
        isGrabbed = true;
    }

    public virtual void ReleaseItem()
    {

        if (!IsOwner) return;

        if (!isGrabbed) return;
        ReleaseItemOnServerRpc();
        KinematicOnServerRpc(false);
        isGrabbed = false;
    }

    [ServerRpc(RequireOwnership = false)]
    private void RequestOwnershipServerRpc(ulong clientId)
    {
        NetworkObject playerNetObject = NetworkManager.Singleton.SpawnManager.GetPlayerNetworkObject(clientId);
        networkObject.ChangeOwnership(clientId);
        networkObject.TrySetParent(playerNetObject.transform);
    }

    [ServerRpc(RequireOwnership = false)]
    private void KinematicOnServerRpc(bool kin)
    {
        if (kin)
        {
            GetComponent<Rigidbody>().useGravity = false;
            GetComponent<Rigidbody>().detectCollisions = true;
            GetComponent<Rigidbody>().isKinematic = false;
            GetComponent<Rigidbody>().linearVelocity = Vector3.zero;
            GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
            GetComponent<Rigidbody>().freezeRotation = true;
        } else
        {
            GetComponent<Rigidbody>().freezeRotation = false;
            GetComponent<Rigidbody>().useGravity = true;
        }
        KinematicOnClientRpc(kin);
    }

    [ClientRpc]
    private void KinematicOnClientRpc(bool kin)
    {
        if (kin)
        {
            GetComponent<Rigidbody>().useGravity = false;
            GetComponent<Rigidbody>().detectCollisions = true;
            GetComponent<Rigidbody>().isKinematic = false;
            GetComponent<Rigidbody>().linearVelocity = Vector3.zero;
            GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
            GetComponent<Rigidbody>().freezeRotation = true;
        }
        else
        {
            GetComponent<Rigidbody>().freezeRotation = false;
            GetComponent<Rigidbody>().useGravity = true;
            GetComponent<NetworkTransform>().enabled = true;
        }
    }

    [ServerRpc(RequireOwnership = false)]
    private void ReleaseItemOnServerRpc()
    {
        networkObject.ChangeOwnership(0);
        networkObject.TryRemoveParent();
    }
}
