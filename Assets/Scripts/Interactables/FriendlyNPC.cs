using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//el IInteractuable (las interfaces) de primeras chilla porque pide que se implemente la interfaz
//si pasas el cursor por encima del error debe de aparecer algo de Fix error y la primera opcion debe de ser algo de implementar inferfaz
public class FriendlyNPC : MonoBehaviour, IInteractuable
{
    [SerializeField] private Dialogue dialogue;

    //este script, al interactuar mediante la interface, muestra el dialogo asignado
    public void Interact()
    {
        DialogueManager.singleton.BeginDialogue(dialogue);
    }

    //DEBUGGING: se cambiara por interactuar con el NPC
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Interact();
        }
    }
}
