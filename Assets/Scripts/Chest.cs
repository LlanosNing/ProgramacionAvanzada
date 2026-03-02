using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour
{
    [SerializeField] private bool isOpened;
    [SerializeField] private Material openedMat;
    //el identificador que lo diferencia del resto de cofres
    [SerializeField] private uint chestID;

    private void Start()
    {
        //comprueba en la lista de corres abiertos si este cofre lo esta
        if (PersistenInfo.singleton.IsChestOpened(chestID))
        {
            Open();
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player") && isOpened == false)
        {
            Open();
        }
    }

    void Open()
    {
        isOpened = true;
        GetComponent<Renderer>().material = openedMat;
        //al abrirse, el cofre se añade a la lista de cofres ya abiertos
        PersistenInfo.singleton.AddOpenChest(chestID);
    }

    //para solo marcar el cofre como que esta abierto, pero sin hacer nada mas (se abre falsamente)
    void SetOpen()
    {
        isOpened = true;
        GetComponent<Renderer>().material = openedMat;
    }
}
