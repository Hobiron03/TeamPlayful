using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public GameObject GameController;
    GameController gameController;

    public GameObject selectHostOrPlayerUI;
    public GameObject resultUI;
    public GameObject InputProfileUI;
    public GameObject HostStandbyUI;

    public GameObject IntroducePlayerUI;
    public TextMeshProUGUI IntroduceNameText;
    public TextMeshProUGUI IntroduceHobbyText;

    public TextMeshProUGUI participantNumText;
    public TMP_InputField inputFieldName;

    public GameObject countDownUI;
    public TextMeshProUGUI countDownText;

    void Start ()
    {
        gameController = GameController.GetComponent<GameController> ();
    }

    // Start is called before the first frame update
    public void OnParticipateAsPlayerButtonPush ()
    {
        InputProfileUI.SetActive (true);
    }

    public void OnParticipateAsHostButtonPush ()
    {
        gameController.CreateRoom ();
        HostStandbyUI.SetActive (true);
    }

    public void OnInputProfileCompleteButtonPush ()
    {
        gameController.ParticipateRoom ();
    }

    public void OnPlayerParticipateCompleteButtonPush ()
    {
        //自己紹介の開始
        gameController.StartIntroduction ();
        HostStandbyUI.SetActive (false);
    }

    IEnumerator CountDownCoroutine ()
    {
        countDownUI.SetActive (true);
        yield return new WaitForSeconds (1.5f);

        countDownText.text = "3";
        yield return new WaitForSeconds (1.0f);

        countDownText.text = "2";
        yield return new WaitForSeconds (1.0f);

        countDownText.text = "1";
        yield return new WaitForSeconds (1.0f);

        // countDown.text = "スタート";
        yield return new WaitForSeconds (1.0f);

        // countDownUI.SetActive (false);
        gameController.GameStart ();
    }
}