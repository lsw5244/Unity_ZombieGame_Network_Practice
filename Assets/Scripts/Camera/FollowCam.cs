using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCam : MonoBehaviour
{
    private Transform _target;

    private Vector3 distance;

    private bool IsOn = false;

    private void Awake()
    {
        distance = -transform.position;
    }

    public void SetTarget(Transform target)
    {
        _target = target;

        //distance = _target.position - transform.position;

        IsOn = true;
    }

    void LateUpdate()
    {
        if(IsOn == true)
        {
            // 카메라의 위치를 타겟으로부터 일정 거리 떨어지게 한다.
            transform.position = _target.position - distance;

        }
    }
}
