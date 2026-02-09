using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    [SerializeField] private Image itemPrefab;
    [SerializeField] private Transform itemLayout; //todos los objetos se emparentan aqui

    public ItemInfo item;
    void Start()
    {
        //el += es meterlo en la caja
        //añadir la funcion CreateItem al callbacl del inventario cuando se añade un objeto
        //importante que la funcion reciba un ItemIndo como parametro o llora muy fuerte
        Inventory.Instance.onAddedItem += CreateItem;
    }

    //private void Update()
    //{
    //    if (Input.GetKeyDown(KeyCode.I))
    //    {
    //        CreateItem(item);
    //    }
    //}


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
        Image newItem = Instantiate(itemPrefab, itemLayout);
        //cambiar el sprite de la imagen al icono del objeto
        newItem.sprite = itemInfo.icon;
    }
}
