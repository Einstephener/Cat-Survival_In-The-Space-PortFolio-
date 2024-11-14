using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreviewObject : MonoBehaviour
{
    //출돌한 오브젝트의 콜라이더
    public List<Collider> colliderList = new();

    public LayerMask layers; // 레이어 마스크

    //[SerializeField]
    //private int layerGround; // 지상레이어
    //[SerializeField]
    //private int layerNoNatureZone;
    //[SerializeField]
    //private int layerDefault;

    //private const int IGNORE_RAYCAST_LAYER = 2; // 기본값  2 

    [Header("#Materials")]
    [SerializeField]
    private Material green;
    [SerializeField]
    private Material red;


    private void Update()
    {
        ChageColoerUpdate();
    }

    #region Color
    private void ChageColoerUpdate()
    {
        if (colliderList.Count > 0)
        {
            //red
            SetColor(red);
        }
        else
        {
            //green
            SetColor(green);
        }
    }

    private void SetColor(Material material)
    {
        MeshRenderer meshRenderer = GetComponent<MeshRenderer>();

        if (meshRenderer != null)
        {
            // Materials 배열에 접근하기
            Material[] materials = meshRenderer.materials;

            // 모든 메터리얼의 색상을 변경
            for (int i = 0; i < materials.Length; i++)
            {
                materials[i].color = material.color; // 전달받은 메터리얼의 색상으로 변경
            }

            // 변경된 materials 배열을 다시 할당
            meshRenderer.materials = materials;
        }
    }

    #endregion

    private bool IsLayerInMask(int layer)
    {
        return (layers & (1 << layer)) != 0;
    }

    #region Trigger 
    private void OnTriggerEnter(Collider other)
    {
        
        if (!IsLayerInMask(other.gameObject.layer))
        {
            colliderList.Add(other);
        }

    }

    private void OnTriggerExit(Collider other)
    {
        colliderList.Remove(other);
    }
    #endregion

    public bool IsBuildeble()
    {
        return colliderList.Count == 0;
    }
}
