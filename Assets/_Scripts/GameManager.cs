using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
public class GameManager : MonoBehaviour {
    public static GameManager instance;

    public GameObject mainMenu, inGameHUD, bossUI;

    public Text ScoreTxt,levelTxt;
    public Image levelPanel;
    public GameObject player;
    public static string gameState;
    public static int score, highScore=0, currentLevel=0;
    AudioSource speaker;
    private void Awake()
    {
        instance = this; 
    }
    // Use this for initialization
    void Start () {
        ResetVariables();
        speaker = GetComponent<AudioSource>();

    }

    void ResetVariables(){
        currentLevel = PlayerPrefs.GetInt("currentLevel");
        highScore = PlayerPrefs.GetInt("highScore");
        gameState = "MainMenu";
        UpdateCurrentLevel();

    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void StartGame(){
        gameState = "InGame";
        score = 0;
        UpdateScore();
        mainMenu.SetActive(false);
        inGameHUD.SetActive(true);
    }

    public void CheckHighScore(){
        
        if(score>highScore){
            highScore = score;
            PlayerPrefs.SetInt("highScore", highScore);
        }
    }

    public void PlaySound(AudioClip clip)
    {
        speaker.PlayOneShot(clip);
    }

    public void UpdateScore()
    {
        ScoreTxt.text = "Score : " + score;
        CheckHighScore();
    }

    public void UpdateCurrentLevel()
    {
        levelTxt.text = "Level "+currentLevel;
        PlayerPrefs.SetInt("currentLevel", currentLevel);
    }

    public void UpdateLevel(int levelNo){
        currentLevel = levelNo;
        levelTxt.text = "Level " +levelNo + "/" +PathSpawner.instance.levels.Count; 
    }

    public void UpdateBlocks(int blockNo){
        float fillValue = blockNo / PathSpawner.instance.levels[currentLevel].blockCount;
        levelPanel.transform.DOScaleX(fillValue, 2);
    }


    public void UpdateBossHP(int botHitCount)
    {
        float fillValue = (float)botHitCount / (float)PathSpawner.instance.levels[currentLevel].bossHP;
        print(fillValue);
        levelPanel.transform.DOScaleX(fillValue, 2);
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
