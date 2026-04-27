using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable] //pa que se vea en el editor
public struct SpawnPoint
{
    public string id;
    public Transform point;
}
public class PlayerSpawnManagement : MonoBehaviour
{
    //lista con todos los puntos de spawn de cada escena    
    [SerializeField] private List<SpawnPoint> spawnPoints;
    [SerializeField] private string spawnID;

    private Transform player;

    private void Start()
    {
        //buscar y guardar al objeto del jugador
        player = GameObject.FindWithTag("Player").transform;
        //acceder a la escena en la que se encuentra actualmente
        Scene currentScene = SceneManager.GetActiveScene();
        //buscar el punto con la ID guardada
        Transform spawnPoint = GetSpawnPoint(spawnID);
        //modificar posicion y rotacion del personaje
        player.position = spawnPoint.position;
        player.rotation = spawnPoint.rotation;
    }

    Transform GetSpawnPoint(string idToGet)
    {
        for (int i = 0; i < spawnPoints.Count; i++)
        {
            if(spawnPoints[i].id == idToGet)
            {
                return spawnPoints[i].point;
            }
        }
        return null;
    }
}
