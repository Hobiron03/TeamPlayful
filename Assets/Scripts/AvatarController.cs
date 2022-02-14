using System.Diagnostics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using DG.Tweening;


public class AvatarController : MonoBehaviour
{

    private Vector3 acceleration;
    private Vector3 preAcceleration;
    private float dotProduct;

    private float moveInterval = 0.3f;
    private float moveTimer = 0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        moveTimer += Time.deltaTime;
        if(Input.GetKeyDown(KeyCode.Space) || ShakeCheck()){
            MoveStraigt();
        }   
    }


    bool ShakeCheck()
    {
        preAcceleration = acceleration;
        acceleration = Input.acceleration;
        dotProduct = Vector3.Dot(acceleration, preAcceleration);
        if (dotProduct < 0)
        {
            return true;
        }

        return false;
    }
    

    void MoveStraigt()
    {
        if(moveTimer > moveInterval){
            transform.DOJump(new Vector3(transform.position.x, 0, transform.position.z + 1), 1.0f, 1, 0.3f).SetEase(Ease.Linear);//jumpアニメーション
            moveTimer = 0;
        }

        // Vector3 newPos = new Vector3(transform.position.x, transform.position.y, transform.position.z+ 1);
        // transform.DOMove(newPos, 0.4f);

        // UIController.GetComponent<UIController>().IncreaseDist();
        // audio.PlayOneShot(jumpSound);

        // flickDir = FLICK_DIR.IDOL;
        // OpeHist.Push(STRIGHT);
    }
}
