using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class StartScene : BaseScene
{
    public TMP_Text tmpText;
    public float blinkSpeed = 1.0f; 

    private float alpha = 0f; 
    private bool increasing = true; 

    protected override bool Initialize()
    {
        if (!base.Initialize()) return false;



        return true;
    }

    public void MoveNextScene()
    {
        Main.Scene.LoadScene("MainScene");
    }
    private void Update()
    {
        if (Input.anyKeyDown)
        {
            MoveNextScene();
        }

        alpha += (increasing ? 1 : -1) * blinkSpeed * Time.deltaTime;
        alpha = Mathf.Clamp01(alpha);

        if (alpha >= 1f) increasing = false;
        else if (alpha <= 0f) increasing = true;
                
        if (tmpText != null)
        {
            Color color = tmpText.color;
            color.a = alpha;
            tmpText.color = color;
        }
        
    }

}
