using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] private string sceneToLoad;
    [SerializeField] private Vector3 lastPlayerPosition;

    private void Awake()
    {
        //añadir función al callback de cargar datos
        SaveManager.OnLoadedData += LoadSceneInfo;

        void LoadSceneInfo(SaveData saveData)
        {
            sceneToLoad = saveData.sceneInfo.name;
            lastPlayerPosition = saveData.sceneInfo.lastPosition;
        }
    }

    public void Play()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneToLoad);
    }
}
