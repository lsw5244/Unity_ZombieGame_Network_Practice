using Photon.Pun;
using UnityEngine;

// 플레이어 캐릭터를 조작하기 위한 사용자 입력을 감지
// 감지된 입력값을 다른 컴포넌트들이 사용할 수 있도록 제공
public class PlayerInput : MonoBehaviourPun
{
    public string moveAxisName = "Vertical"; // 앞뒤 움직임을 위한 입력축 이름
    public string rotateAxisName = "Horizontal"; // 좌우 회전을 위한 입력축 이름
    public string fireButtonName = "Fire1"; // 발사를 위한 입력 버튼 이름
    public string reloadButtonName = "Reload"; // 재장전을 위한 입력 버튼 이름

    // 값 할당은 내부에서만 가능
    public float moveInput { get; private set; } // 감지된 움직임 입력값
    public float rotateInput { get; private set; } // 감지된 회전 입력값
    public bool CanFire { get; private set; } // 감지된 발사 입력값
    public bool CanReload { get; private set; } // 감지된 재장전 입력값

    private const float MOVE_SCALE = 1f / 3f;

    // 매프레임 사용자 입력을 감지
    private void Update()
    {
        if (false == photonView.IsMine)
        {
            return;
        }
        // 게임오버 상태에서는 사용자 입력을 감지하지 않는다
        if (/*GameManager.instance != null && */GameManager.Instance.IsGameover)
        {
            moveInput = 0;
            rotateInput = 0;
            CanFire = false;
            CanReload = false;
            return;
        }

        moveInput = Input.GetAxis(moveAxisName);
        rotateInput = Input.GetAxis(rotateAxisName);
        CanFire = Input.GetButton(fireButtonName);
        CanReload = Input.GetButtonDown(reloadButtonName);
    }
}