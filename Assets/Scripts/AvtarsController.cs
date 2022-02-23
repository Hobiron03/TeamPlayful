using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Photon.Pun;
using TMPro;
using UnityEngine;

public class AvtarsController : MonoBehaviour
{
    private int shakeNum = 0;

    private float moveInterval = 0.3f;
    private float moveTimer = 0f;
    // Start is called before the first frame update

    // Update is called once per frame
    void Update ()
    {
        moveTimer += Time.deltaTime;
        if (Input.GetKeyDown (KeyCode.Space))
        {
            MoveStraight ();
        }
    }

    public void MoveStraight ()
    {
        if (moveTimer > moveInterval)
        {
            transform.DOJump (new Vector3 (transform.position.x, 0, transform.position.z + 1), 1.0f, 1, 0.3f).SetEase (Ease.Linear); //jumpアニメーション
            moveTimer = 0;
        }
    }
}