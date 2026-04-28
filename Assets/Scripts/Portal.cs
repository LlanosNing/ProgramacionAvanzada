using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Portal : MonoBehaviour
{
    [SerializeField] private int sceneIndex = 0;
    [SerializeField] private GameObject door;
    //la ID del punto de spawn al que debe ir en la siguiente escena
    [SerializeField] private string spawnPointID;

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player") == true)
        {
            /* tween para rotar al objeto hacia -88.96 en el eje Y
             * con EaseOutBacl se cambia la curva de animacion que usa
             * con setOnComplete se le añade una funcion para que la llame cuando termine el Tween
             */
            door.LeanRotateY(-88.969f, 2f).setEaseOutBack().setOnComplete(()=> SceneManager.LoadScene(sceneIndex));
            //SceneManager.LoadScene(sceneIndex);
            LeanTween.delayedCall(1, SceneTransitions.Singleton.FadeIn);
            //asignar la nueva ID de spawn en el PersistenInfo
            PersistenInfo.singleton.currentSpawnPointID = spawnPointID;
        }
    }
}
