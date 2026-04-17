using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "InventoryData", menuName = "Scriptable Objects/InventoryData")]
public class InventoryData : ScriptableObject
{
    public List<_Resources> heldResources;

    public List<Item> heldItems;

    public string playerID;
}
