using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDatabase
{
    private static Dictionary<string, ItemInfo> allItems;

    //añadir para que se llame automaticamente al empezar el juego
    //el parametro dentro de los parentesis sirce para que se llame ANTES que Awake y asi
    //carga la lista de objetos antes que todo lo de,as
    [RuntimeInitializeOnLoadMethod (RuntimeInitializeLoadType.BeforeSceneLoad)]
    //busca en el proyecto todos los objetos que haya e inicializa el diccionario con todos ellos
    void GetItem()
    {
        allItems = new Dictionary<string, ItemInfo>();
        //busca en la carpeta de Items todos los ItemInfo que hayamos guardado dentro
        //de la carpeta llamada Resources
        ItemInfo[] foundItems = Resources.LoadAll<ItemInfo>("Items");
        //Por cada objeto cargado, hay que añadirlo al diccionario
        //junto a su nombre para identificarlo
        foreach (ItemInfo foundItem in foundItems) 
        { 
            //como identificador se añade el propio nombre del objeto
            allItems.Add(foundItem.name, foundItem);
        }
    }

    public static ItemInfo FindItem(string itemName)
    {
        return allItems[itemName];
    }
}
