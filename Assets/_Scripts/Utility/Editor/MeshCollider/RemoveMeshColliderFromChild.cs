using UnityEngine;
using UnityEditor;

public class RemoveMeshColliderFromChild : EditorWindow
{
    [MenuItem("Tools/Remove Mesh Colliders from Child Objects")]
    public static void RemoveMeshColliders()
    {
        // 여러 개의 프리팹을 선택할 수 있도록 설정
        Object[] selectedObjects = Selection.objects;

        foreach (Object obj in selectedObjects)
        {
            // 선택된 오브젝트가 프리팹 에셋인지 확인
            GameObject prefab = obj as GameObject;

            if (prefab != null)
            {
                // Prefab 에셋에 접근하여 Prefab Instance를 편집
                string prefabPath = AssetDatabase.GetAssetPath(prefab);
                GameObject prefabRoot = PrefabUtility.LoadPrefabContents(prefabPath);

                // 프리팹의 모든 자식 오브젝트를 순회하여 Mesh Collider 삭제
                MeshCollider[] meshColliders = prefabRoot.GetComponentsInChildren<MeshCollider>();

                foreach (MeshCollider meshCollider in meshColliders)
                {
                    DestroyImmediate(meshCollider, true);
                }

                // Prefab 저장 및 Unload
                PrefabUtility.SaveAsPrefabAsset(prefabRoot, prefabPath);
                PrefabUtility.UnloadPrefabContents(prefabRoot);
            }
        }

        Debug.Log("Mesh Colliders have been removed from the child objects of selected prefabs.");
    }
}
