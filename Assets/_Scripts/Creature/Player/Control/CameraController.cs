using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private float _limitMinX = -80;
    private float _limitMaxX = 50;

    public void RotateTo(float _eulerAngleY, float _eulerAngleX)
    {
        // x축 회전 값 제한 각도.
        _eulerAngleX = ClampAngle(_eulerAngleX, _limitMinX, _limitMaxX);

        // 실제 카메라의 Quaternion 회전에 적용.
        transform.rotation = Quaternion.Euler(_eulerAngleX, _eulerAngleY, 0);
    }

    private float ClampAngle(float angle, float min, float max)
    {
        if (angle < -360) angle += 360;
        if (angle > 360) angle -= 360;

        // 각도가 min <= angle <= max을 유지하도록 범위 제한.
        return Mathf.Clamp(angle, min, max);
    }
}
