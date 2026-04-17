using UnityEngine;

public partial class Item
{
    [System.Serializable]
    public struct _Item
    {
        public string name;

        public Sprite icon;

        public string description;

        public int maxStackSize;

        public GameObject prefab;

        // Weapon - 0 : Consumable - 1 : Throwable - 2 : Structure - 3
        public int typeOfItem;
    }
}
