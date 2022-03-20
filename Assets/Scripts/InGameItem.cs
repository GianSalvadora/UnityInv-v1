using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameItem : MonoBehaviour
{
    public Item baseItem;
    public int amount;
    SpriteRenderer sr;
    private void Awake()
    {
        sr = this.GetComponent<SpriteRenderer>();
    }
    public void OnIstantiate()
    {
        sr.sprite = baseItem.itemImage;
        gameObject.name = baseItem.name + amount;
    }
}
