using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOver : Dialog
{
    public void replay()
    {
        Show(false);
        ReloatScene();
    }
    public void backhome()
    {
        Show(false);
        ReloatScene();
    }
    public void exitGame()
    {
        Show(false);
        Application.Quit();
    }
    private void ReloatScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
