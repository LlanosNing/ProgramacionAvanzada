using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConsumibleSystem : MonoBehaviour
{
    //todos los consumibles que queramos llevar equipados
    [SerializeField] private List<ConsumibleSlot> slots;

    private void Update()
    {
        //comprobar si se ha pulsado la tecla de alguno de los slots
        for (int i = 0; i < slots.Count; i++)
        {
            if (Input.GetKeyDown(slots[i].key))
            {
                //se usa lo que haya asignado a ese slot
                slots[i].Use();
            }
        }   
    }
}

//Los corchetes es para que se vea en el editor
[System.Serializable]
public struct ConsumibleSlot
{
    public ItemInfo consumible;
    public KeyCode key;

    public void AssignConsumible(ItemInfo item)
    {
        consumible = item;
    }

    public void Use()
    {
        Debug.Log($"Used slot with item {consumible.name}");
        //usar el objeto equipado en el slot
        consumible.Use();
    }
}
