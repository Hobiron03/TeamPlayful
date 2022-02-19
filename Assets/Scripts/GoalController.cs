using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalController : MonoBehaviour {
    public GameObject GameController;

    // Start is called before the first frame update
    void Start () {

    }

    // Update is called once per frame
    void Update () {

    }

    private void OnTriggerEnter (Collider other) {
        // Debug.Log ("HIT");
        if (other.gameObject.tag == "Player") {
            GameController.GetComponent<GameController> ().Goal ();
        }
    }
}