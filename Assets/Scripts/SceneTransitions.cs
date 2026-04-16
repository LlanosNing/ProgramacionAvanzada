using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneTransitions : MonoBehaviour
{
    public static SceneTransitions Singleton;

    private void Awake()
    {
        if(Singleton == null)
        {
            Singleton = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else Destroy(this.gameObject);
    }

    [SerializeField] private CanvasGroup fadeImage;

    public void FadeIn()
    {
        //inicializar el alfa a 0 por si acaso
        fadeImage.alpha = 0;
        //activar la imageny hacer un tween para aumentar su alfa a 1
        fadeImage.gameObject.SetActive(true);
        fadeImage.LeanAlpha(1f, .5f);
        //esperar un tiempo antes de ejecutar el Fade Out
        LeanTween.delayedCall(2f, FadeOut);
    }
     public void FadeOut()
    {
        //inicializar el alfa a 1 por si acaso
        fadeImage.alpha = 1;
        //tween para desvanecer el panel
        fadeImage.LeanAlpha(0f, .5f).setOnComplete(()=> fadeImage.gameObject.SetActive(false));
    }
}
