using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;

public class ItemsInteract : MonoBehaviour
{
    [SerializeField] private TMP_Text _titleItem;
    [SerializeField] private TMP_Text _DescriptionItem;
    [SerializeField] private Camera _camera;

    private GameObject playerObject;
    private Camera playerCamera;
    private GameObject grabbedItem;
    private bool playerIsGrab;

    void Start()
    {
        _titleItem.SetText("");
        _DescriptionItem.SetText("");
        playerObject = NetworkManager.Singleton.LocalClient.PlayerObject.gameObject;
        playerCamera = playerObject.GetComponentInChildren<Camera>();
        _camera = playerCamera;
    }

    void Update()
    {

        Ray ray = _camera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            if (hit.collider != null && hit.collider.gameObject.GetComponent<ItemClass>() != null)
            {
                _titleItem.SetText(hit.collider.gameObject.GetComponent<ItemClass>().getName());
                _DescriptionItem.SetText(hit.collider.gameObject.GetComponent<ItemClass>().getDescription());
                if (Input.GetMouseButtonDown(0))
                {
                    hit.collider.GetComponent<ItemClass>().InteractItem();
                }
                if (Input.GetKeyDown(KeyCode.E))
                {
                    ItemClass item = hit.collider.GetComponent<ItemClass>();

                    if (item.IsGrab() && !item.isGrabbed)
                    {
                        item.GrabItem(hit.collider.gameObject);
                        grabbedItem = hit.collider.gameObject;
                        playerIsGrab = true;
                    }
                    else if (playerIsGrab && grabbedItem.GetComponent<ItemClass>().isGrabbed)
                    {
                        grabbedItem.GetComponent<ItemClass>().ReleaseItem();
                        playerIsGrab = false;
                    }
                }
            }
            else
            {
                if (Input.GetKeyDown(KeyCode.E))
                {
                    if (playerIsGrab && grabbedItem.GetComponent<ItemClass>().isGrabbed)
                    {
                        grabbedItem.GetComponent<ItemClass>().ReleaseItem();
                        playerIsGrab = false;
                    }
                }
                _titleItem.SetText("");
                _DescriptionItem.SetText("");
            }
        }
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            GameObject.Find("GameManager").GetComponent<InventoryScript>().RemoveItem(0);
        }
    }

}
