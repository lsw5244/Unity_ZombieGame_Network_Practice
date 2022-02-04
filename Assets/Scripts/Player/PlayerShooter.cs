using UnityEngine;

// 주어진 Gun 오브젝트를 쏘거나 재장전
// 알맞은 애니메이션을 재생하고 IK를 사용해 캐릭터 양손이 총에 위치하도록 조정
public class PlayerShooter : MonoBehaviour
{
    public Gun Gun; // 사용할 총
    public Transform GunPivot; // 총 배치의 기준점
    public Transform LeftHandMount; // 총의 왼쪽 손잡이, 왼손이 위치할 지점
    public Transform RightHandMount; // 총의 오른쪽 손잡이, 오른손이 위치할 지점

    private PlayerInput _playerInput; // 플레이어의 입력
    private Animator _playerAnimator; // 애니메이터 컴포넌트

    private void Start()
    {
        // 사용할 컴포넌트들을 가져오기
        _playerInput = GetComponent<PlayerInput>();
        _playerAnimator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        // 슈터가 활성화될 때 총도 함께 활성화
        Gun.gameObject.SetActive(true);
    }

    private void OnDisable()
    {
        // 슈터가 비활성화될 때 총도 함께 비활성화
        Gun.gameObject.SetActive(false);
    }

    private void Update()
    {
        // 입력을 감지하고 총 발사하거나 재장전
        if(_playerInput.CanFire)
        {
            Camera.main.ScreenPointToRay(Input.mousePosition);

            Gun.Fire();
        }
        else if(_playerInput.CanReload)
        {
            if(Gun.HasReload())
            {
                _playerAnimator.SetTrigger(PlayerAnimID.RELOAD/* AnimationID.RELOAD*/);
            }
        }
        UpdateUI();
    }

    // 탄약 UI 갱신
    private void UpdateUI()
    {
        //if (Gun != null && UIManager.Instance != null)
        {
            // UI 매니저의 탄약 텍스트에 탄창의 탄약과 남은 전체 탄약을 표시
            UIManager.Instance.UpdateAmmoText(Gun.magAmmo, Gun.ammoRemain);
        }
    }

    // 애니메이터의 IK 갱신
    private void OnAnimatorIK(int layerIndex)
    {
        // 총 위치 대충 맞춰줌
        GunPivot.position = _playerAnimator.GetIKHintPosition(AvatarIKHint.RightElbow);
        // 손 위치 맞추기
        setIkTransform(AvatarIKGoal.LeftHand, LeftHandMount);
        setIkTransform(AvatarIKGoal.RightHand, RightHandMount);
    }
    // 목표지점? 적용 할 애니메이션 ?, 실제로 옮길 곳
    private void setIkTransform(AvatarIKGoal goal, Transform golaTransfrom, float weight = 1f)
    {
        // 가중치를 두고 계산 ( 기존 움직임 대비 얼마나 움직(수정될것)일건지 ) == 원래 애니메이션에서 움직일 거리와 목표로 하는 곳으로 움직일 거리의 차이 ?
        _playerAnimator.SetIKPositionWeight(goal, weight);
        _playerAnimator.SetIKRotationWeight(goal, weight);
        // 손의 포지션 움직이기
        _playerAnimator.SetIKPosition(goal, golaTransfrom.position);
        _playerAnimator.SetIKRotation(goal, golaTransfrom.rotation);
    }
}