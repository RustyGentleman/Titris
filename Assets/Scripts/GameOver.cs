using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOver : MonoBehaviour
{
	public GameObject gameOver, death, toStart;

	public void ShowGameOver(int score, bool highscore){
    	//gameOver.SetActive(true);
        gameOver.transform.GetChild(0).GetComponent<Text>().enabled = true;
        gameOver.transform.GetChild(3).GetComponent<Text>().enabled = true;
        gameOver.transform.GetChild(4).GetComponent<SpriteRenderer>().enabled = true;
        gameOver.transform.GetChild(3).GetComponent<Text>().text = ""+score;
        if (!highscore){
            gameOver.transform.GetChild(1).GetComponent<Text>().enabled = true;
        }else{
            gameOver.transform.GetChild(2).GetComponent<Text>().enabled = true;
            gameOver.transform.GetChild(2).GetComponent<Animator>().enabled = true;
        }
        death.SetActive(true);
	}
    // public void HideGameOver(){
        // if (GameMaster.GM.debug) Debug.Log("DEBUG - HideGameOver: Hiding...");
        // gameUI.SetActive(true);
        // gameOver.SetActive(false);
        // if (GameMaster.GM.debug) Debug.Log("DEBUG - HideGameOver: Hidden");
    // }
}
