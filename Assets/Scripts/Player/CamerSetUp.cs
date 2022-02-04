using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamerSetUp : MonoBehaviourPun
{
    private Camera _mainCamera;
    // Start is called before the first frame update
    void Start()
    {
        if(photonView.IsMine)   // 내거일때만 ?
        {
            Camera.main.GetComponent<FollowCam>().SetTarget(transform);
        }
    }
}
