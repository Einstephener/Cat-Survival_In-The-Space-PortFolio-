using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public void RotateTo(float _eulerAngleX)
    {
        // 실제 카메라의 Quaternion 회전에 적용.
        transform.localRotation = Quaternion.Euler(_eulerAngleX, 0, 0);
    }

    public void SitSightChange(bool isSit)
    {
        if (isSit)
        {
            transform.localPosition = new Vector3(0, -1f, 0);
        }
        else
        {
            transform.localPosition = new Vector3(0, 0, 0);
        }
    }
}
