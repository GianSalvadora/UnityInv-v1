using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Inventory : MonoBehaviour
{
    public InventorySlot slotCurrent;
    [SerializeField] TextMeshProUGUI Description;
    [SerializeField] Text itemName;
    [SerializeField] Image itemImage;
    [SerializeField] GameObject toDisable;
    [SerializeField] Transform childBearer;

    public Transform output;
    public GameObject outputObj;

    public List<InventorySlot> invList;

    private void Start()
    {
        foreach (Transform child in childBearer)
        {
            invList.Add(child.GetComponent<InventorySlot>());
        }
    }

    void UpdateUI(Item obj)
    {
        if (obj != null)
        {
            Description.text = obj.itemDesc;
            itemName.text = obj.name;
            itemImage.sprite = obj.itemImage;
            return;
        }

        Description.text = "Select an item";
        itemName.text = "Select an item";
        itemImage.sprite = null;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            toDisable.SetActive(!toDisable.activeSelf);
        }
    }
    public void OnSlotClick(InventorySlot slotSelected)
    { 
            if (slotSelected.itemObj != null)
            {
                slotCurrent = slotSelected;
            }
            else if (slotSelected.itemObj == null)
            {
                slotCurrent = null;
                UpdateUI(null);
                return; 
            }
        
        UpdateUI(slotCurrent.itemObj);
    }

    public void OnSpecialSlotClick(SpecialSlot slot)
    {
        if(slot.equippedItem == null)
        {
            return;
        }
        UpdateUI(slot.equippedItem);

        slotCurrent = null;
    }

    public void OnUseClick()
    {
        if (slotCurrent == null ||slotCurrent.itemObj == null)
        {
            return;
        }
        if (slotCurrent.itemObj.equipable)
        {
            return;
        }

        slotCurrent.itemAmount--;
        slotCurrent.UpdateSlot();
        slotCurrent.itemObj.Use();

        if(slotCurrent.itemAmount <= 0)
        {
            slotCurrent = null;
            UpdateUI(null);
        }
    }

    public void OnThrowClick()
    {
        if (slotCurrent == null || slotCurrent.itemObj == null)
            return;
        ThrowItem(slotCurrent.itemObj, slotCurrent.itemAmount);
        slotCurrent.itemAmount = 0;
        slotCurrent.UpdateSlot();

        slotCurrent = null;

        UpdateUI(null);
    }

    private void ThrowItem(Item itm, int amt)
    {
        InGameItem outp = Instantiate(outputObj, output.position, Quaternion.identity).GetComponent<InGameItem>();
        outp.baseItem = itm;
        outp.amount = amt;
        outp.OnIstantiate();
    }

    public void OnItemRecieve(InGameItem received)//fix 4 equipables
    {
        if(received ==  null)
        {
            return;
        }

        bool emptyExist = false;
        bool similarExist = false;
        InventorySlot empty =null;
        InventorySlot similar = null;
        for (int x = 0; x < invList.Count; x++)
        {
            InventorySlot loopCurrent = invList[x];
             if(loopCurrent.itemObj == null && !emptyExist)
             {
                empty = loopCurrent;
                emptyExist = true;
             }
            if (loopCurrent.itemObj == received.baseItem && !similarExist)
            { 
                    similar = loopCurrent;
                    similarExist = true;
            }
        }

        if(emptyExist && received.baseItem.equipable)
        {
            empty.itemAmount = 64;
            empty.itemObj = received.baseItem;

            empty.UpdateSlot();
            received.gameObject.SetActive(false);
            return;
        }

        if (similarExist)
        {
            int total = similar.itemAmount + received.amount;
            if(total <= 64)
            {
                similar.itemAmount = total;
                received.gameObject.SetActive(false);
                similar.UpdateSlot();
                return;
            }
            else if(total > 64)
            {
                if (emptyExist)
                {
                    similar.itemAmount = 64;

                    empty.itemObj = received.baseItem;
                    empty.itemAmount = total - 64;

                    similar.UpdateSlot();
                    empty.UpdateSlot();
                    received.gameObject.SetActive(false);
                    return;
                }
                else
                {
                    similar.itemAmount = 64;
                    received.amount = total - 64;

                    similar.UpdateSlot();
                }
            }
        }
        else if(!similarExist && emptyExist)
        {
            empty.itemAmount = received.amount;
            empty.itemObj = received.baseItem;

            empty.UpdateSlot();
            received.gameObject.SetActive(false);
        }
    }
}
