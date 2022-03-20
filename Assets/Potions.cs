using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Potion", menuName = "Item/Potion")]
public class Potions : Item
{
    public override void Use()
    {
        Debug.Log("Drank " + name);;
    }

}
