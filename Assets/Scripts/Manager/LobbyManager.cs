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
        PhotonNetwork.GameVersion = GAME_VERSION;               // ���� ���� ����

        PhotonNetwork.ConnectUsingSettings();                   // ���� �õ� (���漭��)

        _joinButton.onClick.RemoveListener(Connect);            // ��ư�� �濡 �����ϴ� �̺�Ʈ �־��ֱ�
        _joinButton.onClick.AddListener(Connect);

        _joinButton.interactable = false;                       // Join��ư ��Ȱ��ȭ

        _connectionInfoText.text = "������ ������ ������...";    // ������ �ؽ�Ʈ ����ֱ�
    }

    public override void OnConnectedToMaster()      // ������ ������ ���� �Ǿ��� �� ȣ��Ǵ� �Լ�
    {
        _joinButton.interactable = true;

        _connectionInfoText.text = "������ ������ �����Ͽ����ϴ�.";
    }

    public override void OnDisconnected(DisconnectCause cause)  // ������ �������� �� ȣ��Ǵ� �Լ�
    {
        _joinButton.interactable = false;

        _connectionInfoText.text = "������ �������� ������ ���������ϴ�.\n������ ��...";

        PhotonNetwork.ConnectUsingSettings(); // ���� ������ ������ �õ��ϱ�

        Debug.Log($"Disconnect Cause : {cause}");
    }

    public void Connect()
    {
        _joinButton.interactable = false;
        // �Լ� ���� ������ ����Ǿ����� �Լ� ���� �� ������ ������������ ����Ͽ� �ѹ� �� ������ Ȯ�� ��
        if(PhotonNetwork.IsConnected) // ���� ������ ���� �Ǿ��� ��
        {
            _connectionInfoText.text = "Connect to Room..";

            PhotonNetwork.JoinRandomRoom(); // ������ ����(��)�� �����ϱ�
        }
        else //���� ������ ��
        {
            _connectionInfoText.text = "������ �������� ������ ���������ϴ�.\n������ ��...";

            PhotonNetwork.ConnectUsingSettings(); // ������ �õ��ϱ�
        }
    }

    public override void OnJoinRandomFailed(short returnCode, string message) // ������ �濡 ���� ������ ��
    {
        _connectionInfoText.text = "���� �����ϴ�, ���ο� ���� ����ϴ�.";

        PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = 4 }); // ���̸� �˾Ƽ� ���ϰ� �ؼ� �����, �ִ��ο� �����

    }

    public override void OnJoinedRoom()
    {
        _connectionInfoText.text = "���� ������ ���� ����";

        PhotonNetwork.LoadLevel(/*"Main Scene"*/(int)ESceneID.Main); // loadScene ����. ���� ��� ������� �ű�� ?
        
    }
}
