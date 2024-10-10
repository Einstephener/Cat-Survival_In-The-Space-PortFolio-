using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectMatertial : MonoBehaviour
{
    private int curCount;
    public int maxCount;
    public GameObject MaterialPrefab;

    private void OnEnable()
    {
        curCount = maxCount;
    }
    public void SpitMaterial()
    {
        if (curCount > 0)
        {
            int a = Random.Range(1, 4);
            Vector3 plusPosition = new Vector3(0, 1, 0);

            switch (a)
            {
                case 1:
                    plusPosition = new Vector3(0, 1, 1);
                    break;

                case 2:
                    plusPosition = new Vector3(0, 1, -1);
                    break;

                case 3:
                    plusPosition = new Vector3(1, 1, 0);
                    break;

                case 4:
                    plusPosition = new Vector3(-1, 1, 0);
                    break;
            }

            Instantiate(MaterialPrefab,transform.position + plusPosition, Quaternion.identity);

            curCount--;
            Debug.Log(curCount);
            if(curCount == 0)
            {
                Destroy(this.gameObject);
            }
        }
    }


}
