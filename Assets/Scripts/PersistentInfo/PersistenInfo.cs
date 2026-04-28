using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersistenInfo : MonoBehaviour
{
    public static PersistenInfo singleton;

    //listas con las ID de todos los cofres abiertos
    [SerializeField] private List<uint> openChests = new List<uint>(); //el uint es como un int pero no admite valores negativos
    //guarda la ID del punto en el que haya que spawnear en ese momento
    public string currentSpawnPointID;

    //para las instancias se usa el awake en vez del start
    private void Awake()
    {
        //cuando no hay nadie como singleton, se asigna y se marca para que no se destruya
        if(singleton == null)
        {
            singleton = this;
            DontDestroyOnLoad(gameObject);

            //añadir una función al callback de datos cargados
            //este código tan feo D: es una función anónima. Es como una función normal
            //pero se crea en el momento para añadirla al callbacl¡k
            //entre los paréntesis hay que añadir un SaveData porque el callback lo usa como parámetro
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
        //añadir la función de guardar al callback de guardar datos
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
        //devuelve true o false en función de si el cofre esta en la lista de abiertos
        return openChests.Contains(chestID);
    }

}
