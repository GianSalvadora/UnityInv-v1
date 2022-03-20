using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InventorySlot : MonoBehaviour, IPointerClickHandler
{
    public Image iconSlot;
    public Item itemObj;
    static Inventory mainInv;
    [SerializeField] Slider amountSlider;

    public int itemAmount;

    private void Start()
    { 
        if(itemObj == null)
        {
            itemAmount = 0;
        }
        if(mainInv == null)
        {
            mainInv = FindObjectOfType<Inventory>();
        }
        UpdateSlot();
    }

    public void UpdateSlot()
    {
        if(itemAmount <= 0)
        {
            itemObj = null;
        }

        if (itemObj != null)
            iconSlot.sprite = itemObj.itemImage;
        else
            iconSlot.sprite = null;
        amountSlider.value = itemAmount;
    }
    public void OnPointerClick(PointerEventData eventData)
    { 
        mainInv.OnSlotClick(this, eventData.pointerId * - 1);
    }
}
