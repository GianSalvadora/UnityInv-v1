using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SpecialSlot : MonoBehaviour, IDropHandler, IDragHandler, IPointerClickHandler
{
    [SerializeField] private Image ItemDisp;
    public Item equippedItem;
    private Item prevItem=null;
    public Item.Slot Type;
    private static Inventory mainInv;
    private static Player player;


    private void Update()
    {
        if(prevItem != equippedItem)
        {
            if (equippedItem != null)
            {
                if (prevItem != null)
                {
                    player.ChangeStats(-prevItem.GetData(), Type);
                    player.ChangeStats(equippedItem.GetData(), Type);
                    prevItem = equippedItem;
                }
                else if(prevItem == null)
                {
                    player.ChangeStats(equippedItem.GetData(), Type);
                    prevItem = equippedItem;
                }
            }
            else if(equippedItem == null)
            {
                player.ChangeStats(-prevItem.GetData(), Type);
                prevItem = null;
            }
        }
    }

    private void Start()
    {
        if (mainInv == null)
        {
            mainInv = FindObjectOfType<Inventory>();
        }
        if (player == null)
        {
            player = FindObjectOfType<Player>();
        }
    }

    public void OnDrop(PointerEventData eventData)
    {
        if(eventData.pointerDrag.TryGetComponent<InventorySlot>(out InventorySlot from))
        {
            OnReceived(from);
        }
    }

    private void OnReceived(InventorySlot from)
    {
        if(from.itemObj == null)
        {
            return;
        }
        Item fromObj = from.itemObj;

        if (Type != fromObj.Type)
        {
            return;
        }

        if (fromObj.equipable && equippedItem == null)
        {
            SetDisp(fromObj.itemImage);

            equippedItem = fromObj;
            from.itemObj = null;
            from.UpdateSlot();
        }
        else if(fromObj.equipable && equippedItem != null)
        {
            SetDisp(fromObj.itemImage);

            Item temp = equippedItem;
            equippedItem = fromObj;
            from.itemObj = temp;
            from.UpdateSlot();
        }
    }

    private void SetDisp(Sprite img)
    {
        ItemDisp.enabled = true;
        ItemDisp.sprite = img;
    }

    public void UpateSlot()
    {
        if(equippedItem == null)
        {
            ItemDisp.enabled = false;
            ItemDisp.sprite = null;
        }
        else
        {
            SetDisp(equippedItem.itemImage);
        }
        
    }

    public void OnDrag(PointerEventData eventData)
    {
        
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        mainInv.OnSpecialSlotClick(this);
    }
}
