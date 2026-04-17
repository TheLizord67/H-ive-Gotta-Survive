using UnityEngine;

public partial class _Resources
{
    [System.Serializable] 
    public struct Resource
    {
        public string name;
        
        public Sprite icon;

        public string description;

        public int maxStackSize;

        public GameObject prefab;
    }
}
