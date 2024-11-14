using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private Coroutine _crouchCoroutine; // 코루틴 핸들
    private float _sitSight = .8f;
    private float _idleSight = 1.7f;
    
    public void RotateTo(float eulerAngleX)
    {
        // 실제 카메라의 Quaternion 회전에 적용.
        transform.localRotation = Quaternion.Euler(eulerAngleX, 0, 0);
    }
    public void SitSightChange(bool isSit)
    {
        // 현재 진행 중인 코루틴이 있다면 중단
        if (_crouchCoroutine != null)
        {
            StopCoroutine(_crouchCoroutine);
        }
        // 새로운 코루틴 실행
        _crouchCoroutine = StartCoroutine(CrouchCoroutine(isSit));
    }

    private IEnumerator CrouchCoroutine(bool isSit)
    {
        float applyCrouchPosY = isSit ? _sitSight : _idleSight;
        float _posY = transform.localPosition.y;
        int count = 0;

        while (Mathf.Abs(_posY - applyCrouchPosY) > 0.01f) // 위치 차이가 0.01 이상일 때만
        {
            count++;
            _posY = Mathf.Lerp(_posY, applyCrouchPosY, 0.5f);
            transform.localPosition = new Vector3(0, _posY, 0.6f);
            if (count > 15)
                break;
            yield return null;
        }
        transform.localPosition = new Vector3(0, applyCrouchPosY, 0.6f);

        // 코루틴이 끝나면 null로 초기화
        _crouchCoroutine = null;
    }


}
