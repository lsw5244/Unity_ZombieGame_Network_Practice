using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cil1 : Parent1
{

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButton(2))
        {
            DebugPrintFunc();
        }
    }

    public override void DebugPrintFunc()
    {
        Debug.Log("Child Print");
        base.DebugPrintFunc();
    }

    public override void Die()
    {
        //base.Die();
        Debug.Log("Child Die");
        base.Die();
    }
}
