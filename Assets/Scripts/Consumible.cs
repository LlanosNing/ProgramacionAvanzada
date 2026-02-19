using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Consumible")]
public class Consumible : ItemInfo //si hereda de ItemInfo significa que tiene todo lo de un ItemInfo (que es un scriptable object) y todo lo que más queiras meterle
{
    //si cura o resta vida al jugador
    public int healthAmount = 0;
    //si modifica la velocidad de movimiento del jugador
    public float moveSpeedAmount = 0;
    //si es un buff/debuff, cuanto dura
    public float duration = 0;

    //sobreescribir la funcion Use de la clase base para que gaste este objeto al usarlo
    public override void Use()
    {
        Inventory.Instance.RemoveItem(this);
    }
}
