using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class GameMaster : MonoBehaviour{
	public static GameMaster GM;

	public float gameVolume = 1.0f;

	public bool debug = false, info = false, testMode = false;
    public static bool doFlicker = false;
	public float flickerSpeed = 0.5f;
	public float fallSpeed = (1/60)*53;
    public int selectedSet;
    public bool[] setCounterRotation;
    public GameObject[] set1, set2, set3, set4;
    public GameObject spawnyboi;
    public GameObject gpause;
    public AudioClip[] audioClips;
	public float localFallSpeed;
	int level = 0;
	int totalLineClears = 0;
	int score = 0;
    bool flickerStage = false;
    float timerFlicker;
	Tetramino[] found;
	bool gameStart = false;
	bool gameEnded = false;
	public bool gamePaused = false;
	float pitch = 1.0f;
	AudioSource audioSource;
	AudioMixerGroup pitchBendGroup;

	void Start(){
		Initialize();
	}
	void Update(){
		if (doFlicker){
			if (found == null) found = FindObjectsOfType<Tetramino>();
			Flicker();
		}
		if (!gameStart && Input.GetKeyDown(KeyCode.Space)){
			Instantiate(spawnyboi, spawnyboi.transform.position, Quaternion.identity);
			gameStart = true;
			FindObjectOfType<GameOver>().toStart.SetActive(false);
			this.GetComponent<AudioSource>().Play();
			GameObject.Find("HowDo").GetComponent<SpriteRenderer>().enabled = false;
			Transform scoreboard = GameObject.Find("Scoreboard").transform;
			ToggleScoreboard(false);
		}
		//Debug.Log("Update");
		if (gameStart && !gameEnded && (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.F1))){
			if (!gamePaused){
				Debug.Log("pause");
				Time.timeScale = 0;
				audioSource.Pause();
				gpause.SetActive(true);
				gamePaused = !gamePaused;
				GameObject.Find("HowDo").GetComponent<SpriteRenderer>().enabled = true;
				ToggleScoreboard(true);
			}else{
				Debug.Log("depause");
				Time.timeScale = 1;
				audioSource.UnPause();
				gpause.SetActive(false);
				gamePaused = !gamePaused;
				GameObject.Find("HowDo").GetComponent<SpriteRenderer>().enabled = false;
				ToggleScoreboard(false);
			}
		}
	}
	void Awake(){
	// void OnValidate(){
		// if (debug) Debug.Log("DEBUG - Something went wrong, destroying GM.");
		// if (GM != null){
			//Destroy(GM);
			// DestroyImmediate(GM, true);
		// }
		// else GM = this;
		// DontDestroyOnLoad(this);
		GM = this;
	}

	public void ToggleScoreboard(bool ans){
		Transform scoreboard = GameObject.Find("Scoreboard").transform;
			for (int i=0; i<12; i++){
				if (i<10){
					scoreboard.GetChild(i).GetComponent<Text>().enabled = ans;
					scoreboard.GetChild(i).transform.GetChild(0).GetComponent<Text>().enabled = ans;
				}
				else if (i == 10) scoreboard.GetChild(i).GetComponent<Text>().enabled = ans;
				else if (i == 11){
					scoreboard.GetChild(i).GetComponent<Image>().enabled = ans;
					scoreboard.GetChild(i).transform.GetChild(0).GetComponent<Text>().enabled = ans;
				}
			}
	}
	public void Initialize(){
		level = 0;
		totalLineClears = 0;
		score = 0;
		localFallSpeed = fallSpeed;
		doFlicker = false;
		gameStart = false;
		if (PlayerPrefs.HasKey("Style")){
			selectedSet = PlayerPrefs.GetInt("Style");
			if (debug) Debug.Log("DEBUG - GameMasterStart: selectedSet is "+selectedSet);
		}
		else if (debug) Debug.Log("DEBUG - GameMasterStart: selectedSet is "+selectedSet+"; no key.");
		if (PlayerPrefs.HasKey("Volume")){
			gameVolume = (1.0f/4)*PlayerPrefs.GetInt("Volume");
			if (debug) Debug.Log("DEBUG - GameMasterStart: gameVolume is "+gameVolume);
		}
		else if (debug) Debug.Log("DEBUG - GameMasterStart: gameVolume is "+gameVolume+"; no key.");
		audioSource = this.GetComponent<AudioSource>();
		audioSource.volume = gameVolume;
		pitchBendGroup = audioSource.outputAudioMixerGroup;
		pitch = 1.0f;
		if (debug) Debug.Log("DEBUG - GameMasterStart: Started.");
	}
	public void PlayClip(int i){
		if (debug) Debug.Log("DEBUG - PlayClip: Playing clip "+i);
		AudioSource.PlayClipAtPoint(audioClips[i], GameObject.Find("MainCamera").transform.position, gameVolume);
	}
	public bool getSetCounterRotation(){return setCounterRotation[selectedSet];}
	// public void Restart(){
	// 	Start();
	// 	FindObjectOfType<GameOver>().HideGameOver();
	// 	found = FindObjectsOfType<Tetramino>();
	// 	foreach (Tetramino e in found){
	// 		Destroy(e.gameObject);
	// 		//DestroyImmediate(e.gameObject, true);
	// 	}
	// }
	public void EndGame(){
		string message = "INFO - EndGame: Highscores:";
		gameEnded = true;
		doFlicker = true;
		found = FindObjectsOfType<Tetramino>();
		Destroy(FindObjectOfType<Spawner>().next);
		//Destroy(GameObject.Find("Spawner"));
		this.GetComponent<AudioSource>().Stop();
		bool isHSset = false;
		int[] highscore = new int[10];
		int newHighScore = -1;
		for (int i=0; i<10; i++){
			if (PlayerPrefs.HasKey("Highscore"+i))
				highscore[i] = PlayerPrefs.GetInt("Highscore"+i);
			else highscore[i] = 0;
			// if (highscore[i] == 0 && highscore[i] < score){
			// 	for (int j=9; j>i; j--) highscore[j] = highscore[j-1];
			// 	highscore[i] = score;
			// 	isHSset = true;
			// 	newHighScore = i;
			// }
			// if (isHSset) break;
		}
		if (!isHSset) for (int i=0; i<10; i++){
			bool isSet = false;
			if (highscore[i] < score){
				for (int j=9; j>i; j--) highscore[j] = highscore[j-1];
				highscore[i] = score;
				newHighScore = i;
				isSet = true;
				break;
			}
			if (isSet) break;
		}
		for (int i=0; i<10; i++){
			PlayerPrefs.SetInt("Highscore"+i, highscore[i]);
			message += " "+highscore[i]+",";
		}
		if (newHighScore != -1){
			FindObjectOfType<Scoreboard>().UpdateBoard(newHighScore);
			FindObjectOfType<GameOver>().ShowGameOver(score, true);
		}
		else{
			FindObjectOfType<Scoreboard>().UpdateBoard();
			FindObjectOfType<GameOver>().ShowGameOver(score, false);
		}
		ToggleScoreboard(true);
		if (GameMaster.GM.info) Debug.Log(message);
	}
	void Flicker(){
		// if (debug) Debug.Log("DEBUG - Flicker: Entered function.");
		if (Time.time - timerFlicker > flickerSpeed){
			// if (debug) Debug.Log("DEBUG - Flicker: Timer checked. Sample from 'found': "+found[0]);
			foreach (Tetramino parent in found){
				if (parent == null) continue;
				// if (debug) Debug.Log("DEBUG - Flicker: Iterating parents.");
				SpriteRenderer sprite;
				foreach (Transform child in parent.gameObject.transform){
					// if (debug) Debug.Log("DEBUG - Flicker: Iterating children.");
					sprite = child.gameObject.GetComponent<SpriteRenderer>();
					if (flickerStage){
						// if (debug) Debug.Log("DEBUG - Flicker: Turning white.");
						sprite.color = Color.white;
					}
	            	else{
						// if (debug) Debug.Log("DEBUG - Flicker: Turning original color.");
	            		sprite.color = parent.originalColor;
	            	}
	            }
			}
        timerFlicker = Time.time;
        flickerStage = !flickerStage;
		}
	}
	public void addLineClears(int lines){
        switch (lines){
            default: break;
            case 1:{
            	score += 40*(level+1);
				if (debug) Debug.Log("DEBUG - Adding "+(score+40*(level+1))+" to the score.");
            	break;
            }
            case 2:{
            	score += 100*(level+1);
				if (debug) Debug.Log("DEBUG - Adding "+(score+100*(level+1))+" to the score.");
            	break;
            }
            case 3:{
            	score += 300*(level+1);
				if (debug) Debug.Log("DEBUG - Adding "+(score+300*(level+1))+" to the score.");
            	break;
            }
            case 4:{
            	score += 1200*(level+1);
				if (debug) Debug.Log("DEBUG - Adding "+(score+1200*(level+1))+" to the score.");
            	break;
            }
        }
		GameObject.Find("Score").GetComponent<Text>().text = ""+score;
        totalLineClears += lines;
        if (totalLineClears >= level+10){
        	level++;
        	localFallSpeed -= 4/60f;
        	totalLineClears -= level+10;
        	GameObject.Find("Level").GetComponent<Text>().text = ""+level;
        	pitch *= 62/60f;
        	audioSource.pitch = pitch;
        	Debug.Log(pitchBendGroup);
        	pitchBendGroup.audioMixer.SetFloat("pitchBend", 1f / pitch);
        }
        GameObject.Find("ToNext").GetComponent<Text>().text = ""+((level+10)-totalLineClears);
	}
	public void addToScore(int value){
		if (value <= 0) return;
		score += value;
		GameObject.Find("Score").GetComponent<Text>().text = ""+score;
		if (debug) Debug.Log("DEBUG - Adding "+value+" to the score.");
	}
}
