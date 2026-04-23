using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//Para poder crear y leer archivos
using System.IO;
using UnityEngine.Events;

//Si se va a guardar con Json, hay que marcar la estructura como serializable
//usamos una class para poder modificar su valor desde distintos scripts y que se guarde
[System.Serializable]
public class SaveData
{
    //Lista de cofres ya abiertos
    public List<uint> openChestsIDs;
    internal List<ItemSaveData> items;
}

public class SaveManager
{
    static string fileName = "ReadMe.kebab";
    static SaveData saveData = new SaveData();

    //callback que se lllama cuando carga la informacion
    public static UnityAction<SaveData> OnLoadedData;
    //callback para guardar informacion
    public static UnityAction<SaveData> OnSaveData;

    public static void Save()
    {
        //llamar al callback para que todos los objetos guarden su informacion en el SaveData
        OnSaveData?.Invoke(saveData);
        //Transformar el SaveData en una string con formato Json
        string dataJson = JsonUtility.ToJson(saveData);
        //generar la ruta del archivo con PersistentDatapath y el nombre que queremos
        string filePath = Path.Combine(Application.persistentDataPath, fileName);
        //encriptar la informacion en formato Json
        dataJson = XOREncryption.EncryptDecrypt(dataJson);
        //crear el archivo de guardado en una ruta con un nombre y los datos Json
        File.WriteAllText(filePath, dataJson);
    }

    /*para que Unity llame a esta funcion cuando se inicie la escena, como si fuera un Start
     por defecto se llama despues del Awake
     como el script no es mono Behaviour (es como si existiese en el juego y ya),
     no tiene start, asi que esto es para que se ejecute al inicio
     */
    [RuntimeInitializeOnLoadMethod] 
    public static void Load()
    {
        //generar la ruta del archivo con PersistentDataPath y el nombre que queremos
        string filePath = Path.Combine(Application.persistentDataPath, fileName);
        //si no hay informacion guardada no carga ningun dato
        if(File.Exists(filePath) == false)
        {
            return;
        }
        //leer los archivos de guardado en formato Json
        string dataJson = File.ReadAllText(filePath);
        //encriptar la informacion en formato Json
        dataJson = XOREncryption.EncryptDecrypt(dataJson);
        //trandformar los fatos en formato Kson en una struct SaveData
        SaveData saveData = JsonUtility.FromJson<SaveData>(dataJson);
        //una vez esta todo cargado, se llama al callback pasando esta informacion
        OnLoadedData?.Invoke(saveData); //la ? sirve para que si esta vacio tira null ref (antes de usarlo comprueba que no este vacio)
    }
}
