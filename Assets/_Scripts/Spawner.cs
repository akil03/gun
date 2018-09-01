using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class Spawner : MonoBehaviour {
    public static Spawner instance;
    public GameObject Enemy;
    public Vector3 minSpawn, maxSpawn;
	// Use this for initialization
	void Start () {
        instance = this;
        DelayedRandomeSpawn();

    }
	
	// Update is called once per frame
	void Update () {
		
	}


    public void RandomSpawn()
    {
        GameObject go = Instantiate(Enemy, new Vector3(Random.Range(minSpawn.x, maxSpawn.x), -1.59f, Random.Range(minSpawn.z, maxSpawn.z)), Quaternion.identity);
        go.transform.localScale = Vector3.zero;
        go.transform.DOScale(Vector3.one, .8f).SetEase(Ease.OutElastic);
    }

    public void DelayedRandomeSpawn()
    {
        Invoke("RandomSpawn", 1);
    }
}
