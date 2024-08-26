using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagerEOD : MonoBehaviour
{
    // 씬 매니저.
    public void LoadScene(string sceneName)
    {
        //Main.Clear();
        SceneManager.LoadScene(sceneName);
    }
}
