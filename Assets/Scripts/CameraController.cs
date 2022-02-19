using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class CameraController : MonoBehaviour {
    private float speed = 0.2f; //オブジェクトのスピード
    private int radius = 10; //円を描く半径
    private Vector3 defPosition; //defPositionをVector3で定義する。
    float x;
    float z;

    private bool isInstantiateMyCharacter = false;

    // Start is called before the first frame update
    void Start () {
        defPosition = transform.position; //defPositionを自分のいる位置に設定する。
    }

    // Update is called once per frame
    void Update () {
        if (!isInstantiateMyCharacter) {
            x = radius * Mathf.Sin (Time.time * speed);
            z = radius * Mathf.Cos (Time.time * speed);
            transform.position = new Vector3 (x + defPosition.x, defPosition.y, z + defPosition.z);
        }
    }

    void forcusCameraToMyCharacter () {
        isInstantiateMyCharacter = true;
    }
}