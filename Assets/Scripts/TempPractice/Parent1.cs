using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parent1 : MonoBehaviour
{
    public virtual void DebugPrintFunc()
    {
        Debug.Log("Parnet Print");
        Die(); // �ڽĿ� �������̵� �� ���� ������ �ڽ��� Dieȣ��
    }

    public virtual void Die()
    {
        Debug.Log("Parent Die");
    }
}
