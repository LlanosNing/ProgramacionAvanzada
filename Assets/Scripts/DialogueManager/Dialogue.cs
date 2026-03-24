using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Dialogue")]
public class Dialogue : ScriptableObject
{
    //Los dos personajes dialogantes. No tienen por que estar asignados ambos, puede ser uno solo
    public Character character1, character2;
    //Lista con todas las lineas de dialogo
    public List<DialogueLine> lines = new List<DialogueLine>();

    public string GetLineText(int index)
    {
        return lines[index].text;
    }

    //devuelve el personaje que este diciendo una linea en concreto
    public Character GetCharacter(int index)
    {   
        return lines[index].whoSaysThis == DialogueCharacterType.Character1 ? character1 : character2;
    }
}

[System.Serializable]
public struct Character
{
    public string name;
    public Sprite icon;
}

[System.Serializable]
public struct DialogueLine
{
    [TextArea]
    //Con TextArea la zona de escritura y lectura de la string es más grande
    public string text;
    //A que personaje pertenece esta linea de dialogo
    public DialogueCharacterType whoSaysThis;
}

public enum DialogueCharacterType
{
    Character1, Charater2
}