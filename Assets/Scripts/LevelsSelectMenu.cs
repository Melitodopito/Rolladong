using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelsSelectMenu : MonoBehaviour
{

    public void Loadlevel(string levelName){

        SceneManager.LoadScene(levelName);
    }
}
