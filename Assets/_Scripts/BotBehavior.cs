using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class BotBehavior : MonoBehaviour {
    public Transform crossHair, crossHairPoint;
    public GameObject bullet;
    public Transform[] KnifePoints;
    public Transform endCamPoint;
    public float movementInterval;
    bool movementStarted;
    int point = 0;
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    


    public void SetBotPosition()
    {
        transform.DOLocalMoveX(0, 1);
    }


    public void SetAim()
    {
        if (movementInterval > 0)
        {
            PlayerController.instance.isFired = false;
            if (!movementStarted)
            {
                PlayerController.instance.KnifeFire();
                KnifeMove();
            }
                
        }
        else
            Shoot();


        
    }


    void KnifeMove()
    {
        movementStarted = true;

        float actualInterval;
        if (point == 0 || point == KnifePoints.Length - 1)
            actualInterval = movementInterval / 1.5f;
        else
            actualInterval = movementInterval;

        transform.DOMove(new Vector3(KnifePoints[point].position.x, transform.position.y, KnifePoints[point].position.z), actualInterval, false).SetEase(Ease.Linear).OnComplete(() =>
        {
            point++;
            
            if (point == KnifePoints.Length)
                PlayerController.instance.PlayDead();
            else
            {
                KnifeMove();
                
            }
                
        });

    }

    void Shoot()
    {
        crossHair.gameObject.SetActive(true);
        crossHair.DOLocalRotate(new Vector3(0, 0, 0), 0.3f, RotateMode.Fast).SetEase(Ease.Linear).OnComplete(() =>
        {
            Instantiate(bullet, crossHairPoint.position, crossHairPoint.rotation);
        });
    }
}
