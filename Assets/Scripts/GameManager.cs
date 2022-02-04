using Photon.Pun;
using UnityEngine;
using UnityEngine.SceneManagement;

// 점수와 게임 오버 여부를 관리하는 게임 매니저
public class GameManager : MonoBehaviourPunCallbacks, IPunObservable
{
    // 싱글톤 접근용 프로퍼티
    public static GameManager Instance
    {
        get
        {
            // 만약 싱글톤 변수에 아직 오브젝트가 할당되지 않았다면
            if (_instance == null)
            {
                // 씬에서 GameManager 오브젝트를 찾아 할당
                _instance = FindObjectOfType<GameManager>();
            }

            // 싱글톤 오브젝트를 반환
            return _instance;
        }
    }

    private static GameManager _instance; // 싱글톤이 할당될 static 변수

    private int _score = 0; // 현재 게임 점수
    public bool IsGameover { get; private set; } // 게임 오버 상태

    [SerializeField]
    private GameObject PlayerPrefab;

    private void Awake()
    {
        // 씬에 싱글톤 오브젝트가 된 다른 GameManager 오브젝트가 있다면
        if (Instance != this)
        {
            // 자신을 파괴
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        Vector3 randomSpawnPos = Random.insideUnitSphere * 5f;
        randomSpawnPos.y = 0f;

        /*GameObject player = */PhotonNetwork.Instantiate(PlayerPrefab.name, randomSpawnPos, Quaternion.identity);

        // 플레이어 캐릭터의 사망 이벤트 발생시 게임 오버
        //FindObjectOfType<PlayerHealth>().OnDeath += EndGame;
    }
    
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            PhotonNetwork.LeaveRoom(); // 방 나가기
        }
    }

    public override void OnLeftRoom()
    {
        SceneManager.LoadScene((int)ESceneID.Lobby);
    }

    // 점수를 추가하고 UI 갱신
    public void AddScore(int newScore)
    {
        // 게임 오버가 아닌 상태에서만 점수 증가 가능
        if (IsGameover == false)
        {
            // 점수 추가
            _score += newScore;
            // 점수 UI 텍스트 갱신
            UIManager.Instance.UpdateScoreText(_score);
        }
    }

    // 게임 오버 처리
    public void EndGame()
    {
        // 게임 오버 상태를 참으로 변경
        IsGameover = true;
        // 게임 오버 UI를 활성화
        UIManager.Instance.SetActiveGameoverUi(true);
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if(stream.IsWriting)
        {
            stream.SendNext(_score);
        }
        else
        {
            _score = (int)stream.ReceiveNext();
            UIManager.Instance.UpdateScoreText(_score);
        }
    }
}