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
        // ī�޶��� ��ġ�� Ÿ�����κ��� ���� �Ÿ� �������� �Ѵ�.
        transform.position = Target.position - distance;
    }
}
