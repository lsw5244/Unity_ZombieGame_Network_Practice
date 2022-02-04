using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using Photon.Realtime;

public class LobbyManager : MonoBehaviourPunCallbacks
{
    private const string GAME_VERSION = "1";

    [SerializeField]
    private Text _connectionInfoText;
    
    [SerializeField]
    private Button _joinButton;

    // Start is called before the first frame update
    void Start()
    {
        PhotonNetwork.GameVersion = GAME_VERSION;               // 게임 버전 설정

        PhotonNetwork.ConnectUsingSettings();                   // 접속 시도 (포톤서버)

        _joinButton.onClick.RemoveListener(Connect);            // 버튼에 방에 접속하는 이벤트 넣어주기
        _joinButton.onClick.AddListener(Connect);

        _joinButton.interactable = false;                       // Join버튼 비활성화

        _connectionInfoText.text = "마스터 서버에 접속중...";    // 접속중 텍스트 띄워주기
    }

    public override void OnConnectedToMaster()      // 마스터 서버에 접속 되었을 때 호출되는 함수
    {
        _joinButton.interactable = true;

        _connectionInfoText.text = "마스터 서버에 접속하였습니다.";
    }

    public override void OnDisconnected(DisconnectCause cause)  // 연결이 끊어졌을 때 호출되는 함수
    {
        _joinButton.interactable = false;

        _connectionInfoText.text = "마스터 서버와의 연결이 끊어졌습니다.\n재접속 중...";

        PhotonNetwork.ConnectUsingSettings(); // 포톤 서버에 재접속 시도하기

        Debug.Log($"Disconnect Cause : {cause}");
    }

    public void Connect()
    {
        _joinButton.interactable = false;
        // 함수 진입 전에는 연결되었지만 함수 실행 중 연결이 끊어졌을때를 대비하여 한번 더 연결을 확인 함
        if(PhotonNetwork.IsConnected) // 포톤 서버와 연결 되었을 때
        {
            _connectionInfoText.text = "Connect to Room..";

            PhotonNetwork.JoinRandomRoom(); // 랜덤한 서버(방)에 접속하기
        }
        else //접속 못했을 때
        {
            _connectionInfoText.text = "마스터 서버와의 연결이 끊어졌습니다.\n재접속 중...";

            PhotonNetwork.ConnectUsingSettings(); // 재접속 시도하기
        }
    }

    public override void OnJoinRandomFailed(short returnCode, string message) // 랜덤한 방에 접속 못했을 떄
    {
        _connectionInfoText.text = "방이 없습니다, 새로운 방을 만듭니다.";

        PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = 4 }); // 방이름 알아서 정하게 해서 만들기, 최대인원 만들기

    }

    public override void OnJoinedRoom()
    {
        _connectionInfoText.text = "포톤 서버에 접속 성공";

        PhotonNetwork.LoadLevel(/*"Main Scene"*/(int)ESceneID.Main); // loadScene 같다. 룸의 모든 사람들을 옮긴다 ?
        
    }
}
