using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "Item/Item")]
public class Item : ScriptableObject
{
    public bool equipable;
    public static Player player;
    public string itemName = "Item";
    public string itemDesc = "Desc";
    public Sprite itemImage;

    public virtual void Use()
    {
        Debug.Log("Used " + name);
    }
    public virtual void Unequip() 
    {
        
    }
}
