using UnityEngine;
using UnityEditor;

public class AddMeshColliderToChild : EditorWindow
{
    [MenuItem("Tools/Add Mesh Colliders with Convex to Child Objects")]
    public static void AddMeshColliders()
    {
        // 여러 개의 프리팹을 선택할 수 있도록 설정
        Object[] selectedObjects = Selection.objects;

        foreach (Object obj in selectedObjects)
        {
            // 선택된 오브젝트가 프리팹일 경우만 처리
            GameObject prefab = obj as GameObject;

            if (prefab != null)
            {
                // 프리팹의 모든 자식 오브젝트를 순회
                MeshRenderer[] meshRenderers = prefab.GetComponentsInChildren<MeshRenderer>();

                foreach (MeshRenderer meshRenderer in meshRenderers)
                {
                    GameObject childObj = meshRenderer.gameObject;

                    // 자식 오브젝트에 Mesh Collider가 없으면 추가
                    MeshCollider meshCollider = childObj.GetComponent<MeshCollider>();
                    if (meshCollider == null)
                    {
                        meshCollider = childObj.AddComponent<MeshCollider>();
                    }

                    // Convex 옵션을 체크
                    meshCollider.convex = true;
                }

                // 프리팹 저장
                PrefabUtility.SavePrefabAsset(prefab);
            }
        }

        Debug.Log("Mesh Colliders with Convex option have been added to the child objects of selected prefabs.");
    }
}
