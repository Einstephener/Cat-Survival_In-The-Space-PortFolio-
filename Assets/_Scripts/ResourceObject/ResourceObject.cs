using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceObject : MonoBehaviour
{
    public ItemData itemData;
    public int quantityPerHit = 1;
    public int capacity;

    public void Gather(Vector3 hitPoint, Vector3 hitNormal)
    {
        Debug.Log("Test");
        for (int i = 0; i < quantityPerHit; i++)
        {
            if (capacity <= 0) 
            { 
                break; 
            }
            capacity -= 1;
            Instantiate(itemData.DropPrefab, hitPoint + Vector3.up, Quaternion.LookRotation(hitNormal, Vector3.up));

        }
        if (capacity <= 0)
        {
            Destroy(gameObject);
        }
    }

    //public void Fishing(Vector3 hitPoint)
    //{
    //    for (int i = 0; i < quantityPerHit; i++)
    //    {
    //        if (capacity <= 0) { break; }
    //        capacity -= 1;
    //        GameObject fish = Instantiate(itemData.DropPrefab, hitPoint, Quaternion.LookRotation(playerInputController.instance.transform.position, Vector3.up));
    //    }

    //}
}
