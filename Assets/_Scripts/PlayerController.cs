using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class PlayerController : MonoBehaviour {
    public static PlayerController instance;
    public Transform mainCam;
	public Transform playerMesh,crossHair,crossHairPoint,holsterPivot,knifePivot,deadPosition;
    public GameObject Pistol, AimLine;

    public GameObject bullet;
    public float loopDuration;
    public bool isFired=false;

    private void Awake()
    {
        instance = this;
    }
    // Use this for initialization
    void Start () {
        SetAim();

    }
	
	// Update is called once per frame
	void Update () {
        
        if (Input.GetMouseButtonDown(0))
        {
            switch(GameManager.gameState){
                case "MainMenu":
                    GameManager.instance.StartGame();
                    break;
                case "InGame":
                    if(!isFired){
                        isFired = true;
                        Instantiate(bullet, crossHairPoint.position, crossHairPoint.rotation);
                    }
                    break;
                case "Result":
                    GameManager.instance.ReloadLevel();
                    break;
            }


            
        }
	}


	public void SetAim()
	{
		crossHair.DOLocalRotate(new Vector3(0, 15, 0), loopDuration, RotateMode.Fast).SetEase(Ease.Linear).OnComplete(() =>
		{
            crossHair.DOLocalRotate(new Vector3(0, -15, 0), loopDuration, RotateMode.Fast).SetEase(Ease.Linear).OnComplete(() =>
            {
                SetAim();
            });
        });
	}


    public void TakeAim()
    {
        
       holsterPivot.DOLocalRotate(crossHair.localRotation.eulerAngles, 0.15f, RotateMode.Fast).SetEase(Ease.Linear).OnComplete(() =>
        {
           
            Pistol.transform.SetParent(crossHair.transform.GetChild(0));
            Pistol.transform.localScale = Vector3.one;
            Pistol.transform.localPosition = Vector3.zero;
            Pistol.transform.DOLocalRotate(new Vector3(0, 0, 0), 0.15f, RotateMode.Fast).SetEase(Ease.Linear).OnComplete(() =>
            {
                AimLine.SetActive(true);
                isFired = false;

            });

        });


    }

    public void HoldFire()
    {
        AimLine.SetActive(false);
        holsterPivot.rotation = crossHair.rotation;
        Pistol.transform.SetParent(holsterPivot.transform.GetChild(0));
        Pistol.transform.localScale = Vector3.one;
        Pistol.transform.localPosition = Vector3.zero;
        Pistol.transform.DOLocalRotate(new Vector3(0, 0, 0), 0.15f, RotateMode.Fast).SetEase(Ease.Linear);
        holsterPivot.DOLocalRotate(new Vector3(0, -180, 0), 0.15f, RotateMode.Fast).SetEase(Ease.Linear).OnComplete(() =>
        {
            MovePlayer();
        });

        
       
    }

    public void KnifeFire()
    {
        Pistol.transform.SetParent(knifePivot.transform.GetChild(0));
        Pistol.transform.DOLocalMove(new Vector3(0, 0, 0), 0.15f, false).SetEase(Ease.Linear);
        Pistol.transform.DOLocalRotate(new Vector3(0, 0, 0), 0.15f, RotateMode.Fast).SetEase(Ease.Linear);
    }

    public void MovePlayer()
    {
        transform.DOMove(targetPosition, 0.3f).OnComplete(() =>
        {
            transform.DORotate(targetRotation, 0.3f, RotateMode.Fast).OnComplete(() => {
                TakeAim();
                PathSpawner.instance.levelBlocks[0].GetComponent<LevelProperties>().Enemies[0].SetBotPosition();
            });
            PathSpawner.instance.UpdateLevel();
        });
    }


    Vector3 targetPosition, targetRotation;
    public void ReachNextPosition(Transform target)
    {

        targetPosition = PathSpawner.instance.levelBlocks[1].transform.position;
        targetRotation = PathSpawner.instance.levelBlocks[1].transform.rotation.eulerAngles;
       
        HoldFire();
      
        
    }


    public void IncreaseSpeed()
    {
        loopDuration -= 0.01f;
        loopDuration = Mathf.Clamp(loopDuration, 0.6f, 1);
    }


    public void PlayDead()
    {
        AimLine.SetActive(false);
        Pistol.SetActive(false);
        GameManager.gameState = "Result";
        
        playerMesh.DOMove(deadPosition.position, 0.5f, false).OnComplete(()=>
        {
            mainCam.DOMove(PathSpawner.instance.levelBlocks[0].GetComponent<LevelProperties>().Enemies[0].endCamPoint.position, 0.3f);
            mainCam.DORotate(PathSpawner.instance.levelBlocks[0].GetComponent<LevelProperties>().Enemies[0].endCamPoint.rotation.eulerAngles, 0.1f);
        });
        playerMesh.DORotate(deadPosition.rotation.eulerAngles, 0.3f, RotateMode.Fast);
        GameManager.instance.DelayedReload(3);
    }

}
