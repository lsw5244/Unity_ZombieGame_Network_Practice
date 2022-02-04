using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCam : MonoBehaviour
{
    private Transform _target;

    private Vector3 distance;

    public void SetTarget(Transform transform)
    {
        _target = transform;

        distance = _target.position - transform.position;
    }

    void LateUpdate()
    {
        // 카메라의 위치를 타겟으로부터 일정 거리 떨어지게 한다.
        transform.position = _target.position - distance;
    }
}
