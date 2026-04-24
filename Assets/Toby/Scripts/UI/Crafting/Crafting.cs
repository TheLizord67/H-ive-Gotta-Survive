using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Crafting : MonoBehaviour
{
    [SerializeField] private Recipies recipieList;
    
    [SerializeField] private InventoryManager playerData;

    [SerializeField] private GameObject craftingMenu;

    [SerializeField] private Image craftedIcon;

    [SerializeField] private List<GameObject> craftingSlots;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
