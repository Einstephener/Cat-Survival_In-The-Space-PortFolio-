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
}
