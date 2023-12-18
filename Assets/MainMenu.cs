using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Play()
    {
        SceneManager.LoadScene("Instruksi");
    }
    
    public void Next()
    {
        SceneManager.LoadScene("Depan");
    }

    public void Menu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void Nomer1()
    {
        SceneManager.LoadScene("Level1");
    }

    public void Nomer2()
    {
        SceneManager.LoadScene("Level2");
    }

    public void Nomer3()
    {
        SceneManager.LoadScene("Level3");
    }

    public void Exit()
    {
        Application.Quit();
    }
}
