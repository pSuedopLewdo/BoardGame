using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Menus : MonoBehaviour
{
    public float timeRemaining;
    public float roundTimer;
    public GuessManager guessManager;
    public Text timeLeftText;
    public GameObject endGameMenu, revealLetters, mainMenuObj;
    public GameScreenState gState = GameScreenState.REVEAL;


    public enum GameScreenState
    {
        PLAY,
        REVEAL,
        END,
        MAIN,
        RESET,
        RESULTS,
        WAITING,
    }
    private void FixedUpdate()
    {
        switch (gState)
        {
            case GameScreenState.PLAY:
                Play();
                break;
            case GameScreenState.REVEAL:
                Reveal();
                break;
            case GameScreenState.END:
                EndMenu();
                break;
            case GameScreenState.MAIN:
                MainMenu();
                break;
            case GameScreenState.RESET:
                Reset();
                break;
            case GameScreenState.RESULTS:
                break;
            case GameScreenState.WAITING:
                Waiting();
                break;
            default:
                break;
        }
    }

    private void Play()
    {
        endGameMenu.SetActive(false);
        revealLetters.SetActive(false);
        mainMenuObj.SetActive(false);
        
        //countdown
        if (timeRemaining > 0)
        {
            timeLeftText.gameObject.SetActive(true);
            timeRemaining -= Time.deltaTime;
            var m = timeRemaining * 10;
            var i = Mathf.Round(m);
            timeLeftText.text = (i /= 10).ToString();
        }
        else
        {
            timeLeftText.gameObject.SetActive(false);

            //Timer ran out end game
            gState = GameScreenState.END;
        }
    }
    
    private void Reveal()
    {
        endGameMenu.SetActive(false);
        mainMenuObj.SetActive(false);
        
        //Shows the Reveal screen and awaits a click
        revealLetters.SetActive(true);
        timeRemaining = roundTimer;
        gState = GameScreenState.WAITING;
    }

    public void RevealButtonClicked()
    {
        gState = GameScreenState.PLAY;
    }

    public void RestartButtonClicked()
    {
        gState = GameScreenState.RESET;
    }

    public void EndButtonClicked()
    {
        gState = GameScreenState.END;
    }
    public void StartGameSetReveal()
    {
        gState = GameScreenState.REVEAL;
    }

    public void Waiting()
    {
        
    }
    
    private void EndMenu()
    {
        endGameMenu.SetActive(true);
        guessManager.EndOfGame();
        guessManager.ChangeTextAfterGuess();
        gState = GameScreenState.WAITING;
    }
    
    public void MainMenu() 
    {
        mainMenuObj.SetActive(true);
    }

    private void Reset()
    {
        timeRemaining = roundTimer;
        guessManager.Reset();
        gState = GameScreenState.REVEAL;
    }

    public void BogglePlayButton()
    {
        gState = GameScreenState.RESET;
    }
}
