using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Linq;

public class SaveSystem : MonoBehaviour
{

    Inventory mainInv;

    private void Start()
    {
        mainInv = FindObjectOfType<Inventory>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            SaveData();
        }

        if (Input.GetKeyDown(KeyCode.L))
        {

            string info = File.ReadAllText(Application.persistentDataPath + "/InventorySave.json");
            string[] datasTemp  = info.Split(' ');
            string[] datas = new string[datasTemp.Length - 1];
            for (int i = 0; i < datasTemp.Length - 1; i++)
            {
                datas[i] = datasTemp[i];
            }
            foreach (string x in datas)
            {
                Slot slot = new Slot();
                slot = JsonUtility.FromJson<Slot>(x);
                slot.LoadSlot(mainInv);
            }

            string specialInfo = File.ReadAllText(Application.persistentDataPath + "/SpecialInventorySave.json");
            string[] specialDatasTemp = specialInfo.Split(' ');
            string[] specialDatas = new string[specialDatasTemp.Length - 1];
            for (int i = 0; i < specialDatasTemp.Length - 1; i++)
            {
                specialDatas[i] = specialDatasTemp[i];
            }
            foreach(string x in specialDatas)
            {
                LocalSpecialSlot slot = new LocalSpecialSlot();
                slot = JsonUtility.FromJson<LocalSpecialSlot>(x);
                slot.LoadSlot(mainInv);
            }
        }
    }
    private void SaveData()
    {

        string normalInvData = "";
        string specialInvdata = "";
        List<InventorySlot> slotList = mainInv.invList;
        for (int i = 0; i < slotList.Count; i++)
        {
            Slot temp = new Slot();
            temp.item = slotList[i].itemObj;
            temp.itemAmount = slotList[i].itemAmount;
            temp.index = i;
            normalInvData += JsonUtility.ToJson(temp) + " ";
        }
        List<SpecialSlot> specialSlots = mainInv.specialInvList;
        for (int i = 0; i < specialSlots.Count; i++)
        {
            LocalSpecialSlot temp = new LocalSpecialSlot();
            temp.item = specialSlots[i].equippedItem;
            temp.index = i;
            specialInvdata += JsonUtility.ToJson(temp) + " ";
        }
        File.WriteAllText(Application.persistentDataPath + "/InventorySave.json", normalInvData);
        File.WriteAllText(Application.persistentDataPath + "/SpecialInventorySave.json", specialInvdata);
    }
}

public class Slot
{
    public int itemAmount;
    public Item item;
    public int index;

    public void LoadSlot(Inventory mainInv)
    {
        mainInv.LoadSlot(item, itemAmount, index);
    }
}

public class LocalSpecialSlot
{
    public Item item;
    public int index;

    public void LoadSlot(Inventory mainInv)
    {
        mainInv.LoadSpecialSlot(item, index);
    }
}
                    