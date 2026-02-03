using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public ItemInfo item;

    //aqui se guardan todos los objetos que tengamos y su cantidad (dictionary es el nombre propio)
    private Dictionary<string, uint> items = new Dictionary<string, uint>(); //la U es para que no salgan los numeros negativos 

    //crear una instancia publica para este script
    public static Inventory Instance;

    
    public void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        //estos son los parametros del scriptableObject
        Debug.Log($"canBeStacked: {item.name}");
        Debug.Log($"Nombre: {item.name}");
        Debug.Log($"canBeDiscarded: {item.name}");
    }

    private void Update()
    {
        foreach (var item in items)
        {
            Debug.Log($"{item.Key} Quantity: {item.Value}"); //esto es porque Unity no deja ver lo que hay en el inventario (en el editor)
        }
    }

    public void AddItem(ItemInfo item)
    {
        //Si el objeto no esta en el inventario, lo añade y ya
        if (items.ContainsKey(item.name) == false)
        {
            items.Add(item.name, 1);
        }
        //si el objeto ya esta en el inventario
        else
        {
            if(item.canBeStacked == true)
            {
                //accedemos al valor a taves del nombre del objeto
                //como el nombre es la key, se usa para acceder a cada objeto por separado
                items[item.name] += 1;
            }
            else //si el objeto NO se puede stackear se añade de nuevo al diccionario
            {
                items.Add(item.name, 1);
            }


        }
    }
}
