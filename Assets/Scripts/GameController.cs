using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;

public class GameController : MonoBehaviourPunCallbacks {
    public GameObject selectHostOrPlayerUI;
    public GameObject resultUI;
    public GameObject InputProfileUI;
    public GameObject HostStandbyUI;

    public GameObject IntroducePlayerUI;
    public TextMeshProUGUI IntroduceNameText;
    public TextMeshProUGUI IntroduceHobbyText;

    public TextMeshProUGUI participantNumText;
    public TMP_InputField inputFieldName;

    public GameObject followPlayerCamera;

    public GameObject avatars;

    private int participantNum = 0;

    private List<GameObject> players = new List<GameObject> ();

    public void OnParticipateAsPlayerButtonPush () {
        InputProfileUI.SetActive (true);
    }

    public void OnParticipateAsHostButtonPush () {
        CreateRoom ();
        HostStandbyUI.SetActive (true);
    }

    public void OnInputProfileCompleteButtonPush () {
        ParticipateRoom ();
    }

    public void OnPlayerParticipateCompleteButtonPush () {
        //自己紹介の開始
        photonView.RPC (nameof (IntroducePlayers), RpcTarget.All);
        HostStandbyUI.SetActive (false);
    }

    [PunRPC]
    private void AddPlayerList (GameObject avatar) {
        avatar.transform.parent = avatars.transform;
    }

    void InstatiateCharacter () {
        // Debug.Log (PhotonNetwork.CountOfPlayersInRooms);
        switch (PhotonNetwork.CountOfPlayersInRooms) {
            case 1:
                GameObject avatar = PhotonNetwork.Instantiate ("Avatar1", new Vector3 (-1.0f, 0.1f, 0f), Quaternion.Euler (-90, 180, 0));
                followPlayerCamera.GetComponent<CinemachineVirtualCamera> ().Follow = avatar.transform;
                break;
            case 2:
                GameObject avatar2 = PhotonNetwork.Instantiate ("Avatar2", new Vector3 (1.0f, 0.1f, 0f), Quaternion.identity);
                followPlayerCamera.GetComponent<CinemachineVirtualCamera> ().Follow = avatar2.transform;
                break;
            default:
                PhotonNetwork.Instantiate ("Avatar2", new Vector3 (2.0f, 0.1f, 0f), Quaternion.identity);
                break;
        }
    }

    public void CreateRoom () {
        PhotonNetwork.NickName = "Host";
        PhotonNetwork.ConnectUsingSettings ();
    }

    public void ParticipateRoom () {
        PhotonNetwork.NickName = inputFieldName.text;
        PhotonNetwork.ConnectUsingSettings ();
    }

    public override void OnConnectedToMaster () {
        // ランダムなルームに参加する
        PhotonNetwork.JoinRandomRoom ();
    }

    // ランダムで参加できるルームが存在しないなら、新規でルームを作成する
    public override void OnJoinRandomFailed (short returnCode, string message) {
        var roomOptions = new RoomOptions ();
        roomOptions.MaxPlayers = 3;
        PhotonNetwork.CreateRoom (null, roomOptions);
    }

    public override void OnJoinedRoom () {
        //マスターは傍観者
        if (!PhotonNetwork.IsMasterClient) {
            InstatiateCharacter ();
        }

        selectHostOrPlayerUI.SetActive (false);
        InputProfileUI.SetActive (false);
    }

    public void Goal () {
        resultUI.SetActive (true);
    }

    [PunRPC]
    public void IntroducePlayers () {
        foreach (var photonView in PhotonNetwork.PhotonViewCollection) {
            Debug.Log ($"{photonView.gameObject.tag}({photonView.ViewID})");
            if (photonView.gameObject.tag == "Player") {
                players.Add (photonView.gameObject);
            }
        }

        StartCoroutine ("StartIntoroduction");
    }

    public void UpdateParticipantNum () {
        participantNum++;
        participantNumText.text = $"現在 {participantNum} 人";
    }

    IEnumerator StartIntoroduction () {

        for (int i = 0; i < players.Count; i++) {
            var player = GameObject.Find ($"Avatar{i+1}(Clone)");
            player.transform.Find ("VCam").gameObject.GetComponent<CinemachineVirtualCamera> ().enabled = true;
            yield return new WaitForSeconds (2);

            player.GetComponent<AvatarController> ().HideMyNameFromOthers ();
            IntroducePlayerUI.SetActive (true);
            IntroduceNameText.text = player.GetComponent<AvatarController> ().returnName ();

            yield return new WaitForSeconds (5);
            player.GetComponent<AvatarController> ().InformNameToOthers ();
        }

        foreach (var player in players) {
            player.transform.Find ("VCam").gameObject.GetComponent<CinemachineVirtualCamera> ().enabled = false;
        }

        IntroducePlayerUI.SetActive (false);

    }

}