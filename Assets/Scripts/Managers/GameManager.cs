using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    AsyncOperation loadingOperation;
    public enum GameScene
    {
        MainMenu = 0,
        Game = 1
    }

    public enum UIState
    {
        MainMenu,
        Options,
        Shop,
        Leaderboard,
        Loading,
        Countdown,
        InGame,
        Pause,
        GameEnd
    }

    [Header("UI")]
    public Canvas MainUICanvas;
    public GameObject StartPanel;
    public GameObject OptionsPanel;
    public GameObject ShopPanel;
    public GameObject CountDownPanel;
    public GameObject PausePanel;
    public GameObject GamePanel;
    public GameObject GameEndPanel;
    public GameObject LoadingPanel;

    //Eventually AudioManager
    [Header("Mute UI")]
    public GameObject mutedIcon;
    public GameObject unMutedIcon;
    public Action<bool> OnMuteEvent;
    public bool isMusicMuted;

    [Header("ContDownUI")]
    public ContDownPanel CountPanelScript;

    [Header("InGame")]
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI multiplierText;

    public void OnPressPlay()
    {
        //ShowUI(UIState.Loading);
        LoadScene(GameScene.Game);
    }

    public void OnPressQuit() { }

    public void OnPressOptions()
    {
        ShowUI(UIState.Options);
    }

    public void OnPressShop()
    {
        ShowUI(UIState.Shop);
    }

    public void OnPressPause()
    {
        Time.timeScale = 0f;
        ShowUI(UIState.Pause);
    }

    public void OnPressResume()
    {
        Time.timeScale = 1f;
        ShowUI(UIState.InGame);
    }

    public void OnPressLeaderBoard()
    {
        ShowUI(UIState.Leaderboard);
    }

    //Call all events to mute what is needed
    //Only UI here with Invoke
    public void Mute()
    {
        OnMuteEvent?.Invoke(isMusicMuted);

        if (isMusicMuted)
        {
            unMutedIcon.gameObject.SetActive(true);
            mutedIcon.gameObject.SetActive(false);
        }
        else
        {
            unMutedIcon.gameObject.SetActive(false);
            mutedIcon.gameObject.SetActive(true);
        }

        isMusicMuted = !isMusicMuted;
    }

    public void LoadScene(GameScene scene)
    {
        ShowUI(UIState.Loading);
        StartCoroutine(LoadSceneAsync(scene));
    }

    IEnumerator LoadSceneAsync(GameScene scene)
    {
        yield return null;

        loadingOperation = SceneManager.LoadSceneAsync((int)scene);
        loadingOperation.allowSceneActivation = false;

        while (loadingOperation.progress < 0.9f)
        {
            float progress = Mathf.Clamp01(loadingOperation.progress / 0.9f);
            // Update loading visuals here
            yield return null;
        }

        yield return new WaitForSeconds(0.2f);

        loadingOperation.allowSceneActivation = true;

        // Wait one frame so scene is fully active
        yield return null;

        OnSceneReady(scene);
    }

    void OnSceneReady(GameScene scene)
    {
        switch (scene)
        {
            case GameScene.MainMenu:
                ShowUI(UIState.MainMenu);
                break;
            case GameScene.Game:
                ShowUI(UIState.Countdown);
                break;
        }
    }

    void HideAll()
    {
        StartPanel.SetActive(false);
        OptionsPanel.SetActive(false);
        ShopPanel.SetActive(false);

        CountDownPanel.SetActive(false);
        GamePanel.SetActive(false);
        PausePanel.SetActive(false);
        GameEndPanel.SetActive(false);

        LoadingPanel.SetActive(false);
    }

    public void ShowUI(UIState state)
    {
        HideAll();

        switch (state)
        {
            case UIState.MainMenu: StartPanel.SetActive(true); break;
            case UIState.Options: OptionsPanel.SetActive(true); break;
            case UIState.Shop: ShopPanel.SetActive(true); break;

            case UIState.Countdown: CountDownPanel.SetActive(true); break;
            case UIState.InGame: GamePanel.SetActive(true); break;
            case UIState.Pause: PausePanel.SetActive(true); break;
            case UIState.GameEnd: GameEndPanel.SetActive(true); break;

            case UIState.Loading: LoadingPanel.SetActive(true); break;
        }
    }

    public void UpdateInGameUI(int gameScore) 
    {
        scoreText.text = gameScore.ToString();
    }
}
