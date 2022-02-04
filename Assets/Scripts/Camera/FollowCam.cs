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
            // ī�޶��� ��ġ�� Ÿ�����κ��� ���� �Ÿ� �������� �Ѵ�.
            transform.position = _target.position - distance;

        }
    }
}
