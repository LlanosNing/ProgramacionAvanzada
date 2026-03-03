using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//Para poder crear y leer archivos
using System.IO;

//Si se va a guardar con Json, hay que marcar la estructura como serializable
[System.Serializable]
public struct SaveData
{
    //Lista de cofres ya abiertos
    public List<uint> openChestsIDs;
}

public class SaveManager
{
    static string fileName = "ReadMe.kebab";

    public static void Save(List<uint> openChests)
    {
        //Crear unos datos de guardado nuevos
        SaveData saveData = new SaveData();
        saveData.openChestsIDs = new List<uint>();
        //Asignar a los datos de guardado la lista de cofres abiertos (nueva lista con los mismos valores)
        saveData.openChestsIDs = new List<uint>(openChests);
        //Transformar el SaveData en una string con formato Json
        string dataJson = JsonUtility.ToJson(saveData);
        //generar la ruta del archivo con PersistentDatapath y el nombre que queremos
        string filePath = Path.Combine(Application.persistentDataPath, fileName);
        //si el archivo no existe, se crea
        if (File.Exists(filePath) == false)
        {
            File.Create(filePath);
        }
        //crear el archivo de guardado en una ruta con un nombre y los datos Json
        File.WriteAllText(Application.persistentDataPath + fileName, dataJson);
    }
}
