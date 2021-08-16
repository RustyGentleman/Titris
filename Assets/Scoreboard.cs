using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Scoreboard : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
    	UpdateBoard();
    }

    public void UpdateBoard(){
    	if (GameMaster.GM.debug) Debug.Log("DEBUG - UpdateBoard: Pulling board");
        for (int i=0; i<10; i++){
        	if (PlayerPrefs.HasKey("Highscore"+i))
				this.gameObject.transform.GetChild(i).GetChild(0).GetComponent<Text>().text = ""+PlayerPrefs.GetInt("Highscore"+i);
			else this.gameObject.transform.GetChild(i).GetChild(0).GetComponent<Text>().text = "--------------";
        }
    }
    public void UpdateBoard(int newscore){
    	if (GameMaster.GM.debug) Debug.Log("DEBUG - UpdateBoard: Pulling board and highlighting new highscore");
        for (int i=0; i<10; i++){
        	if (PlayerPrefs.HasKey("Highscore"+i))
				this.gameObject.transform.GetChild(i).GetChild(0).GetComponent<Text>().text = ""+PlayerPrefs.GetInt("Highscore"+i);
			else this.gameObject.transform.GetChild(i).GetChild(0).GetComponent<Text>().text = "--------------";
			if (i == newscore) this.gameObject.transform.GetChild(i).GetChild(0).GetComponent<Animator>().enabled = true;
        }
    }
    public void ResetBoard(){
    	if (GameMaster.GM.debug) Debug.Log("DEBUG - ResetBoard: Resetting board");
        for (int i=0; i<10; i++){
        	PlayerPrefs.SetInt("Highscore"+i, 0);
        }
        UpdateBoard();
    }
}
