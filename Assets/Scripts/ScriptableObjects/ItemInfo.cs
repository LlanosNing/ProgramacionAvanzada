using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/ItemInfo")] //El / hace que se cree un submenu (como una carpeta)
public class ItemInfo : ScriptableObject
{
    public new string name;
    public bool canBeStacked = true; //si se puede apilar
    public bool canBeDiscard =false; //si es de un solo uso
}
