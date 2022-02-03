using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCam : MonoBehaviour
{
    public Transform Target;

    private Vector3 distance;
    void Start()
    {
        distance = Target.position - transform.position;
    }

    void LateUpdate()
    {
        // 카메라의 위치를 타겟으로부터 일정 거리 떨어지게 한다.
        transform.position = Target.position - distance;
    }
}
