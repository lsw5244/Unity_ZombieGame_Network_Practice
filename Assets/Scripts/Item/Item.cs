using Photon.Pun;
using UnityEngine;

public abstract class Item : MonoBehaviourPun
{
    public void Use(GameObject target)
    {
        useHelper(target);
        PhotonNetwork.Destroy(gameObject);
    }

    protected abstract void useHelper(GameObject target);
}


