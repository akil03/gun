using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class GameManager : MonoBehaviour {
    public static GameManager instance;
    public Text ScoreTxt;
    public GameObject player;
    AudioSource speaker;
    private void Awake()
    {
        instance = this;    
    }
    // Use this for initialization
    void Start () {
        speaker = GetComponent<AudioSource>();

    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void PlaySound(AudioClip clip)
    {
        speaker.PlayOneShot(clip);
    }

    public void UpdateScore(int score)
    {
        ScoreTxt.text = "Score : " + score;
    }

    public void DelayedReload(float delayTime)
    {
        Invoke("ReloadLevel", delayTime);
    }

    public void ReloadLevel()
    {
        Application.LoadLevel(0);
    }
}
