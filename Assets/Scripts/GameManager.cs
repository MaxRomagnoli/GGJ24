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

    public void EnteredPNG()
    {
        GameObject _go = raycastSomething.GetHittedGameObject();
        if(_go == null) { Debug.LogWarning("GameObject from Raycasting not found"); return; }

        Dialogue dialogue = _go.GetComponent<Dialogue>();
        SetDialogue(dialogue);

        interactPanel.SetActive(true);
    }

    public void ExitPNG()
    {
        interactPanel.SetActive(false);
        dialoguePanel.SetActive(false);
        popUpPanel.SetActive(false);
    }

    void lockPlayer(bool _set)
    {
        raycastSomething.enabled = !_set;
        starterAssetsInputs.SetCursorState(!_set);
        starterAssetsInputs.cursorInputForLook = !_set;
        firstPersonController.CanMove = !_set;
    }

    public void ClosePopUp()
    {
        dialoguePanel.SetActive(true);
        popUpPanel.SetActive(false);
    }

    public void SetDialogue(Dialogue dialogue)
    {
        if(dialogue == null) {
            interactPanel.SetActive(false);
            dialoguePanel.SetActive(false);
            lockPlayer(false);
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

    public bool InDialogue()
    {
        Debug.Log("InDialogue");
        if(interactPanel.activeInHierarchy)
        {
            lockPlayer(true);
            interactPanel.SetActive(false);
            dialoguePanel.SetActive(true);
            return true;
        } else if(dialoguePanel.activeInHierarchy)
        {
            return true;
        }
        return false;
    }

    public void SetDialoguePanel(bool _set)
    {
        dialoguePanel.SetActive(_set);
    }
}