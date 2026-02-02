using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public ItemInfo item;

    void Start()
    {
        Debug.Log($"Nombre: {item.name}");
        Debug.Log($"canBeStacked: {item.name}");
        Debug.Log($"canBeDiscarded: {item.name}");
    }
}
