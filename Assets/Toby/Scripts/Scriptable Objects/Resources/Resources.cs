using UnityEngine;

[CreateAssetMenu(fileName = "Resources", menuName = "Scriptable Objects/Resources")]
public class Resources : ScriptableObject
{
    [System.Serializable] 
    private struct Resource
    {
        public string name;
        
        public Sprite inventorySprite;

        public string description;

        
    }
}
