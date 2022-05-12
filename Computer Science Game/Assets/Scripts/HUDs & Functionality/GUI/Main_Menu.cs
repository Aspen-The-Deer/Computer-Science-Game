using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
// ^ Default unity functionality requirements + Scene Tools.

public class Main_Menu : MonoBehaviour
{

    // When called, set the active scene
    // to 1 (Level 1)
    public void PlayGame()
    {
        SceneManager.LoadScene(1);
    }

    // When called, Exit the game.
    public void QuitGame()
    {
        Application.Quit();
    }
}
