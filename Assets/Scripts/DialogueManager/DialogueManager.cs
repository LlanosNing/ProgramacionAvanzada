using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager singleton;

    private void Awake()
    {
        if(singleton == null)
        {
            singleton = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else Destroy(this.gameObject);
    }

    [SerializeField] private Dialogue currentDialogue;
    [SerializeField] private Image characterIcon;
    [SerializeField] private TMP_Text characterNameTxt;
    [SerializeField] private TMP_Text dialogueLineTxt;

    private int currentLine = 0;//la linea de dialogo que se debe mostrar
    private Canvas canvas; //el componente canvas que lleva el manager
    private bool inDialogue = false; //pa veh si hay un dialogo en curso

    private void Start()
    {
        canvas = GetComponent<Canvas>();
        //desactivar al inicio por zi acazo
        canvas.enabled = false;
    }

    public void BeginDialogue(Dialogue dialogue)
    {
        //asignar el nueov dialogo actual
        currentDialogue = dialogue;
        //MUSHO IMPORTANTE reiniciar la linea actual al empezar un nuevo dialogo
        currentLine = 0;
        //activar el canvas
        canvas.enabled = true;
        //marcar que hay un dialogo en curso
        inDialogue = true;
        //mostrar la primera linea de dialogo
        ShowDialogueLine();
    }

    void ShowDialogueLine()
    {
        //Actualizar el texto de la línea de diálogo
        dialogueLineTxt.text = currentDialogue.GetLineText(currentLine);
        //actualizar el icono con el perosnaje que diga esta linea y con su nombre
        characterIcon.sprite = currentDialogue.GetCharacter(currentLine).icon;
        characterNameTxt.text = currentDialogue.GetCharacter(currentLine).name;
    }

    public void NextLine()
    {
        //si ha llegado a la ultima linea de dialogo, se cierra
        if (currentLine >= currentDialogue.lines.Count) 
        {
            EndDialogue();
            return;
        }

        currentLine++;
        ShowDialogueLine();
    }

    void EndDialogue()
    {
        canvas.enabled = false;
        //marcar que ya no hay ningun dialogo en curso
        inDialogue = false;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return) && inDialogue == true)
        {
            NextLine();
        }
    }
}
