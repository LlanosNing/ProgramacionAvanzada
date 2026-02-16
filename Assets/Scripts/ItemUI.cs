using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using JetBrains.Annotations;

public class ItemUI : MonoBehaviour
{
    //el objeto asociado a este objeto de la UI del inventario
    public ItemInfo itemInfo;
    [SerializeField] private Image icon;
    [SerializeField] private TMP_Text amountTxt;

    //asignar el objeto asociado y actualizar los elementos de la UI
    public void SetItem(ItemInfo info)
    {
        itemInfo = info;
        icon.sprite = itemInfo.icon;

        if(info.canBeStacked == false)
        {
            amountTxt.gameObject.SetActive(false);
        }
    }

    //actualizar el texto con la cantidad de objetos disponibles
    public void UpdateAmount(uint amount)
    {
        amountTxt.text = amount.ToString();
    }
}

