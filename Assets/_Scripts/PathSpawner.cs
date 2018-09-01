using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PathSpawner : MonoBehaviour {
    public static PathSpawner instance;
    public GameObject[] LevelObjs;
    
    public List<GameObject> Levels;

    public int minSpawnedPath;
	// Use this for initialization
	void Start () {
        instance = this;

        for (int i = 0; i < minSpawnedPath; i++)
            Spawn();

        Levels[0].GetComponent<LevelProperties>().Enemies[0].SetBotPosition();
    }
	
	// Update is called once per frame
	void Update () {
		
	}


    public void Spawn()
    {
        GameObject Go = Instantiate(LevelObjs[Random.Range(0, LevelObjs.Length)]);
        if (Levels.Count == 0)
        {
            Go.transform.position = PlayerController.instance.transform.position;
            Go.transform.rotation = PlayerController.instance.transform.rotation;
        }
        else
        {
            Go.transform.position = Levels[Levels.Count - 1].GetComponent<LevelProperties>().endPoint.position;
            int rand = Random.Range(0, 3);
            if (rand == 0)
            {
                Levels[Levels.Count - 1].GetComponent<LevelProperties>().endLeft.SetActive(false);
                Levels[Levels.Count - 1].GetComponent<LevelProperties>().Enemies[0].transform.localPosition = new Vector3(-1, 0, 0);
                Go.transform.rotation = Quaternion.Euler(0, Levels[Levels.Count - 1].transform.eulerAngles.y - Random.Range(90,91), 0);
            }
            else
            {
                Levels[Levels.Count - 1].GetComponent<LevelProperties>().endRight.SetActive(false);
                Levels[Levels.Count - 1].GetComponent<LevelProperties>().Enemies[0].transform.localPosition = new Vector3(-1, 0, 0);
                Go.transform.rotation = Quaternion.Euler(0, Levels[Levels.Count - 1].transform.eulerAngles.y + Random.Range(90, 91), 0);
            }
            
        }
        Levels.Add(Go);

        if (Levels.Count > minSpawnedPath)
        {
            GameObject temp = Levels[0];
            Levels.RemoveAt(0);
            Destroy(temp);
        }

        


    }


    public class Path
    {
        public List<GameObject> Blocks = new List<GameObject>();
        public GameObject First, Last, SpawnedEnemy;
    }
}
