using Photon.Pun;
using System;
using UnityEngine;
using UnityEngine.UI;
// 생명체로서 동작할 게임 오브젝트들을 위한 뼈대를 제공
// 체력, 데미지 받아들이기, 사망 기능, 사망 이벤트를 제공
public class LivingEntity : MonoBehaviourPun, IDamageable
{
    public Slider HealthSlider; // 체력을 표시할 UI 슬라이더

    public float StartingHealth = 100f; // 시작 체력
    public float CurrentHealth {
        get { return _health; }
        protected set
        {
            _health = value;

            if(HealthSlider != null)
            {
                HealthSlider.value = _health;
            }
        }
    } // 현재 체력
    public bool IsDead { get; protected set; } // 사망 상태
    public event Action OnDeath; // 사망시 발동할 이벤트     // Action은 반환형 void 반환형 x

    private float _health;

    [PunRPC] // 서버에 의해서 호출될 수 있음 (특성)
    public void ApplyUpdatedHealth(float newHelath, bool isDead)
    {
        _health = newHelath;
        IsDead = IsDead;
    }
    // 생명체가 활성화될때 상태를 리셋
    protected virtual void OnEnable()
    {
        if(HealthSlider != null)
        {
            HealthSlider.gameObject.SetActive(true);
            HealthSlider.maxValue = StartingHealth;
        }
        // 사망하지 않은 상태로 시작
        IsDead = false;
        // 체력을 시작 체력으로 초기화
        CurrentHealth = StartingHealth;
    }

    [PunRPC]// 데미지를 입는 기능
    public virtual void TakeDamage(float damage, Vector3 hitPoint, Vector3 hitNormal)
    {
        // 마스터 컴퓨터에서 데미지 연산 후 다른 컴퓨터들에게도 연산하라고 신호 보내줌
        if(PhotonNetwork.IsMasterClient) // 방 주인인지 확인( 방 연사람 )
        {
            // 데미지만큼 체력 감소
            CurrentHealth -= damage;
            // 나머지 컴퓨터에서도 health와 isDead의 정보를 업데이트 하도록 ApplyUpdatedHealth호출시킴
            photonView.RPC("ApplyUpdatedHealth", RpcTarget.Others, CurrentHealth, IsDead); 
            // 데미지를 입은 뒤 사망 처리를 실행하도록 함
            photonView.RPC("TakeDamage", RpcTarget.Others, damage, hitPoint, hitNormal);
        }

        // 체력이 0 이하 && 아직 죽지 않았다면 사망 처리 실행
        if (CurrentHealth <= 0 && IsDead == false)
        {
            Die();
        }
    }

    [PunRPC]// 체력을 회복하는 기능
    public virtual void RestoreHealth(float newHealth)
    {
        if (IsDead)
        {
            // 이미 사망한 경우 체력을 회복할 수 없음
            return;
        }
        if(PhotonNetwork.IsMasterClient)
        {
            // 체력 추가
            CurrentHealth += newHealth;

            photonView.RPC("ApplyUpdatedHealth", RpcTarget.Others, CurrentHealth, IsDead); // 나머지 애들도 실행하라고 매시지 보내기
            photonView.RPC("RestoreHealth", RpcTarget.Others, newHealth);
        }
    }

    // 사망 처리
    public virtual void Die()
    {
        // onDeath 이벤트에 등록된 메서드가 있다면 실행
        OnDeath?.Invoke();

        // 사망 상태를 참으로 변경
        IsDead = true;

        HealthSlider?.gameObject.SetActive(false);
    }
}