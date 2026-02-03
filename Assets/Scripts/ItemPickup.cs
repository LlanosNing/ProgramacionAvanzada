using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    public ItemInfo itemInfo;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //hacer cosas (no entiendo a que se refiere fran con esto)
            Inventory.Instance.AddItem(itemInfo);
        }
    }
}
