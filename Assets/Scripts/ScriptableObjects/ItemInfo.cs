using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/ItemInfo")] //El / hace que se cree un submenu (como una carpeta)
public class ItemInfo : ScriptableObject
{
    public new string name;
    public bool canBeStacked = true; //si se puede apilar
    public bool canBeDiscard =false; //si es de un solo uso
    public Sprite icon; //el icono que se vera dentro del inventario
    public ItemType type; //tipo de objeto que es

    //para que todos los objetos tengan una funcion por defceto de usar
    //usamos la palabra clave VIRTUAL para que se pueda sobreescribir esta funcion
    public virtual void Use()
    {
        if (type == ItemType.Consumible) 
        {
            Debug.Log($"Used standard item: {name}");
        }
    }
}

public enum ItemType
{
    Consumible, Equipable, Junk
}
