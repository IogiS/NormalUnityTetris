using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenucontroller : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayGame() 
    {
        SceneManager.LoadScene("level");
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void Pause()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
