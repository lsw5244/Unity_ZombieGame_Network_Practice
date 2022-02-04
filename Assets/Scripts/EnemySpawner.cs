using ExitGames.Client.Photon;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 적 게임 오브젝트를 주기적으로 생성
public class EnemySpawner : MonoBehaviourPun, IPunObservable
{
    public Enemy EnemyPrefab; // 생성할 적 AI
    public int MaxEnemyCount;

    public Transform[] SpawnPoints; // 적 AI를 소환할 위치들

    public float MaxDamage = 40f; // 최대 공격력
    public float MinDamage = 20f; // 최소 공격력

    public float MaxHealth = 200f; // 최대 체력
    public float MinHealth = 100f; // 최소 체력

    public float MaxSpeed = 3f; // 최대 속도
    public float MinSpeed = 1f; // 최소 속도

    public Color StrongEnemyColor = Color.red; // 강한 적 AI가 가지게 될 피부색

    //private List<Enemy> _enemies = new List<Enemy>(); // 생성된 적들을 담는 리스트
    private int _remainEnemyCount = 0;
    private int _wave = 0; // 현재 웨이브

    private int _pointCount;

    private Enemy[] _enemyPool;

    private void Awake()
    {
        _pointCount = SpawnPoints.Length;

        //_enemyPool = new Enemy[MaxEnemyCount];
        //for(int i = 0; i < MaxEnemyCount; ++i)
        //{
        //    _enemyPool[i] = Instantiate(EnemyPrefab);
        //}

        PhotonPeer.RegisterType(typeof(Color), 128, ColorSerializer.Serialize, ColorSerializer.Deserialize);
    }

    private void Update()
    {
        if(PhotonNetwork.IsMasterClient)
        {
            // 게임 오버 상태일때는 생성하지 않음
            if (/*GameManager.Instance != null && */GameManager.Instance.IsGameover)
            {
                return;
            }

            // 적을 모두 물리친 경우 다음 스폰 실행
            if (/*_enemies.Count*/_remainEnemyCount <= 0)
            {
                spawnWave();
            }
        }

        // UI 갱신
        updateUI();
    }

    // 웨이브 정보를 UI로 표시
    private void updateUI()
    {
        // 현재 웨이브와 남은 적의 수 표시
        //UIManager.Instance.UpdateWaveText(_wave, _enemies.Count);
        UIManager.Instance.UpdateWaveText(_wave, _remainEnemyCount);
    }

    // 현재 웨이브에 맞춰 적을 생성
    private void spawnWave()
    {
        ++_wave;

        _remainEnemyCount = Mathf.RoundToInt(_wave * 1.5f);

        for(int i = 0; i < _remainEnemyCount; ++i)
        {
            float enemyIntensity = Random.Range(0f, 1f);

            CreateEnemy(enemyIntensity);
        }

    }

    // 적을 생성하고 생성한 적에게 추적할 대상을 할당
    private void CreateEnemy(float intensity)
    {
        float health = Mathf.Lerp(MinHealth, MaxHealth, intensity); // min ~ max 의 intensity만큼의 비율 가져옴(선형보간)
        float damage = Mathf.Lerp(MinDamage, MaxDamage, intensity);
        float speed = Mathf.Lerp(MinSpeed, MaxSpeed, intensity);
        Color skinColor = Color.Lerp(Color.white, StrongEnemyColor, intensity);

        int spawnIndex = Random.Range(0, _pointCount);
        Transform spawnPoint = SpawnPoints[spawnIndex];

        GameObject enemyObj = PhotonNetwork.Instantiate(EnemyPrefab.gameObject.name, spawnPoint.position, spawnPoint.rotation); // 
        Enemy enemy = enemyObj.GetComponent<Enemy>();

        enemy.photonView.RPC("Setup", RpcTarget.All, health, damage, speed, skinColor);
        //enemy.Setup(health, damage, speed, skinColor);
        //_enemies.Add(enemy);
        //enemy.OnDeath += () => _enemies.Remove(enemy);
        enemy.OnDeath += () => 
        {
            --_remainEnemyCount;
            //Destroy(enemy.gameObject, 10f);

            StartCoroutine(destroyAfter(enemy.gameObject, 10f));
            GameManager.Instance.AddScore(100);
        };
        //enemy.OnDeath += reduceEnemyCount;
        //enemy.OnDeath += () => { Destroy(enemy.gameObject, 10f); };
        //enemy.OnDeath += () => { GameManager.Instance.AddScore(100); };
    }

    IEnumerator destroyAfter(GameObject obj, float delay)
    {
        yield return new WaitForSeconds(delay);
        PhotonNetwork.Destroy(obj);
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if(stream.IsWriting)
        {
            stream.SendNext(_remainEnemyCount);
            stream.SendNext(_wave);
        }
        else
        {
            _remainEnemyCount = (int)stream.ReceiveNext();
            _wave = (int)stream.ReceiveNext();
        }
    }

    //private void reduceEnemyCount()
    //{
    //    --_remainEnemyCount;
    //}

    //private void destroyHelper(GameObject Enemy)
    //{
    //    Destroy(Enemy, 10f);
    //}
}