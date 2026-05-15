using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Crafting : MonoBehaviour
{
    [SerializeField] private Recipies recipieList;

    [SerializeField] private GameObject craftingMenu;

    [SerializeField] private GameObject crafted;

    [SerializeField] private GameObject craftedSlot;

    [SerializeField] private GameObject craftedIcon;

    [SerializeField] private List<GameObject> craftingSlots;

    [SerializeField] private List<ItemHandler> items;

    [SerializeField] private List<_Resources> resources;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        SetCraftingSlots();
        MakeRecipie();
        if (crafted.transform.childCount == 0)
        {
            foreach (var item in items)
            {
                Destroy(item.gameObject);
            }
            craftedIcon = Instantiate(craftedSlot, crafted.transform);
        }
    }
    public void SetCraftingSlots()
    {
        items.Clear();
        foreach (var item in craftingSlots)
        {
            int childCount = item.transform.childCount;
            if (childCount > 0)
            {
                Transform child = item.transform.GetChild(0);
                if (child.GetComponent<ItemHandler>() == true)
                { 
                    items.Add(child.GetComponent<ItemHandler>());
                    if (child.GetComponent<ItemHandler>().stack > 0)
                    {
                        for (int i = 1; i < child.GetComponent<ItemHandler>().stack; i++)
                        {
                            items.Add(child.GetComponent<ItemHandler>());
                        }
                    }
                }
            }
        }
    }
    public void MakeRecipie()
    {
        resources.Clear();
        foreach (var itemHand in items)
        {
            resources.Add(itemHand.resourceData);
        }
        foreach (var recip in recipieList.recipies)
        {
            bool what = resources.Count() == recip.resources.Count();
            Debug.Log(what);
            if (resources.Count() == recip.resources.Count())
            {
                for (int i = 0; i < resources.Count(); i++)
                {
                    if (resources[i] == recip.resources[i])
                    {
                        craftedIcon.GetComponent<Image>().enabled = true;
                        craftedIcon.GetComponent<ItemHandler>().itemData = recip.result;
                    }
                    else
                    {
                        craftedIcon.GetComponent<Image>().enabled = false;
                        i = resources.Count() + 1;
                        craftedIcon.GetComponent<ItemHandler>().itemData = null;
                    }
                }
            }
        }
    }
}
