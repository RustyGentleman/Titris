using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VolumeButton : MonoBehaviour
{
	public Sprite[] icons;
	int level = 4;

	void Start(){
		if (PlayerPrefs.HasKey("Volume")) level = PlayerPrefs.GetInt("Volume");
		this.GetComponent<Image>().sprite = icons[level-1];
		GameObject.Find("IconExtra").GetComponent<AudioSource>().volume = (1.0f/4)*level;
	}
	public int ChangeVolume(){
		if (++level > 4) level %= 4;
		PlayerPrefs.SetInt("Volume", level);
		if (GameMaster.GM.debug) Debug.Log("DEBUG - ChangeVolume: Setting volume icon to index "+(level-1));
		this.GetComponent<Image>().sprite = icons[(level-1)];
		GameObject.Find("IconExtra").GetComponent<AudioSource>().volume = (1.0f/4)*level;
		return level;
	}
}
