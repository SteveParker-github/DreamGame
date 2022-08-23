using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuController : MonoBehaviour
{
    [SerializeField] private bool isDevMode = false;
    private GameObject mainPanel;
    private GameObject loadGamePanel;
    private GameObject saveGamePanel;
    private MainMenuStateFactory states;
    private MainMenuBaseState currentState;
    private bool isMidGame = false;

    public bool IsDevMode { get => isDevMode; }
    public GameObject MainPanel { get => mainPanel; }
    public GameObject LoadGamePanel { get => loadGamePanel; }
    public GameObject SaveGamePanel { get => saveGamePanel; }
    public MainMenuBaseState CurrentState { get => currentState; set => currentState = value; }
    public bool IsMidGame { get => isMidGame; set => isMidGame = value; }

    // Start is called before the first frame update
    void Start()
    {
        mainPanel = transform.Find("MainPanel").gameObject;
        loadGamePanel = transform.Find("LoadGamePanel").gameObject;
        saveGamePanel = transform.Find("SaveGamePanel").gameObject;
        loadGamePanel.SetActive(false);
        saveGamePanel.SetActive(false);

        states = new MainMenuStateFactory(this);
        currentState = states.MainPanelState();
        currentState.EnterState();
    }

    private void Update()
    {
        currentState.UpdateState();
    }
}
