using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class VirtualCamera : MonoBehaviour {
    private CinemachineVirtualCamera _virtualCamera;

    // Start is called before the first frame update
    void Start () {
        _virtualCamera = GetComponent<CinemachineVirtualCamera> ();
    }

    void SetFollowPlayer (GameObject Player) {
        _virtualCamera.Follow = Player.transform;
    }

    // Update is called once per frame
    void Update () {

    }
}