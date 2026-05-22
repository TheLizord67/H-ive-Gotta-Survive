using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public string playerName;

    public InventoryData inventoryData;

    public List<InventoryData> allStoredInventoryData;

    public List<GameObject> inventorySlots;

    public GameObject genericResource;
    public void LoadAllFromResources()
    {
        InventoryData[] loadedAssets = Resources.LoadAll<InventoryData>("Inventory");
        allStoredInventoryData = new List<InventoryData>(loadedAssets);
    }

    void Start()
    {
        LoadAllFromResources();
        bool haveData = false;
        //player name grabbed by steam name
        foreach (var data in allStoredInventoryData)
        {
            if (data.playerID == playerName)
            {
                haveData = true;
                inventoryData = data;
                SetInventory();
                return;
            }
            if (data.playerID != playerName)
            {
                haveData = false;
            }
        }
        if (haveData == false)
        {
            inventoryData = ScriptableObject.CreateInstance<InventoryData>();
            allStoredInventoryData.Add(inventoryData);
            inventoryData.playerID = playerName;
            SetInventory();
        }
    }

    public void SetInventory()
    {
        foreach (var resource in inventoryData.heldResources)
        {
            foreach (var slot in inventorySlots)
            {
                if (slot.transform.childCount > 0)
                {
                    if (slot.transform.GetChild(0).GetComponent<ItemHandler>().resourceData == resource)
                    {
                        if (slot.transform.GetChild(0).GetComponent<ItemHandler>().stack == resource.resource.maxStackSize)
                        {
                            Debug.Log("Move On");
                        }
                        else
                        {
                            slot.transform.GetChild(0).GetComponent<ItemHandler>().stack += 1;
                            Instantiate(genericResource, slot.transform.GetChild(0).transform);
                            break;
                        }
                    }
                }
                if (slot.transform.childCount == 0)
                {
                    GameObject item = Instantiate(genericResource, slot.transform);
                    item.GetComponent<ItemHandler>().resourceData = resource;
                    break;
                }
            }
        }
        foreach (var item in inventoryData.heldItems)
        {
            foreach (var slot in inventorySlots)
            {
                if (slot.transform.childCount > 0)
                {
                    if (slot.transform.GetChild(0).GetComponent<ItemHandler>().itemData == item)
                    {
                        if (slot.transform.GetChild(0).GetComponent<ItemHandler>().stack == item.item.maxStackSize)
                        {
                            break;
                        }
                        else
                        {
                            slot.transform.GetChild(0).GetComponent<ItemHandler>().stack += 1;
                            Instantiate(genericResource, slot.transform.GetChild(0).transform);
                            break;
                        }
                    }
                    else
                    {

                    }
                }
                if (slot.transform.childCount == 0)
                {
                    GameObject itemObject = Instantiate(genericResource, slot.transform);
                    itemObject.GetComponent<ItemHandler>().itemData = item;
                    itemObject.GetComponent<ItemHandler>().isItem = true;
                    break;
                }
            }
        }
        inventoryData.heldResources.Clear();
        inventoryData.heldItems.Clear();
    }
    public void ResetInventory()
    {
        foreach (var slot in inventorySlots)
        {
            if (slot.transform.childCount > 0)
            {
                Destroy(slot.transform.GetChild(0).gameObject);
            }
        }
    }

    void Update()
    {
        inventoryData.heldResources.Clear();
        inventoryData.heldItems.Clear();
        foreach (var slot in inventorySlots)
        {
            if (slot.transform.childCount > 0)
            {
                GameObject child = slot.transform.GetChild(0).gameObject;
                if (child.GetComponent<ItemHandler>().isItem == false)
                {
                    inventoryData.heldResources.Add(child.GetComponent<ItemHandler>().resourceData);
                    if (child.GetComponent<ItemHandler>().stack > 0)
                    {
                        for (int i = 1; i < child.GetComponent<ItemHandler>().stack; i++)
                        {
                            inventoryData.heldResources.Add(child.GetComponent<ItemHandler>().resourceData);
                        }
                    }
                }
                else
                {
                    inventoryData.heldItems.Add(child.GetComponent<ItemHandler>().itemData);
                    if (child.GetComponent<ItemHandler>().stack > 0)
                    {
                        for (int i = 1; i < child.GetComponent<ItemHandler>().stack; i++)
                        {
                            inventoryData.heldItems.Add(child.GetComponent<ItemHandler>().itemData);
                        }
                    }
                }
            }
        }
    }
}
