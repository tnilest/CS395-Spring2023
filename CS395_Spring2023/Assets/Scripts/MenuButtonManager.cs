using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuButtonManager : MonoBehaviour //manager script for menu button operations.
{
    public int firstLevel;
    public int secondLevel;
    public int thirdLevel;

    public void StartGame()
    {
        SceneManager.LoadScene(firstLevel);
    }

    public GameObject TitlePanel;
    public GameObject ButtonCanvas;
    public GameObject LevelMenu;
    public void LevelSelect()
    {
        LevelMenu.SetActive(true);
        ButtonCanvas.SetActive(false);
        TitlePanel.SetActive(false);
    }

    public void chooseLevel(int level)
    {
        SceneManager.LoadScene(level);
    }
 
    public void ViewCredits()
    {
        ButtonCanvas.SetActive(false);
        TitlePanel.SetActive(false);
        CreditsMenu.SetActive(true);
    }

    public GameObject CreditsMenu;
    public void ReturnToMain() {
        LevelMenu.SetActive(false);
        CreditsMenu.SetActive(false);
        ButtonCanvas.SetActive(true);
        TitlePanel.SetActive(true);
    }

    public void ExitGame() {
        Application.Quit();
    }
}
