using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEditor;
using StarterAssets;

public class GameManager : MonoBehaviour
{
    [SerializeField] FirstPersonController firstPersonController;
    [SerializeField] RaycastSomething raycastSomething;
    [SerializeField] EventSystem eventSystem;
    [SerializeField] StarterAssetsInputs starterAssetsInputs;
    [SerializeField] LookAt playerLookAt;
    [SerializeField] AudioSource soundtrack;

    [Header("UI")]
    [SerializeField] GameObject interactPanel;
    [SerializeField] GameObject dialoguePanel;
    [SerializeField] Text dialogueNameText;
    [SerializeField] Text dialogueText;
    [SerializeField] Transform buttonPanel;
    [SerializeField] GameObject buttonPrefab;
    [SerializeField] Font fallbackFont;

    [Header("Pause menu")]
    [SerializeField] Text pauseMenuText;
    [SerializeField] GameObject otherPanels;
    [SerializeField] GameObject pauseMenu;
    [SerializeField] GameObject resumeButton;
    [SerializeField] GameObject restartButton;

    [Header("Game over menu")]
    [SerializeField] GameObject gameOverPanel;
    [SerializeField] string gameOverMessage = "Sei morto";
    [SerializeField] float euphoriaEffect = 5f;
    [SerializeField] int drugsBeforeDie = 3;

    [Header("Pop up")]
    [SerializeField] GameObject popUpPanel;
    [SerializeField] Text popUpText;
    [SerializeField][TextArea] List<string> noMessages;

    [Header("GuyWhoExplode")]
    [SerializeField] GameObject guyWhoExplodeLight;
    [SerializeField] ParticleSystem guyWhoExplodeParticles;
    [SerializeField] GameObject[] guyWhoExplodeParts;

    private Dialogue currentDialogue;
    private float euphoria = 0; // Seconds before stop euphoria

    // Start is called before the first frame update
    void Start()
    {
        interactPanel.SetActive(false);
        dialoguePanel.SetActive(false);
        popUpPanel.SetActive(false);
        pauseMenu.SetActive(false);
        gameOverPanel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        // If euphoria, jump for 10 seconds
        if(euphoria > 0) {
            euphoria -= Time.deltaTime;
        }

        // If esc is triggered
        if(starterAssetsInputs.esc) {
            Debug.Log("Esc pressed");
            starterAssetsInputs.esc = false;
            if(!pauseMenu.activeInHierarchy) {
                PauseMenu();
            } else {
                ResumeGame();
            }
        }
    }

    public void PauseMenu()
    {
        LockPlayer(true);
        otherPanels.SetActive(false);
        pauseMenu.SetActive(true);
        eventSystem.SetSelectedGameObject(resumeButton);
    }

    public void ResumeGame()
    {
        otherPanels.SetActive(true);
        pauseMenu.SetActive(false);
        if(!dialoguePanel.activeInHierarchy) { LockPlayer(false); }
    }

