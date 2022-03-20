using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Equipable", menuName = "Item/Equipable")]
public class Equipable : Item
{
    public int defense;

    public override void Use()
    { 
        if(player == null)
        {
            player = FindObjectOfType<Player>();
        }

        player.OnChangeArmor(defense, false);
    }
    public override void Unequip()
    {
        player.OnChangeArmor(defense, true);
    }
}
