using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using StarterAssets;

public class GameManager : MonoBehaviour
{
    [SerializeField] FirstPersonController firstPersonController;
    [SerializeField] RaycastSomething raycastSomething;
    [SerializeField] StarterAssetsInputs starterAssetsInputs;

    [Header("UI")]
    [SerializeField] GameObject interactPanel;
    [SerializeField] GameObject dialoguePanel;
    [SerializeField] GameObject popUpPanel;
    [SerializeField] TextMeshProUGUI dialogueText;
    [SerializeField] Transform buttonPanel;
    [SerializeField] GameObject buttonPrefab;

    // Start is called before the first frame update
    void Start()
    {
        interactPanel.SetActive(false);
        dialoguePanel.SetActive(false);
        popUpPanel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        // TODO: Typing
    }

    public void ClosePopUp()
    {
        dialoguePanel.SetActive(true);
        popUpPanel.SetActive(false);
    }

    public void SetInteractPanel(bool _set)
    {
        GameObject _go = raycastSomething.GetHittedGameObject();
        if(_go == null) {
            interactPanel.SetActive(false);
            return;
        }

        Dialogue dialogue = _go.GetComponent<Dialogue>();
        SetDialogue(dialogue);

        interactPanel.SetActive(_set);

        starterAssetsInputs.cursorLocked = !_set;
        starterAssetsInputs.cursorInputForLook = !_set;
        firstPersonController.CanMove = !_set;
    }

    public void SetDialogue(Dialogue dialogue)
    {
        if(dialogue == null) {
            interactPanel.SetActive(false);
            dialoguePanel.SetActive(false);
            return;
        }

        dialogueText.text = dialogue.GetQuestion();
        dialogueText.color = dialogue.GetColor();

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

            Button _button = _newButton.GetComponent<Button>();
            _button.onClick.AddListener(() => { this.SetDialogue(_answer.GetOtherDialogue()); } );
        }
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
