using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
	public GameObject play, settings, quit, volume, style, settingsSection;
	public GameObject[] styleSection;

    void Start(){
        if (PlayerPrefs.HasKey("Style"))
            style.GetComponent<Image>().sprite = style.GetComponent<StyleButton>().icons[PlayerPrefs.GetInt("Style")];
    }

    public void Play()
    {
        // PlayerPrefs.Save();
        // SceneManager.LoadScene("Game");
        // GameMaster.GM.Initialize();
        GameObject.Find("Blackout").GetComponent<YouDied>().BlackOut(2);
    }
    public void Settings(){
    	// settingsSection.SetActive(true);
        settingsSection.transform.GetChild(0).GetComponent<Image>().enabled = true;
        settingsSection.transform.GetChild(1).GetComponent<Image>().enabled = true;
        settingsSection.transform.GetChild(2).GetComponent<Image>().enabled = true;
    	// settings.SetActive(false);
        settings.GetComponent<Image>().enabled = false;
        settings.transform.GetChild(0).GetComponent<Text>().enabled = false;
    }
    public void Quit(){
        GameObject.Find("Blackout").GetComponent<YouDied>().BlackOut(0);
    }
    public void SettingsVolume(){
        GameMaster.GM.gameVolume = (1.0f/4)*volume.GetComponent<VolumeButton>().ChangeVolume();
        if (GameMaster.GM.debug) Debug.Log("DEBUG - ChangeVolume: Volume set to "+GameMaster.GM.gameVolume);
    }
    public void SettingsStyle(){
    	foreach (GameObject child in styleSection){
    		child.GetComponent<Image>().enabled = true;
    	}
    	// settingsSection.SetActive(false);
        settingsSection.transform.GetChild(0).GetComponent<Image>().enabled = false;
        settingsSection.transform.GetChild(1).GetComponent<Image>().enabled = false;
        settingsSection.transform.GetChild(2).GetComponent<Image>().enabled = false;
    }
    public void SettingsBack(){
        // settingsSection.SetActive(false);
        settingsSection.transform.GetChild(0).GetComponent<Image>().enabled = false;
        settingsSection.transform.GetChild(1).GetComponent<Image>().enabled = false;
        settingsSection.transform.GetChild(2).GetComponent<Image>().enabled = false;
        // settings.SetActive(true);
        settings.GetComponent<Image>().enabled = true;
        settings.transform.GetChild(0).GetComponent<Text>().enabled = true;
    }
    public void SettingsStyleSelect(int e){
    	GameMaster.GM.selectedSet = e;
        PlayerPrefs.SetInt("Style", e);
    	Image image = style.GetComponent<Image>();
    	image.sprite = style.GetComponent<StyleButton>().icons[e];
    	// settingsSection.SetActive(true);
        settingsSection.transform.GetChild(0).GetComponent<Image>().enabled = true;
        settingsSection.transform.GetChild(1).GetComponent<Image>().enabled = true;
        settingsSection.transform.GetChild(2).GetComponent<Image>().enabled = true;
    	foreach (GameObject child in styleSection) child.GetComponent<Image>().enabled = false;
    }
}
