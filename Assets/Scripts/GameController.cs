using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;

public class GameController : MonoBehaviourPunCallbacks
{
    public UIManager UIManager;

    public GameObject followPlayerCamera;

    public GameObject avatars;
    private int participantNum = 0;
    private List<GameObject> players = new List<GameObject> ();

    public void GameStart ()
    {
        Debug.Log ("Game Start");
        // プレイヤーの操作を可能にする
        foreach (var player in players)
        {
            player.GetComponent<AvatarController> ().EnableOperation ();
        }

    }

    public void StartIntroduction ()
    {
        photonView.RPC (nameof (IntroducePlayers), RpcTarget.All);
    }

    void StartCountDown ()
    {
        UIManager.StartCountDown ();
    }

    [PunRPC]
    private void AddPlayerList (GameObject avatar)
    {
        avatar.transform.parent = avatars.transform;
    }

    void InstatiateCharacter ()
    {
        // Debug.Log (PhotonNetwork.CountOfPlayersInRooms);
        switch (PhotonNetwork.CountOfPlayersInRooms)
        {
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

    public void CreateRoom ()
    {
        PhotonNetwork.NickName = "Host";
        PhotonNetwork.ConnectUsingSettings ();
    }

    public void ParticipateRoom ()
    {
        // PhotonNetwork.NickName = inputFieldName.text;
        PhotonNetwork.NickName = UIManager.GetComponent<UIManager> ().inputFieldName.text;
        PhotonNetwork.ConnectUsingSettings ();
    }

    public override void OnConnectedToMaster ()
    {
        // ランダムなルームに参加する
        PhotonNetwork.JoinRandomRoom ();
    }

    // ランダムで参加できるルームが存在しないなら、新規でルームを作成する
    public override void OnJoinRandomFailed (short returnCode, string message)
    {
        var roomOptions = new RoomOptions ();
        roomOptions.MaxPlayers = 3;
        PhotonNetwork.CreateRoom (null, roomOptions);
    }

    public override void OnJoinedRoom ()
    {
        //マスターは傍観者
        if (!PhotonNetwork.IsMasterClient)
        {
            InstatiateCharacter ();
        }

        UIManager.GetComponent<UIManager> ().selectHostOrPlayerUI.SetActive (false);
        UIManager.GetComponent<UIManager> ().InputProfileUI.SetActive (false);
    }

    public void Goal ()
    {
        UIManager.GetComponent<UIManager> ().resultUI.SetActive (true);
    }

    [PunRPC]
    public void IntroducePlayers ()
    {
        foreach (var photonView in PhotonNetwork.PhotonViewCollection)
        {
            Debug.Log ($"{photonView.gameObject.tag}({photonView.ViewID})");
            if (photonView.gameObject.tag == "Player")
            {
                players.Add (photonView.gameObject);
            }
        }

        StartCoroutine ("StartIntoroductionCoroutine");
    }

    public void UpdateParticipantNum ()
    {
        participantNum++;
        UIManager.participantNumText.text = $"現在 {participantNum} 人";
    }

    IEnumerator StartIntoroductionCoroutine ()
    {
        for (int i = 0; i < players.Count; i++)
        {
            var player = GameObject.Find ($"Avatar{i+1}(Clone)");

            player.transform.Find ("VCam").gameObject.GetComponent<CinemachineVirtualCamera> ().enabled = true;
            yield return new WaitForSeconds (2);

            player.GetComponent<AvatarController> ().HideMyNameFromOthers ();
            UIManager.IntroducePlayerUI.SetActive (true);
            UIManager.IntroduceNameText.text = player.GetComponent<AvatarController> ().returnName ();

            yield return new WaitForSeconds (5);
            player.GetComponent<AvatarController> ().InformNameToOthers ();
        }

        foreach (var player in players)
        {
            player.transform.Find ("VCam").gameObject.GetComponent<CinemachineVirtualCamera> ().enabled = false;
        }

        UIManager.IntroducePlayerUI.SetActive (false);
        yield return new WaitForSeconds (2);

        foreach (var player in players)
        {
            player.GetComponent<AvatarController> ().InformNameToOthers ();
            player.transform.parent = avatars.transform;
        }
        StartCountDown ();
    }
}