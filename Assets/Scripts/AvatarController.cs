using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using DG.Tweening;
using Photon.Pun;
using TMPro;
using UnityEngine;

public class AvatarController : MonoBehaviourPunCallbacks
{

    public GameObject GameController;
    public AvtarsController avatarsController;

    private string name;
    private string hobby;
    private string fabFood;

    private Vector3 acceleration;
    private Vector3 preAcceleration;
    private float dotProduct;

    private float moveInterval = 0.3f;
    private float moveTimer = 0f;

    public GameObject TPSCamera;

    public TextMeshProUGUI displayNameText;

    private bool isOperateable = false;

    public GameObject nameCanvas;

    // Start is called before the first frame update
    void Start ()
    {
        SetName (photonView.Owner.NickName);
        GameController = GameObject.Find ("GameController");
        avatarsController = GameObject.Find ("Avatars").GetComponent<AvtarsController> ();
        GameController.GetComponent<GameController> ().UpdateParticipantNum ();
    }

    // Update is called once per frame
    void Update ()
    {
        moveTimer += Time.deltaTime;
        if (photonView.IsMine)
        {
            if (isOperateable)
            {
                if (IsShake ())
                {
                    //ここを変えることでみんなで動けるようにする
                    photonView.RPC (nameof (ConveyAvatarsContorollerShake), RpcTarget.All);
                }
            }
        }
    }

    public string returnName ()
    {
        return name;
    }

    public void EnableOperation ()
    {
        isOperateable = true;
    }

    public void DisableOperation ()
    {
        isOperateable = false;
    }

    bool IsShake ()
    {
        preAcceleration = acceleration;
        acceleration = Input.acceleration;
        dotProduct = Vector3.Dot (acceleration, preAcceleration);
        if (dotProduct < 0)
        {
            return true;
        }
        return false;
    }

    public void toggleOperateState ()
    {
        isOperateable = !isOperateable;
    }

    [PunRPC]
    public void SetProfile (string name, string hobby, string fabFood)
    {
        this.name = name;
        this.hobby = hobby;
        this.fabFood = fabFood;
    }

    [PunRPC]
    void MoveStraight ()
    {
        if (moveTimer > moveInterval)
        {
            transform.DOJump (new Vector3 (transform.position.x, 0, transform.position.z + 1), 1.0f, 1, 0.3f).SetEase (Ease.Linear); //jumpアニメーション
            moveTimer = 0;
        }
    }

    [PunRPC]
    void ConveyAvatarsContorollerShake ()
    {
        avatarsController.MoveStraight ();
    }

    //その場でジャンプをする
    [PunRPC]
    void Jump ()
    {
        if (moveTimer > moveInterval)
        {
            transform.DOJump (new Vector3 (transform.position.x, 0, transform.position.z), 1.0f, 1, 0.3f).SetEase (Ease.Linear); //jumpアニメーション
            moveTimer = 0;
        }
    }

    public void InformNameToOthers ()
    {
        photonView.RPC (nameof (DisplayName), RpcTarget.All);
    }

    public void HideMyNameFromOthers ()
    {
        photonView.RPC (nameof (HideName), RpcTarget.All);
    }

    [PunRPC]
    void DisplayName ()
    {
        nameCanvas.SetActive (true);
    }

    [PunRPC]
    void HideName ()
    {
        nameCanvas.SetActive (false);
    }

    [PunRPC]
    void SetName (string name)
    {
        this.name = name;
        displayNameText.text = name;
    }
}