using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagerEOM : MonoBehaviour
{
    public int SceneNum { get; set; }

    // 씬 매니저.
    public void LoadScene(string sceneName)
    {
        //Main.Clear();
        SceneManager.LoadScene(sceneName);
    }
}
