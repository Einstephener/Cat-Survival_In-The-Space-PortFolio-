using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PreviewObjectEditor : Editor
{
    public override void OnInspectorGUI()
    {
        PreviewObject previewObject = (PreviewObject)target;

        // 레이어 마스크 필드 추가 (Culling Mask와 유사)
        previewObject.layers = EditorGUILayout.MaskField("Layers", previewObject.layers, GetLayerNames());

        // 기본 인스펙터 필드 그리기
        DrawDefaultInspector();
    }

    private string[] GetLayerNames()
    {
        // 레이어 이름을 배열로 반환
        string[] layerNames = new string[32];
        for (int i = 0; i < 32; i++)
        {
            layerNames[i] = LayerMask.LayerToName(i);
        }
        return layerNames;
    }
}
