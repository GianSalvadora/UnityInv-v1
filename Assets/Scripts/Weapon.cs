using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Weapon", menuName = "Item/Weapon")]
public class Weapon : Equipable 
{
    [SerializeField]int attack;

    private void Awake()
    {
        equipable = true;
    }

    public override int GetData()
    {
        return attack;
    }
}
