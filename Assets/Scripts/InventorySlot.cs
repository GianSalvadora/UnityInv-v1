using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InventorySlot : MonoBehaviour, IPointerClickHandler, IDragHandler, IDropHandler
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
        mainInv.OnSlotClick(this);
    }

    public void OnDrag(PointerEventData eventData)
    {
        
    }

    public void OnDrop(PointerEventData eventData)//equip - equip
    {
        if(eventData.pointerDrag.TryGetComponent<SpecialSlot>(out SpecialSlot from))//if sender is special
        {
            if(from.equippedItem == null)
            {
                return;
            }
            if(itemObj != null && !itemObj.equipable)//if we have an item and it isnt equipable
            {
                return;
            }
            else if(itemObj == null)//we are empty
            {
                itemObj = from.equippedItem;
                itemAmount = 64;

                from.equippedItem = null;
                from.UpateSlot();
                UpdateSlot();
                return;
            }
            if (itemObj.equipable)//else if we are another equipable
            {
                Item temp = itemObj;
                itemObj = from.equippedItem;
                itemAmount = 64;

                from.equippedItem = temp;
                from.UpateSlot();
                UpdateSlot();
            }
        }
        else if(eventData.pointerDrag.TryGetComponent<InventorySlot>(out InventorySlot sender))
        {
            if(sender == this)
            {
                return;
            }

            if(sender.itemObj == null)
            {
                return;
            }

            if (sender.itemObj == itemObj)
            {
                int total = itemAmount + sender.itemAmount;

                if (total <= 64)
                {
                    sender.itemObj = null;
                    sender.itemAmount = 0;
                    sender.UpdateSlot();

                    itemAmount = total;
                    UpdateSlot();
                }
                else if (total > 64)
                {
                    itemAmount = 64;
                    UpdateSlot();

                    sender.itemAmount = total - 64;
                    sender.UpdateSlot();
                }
            }
            else if (itemObj != sender.itemObj && itemObj != null)
            {
                Item temp = itemObj;
                int temp2 = itemAmount;
                itemObj = sender.itemObj;
                itemAmount = sender.itemAmount;
                UpdateSlot();

                sender.itemAmount = temp2;
                sender.itemObj = temp;
                sender.UpdateSlot();
            }
            else if (itemObj == null)
            {
                itemObj = sender.itemObj;
                itemAmount = sender.itemAmount;
                UpdateSlot();

                sender.itemAmount = 0;
                sender.itemObj = null;
                sender.UpdateSlot();
            }

        }
    }
}
