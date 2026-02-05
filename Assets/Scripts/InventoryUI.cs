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

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            CreateItem(item);
        }
    }

    public void CreateItem(ItemInfo itemInfo)
    {
        //crear una nueva imagen y emparentarla al Layout para que lo ponga en su posicion
        Image newItem = Instantiate(itemPrefab, itemLayout);
        //cambiar el sprite de la imagen al icono del objeto
        newItem.sprite = itemInfo.icon;
    }
}
