using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EndGameUIController : MonoBehaviour
{
    [SerializeField] GameObject Popup;
    [SerializeField] TextMeshProUGUI PopupText;

    // Start is called before the first frame update
    void Start()
    {
        Popup.SetActive(false);
        EventManager.Instance.PlayerExit += PlayerWins;
    }

    private void OnDestroy()
    {
        EventManager.Instance.PlayerExit -= PlayerWins;
    }

    private void PlayerWins()
    {
        ShowPopup("Player Wins");
    }

    public void ShowPopup(string _message)
    {
        PopupText.text = _message;
        Popup.SetActive(true);
    }

    public void Quit()
    {
        Debug.Log("Quitting");
        Application.Quit();
    }

    public void Replay()
    {
        Debug.Log("Replay");
    }

    public void NextLevel()
    {
        Debug.Log("Next level");
    }
}
