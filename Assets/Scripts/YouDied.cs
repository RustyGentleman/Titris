using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class YouDied : MonoBehaviour
{
	public GameObject blackout;
	int whereto = 1;

	public void BlackInStop(){
		this.gameObject.SetActive(false);
	}
	public void BlackOut(int e){
		if (GameMaster.GM.debug) Debug.Log("BlackOut: Blacking out");
		blackout.GetComponent<Animator>().enabled = true;
		whereto = e;
	}
	public void ExitTo(){
        PlayerPrefs.Save();
        Time.timeScale = 1;
		if (GameMaster.GM.debug) Debug.Log("ExitTo: Exiting...");
		blackout.GetComponent<Animator>().enabled = true;;
		switch (whereto){
	    	case 0:{
	    		if (GameMaster.GM.debug) Debug.Log("ExitTo: Closing game");
	    		Application.Quit();
	    		break;
	    	}
	    	case 1:{
	    		if (GameMaster.GM.debug) Debug.Log("ExitTo: Exiting to main menu");
	    		SceneManager.LoadScene("MainMenu");
	    		break;
	    	}
	    	case 2:{
	    		if (GameMaster.GM.debug) Debug.Log("ExitTo: Loading game");
	    		Time.timeScale = 1;
	    		SceneManager.LoadScene("Game");
	    		break;
	    	}
		}
	}
}