    public void QuitGame()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }

    public void Restart()
    {
        SceneManager.LoadScene(0);
    }

    public float GetEuphoria()
    {
        return euphoria;
    }

    public void SetEuphoria(float _seconds)
    {
        drugsBeforeDie -= 1;
        if(drugsBeforeDie <= 0) {
            Die();
        } else {
            euphoria = _seconds;
        }
    }

    private void Die()
    {
        pauseMenuText.text = gameOverMessage;
        gameOverPanel.SetActive(true);
        resumeButton.SetActive(false);
        PauseMenu();
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

    void LockPlayer(bool _set)
    {
        // Active or deactive raycast script when player locked
        raycastSomething.enabled = !_set;

        // Lock other input mechanism or looking scripts
        starterAssetsInputs.SetCursorState(!_set);
        starterAssetsInputs.cursorInputForLook = !_set;
        starterAssetsInputs.look = Vector2.zero;
        starterAssetsInputs.jump = false;
        firstPersonController.CanMove = !_set;

        // Activate look at at PNG if present
        GameObject _go = raycastSomething.GetHittedGameObject();
        if(_go != null)
        {
            LookAt _lookAt = _go.GetComponent<LookAt>();
            if(_lookAt != null) { _lookAt.enabled = _set; }
        }

        // Focus on first button of buttonPanel
        eventSystem.SetSelectedGameObject(buttonPanel.GetChild(0).gameObject);
    }

    public void OpenPopUp()
    {
        popUpText.text = noMessages[0];
        noMessages.RemoveAt(0);
        dialoguePanel.SetActive(false);
        popUpPanel.SetActive(true);
    }

    public void ClosePopUp()
    {
        // Remove all the other "no" buttons
        if(noMessages.Count == 0) {
            SetDialogue(currentDialogue);
        }
        
        dialoguePanel.SetActive(true);
        popUpPanel.SetActive(false);
    }

    public void SetDialogue(Dialogue dialogue = null)
    {
        currentDialogue = dialogue;

        if(dialogue == null) {
            interactPanel.SetActive(false);
            dialoguePanel.SetActive(false);
            LockPlayer(false);
            return;
        }

        Font _font = dialogue.GetFont();
        if(_font == null) { _font = fallbackFont; }
        Color _color = dialogue.GetColor();

        dialogueText.text = dialogue.GetQuestion();
        dialogueText.font = _font;
        dialogueText.color = _color;

        dialogueNameText.text = dialogue.GetPNGName();
        dialogueNameText.font = _font;
        dialogueNameText.color = _color;

        // Delete all old buttons
        foreach (Transform child in buttonPanel) {
            GameObject.Destroy(child.gameObject);
        }

        foreach(Answer _answer in dialogue.GetAnswers())
        {
            // Doesn't instantiate the button if is not persistent and there are no more messages for the pop up
            if(!_answer.IsPersistent() && noMessages.Count == 0) { continue; }

            // Instantiate answers with prefab button
            GameObject _newButton = Instantiate(buttonPrefab, buttonPanel.position, Quaternion.identity);

            // Set buttonPanel as parent
            _newButton.transform.SetParent(buttonPanel);
            
            Text _text = _newButton.GetComponentInChildren<Text>();
            _text.text = _answer.GetText();

            Image _image = _newButton.GetComponent<Image>();
            _image.color = _answer.GetColor();

            Button _button = _newButton.GetComponent<Button>();

            if(_answer.IsPersistent()) {
                // Call another dialogue if present when button clicked
                _button.onClick.AddListener(() => { this.SetDialogue(_answer.GetOtherDialogue()); } );
            } else {
                // If is a no message, call the pop up
                _button.onClick.AddListener(() => { this.OpenPopUp(); } );
            }

            // Special functions to add in buttons
            switch(_answer.GetSpecialFunction()) 
            {
                case "follow":
                    _button.onClick.AddListener(() => { this.NewFollowerForPlayer(raycastSomething.GetHittedGameObject()); } );
                    break;
                case "euphoria":
                    _button.onClick.AddListener(() => { this.SetEuphoria(euphoriaEffect); } );
                    break;
                case "dance battle":
                    _button.onClick.AddListener(() => { this.DanceBattle(raycastSomething.GetHittedGameObject(), _answer.GetOtherDialogue()); } );
                    break;
                case "explode":
                    _button.onClick.AddListener(() => { this.Explode(); } );
                    break;
                default:
                    break;
            }
        }
    }

    public void Explode()
    {
        soundtrack.volume = 1f;
        foreach(GameObject _part in guyWhoExplodeParts) {
            _part.SetActive(false);
            guyWhoExplodeLight.SetActive(true);
            guyWhoExplodeParticles.Play();
        }
    }

    public void DanceBattle(GameObject _obj, Dialogue _finalDialogue)
    {
        StartCoroutine(DanceAroundPlayer( _obj, _finalDialogue));
    }

    private IEnumerator DanceAroundPlayer(GameObject _obj, Dialogue _finalDialogue)
    {
        Animator _animator = _obj.GetComponent<Animator>();
        _animator.enabled = true;

        LookAt _lookAt = _obj.GetComponentInChildren<LookAt>();
        _lookAt.enabled = true;
        
        dialoguePanel.SetActive(false);

        // Look at the dancing obj
        playerLookAt.enabled = true;
        playerLookAt.SetTarget(_obj.transform.GetChild(0));

        AiFollower _follower = _obj.GetComponent<AiFollower>();
        _follower.enabled = true;

        // Rotate around for 10 seconds
        yield return new WaitForSeconds(10f);

        playerLookAt.enabled = false;

        dialoguePanel.SetActive(true);
        SetDialogue(_finalDialogue);
    }

    public void NewFollowerForPlayer(GameObject _obj)
    {
        LookAt _lookAt = _obj.GetComponent<LookAt>();
        if(_lookAt) { _lookAt.enabled = true; }

        Follower _follower = _obj.GetComponent<Follower>();
        if(_follower) {
            _follower.enabled = true;
            _follower.AddSpeed(1f);
        }

        AiFollower _aiFollower = _obj.GetComponent<AiFollower>();
        if(_aiFollower) { _aiFollower.enabled = true; }
    }

    public bool InDialogue()
    {
        if(interactPanel.activeInHierarchy)
        {
            OpenDialogue();
            return true;
        }

        return dialoguePanel.activeInHierarchy;
    }

    public void OpenDialogue()
    {
        LockPlayer(true);
        interactPanel.SetActive(false);
        dialoguePanel.SetActive(true);
    }
}
