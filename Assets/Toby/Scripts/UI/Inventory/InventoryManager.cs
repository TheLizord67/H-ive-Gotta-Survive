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
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
