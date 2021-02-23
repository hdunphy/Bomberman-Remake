using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EndGameUIController : MonoBehaviour
{
    [SerializeField] GameObject Popup;
    [SerializeField] TextMeshProUGUI PopupText;
    [SerializeField] Button NextLevelButton;
    [SerializeField] LevelGenerator LevelGenerator;

    public Text Text;

    private bool isMusicPlaying = true;

    // Start is called before the first frame update
    void Start()
    {
        Popup.SetActive(false);
        EventManager.Instance.PlayerExit += PlayerWins;
        EventManager.Instance.PlayerDies += PlayerLoses;
    }

    private void OnDestroy()
    {
        EventManager.Instance.PlayerExit -= PlayerWins;
        EventManager.Instance.PlayerDies -= PlayerLoses;
    }

    private void PlayerWins()
    {
        AudioManager.Instance.PlaySound("Win");
        NextLevelButton.gameObject.SetActive(true);
        ShowPopup("Player Wins");
    }

    private void PlayerLoses()
    {
        AudioManager.Instance.PlaySound("Lose");
        NextLevelButton.gameObject.SetActive(false);
        ShowPopup("Player Loses");
    }

    public void ToggleMusic()
    {
        isMusicPlaying = !isMusicPlaying;
        AudioManager.Instance.ToggleSound("Music", isMusicPlaying);
    }

    public void ShowPopup(string _message)
    {
        PopupText.text = _message;
        Popup.SetActive(true);
    }

    public void ShowMessageString(string _message)
    {
        Text.text = _message;
    }

    public void Quit()
    {
        Debug.Log("Quitting");
        Application.Quit();
    }

    public void Replay()
    {
        Debug.Log("Replay");
        LevelGenerator.ReplayLevel();
        Popup.SetActive(false);
    }

    public void NextLevel()
    {
        Debug.Log("Next level");
        LevelGenerator.NextLevel();
        Popup.SetActive(false);
    }
}
