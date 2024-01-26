using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Dialogue : MonoBehaviour
{
    [SerializeField] private string pngName;
    [SerializeField][TextArea] private string question_ita;
    [SerializeField][TextArea] private string question_eng;
    [SerializeField] private Font font;
    [SerializeField] private Color color;
    [SerializeField] private Answer[] answers;

    public string GetPNGName()
    {
        return pngName;
    }

    public string GetQuestion()
    {
        // TODO: get in base alla lingua selezionata
        return question_ita;
    }
    
    public Font GetFont()
    {
        return font;
    }
    
    public Color GetColor()
    {
        return color;
    }

    public Answer[] GetAnswers()
    {
        return answers;
    }
}

[System.Serializable]
public class Answer
{
    [SerializeField] private string text_ita;
    [SerializeField] private string text_eng;
    [SerializeField] private bool persistent;
    [SerializeField] private string specialFunction;
    [SerializeField] private Color color;
    [SerializeField] private Dialogue otherDialogue;

    public string GetText()
    {
        // TODO: get in base alla lingua selezionata
        return text_ita;
    }

    public bool IsPersistent()
    {
        return persistent;
    }

    public string GetSpecialFunction()
    {
        return specialFunction;
    }
    
    public Color GetColor()
    {
        return color;
    }
    
    public Dialogue GetOtherDialogue()
    {
        return otherDialogue;
    }
}

// Ciao, ti stai divertendo?
// 1. Si
    // Anche io! Vuoi ballare?
    // 1.1 
    // 1.2 
// 2. No