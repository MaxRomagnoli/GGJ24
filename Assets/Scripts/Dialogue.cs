using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Dialogue : MonoBehaviour
{
    [SerializeField][TextArea] private string question_ita;
    [SerializeField][TextArea] private string question_eng;
    [SerializeField] private Answer[] answers;

    public string GetQuestion()
    {
        // TODO: get in base alla lingua selezionata
        return question_ita;
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
    [SerializeField] private UnityEvent action;

    public string GetText()
    {
        // TODO: get in base alla lingua selezionata
        return text_ita;
    }
    
    public Color GetColor()
    {
        return color;
    }
    
    public UnityEvent GetAction()
    {
        return action;
    }
}
