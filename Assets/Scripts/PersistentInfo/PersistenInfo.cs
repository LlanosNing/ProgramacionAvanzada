using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersistenInfo : MonoBehaviour
{
    public static PersistenInfo singleton;

    //listas con las ID de todos los cofres abiertos
    [SerializeField] private List<uint> openChests = new List<uint>(); //el uint es como un int pero no admite valores negativos

    //para las instancias se usa el awake en vez del start
    private void Awake()
    {
        //cuando no hay nadie como singleton, se asigna y se marca para que no se destruya
        if(singleton == null)
        {
            singleton = this;
            DontDestroyOnLoad(gameObject);

            //añadir una funcion al callback de datos cargados
            //este codigo tan feo D: es una funcion anonima. Es como una funcion normal
            //pero se crea en el momento para añadirla al callbacl
            //entre los parentesis hay que añadir un SaveData porque el callbacl lo usa como parametro
            SaveManager.OnLoadedData += (SaveData saveData) =>
            {
                //actualiza la lista de cofres con la que haa cargado
                openChests = new List<uint>(saveData.openChestsIDs);
            };
        }
        //si al iniciar ya hay un singleton, este objeto debe destruirse para que no haya duplicados
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        //añadir la funcion de guardar al callback de guardar datos
        SaveManager.OnSaveData += Save;
    }

    public void AddOpenChest(uint chestID)
    {
        //si la ID no esta en la lista, la añade
        if(openChests.Contains(chestID) == false)
        {
            openChests.Add(chestID);
            //guardar los cofres
            SaveManager.Save();
        }
    }

    //se añade al callback de guardar info
    void Save(SaveData saveData)
    {
        //actualizar los datos de guardado con la lista de cofres abiertos
        saveData.openChestsIDs = new List<uint>(openChests);
    }

    public bool IsChestOpened(uint chestID)
    {
        //devuelve true o false en funcion de si el cofre esta en la lista de abiertos
        return openChests.Contains(chestID);
    }

}
