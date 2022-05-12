using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
// ^ Default unity functionality requirements + Scene Tools.

public class Respawn_Menu : MonoBehaviour
{

    // When first run, Unlock the cursor so that
    // it can be used to use the menu.
    public void Awake()
    {
        Cursor.lockState = CursorLockMode.None;
    }
    
    // When called, set the active scene
    // to 4 (Main Menu)
    public void MainMenu()
    {
        SceneManager.LoadScene(4);
    }

    // When called, Exit the game.
    public void QuitGame()
    {
        Application.Quit();
    }
}
