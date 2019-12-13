using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GamePlayController : MonoBehaviour
{
    [SerializeField]
    private GameObject pausePanel;

    void Awake()
    {
        pausePanel.SetActive(false);
    }

    public void PauseGame()
    {
        Time.timeScale = 0f;
        pausePanel.SetActive(true);
    }

    public void ResumeGame()
    {

        Time.timeScale = 1f;
        pausePanel.SetActive(false);
    }

    public void BackToMenu()
    {

        SceneManager.LoadScene("MainMenu");
    }
}
