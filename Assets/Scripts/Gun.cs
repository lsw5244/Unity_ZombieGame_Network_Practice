using Photon.Pun;
using System.Collections;
using UnityEngine;

// 총을 구현한다
public class Gun : MonoBehaviourPun, IPunObservable
{
    // 총의 상태를 표현하는데 사용할 타입을 선언한다
    public enum EState
    {
        Ready, // 발사 준비됨
        Empty, // 탄창이 빔
        Reloading // 재장전 중
    }


    public Transform fireTransform; // 총알이 발사될 위치

    public ParticleSystem muzzleFlashEffect; // 총구 화염 효과
    public ParticleSystem shellEjectEffect; // 탄피 배출 효과


    public AudioClip shotClip; // 발사 소리
    public AudioClip reloadClip; // 재장전 소리
    private AudioSource gunAudioPlayer; // 총 소리 재생기
    private LineRenderer bulletLineRenderer; // 총알 궤적을 그리기 위한 렌더러

    public float damage = 25; // 공격력
    private float fireDistance = 50f; // 사정거리

    public int RemainedAmmo = 100; // 남은 전체 탄약
    public int magCapacity = 25; // 탄창 용량
    public int MagAmmo; // 현재 탄창에 남아있는 탄약

    public float fireCollTime = 0.12f; // 총알 발사 간격
    public float reloadTime = 1.8f; // 재장전 소요 시간
    private float lastFireTime; // 총을 마지막으로 발사한 시점

    public EState CurrState { get; private set; } // 현재 총의 상태

    private void Awake()
    {
        // 사용할 컴포넌트들의 참조를 가져오기
        bulletLineRenderer = GetComponent<LineRenderer>();
        bulletLineRenderer.positionCount = 2;
        bulletLineRenderer.enabled = false;
        //bulletLineRenderer.SetPosition(0, fireTransform.position);

        gunAudioPlayer = GetComponent<AudioSource>();
    }

    private void OnEnable()
    {
        // 총 상태 초기화
        MagAmmo = magCapacity;

        CurrState = EState.Ready;
        
        lastFireTime = 0f;
    }
    // 현제 자신의 상태 동기화 시켜주기?
    // IPunObservable에 정의되어있음(매 초 한번씩 실행 ?)
    // 데이터 직렬, 역직렬화 (데이터 전송을 위한 기법, 바이트 열로 나누는 것 == 데이터 넘겨주기)
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if(stream.IsWriting)
        {
            stream.SendNext(RemainedAmmo);
            stream.SendNext(MagAmmo);
            stream.SendNext(CurrState);
        }
        else
        {
            RemainedAmmo = (int)stream.ReceiveNext();
            MagAmmo = (int)stream.ReceiveNext();
            CurrState = (EState)stream.ReceiveNext();
        }
    }

    [PunRPC]
    public void AddAmmo(int amount)
    {
        RemainedAmmo += amount;
    }

    // 발사 시도
    public void Fire()
    {
        if(CurrState == EState.Ready && Time.time >= lastFireTime + fireCollTime)
        {
            lastFireTime = Time.time;
            Shot();
        }
    }

    // 실제 발사 처리
    private void Shot()
    {
        photonView.RPC("checkHit", RpcTarget.MasterClient); // 총말 맞았는지 체크를 마스터클라이언트(서버)에 확인 부탁

        --MagAmmo;

        if(MagAmmo <= 0)
        {
            CurrState = EState.Empty;
        }
        //bulletLineRenderer.SetPosition(0, fireTransform.position);
        //RaycastHit hit; // 레이와 충돌한 객체가 담길 공간
        //// 충돌 안했을 때 기본 충돌 포인트 설정
        //Vector3 hitPosition = fireTransform.position + fireTransform.forward * fireDistance;

        //// 시작지점, 방향, 함수에 의해 결과값이 담길 것(아웃 파라미터 ), 레이 길이
        //if (Physics.Raycast(fireTransform.position, fireTransform.forward, out hit, fireDistance))
        //{
        //    // 충돌 처리
        //    hit.collider.GetComponent<IDamageable>()?.TakeDamage(damage, hit.point, hit.normal);

        //    hitPosition = hit.point;
        //}
        
        //StartCoroutine(PlayShotEffect(hitPosition));

        //--magAmmo;

        //if(magAmmo <= 0)
        //{
        //    CurrState = EState.Empty;
        //}
    }
    
    [PunRPC]
    private void checkHit()
    {
        // 충돌 안했을 때 기본 충돌 포인트 설정
        Vector3 hitPosition = fireTransform.position + fireTransform.forward * fireDistance;
        RaycastHit hit; // 레이와 충돌한 객체가 담길 공간

        // 시작지점, 방향, 함수에 의해 결과값이 담길 것(아웃 파라미터 ), 레이 길이
        if (Physics.Raycast(fireTransform.position, fireTransform.forward, out hit, fireDistance))
        {
            // 충돌 처리
            hit.collider.GetComponent<IDamageable>()?.TakeDamage(damage, hit.point, hit.normal);

            hitPosition = hit.point;
        }
        photonView.RPC("processShotEffect", RpcTarget.All, hitPosition);
    }
    [PunRPC]
    private void processShotEffect(Vector3 hitPosition)
    {
        StartCoroutine("PlayShotEffect", hitPosition);
    }

    // 발사 이펙트와 소리를 재생하고 총알 궤적을 그린다
    private IEnumerator PlayShotEffect(Vector3 hitPosition)
    {
        muzzleFlashEffect.Play();

        shellEjectEffect.Play();

        gunAudioPlayer.PlayOneShot(shotClip);   // 해당 클립 한 번 재생

        //bulletLineRenderer.SetPosition(0, fireTransform.position);
        bulletLineRenderer.SetPosition(1, hitPosition);

        // 라인 렌더러를 활성화하여 총알 궤적을 그린다
        bulletLineRenderer.enabled = true;

        // 0.03초 동안 잠시 처리를 대기
        yield return new WaitForSeconds(0.03f);

        // 라인 렌더러를 비활성화하여 총알 궤적을 지운다
        bulletLineRenderer.enabled = false;
    }

    // 재장전 시도
    public bool HasReload()
    {
        if(CurrState == EState.Reloading || RemainedAmmo <= 0 || MagAmmo >= magCapacity)
        {
            return false;
        }

        StartCoroutine(reloadHelper());

        return true;
    }

    // 실제 재장전 처리를 진행
    private IEnumerator reloadHelper()
    {
        // 현재 상태를 재장전 중 상태로 전환
        CurrState = EState.Reloading;

        gunAudioPlayer.PlayOneShot(reloadClip);

        // 재장전 소요 시간 만큼 처리를 쉬기
        yield return new WaitForSeconds(reloadTime);

        int ammoToFill = magCapacity - MagAmmo;

        if(RemainedAmmo < ammoToFill)
        {
            ammoToFill = RemainedAmmo;
        }

        MagAmmo += ammoToFill;

        RemainedAmmo -= ammoToFill;

        //state = State.Ready;

        // 총의 현재 상태를 발사 준비된 상태로 변경
        CurrState = EState.Ready;
    }
}