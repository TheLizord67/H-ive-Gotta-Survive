using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;
public class ItemHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField] public Item itemData;

    [SerializeField] public _Resources resourceData;

    [SerializeField] public bool isItem;

    [SerializeField] private Image thisObject;

    [SerializeField] public int stack;

    [SerializeField] TextMeshProUGUI stackText;

    [SerializeField] public Transform parentAfterDrag;

    [SerializeField] private bool hovered;
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
        Seperate();
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        thisObject.transform.SetParent(parentAfterDrag);
        thisObject.raycastTarget = true;
    }

    public void Seperate()
    {
        if (Input.GetMouseButtonDown(1) == true)
        {
            if (stack > 0)
            {
                GameObject seperated = Instantiate(this.gameObject);
            }
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
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
    }
}
