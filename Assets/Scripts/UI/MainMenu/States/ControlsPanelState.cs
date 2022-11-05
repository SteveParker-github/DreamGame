using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class ControlsPanelState : MainMenuBaseState
{
    public ControlsPanelState(MainMenuController mainMenuController, MainMenuStateFactory mainMenuStateFactory)
    : base(mainMenuController, mainMenuStateFactory)
    { }

    private GameObject backButton;
    private bool isBackTriggered;

    public override void EnterState()
    {
        ctx.ControlsPanel.SetActive(true);
        isBackTriggered = false;
        Transform panel = ctx.ControlsPanel.transform.Find("Panel");
        backButton = panel.Find("BackButton").gameObject;
        backButton.GetComponent<Button>().onClick.AddListener(BackButtonOnClick);
    }
    public override void UpdateState()
    {
        CheckSwitchState();
    }
    public override void ExitState()
    {
        backButton.GetComponent<Button>().onClick.RemoveAllListeners();
        ctx.ControlsPanel.SetActive(false);
    }
    public override void CheckSwitchState()
    {
        if (isBackTriggered)
        {
            SwitchState(factory.MainPanelState());
            return;
        }
    }

    private void BackButtonOnClick()
    {
        isBackTriggered = true;
    }
}