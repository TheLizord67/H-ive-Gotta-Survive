using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
public class CraftingSlot : MonoBehaviour, IDropHandler
{
    [SerializeField] private InventoryManager inventory;
    public void OnDrop(PointerEventData eventData)
    {
        if (transform.childCount == 0)
        {
            GameObject dropped = eventData.pointerDrag;
            ItemHandler itemHandled = dropped.GetComponent<ItemHandler>();
            itemHandled.parentAfterDrag = transform;
        }
        else
        {
            GameObject dropped = eventData.pointerDrag;
            ItemHandler itemHandled = dropped.GetComponent<ItemHandler>();
            Transform child = transform.GetChild(0);
            ItemHandler childItem = child.GetComponent<ItemHandler>();
            if (itemHandled.isItem == childItem.isItem)
            {
                if (itemHandled.isItem == true)
                {
                    if (itemHandled.itemData == childItem.itemData)
                    {
                        if (childItem.stack == childItem.itemData.item.maxStackSize)
                        {
                            return;
                        }
                        else
                        {
                            int difference = 0;
                            childItem.stack += itemHandled.stack;
                            if (childItem.stack > childItem.itemData.item.maxStackSize)
                            {
                                difference = childItem.stack - childItem.resourceData.resource.maxStackSize;
                                Debug.Log(difference);
                                childItem.stack = childItem.itemData.item.maxStackSize;
                                foreach (var slot in inventory.inventorySlots)
                                {
                                    if (slot.transform.childCount == 0)
                                    {
                                        GameObject childs = Instantiate(childItem.gameObject, slot.transform);
                                        childs.GetComponent<ItemHandler>().stack = difference;
                                        break;
                                    }
                                }
                            }
                            Destroy(itemHandled.gameObject);
                        }
                    }
                    else
                    {
                        return;
                    }
                }
                else if (itemHandled.isItem == false)
                {
                    if (itemHandled.resourceData == childItem.resourceData)
                    {
                        if (childItem.stack == childItem.resourceData.resource.maxStackSize)
                        {
                            return;
                        }
                        else
                        {
                            int difference = 0;
                            childItem.stack += itemHandled.stack;
                            if (childItem.stack > childItem.resourceData.resource.maxStackSize)
                            {
                                difference = childItem.stack - childItem.resourceData.resource.maxStackSize;
                                Debug.Log(difference);
                                childItem.stack = childItem.resourceData.resource.maxStackSize;
                                foreach (var slot in inventory.inventorySlots)
                                {
                                    if (slot.transform.childCount == 0)
                                    {
                                        GameObject childs = Instantiate(childItem.gameObject, slot.transform);
                                        childs.GetComponent<ItemHandler>().stack = difference;
                                        break;
                                    }
                                }
                            }
                            Destroy(itemHandled.gameObject);
                        }
                    }
                    else
                    {
                        return;
                    }
                }
            }
            else
            {
                return;
            }
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        inventory = GameObject.Find("Inventory Manager").GetComponent<InventoryManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
