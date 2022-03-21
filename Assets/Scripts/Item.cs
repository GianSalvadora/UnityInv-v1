using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "Item/Item")]
public class Item : ScriptableObject
{
    public bool equipable;
    public static Player player;
    public new string name= "Item";
    public string itemDesc = "Desc";
    public Sprite itemImage;
    public enum Slot
    {
        Item,
        Head,
        Torso,
        Legs,
        Weapon,
        Shield
    }

    public Slot Type;

    public virtual void Use()
    {
        Debug.Log("Used " + name);
    }
    public virtual void Unequip()
    {

    }

    public virtual int GetData()
    {
        return 0;
    }
}
