using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Inventory : MonoBehaviour
{
    public ItemInfo item;

    //aqui se guardan todos los objetos que tengamos y su cantidad (dictionary es el nombre propio)
    private Dictionary<string, uint> items = new Dictionary<string, uint>(); //la U es para que no salgan los numeros negativos 

    //callback que se ejecuta cuando se añada un objeto
    //pasa como parametro la indo de objeto añadido y que cantidad de ese objeto hay
    public UnityAction <ItemInfo, uint> onAddedItem;
    //callback que se ejecuta cuando se elimine un objeto
    public UnityAction<ItemInfo, uint> onRemovedItem;

    //crear una instancia publica para este script
    public static Inventory Instance;

    public void Awake()
    {
        Instance = this;
    }

    void Start()
    {

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
        //ejecutar el callback de que se ha añadido un objeto, pasando su informacion
        //el operador ? comprueba que haya algo en el callback para ejecutarlo
        onAddedItem?.Invoke(item, items[item.name]);
    }

    public void RemoveItem(ItemInfo item)
    {
        if (items.ContainsKey(item.name) == false)
        {
            return;
        }

        //para indicar si al final de la funcion hay que eliminar el objeto del diccionario
        bool removeItem = false;
        //si el objeto esta en el inventario, hay que quitarlo
        // si el objeto se puede stackear, le resta 1 a la cantidad que tengamos
        if (item.canBeStacked == true)
        {
            //accedemos al valor a taves del nombre del objeto
            //como el nombre es la key, se usa para acceder a cada objeto por separado
            items[item.name] -= 1;
            //en cuanto se gasta hay que comprobar si aun nos quedna objetos de ese tipo
            //si no quedan, se eliminan del inventario
            if (items[item.name] <= 0)
            {
                removeItem = true;
            }
        }
        else //si el objeto NO se puede stackear lo elimina del inventario
        {
            removeItem = true;

            //forzar que la cantidad del objeto sea 0
            items[item.name] = 0;
        }

        //llamar al callback de que se ha eliminado un objeto
        onRemovedItem?.Invoke(item, items[item.name]);
        //se comprueba si hay que eliminar el objeto del inventario o no
        if(removeItem == true)
        {
            items.Remove(item.name);
        }
    }

    //devuelve true o false en funcion de si se tiene el objeto especificado o no
    public bool HasItem(ItemInfo itemToFind)
    {
       return items.ContainsKey(itemToFind.name);
    }
}
