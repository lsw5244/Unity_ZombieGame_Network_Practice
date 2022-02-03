using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempScript : MonoBehaviour
{

    private IEnumerator enumerator;
    void Start()
    {
        //enumerator = Fade().GetEnumerator();
        StartCoroutine(Fade());
    }

    void Update()
    {
        enumerator.MoveNext();
    }

    IEnumerator Fade()
    {
        for(int i = 0; i < 10; i++)
        {
            Debug.Log(i);
            yield return new WaitForSeconds(0.5f);
        }
    }
}
