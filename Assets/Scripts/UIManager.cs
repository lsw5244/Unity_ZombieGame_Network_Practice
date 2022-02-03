using UnityEngine;
using UnityEngine.SceneManagement; // 씬 관리자 관련 코드
using UnityEngine.UI; // UI 관련 코드

// 필요한 UI에 즉시 접근하고 변경할 수 있도록 허용하는 UI 매니저
public class UIManager : MonoBehaviour
{
    // 싱글톤 접근용 프로퍼티
    public Text AmmoText; // 탄약 표시용 텍스트
    public Text ScoreText; // 점수 표시용 텍스트
    public Text waveText; // 적 웨이브 표시용 텍스트
    public GameObject GameoverUI; // 게임 오버시 활성화할 UI 
    public static UIManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<UIManager>();
            }

            return _instance;
        }
    }

    private static UIManager _instance; // 싱글톤이 할당될 변수


    // 탄약 텍스트 갱신
    public void UpdateAmmoText(int magAmmo, int remainAmmo)
    {
        AmmoText.text = $"{magAmmo}/{remainAmmo}";// magAmmo + "/" + remainAmmo;
    }

    // 점수 텍스트 갱신
    public void UpdateScoreText(int newScore)
    {
        ScoreText.text = $"Score : {newScore}";// + newScore;
    }

    // 적 웨이브 텍스트 갱신
    public void UpdateWaveText(int waves, int count)
    {
        waveText.text = $"Wave : {waves} \nEnemy Left : {count}";// "Wave : " + waves + "\nEnemy Left : " + count;
    }

    // 게임 오버 UI 활성화
    public void SetActiveGameoverUi(bool isActive)
    {
        GameoverUI.SetActive(isActive);
    }

    // 게임 재시작
    public void Restart()
    {
        SceneManager.LoadScene(/*SceneManager.GetActiveScene().name*/(int)ESceneID.Main);
    }
}