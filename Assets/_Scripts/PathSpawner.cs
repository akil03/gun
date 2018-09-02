using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PathSpawner : MonoBehaviour {
    public static PathSpawner instance;
    public GameObject[] LevelObjs;
    
    public List<GameObject> levelBlocks;
    public List<Level> levels;
    bool isBossLevel;
    public int currentBlock,bossHitCount,minSpawnedPath;
	// Use this for initialization
	void Start () {
        instance = this;

        for (int i = 0; i < minSpawnedPath; i++)
            Spawn();

        levelBlocks[0].GetComponent<LevelProperties>().Enemies[0].SetBotPosition();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void UpdateLevel(){
        
        GameManager.score++;
        GameManager.instance.UpdateScore();


        if(currentBlock==levels[GameManager.currentLevel].blockCount){
            if (bossHitCount > levels[GameManager.currentLevel].bossHP)
            {
                currentBlock = 0;
                isBossLevel = false;
                GameManager.instance.bossUI.SetActive(false);
                GameManager.currentLevel++;
                GameManager.instance.UpdateCurrentLevel();
            }
            else
            {
                bossHitCount++;
                GameManager.instance.UpdateBossHP(bossHitCount);
            }

            if(!isBossLevel){
                isBossLevel = true;
                GameManager.instance.bossUI.SetActive(true);
                bossHitCount = 0;
            }


        }else{
            currentBlock++;
        }
        GameManager.instance.UpdateBlocks(currentBlock);
        Spawn();

    }


    public void Spawn()
    {
        GameObject Go = Instantiate(LevelObjs[Random.Range(0, LevelObjs.Length)]);
        if (levelBlocks.Count == 0)
        {
            Go.transform.position = PlayerController.instance.transform.position;
            Go.transform.rotation = PlayerController.instance.transform.rotation;
        }
        else
        {
            Go.transform.position = levelBlocks[levelBlocks.Count - 1].GetComponent<LevelProperties>().endPoint.position;
            int rand = Random.Range(0, 3);
            if (rand == 0)
            {
                levelBlocks[levelBlocks.Count - 1].GetComponent<LevelProperties>().endLeft.SetActive(false);
                levelBlocks[levelBlocks.Count - 1].GetComponent<LevelProperties>().Enemies[0].transform.localPosition = new Vector3(-1, 0, 0);
                Go.transform.rotation = Quaternion.Euler(0, levelBlocks[levelBlocks.Count - 1].transform.eulerAngles.y - Random.Range(90,91), 0);
            }
            else
            {
                levelBlocks[levelBlocks.Count - 1].GetComponent<LevelProperties>().endRight.SetActive(false);
                levelBlocks[levelBlocks.Count - 1].GetComponent<LevelProperties>().Enemies[0].transform.localPosition = new Vector3(-1, 0, 0);
                Go.transform.rotation = Quaternion.Euler(0, levelBlocks[levelBlocks.Count - 1].transform.eulerAngles.y + Random.Range(90, 91), 0);
            }
            
        }

        if(isBossLevel){
            GameObject boss = Instantiate(levels[GameManager.currentLevel].boss, Go.GetComponent<LevelProperties>().Enemies[0].transform);
            boss.transform.localScale = Vector3.one;
            boss.transform.SetParent (boss.transform.parent.parent);
            Destroy(Go.GetComponent<LevelProperties>().Enemies[0].gameObject);
            Go.GetComponent<LevelProperties>().Enemies[0] = boss.GetComponent<BotBehavior>();
        }

        levelBlocks.Add(Go);

        if (levelBlocks.Count > minSpawnedPath)
        {
            GameObject temp = levelBlocks[0];
            levelBlocks.RemoveAt(0);
            Destroy(temp);
        }

        


    }


    public class Path
    {
        public List<GameObject> Blocks = new List<GameObject>();
        public GameObject First, Last, SpawnedEnemy;
    }

    [System.Serializable]
    public class Level
    {
        public int blockCount, bossHP;
        public GameObject normalEnemy, boss;
    }
}
