using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableDetector : MonoBehaviour
{
    //el objeto interactuable del tipo que sea tiene actualmente en rango
    private IInteractable currentInteractable;


    private void Update()
    {
        //al darle a la tecla de interactuar con un objeto guardado, se interactua con el 
        if (Input.GetKeyDown(KeyCode.F) && currentInteractable != null)
        {
            //llamar a la funcion de Interact
            currentInteractable.Interact();
            //quitar la referencia para que no se pueda interactuar mas de una vez
            currentInteractable = null;
        }
    }

    //esta configurado con layers para que solo pueda detectar interactuables
    private void OnTriggerEnter(Collider other)
    {
        //guardar la interfaz que tenga ese objeto
        currentInteractable = other.GetComponent<IInteractable>();
    }

    private void OnTriggerExit(Collider other)
    {
        currentInteractable = null;
    }
}
