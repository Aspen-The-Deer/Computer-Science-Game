using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
// ^ Default unity functionality requirements + Scene Tools.

public class Level_Select : MonoBehaviour
{

    // When called, set the active scene
    // to 0 (Level 0)
    public void Level0()
    {
        SceneManager.LoadScene(0);
    }

    // When called, set the active scene
    // to 1 (Level 1)
    public void Level1()
    {
        SceneManager.LoadScene(1);
    }

    // When called, set the active scene
    // to 2 (Level 2)
    public void Level2()
    {
        SceneManager.LoadScene(2);
    }

    // When called, set the active scene
    // to 3 (Level 3)
    public void Level3()
    {
        SceneManager.LoadScene(3);
    }
}
