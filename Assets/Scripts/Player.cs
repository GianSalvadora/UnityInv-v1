using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Player : MonoBehaviour
{
    [SerializeField]int baseDamage;
    [SerializeField]int baseDefense;

    int finalDamage;
    int finalDefense;
   [SerializeField]TextMeshProUGUI tmp;
    //movement
    Rigidbody2D rb2d;
    Vector2 dir;
    [SerializeField] float movementSpeed;

    //items
    [SerializeField] float pickupRadius;
    Inventory inv;
    private void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        inv = FindObjectOfType<Inventory>();

        finalDefense = baseDefense;
        finalDamage = baseDamage;
    }

    public void ChangeStats(int stat, Item.Slot type)
    {
        if(type ==Item.Slot.Weapon)
        {
            finalDamage += stat;
        }
        else if(type != Item.Slot.Weapon && type != Item.Slot.Item)
        {
            finalDefense += stat;
        }
        
    }

    private void Update()
    {
        tmp.text = "" + finalDamage + " " + finalDefense;
        dir = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        if (Input.GetKeyDown(KeyCode.Space))
        {
            
            Collider2D[] things = Physics2D.OverlapCircleAll(transform.position, pickupRadius);
            InGameItem  closest = null;
            float prevDist = -1;
            foreach(Collider2D i in things)
            { 
                if(i.TryGetComponent<InGameItem>(out InGameItem itm))
                { 
                    if (closest == null)
                    { 
                        closest = itm;
                        prevDist = Vector2.Distance(closest.transform.position, transform.position);
                    }
                    else if (Vector2.Distance(itm.transform.position, transform.position) < prevDist)
                    { 
                        closest = itm;
                        prevDist = Vector2.Distance(itm.transform.position, transform.position);
                    }
                }
            }
            inv.OnItemRecieve(closest);
        }
    }

    private void FixedUpdate()
    {
        rb2d.velocity = dir * movementSpeed;
    }
}
