using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Spawner : MonoBehaviour
{
    public GameObject[] Tetraminos;
    public GameObject next;
    public bool isFirst = true;
    public GameObject[] bag;
    public int bagIndex = 7;

    // Start is called before the first frame update
    void Start()
    {
        switch (GameMaster.GM.selectedSet){
            case 1:
                Tetraminos = GameMaster.GM.set1;
                break;
            case 2:
                Tetraminos = GameMaster.GM.set2;
                break;
            case 3:
                Tetraminos = GameMaster.GM.set3;
                break;
            case 4:
                Tetraminos = GameMaster.GM.set4;
                break;
            default:
                Tetraminos = GameMaster.GM.set1;
                break;
        }
        bagIndex = 7;
        isFirst = true;
        Spawn();
    }

    public void Restart(){
        if (GameMaster.GM.debug) Debug.Log("DEBUG - SpawnRestart: Restarting...");
        Start();
    }
    public void Spawn(){
    	if (GameMaster.GM.debug) Debug.Log("DEBUG - Spawn: Spawning initial tetramino.");
        GameObject toSpawn;
        if (GameMaster.GM.testMode) toSpawn = Tetraminos[3];
        else toSpawn = Tetraminos[UnityEngine.Random.Range(0,7)];
    	Instantiate(toSpawn, new Vector3(4, 20, 0)+toSpawn.GetComponent<Tetramino>().spawnOffset, Quaternion.identity);
    	isFirst = false;
    	SpawnNext();
    }
    public void SpawnNext(){
        if (GameMaster.GM.debug) Debug.Log("DEBUG - SpawnBag: Spawning next tetramino.");
        if (bagIndex == 7){
        	if (GameMaster.GM.debug) Debug.Log("DEBUG - SpawnBag: Last slot in bag is null, generating new roll.");
        	int[] sequence = {-1,-1,-1,-1,-1,-1,-1};
        	string message = "";
        	GameObject toSpawn;
            // int seed = System.DateTime.Now.Millisecond;
            // UnityEngine.Random.InitState(seed);
            // Debug.Log("Seed is "+seed);
        	for (int i=0; i<7; i++){
        		int candidate = UnityEngine.Random.Range(0,7);
        		// foreach (int item in sequence) message += " "+item;
        		// Debug.Log("Sequence "+message+" Iter "+i+" candidate "+candidate);
        		sequence[i] = candidate;
        		for (int j=0; j<7; j++)
        			if (j != i && sequence[j] == candidate){
        				i--;
        				break;
        			}
        		// message = "";
        	}
        	for(int i=0; i<7; i++){
        		message += " "+sequence[i];
		        if (GameMaster.GM.testMode) toSpawn = Tetraminos[3];
		        else toSpawn = Tetraminos[sequence[i]];
        		bag[i] = Instantiate(toSpawn, transform.position+toSpawn.GetComponent<Tetramino>().nextOffset, Quaternion.identity);
        		bag[i].SetActive(false);
        		bag[i].GetComponent<Tetramino>().enabled = false;
        	}
        	if (GameMaster.GM.debug) Debug.Log("DEBUG - SpawnBag: Sequence is"+message);
        	bagIndex = 0;
        	next = bag[bagIndex++];
        	next.SetActive(true);
        }
        else{
        	if (GameMaster.GM.debug) Debug.Log("DEBUG - SpawnBag: Taking slot "+bagIndex);
        	bag[bagIndex].SetActive(true);
        	next = bag[bagIndex];
        	bagIndex++;
        }
    }
}
