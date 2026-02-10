using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    [SerializeField] private ItemUI itemPrefab;
    [SerializeField] private Transform itemLayout; //todos los objetos se emparentan aqui
    private List<ItemUI> items = new List<ItemUI>();

    //public ItemInfo item;
    void Start()
    {
        //el += es meterlo en la caja
        //añadir la funcion CreateItem al callbacl del inventario cuando se añade un objeto
        //importante que la funcion reciba un ItemIndo como parametro o llora muy fuerte
        Inventory.Instance.onAddedItem += CreateItem;
    }

    public void CreateItem(ItemInfo itemInfo)
    {
        Transform slot = null;

        //buscar en todos los objetos hijos del layout (huecos)
        for (int i = 0; i < itemLayout.childCount; i++)
        {
            //si el huevo no tien eobjetos hijo, significa que esta vacío
            if (itemLayout.GetChild(i).childCount == 0)
            {
                //se asigna al hueco vacío y se sale del bucle
                slot = itemLayout.GetChild(i);
                break;
            }
        }
        //crear una nueva imagen y emparentarla al Layout para que lo ponga en su posicion
        ItemUI newItem = Instantiate(itemPrefab, slot);
        //asignar al objeto de la UI su objeto al que hace referencia
        newItem.SetItem(itemInfo);
        //comprobar si el objeto ya esta en la lista de items o no
        if (items.Contains(newItem) == false)
        {
            items.Add(newItem);
        }
        else
        {
            //buscar en todos los objetos que ya tenga almacenados
            foreach (ItemUI item in items)
            {
                //cuando encuentre el objeto a actualiaar, modifica su texto 
                if (item == newItem)
                {
                    item.UpdateAmount(0);
                }
            }
        }
    }
}
