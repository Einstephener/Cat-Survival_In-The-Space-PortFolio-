using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private float _sitSight = .8f;
    private float _idleSight = 1.8f;
    [SerializeField] GameObject PlayerHead;

    public void RotateTo(float eulerAngleX)
    {
        // 실제 카메라의 Quaternion 회전에 적용.
        transform.localRotation = Quaternion.Euler(eulerAngleX, 0, 0);
    }


    public void SitSightChange(bool isSit)
    {
        float y = isSit ? _sitSight : _idleSight;
        transform.localPosition = new Vector3(0, y, 0.4f);
    }
}
