using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Dialogue")]
public class Dialogue : ScriptableObject
{
    public DialogueCharacter character1, character2;

    //Lista con todas las líneas de diálogo
    public List<DialogueLine> lines = new List<DialogueLine>();

    //Para acceder rapidamente al texto de una línea concreta
    public string GetLineText(int index)
    {
        return lines[index].text;
    }
}

[System.Serializable]
public struct DialogueCharacter
{
    public string name;
    public Sprite icon;
}

[System.Serializable]
public struct DialogueLine
{
    [TextArea]
    public string text;

    //Qué personaje dice esta línea de diálogo
    public DialogueCharacterType whoSaysThis; 
}

public enum DialogueCharacterType
{
    Character1, Character2
}
