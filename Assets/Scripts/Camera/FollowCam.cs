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
        // ī�޶��� ��ġ�� Ÿ�����κ��� ���� �Ÿ� �������� �Ѵ�.
        transform.position = _target.position - distance;
    }
}
