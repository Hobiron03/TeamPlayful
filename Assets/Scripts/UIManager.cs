using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour {
    public GameObject GameController;

    public GameObject selectHostOrPlayerUI;
    public GameObject resultUI;
    public GameObject InputProfileUI;

    // Start is called before the first frame update
    void Start () {

    }

    public void OnParticipateAsPlayerButtonPush () {
        InputProfileUI.SetActive (true);
    }

    public void OnInputProfileCompleteButtonPush () {
        GameController.GetComponent<GameController> ().ParticipateRoom ();
    }
}