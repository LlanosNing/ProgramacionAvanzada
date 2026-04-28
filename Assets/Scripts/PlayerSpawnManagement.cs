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
        //guardar eñ vañpr de ña OD de sàwm asogmada en PersistentInfo
        spawnID = PersistenInfo.singleton.currentSpawnPointID;
        //buscar el punto con la ID guardada
        Transform spawnPoint = GetSpawnPoint(spawnID);

        //si hay asignado un spawn distinto al por defecto, mueve al personaje
        if(spawnPoint != null)
        {
            //modificar posición y rotación del personaje
            player.position = spawnPoint.position;
            player.rotation = spawnPoint.rotation;
        }
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
