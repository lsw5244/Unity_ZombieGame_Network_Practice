using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parent1 : MonoBehaviour
{
    public virtual void DebugPrintFunc()
    {
        Debug.Log("Parnet Print");
        Die(); // 자식에 오버라이드 된 것이 있으면 자식의 Die호출
    }

    public virtual void Die()
    {
        Debug.Log("Parent Die");
    }
}
