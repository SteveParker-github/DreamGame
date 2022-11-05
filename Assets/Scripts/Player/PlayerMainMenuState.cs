using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMainMenuState : PlayerBaseState
{
    public PlayerMainMenuState(PlayerController playerController, PlayerStateFactory playerStateFactory)
    : base(playerController, playerStateFactory)
    { }

    private bool sceneLoading = false;
    public override void EnterState()
    {
        ctx.IsPaused = true;
        ctx.GameManager.IsPaused = true;
        ctx.UIManager.ToggleQuestTracker();
        ctx.AmbientSound.Pause();
        if (!SceneManager.GetSceneByName("MainMenuScene").isLoaded)
        {
            SceneManager.LoadScene("MainMenuScene", LoadSceneMode.Additive);
            sceneLoading = true;           
        }
    }
    public override void UpdateState()
    {
        if (sceneLoading)
        {
            GameObject.Find("MainMenu").GetComponent<MainMenuController>().IsMidGame = true;
            sceneLoading = false;
        }
    }
    public override void ExitState()
    {
        ctx.IsPaused = false;
        ctx.GameManager.IsPaused = false;
        ctx.UIManager.ToggleQuestTracker();
        ctx.AmbientSound.UnPause();
        
        if (SceneManager.GetSceneByName("MainMenuScene").isLoaded)
        {
            SceneManager.UnloadSceneAsync("MainMenuScene");
        }
    }
    public override void CheckSwitchState()
    {
        SwitchState(factory.PlayerWalkingState());
    }
}