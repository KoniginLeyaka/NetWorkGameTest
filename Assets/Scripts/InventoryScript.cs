using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryScript : MonoBehaviour
{
    public List<GameObject> inventory;
    [SerializeField] Image firstSlotIcon;
    [SerializeField] Image secondSlotIcon;
    [SerializeField] Image thirdSlotIcon;

    public int GetSize()
    {
        return inventory.Count;
    }
    public void AddItem(GameObject gameObject)
    {
        if (GetSize() >= 3) return;
        else
        {
            Color color = new();
            color.a = 1;
            switch (GetSize())
            {
                case 1:
                    firstSlotIcon.sprite = gameObject.GetComponent<ItemClass>().IconItem; ;
                    firstSlotIcon.color = color;
                    break;
                case 2:
                    secondSlotIcon.sprite = gameObject.GetComponent<ItemClass>().IconItem; ;
                    secondSlotIcon.color = color;
                    break;
                case 3:
                    thirdSlotIcon.sprite = gameObject.GetComponent<ItemClass>().IconItem; ;
                    thirdSlotIcon.color = color;
                    break;

            }
            inventory.Add(gameObject);
        }
    }
    public void RemoveItem(int index)
    {
        Color color = new();
        color.a = 0;

        if (inventory[index] == null) return;
        inventory.RemoveAt(index);
        switch (index)
        {
            case 0:
                firstSlotIcon.sprite = null;
                firstSlotIcon.color = color;
                break;
            case 1:
                secondSlotIcon.sprite = null;
                secondSlotIcon.color = color;
                break;
            case 2:
                thirdSlotIcon.sprite = null;
                thirdSlotIcon.color = color;
                break;
        }
    }
}
