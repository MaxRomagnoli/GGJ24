using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    [SerializeField] RaycastSomething raycastSomething;

    [Header("UI")]
    [SerializeField] GameObject interactPanel;
    [SerializeField] GameObject dialoguePanel;
    [SerializeField] TextMeshProUGUI dialogueText;
    [SerializeField] Transform buttonPanel;
    [SerializeField] GameObject buttonPrefab;

    private Dialogue dialogue;

    // Start is called before the first frame update
    void Start()
    {
        interactPanel.SetActive(false);
        dialoguePanel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        // TODO: Typing
    }

    public void SetInteractPanel(bool _set)
    {
        GameObject _go = raycastSomething.GetHittedGameObject();
        if(_go == null) {
            interactPanel.SetActive(false);
            return;
        }
        dialogue = _go.GetComponent<Dialogue>();
        if(dialogue == null) {
            interactPanel.SetActive(false);
            return;
        }

        dialogueText.text = dialogue.GetQuestion();

        // Delete all old buttons
        foreach (Transform child in buttonPanel) {
            GameObject.Destroy(child.gameObject);
        }

        // Instantiate answers with prefab button
        foreach(Answer _answer in dialogue.GetAnswers())
        {
            GameObject _newButton = Instantiate(buttonPrefab, buttonPanel.position, Quaternion.identity);
            _newButton.transform.SetParent(buttonPanel); // Set buttonPanel as parent
            
            Text _text = _newButton.GetComponentInChildren<Text>();
            _text.text = _answer.GetText();

            Image _image = _newButton.GetComponent<Image>();
            _image.color = _answer.GetColor();

            // TODO: Change with another dialogue if present (this method doesnt work)
            Button _button = _newButton.GetComponent<Button>();
            _button.onClick = _answer.GetAction() as UnityEngine.UI.Button.ButtonClickedEvent;
        }

        interactPanel.SetActive(_set);
    }

    public bool GetCanStartDialogue()
    {
        if(interactPanel.activeInHierarchy)
        {
            raycastSomething.enabled = false;
            interactPanel.SetActive(false);
            dialoguePanel.SetActive(true);
            return true;
        }
        return false;
    }

    public void SetDialoguePanel(bool _set)
    {
        dialoguePanel.SetActive(_set);
    }
}
