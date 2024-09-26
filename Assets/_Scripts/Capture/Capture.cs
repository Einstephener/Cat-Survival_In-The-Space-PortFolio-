using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public enum Grade
{
    Null,
    Normal,
    Uncommon,
    Rare,
    Legend,
}

public enum Size
{
    POT64,
    POT128,
    POT256,
    POT512,
    POT1024,
}

public class Capture : MonoBehaviour
{
    public Camera cam;
    public RenderTexture rt;
    public Image bg;

    public Grade grade;
    public Size size;

    public GameObject[] obj;
    int nowCnt = 0;

    private void Start()
    {
        cam = Camera.main;
        SettingColor();
        SettingSize();
    }

    public void Create()
    {
        StartCoroutine(CaptureImage());
    }


    IEnumerator CaptureImage()
    {
        yield return null;

        Texture2D tex = new Texture2D(rt.width, rt.height, TextureFormat.ARGB32, false, true);
        RenderTexture.active = rt;
        tex.ReadPixels(new Rect(0, 0, rt.width, rt.height), 0, 0);
        yield return null;

        var data = tex.EncodeToPNG();
        string name = "Thumbnail";
        string extention = ".png";
        string path = Application.persistentDataPath + "/Thumbnail/";

        Debug.Log(path);

        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }

        File.WriteAllBytes(path + name + extention, data);
        yield return null;

    }

    public void AllCreate() 
    {
        StartCoroutine(AllCaptureImage());
    }

    IEnumerator AllCaptureImage()
    {
        while (nowCnt < obj.Length)
        {
            var nowObj = Instantiate(obj[nowCnt].gameObject);

            AdjustCameraDistance(nowObj);

            yield return null;

            Texture2D tex = new Texture2D(rt.width, rt.height, TextureFormat.ARGB32, false, true);
            RenderTexture.active = rt;
            tex.ReadPixels(new Rect(0, 0, rt.width, rt.height), 0, 0);

            yield return null;

            var data = tex.EncodeToPNG();
            string name = $"Thumbnail_{obj[nowCnt].gameObject.name}";
            string extention = ".png";
            string path = Application.persistentDataPath + "/Thumbnail/";

            Debug.Log(path);

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            File.WriteAllBytes(path + name + extention, data);

            yield return null;

            DestroyImmediate(nowObj);
            nowCnt++;

            yield return null;

        }
    }

    //void AdjustCameraDistance(GameObject obj)
    //{
    //    // 오브젝트의 크기를 기준으로 카메라와의 거리 조정
    //    Renderer objRenderer = obj.GetComponent<Renderer>();
    //    if (objRenderer != null)
    //    {
    //        // 오브젝트의 경계 박스를 가져온다
    //        Bounds bounds = objRenderer.bounds;
    //        float objectHeight = bounds.size.y; // 오브젝트의 높이

    //        // 카메라의 FOV와 비율에 따라 거리 계산
    //        float distance = objectHeight / (2.0f * Mathf.Tan(cam.fieldOfView * 0.5f * Mathf.Deg2Rad));

    //        // 카메라의 위치 조정
    //        cam.transform.position = new Vector3(cam.transform.position.x, cam.transform.position.y, -distance);
    //        cam.transform.LookAt(bounds.center); // 오브젝트를 바라보도록 설정
    //    }
    //}
    void AdjustCameraDistance(GameObject obj)
    {
        Renderer objRenderer = obj.GetComponent<Renderer>();
        if (objRenderer != null)
        {
            // 오브젝트의 경계 박스를 가져온다
            Bounds bounds = objRenderer.bounds;
            float objectHeight = bounds.size.y; // 오브젝트의 높이
            float objectWidth = bounds.size.x; // 오브젝트의 너비

            // 카메라의 FOV와 비율에 따라 거리 계산
            float distance = (objectHeight / 2) / Mathf.Tan(cam.fieldOfView * 0.5f * Mathf.Deg2Rad);

            // 오브젝트의 중앙을 기준으로 카메라 위치 설정
            Vector3 targetPosition = bounds.center;

            // 오브젝트의 너비를 고려하여 추가 거리 계산
            float aspectRatio = (float)Screen.width / (float)Screen.height;
            float horizontalDistance = (objectWidth / 2) / Mathf.Tan(cam.fieldOfView * 0.5f * Mathf.Deg2Rad) * aspectRatio;

            // 카메라의 Z 위치 조정
            float finalDistance = Mathf.Max(distance, horizontalDistance);

            // 여유 공간을 추가 (여유 공간 비율 조정)
            float padding = objectHeight * 1.5f; // 여유 공간을 더 넉넉하게 설정
            cam.transform.position = new Vector3(targetPosition.x, targetPosition.y, targetPosition.z - finalDistance - padding); // 카메라를 좀 더 뒤로 이동
            cam.transform.LookAt(targetPosition); // 오브젝트를 바라보도록 설정
        }
    }


    void SettingColor()
    {
        switch (grade)
        {
            case Grade.Null:
                //투명하게
                cam.clearFlags = CameraClearFlags.Color;
                cam.backgroundColor = new Color(1, 1, 1, 0);
                bg.color = new Color(1, 1, 1, 0);
                break;
            case Grade.Normal:
                cam.clearFlags = CameraClearFlags.SolidColor;
                cam.backgroundColor = Color.white;
                bg.color = Color.white;
                break;
            case Grade.Uncommon:
                cam.clearFlags = CameraClearFlags.SolidColor;
                cam.backgroundColor = Color.green;
                bg.color = Color.green;
                break;
            case Grade.Rare:
                cam.clearFlags = CameraClearFlags.SolidColor;
                cam.backgroundColor = Color.blue;
                bg.color = Color.blue;
                break;
            case Grade.Legend:
                cam.clearFlags = CameraClearFlags.SolidColor;
                cam.backgroundColor = Color.yellow;
                bg.color = Color.yellow;
                break;
        }
    }

    void SettingSize()
    {
        switch (size)
        {
            case Size.POT64:
                rt.width = 64;
                rt.height = 64;
                break;
            case Size.POT128:
                rt.width = 128;
                rt.height = 128;
                break;
            case Size.POT256:
                rt.width = 256;
                rt.height = 256;
                break;
            case Size.POT512:
                rt.width = 512;
                rt.height = 512;
                break;
            case Size.POT1024:
                rt.width = 1024;
                rt.height = 1024;
                break;
        }
    }
}
