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

    private int currentLine = 0;

    private void Start()
    {
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
            return;
        }

        currentLine++;
        ShowDialogueLine();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            NextLine();
        }
    }
}
