using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
public class CraftingSlot : MonoBehaviour, IDropHandler
{
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
                            childItem.stack += 1;
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
                            childItem.stack += 1;
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
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
