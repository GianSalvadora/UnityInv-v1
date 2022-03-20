using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Inventory : MonoBehaviour
{
    public InventorySlot slotCurrent;
    [SerializeField]  TextMeshProUGUI Description;
    [SerializeField] Text itemName;
    [SerializeField] Image itemImage;
    [SerializeField] GameObject toDisable;
    [SerializeField] Transform childBearer;

    public Transform output;
    public GameObject outputObj;

    public List<InventorySlot> invList;

    private void Start()
    {
        foreach(Transform child in childBearer)
        {
            invList.Add(child.GetComponent<InventorySlot>());
        }
    }

    void UpdateUI()
    {
        if (slotCurrent != null)
        {
            Description.text = slotCurrent.itemObj.itemDesc;
            itemName.text = slotCurrent.itemObj.itemName;
            itemImage.sprite = slotCurrent.itemObj.itemImage;
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
    public void OnSlotClick(InventorySlot slotSelected, int type)
    {
        if (type == 1)
        {
            if (slotSelected.itemObj != null)
            {
                slotCurrent = slotSelected;
            }
            else if(slotSelected.itemObj == null)
            {
                slotCurrent = null;
            }
        }
        else if(type == 2)
        {
            if (slotCurrent == slotSelected)
                return;

            if(slotCurrent != null)
            {
                if(slotSelected.itemObj == slotCurrent.itemObj)
                {
                    int sC = slotCurrent.itemAmount;
                    int sS = slotSelected.itemAmount;
                    int total = sC + sS;
                    if(total <= 64)
                    {
                        slotCurrent.itemObj = null;
                        slotCurrent.itemAmount = 0;
                        slotCurrent.UpdateSlot();
                        slotCurrent = null;
                        slotSelected.itemAmount += sC;
                        slotSelected.UpdateSlot();
                    }
                    else if(total > 64)
                    {
                        slotCurrent.itemAmount = total - 64;
                        slotSelected.itemAmount = 64;
                        slotCurrent.UpdateSlot();
                        slotSelected.UpdateSlot();
                    }
                }
                else if(slotCurrent.itemObj != slotSelected.itemObj)
                {
                    if(slotSelected.itemObj == null)
                    {
                        slotSelected.itemObj = slotCurrent.itemObj;
                        slotSelected.itemAmount = slotCurrent.itemAmount;
                        slotSelected.UpdateSlot();
                        slotCurrent.itemAmount = 0;
                        slotCurrent.itemObj = null;
                        slotCurrent.UpdateSlot();
                        slotCurrent = slotSelected;
                        return;
                    }
                    Item temp = slotCurrent.itemObj;
                    int temp2 = slotCurrent.itemAmount;

                    slotCurrent.itemObj = slotSelected.itemObj;
                    slotCurrent.itemAmount = slotSelected.itemAmount;
                    slotSelected.itemObj = temp;
                    slotSelected.itemAmount = temp2;


                    slotCurrent.UpdateSlot();
                    slotSelected.UpdateSlot();

                    slotCurrent = slotSelected;
                }
            }
        }
        UpdateUI();
    }

    public void OnUseClick()
    {
        if (slotCurrent == null)
        {
            return;
        }

        slotCurrent.itemAmount--;
        slotCurrent.UpdateSlot();
        slotCurrent.itemObj.Use();

        if(slotCurrent.itemAmount <= 0)
        {
            slotCurrent = null;
            UpdateUI();
        }
    }

    public void OnThrowClick()
    {
        if (slotCurrent == null)
            return;
        ThrowItem(slotCurrent.itemObj, slotCurrent.itemAmount);
        slotCurrent.itemAmount = 0;
        slotCurrent.UpdateSlot();

        slotCurrent = null;

        UpdateUI();
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
