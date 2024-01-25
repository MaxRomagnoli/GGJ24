using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Dialogue : MonoBehaviour
{
    [SerializeField][TextArea] private string question_ita;
    [SerializeField][TextArea] private string question_eng;
    [SerializeField] private Color color;
    [SerializeField] private Answer[] answers;

    public string GetQuestion()
    {
        // TODO: get in base alla lingua selezionata
        return question_ita;
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
    [SerializeField] private Color color;
    [SerializeField] private Dialogue otherDialogue;

    public string GetText()
    {
        // TODO: get in base alla lingua selezionata
        return text_ita;
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