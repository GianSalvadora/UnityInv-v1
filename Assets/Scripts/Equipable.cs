using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Equipable", menuName = "Item/Equipable")]
public class Equipable : Item
{
    public int defense;

    private void Awake()
    {
        equipable = true;
    }

    public override void Use()
    { 
        
    }
    public override void Unequip()
    {
        

    }
}
