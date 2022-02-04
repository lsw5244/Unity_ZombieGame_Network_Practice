using UnityEngine;
using UnityEngine.UI; // UI 관련 코드
using Photon.Pun;
// 플레이어 캐릭터의 생명체로서의 동작을 담당
public class PlayerHealth : LivingEntity
{
    public AudioClip DeathClip; // 사망 소리
    public AudioClip HitClip; // 피격 소리
    public AudioClip ItemPickupClip; // 아이템 습득 소리

    private AudioSource _audioPlayer; // 플레이어 소리 재생기
    private Animator _animator; // 플레이어의 애니메이터

    private PlayerMovement _playerMovement; // 플레이어 움직임 컴포넌트
    private PlayerShooter _playerShooter; // 플레이어 슈터 컴포넌트

    private void Awake()
    {
        // 사용할 컴포넌트를 가져오기
        _audioPlayer = GetComponent<AudioSource>();
        _animator = GetComponent<Animator>();
        _playerMovement = GetComponent<PlayerMovement>();
        _playerShooter = GetComponent<PlayerShooter>();
    }

    protected override void OnEnable()
    {
        // LivingEntity의 OnEnable() 실행 (상태 초기화)

        //HealthSlider.gameObject.SetActive(true);
        //HealthSlider.maxValue = StartingHealth;
        //HealthSlider.value = CurrentHealth;
        
        base.OnEnable();

        _playerMovement.enabled = true;
        _playerShooter.enabled = true;
    }

    //// 체력 회복
    //public override void RestoreHealth(float newHealth)
    //{
    //    // LivingEntity의 RestoreHealth() 실행 (체력 증가)
    //    base.RestoreHealth(newHealth);

    //    //HealthSlider.value = CurrentHealth;
    //}

    // 데미지 처리
    [PunRPC]
    public override void TakeDamage(float damage, Vector3 hitPoint, Vector3 hitDirection)
    {
        // LivingEntity의 OnDamage() 실행(데미지 적용)
        if(IsDead == false)
        {
            _audioPlayer.PlayOneShot(HitClip);
            base.TakeDamage(damage, hitPoint, hitDirection);
        }
    }

    // 사망 처리
    public override void Die()
    {
        // LivingEntity의 Die() 실행(사망 적용)
        base.Die();

        _audioPlayer.PlayOneShot(DeathClip);

        _animator.SetTrigger(AnimationID.DIE);

        _playerMovement.enabled = false;
        _playerShooter.enabled = false;

        Invoke("Respawn", 5f);
    }

    private void OnTriggerEnter(Collider other)
    {
        // 아이템과 충돌한 경우 해당 아이템을 사용하는 처리
        if(IsDead == true)
        {
            return;
        }

        Item item = other.GetComponent<Item>();

        if(item != null)
        {
            if(PhotonNetwork.IsMasterClient)
            {
                item.Use(gameObject);
            }

            _audioPlayer.PlayOneShot(ItemPickupClip);
        }
    }

    public void Respawn()
    {
        if(photonView.IsMine)
        {
            Vector3 randomSpawnPos = Random.insideUnitSphere * 5f;
            randomSpawnPos.y = 0f;

            transform.position = randomSpawnPos;
        }

        gameObject.SetActive(false);
        gameObject.SetActive(true);
    }
}