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

    public void CreateItem(ItemInfo itemInfo, uint amount)
    {
        //buscar si el objeto ya esta en el inventario
        ItemUI duplicateItem = FindItem(itemInfo);
        //si hay un duplicado, se actualiza la cantidad del objeto
        //si no hay duplicado, se crea un objeto nuevo

        if (duplicateItem == null)
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
            //añadir el objeto a la lista
            items.Add(newItem);
        }
        //si hay un duplicado, se actualiza la cantidad del objeto
        else
        {
            duplicateItem.UpdateAmount(amount);
        }
    }

    private ItemUI FindItem(ItemInfo infoToFind)
    {
        //buscamos en todos los objetos el que coincida con la info que buscamos
        foreach (ItemUI item in items)
        {
            //si lo encuentra, lo devuelve
            if (item.itemInfo == infoToFind)
            {
                return item;
            }
        }
        //si no encuentra objeto que coincida, devuelve NULL
        return null;

    }
}
