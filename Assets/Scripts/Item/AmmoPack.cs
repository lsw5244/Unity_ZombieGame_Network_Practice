using UnityEngine;
using Photon.Pun;
// 총알을 충전하는 아이템
public class AmmoPack : Item
{
    public int Ammo = 30; // 충전할 총알 수

    protected override void useHelper(GameObject target)
    {
        // 전달 받은 게임 오브젝트로부터 PlayerShooter 컴포넌트를 가져오기 시도
        PlayerShooter playerShooter = target.GetComponent<PlayerShooter>();

        // PlayerShooter 컴포넌트가 있으며, 총 오브젝트가 존재하면
        //if (playerShooter != null && playerShooter.Gun != null)
        {
            // 총의 남은 탄환 수를 ammo 만큼 더한다
            playerShooter.Gun.RemainedAmmo += Ammo;
            playerShooter.Gun.photonView.RPC("AddAmmo", RpcTarget.All, Ammo);
        }

        // 사용되었으므로, 자신을 파괴
        //Destroy(gameObject);
    }
}