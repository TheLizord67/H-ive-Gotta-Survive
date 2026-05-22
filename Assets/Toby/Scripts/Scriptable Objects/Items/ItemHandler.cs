using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class ItemHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] public Item itemData;

    [SerializeField] public _Resources resourceData;

    [SerializeField] public bool isItem;

    [SerializeField] private Image thisObject;

    [SerializeField] public int stack;

    [SerializeField] TextMeshProUGUI stackText;

    [SerializeField] public Transform parentAfterDrag;

    [SerializeField] private bool hovered;

    [SerializeField] private bool grabbed;

    [SerializeField] private InventoryManager inventory;
    public void OnBeginDrag(PointerEventData eventData)
    {
        parentAfterDrag = transform.parent;
        thisObject.transform.SetParent(transform.root);
        thisObject.transform.SetAsLastSibling();
        thisObject.raycastTarget = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        thisObject.transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        thisObject.transform.SetParent(parentAfterDrag);
        thisObject.raycastTarget = true;
        grabbed = false;
    }

    public void Seperate()
    {
        if (Input.GetMouseButtonDown(1) == true)
        {
            if (stack > 1)
            {
                stack -= 1;
                foreach(var slot in inventory.inventorySlots)
                {
                    if (slot.transform.childCount == 0)
                    {
                        GameObject child = Instantiate(this.gameObject, slot.transform);
                        child.GetComponent<ItemHandler>().stack = 1;
                        break;
                    }
                }
            }
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        inventory = GameObject.Find("Inventory Manager").GetComponent<InventoryManager>();
        if (isItem == true)
        {
            if (itemData != null)
            {
                thisObject.sprite = itemData.item.icon;
            }
        }
        else
        {
            if (resourceData != null)
            {
                thisObject.sprite = resourceData.resource.icon;
            }
        }
    }
    
    // Update is called once per frame
    void Update()
    {
        if (isItem == true)
        {
            if (itemData != null)
            {
                thisObject.sprite = itemData.item.icon;
            }
        }
        else
        {
            if (resourceData != null)
            {
                thisObject.sprite = resourceData.resource.icon;
            }
        }
        stackText.text = stack.ToString();
        if (stack == 1)
        {
            stackText.text = "";
        }
        if (hovered == true)
        {
            Seperate();
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        hovered = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        hovered = false;
    }
}
