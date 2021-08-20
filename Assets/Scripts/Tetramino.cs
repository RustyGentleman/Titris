#define NORMALCLEAR
// #define FADECLEAR
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Tetramino : MonoBehaviour
{
    public Vector3 rotationPoint;
    public Vector3 nextOffset;
    public Vector3 spawnOffset;
    public static float fallSpeed = 0.8f;
    public static bool endGame = false;
    public Color originalColor;
    static int height = 20;
    static int width = 10;
    static Transform[,] grid = new Transform[width, height+3];
    float moveDelay = 0.1f;
    float spinDelay = 0.15f;
    // float elapsed;
    float timerFall;
    float timerSoftDrop;
    float timerMove;
    float timerSpin;
    int toClear = -1;
    int lineClears = 0;
    bool isSoftDropping = false;

    // Start is called before the first frame update
    void Start()
    {
        if (GameMaster.GM.info) Debug.Log("INFO - Start: Current fallSpeed is "+GameMaster.GM.localFallSpeed+".");
        fallSpeed = GameMaster.GM.localFallSpeed;
        timerFall = Time.time;
        originalColor = this.gameObject.transform.GetChild(1).GetComponent<SpriteRenderer>().color;
        if (isGameEnd()){
            GameMaster.GM.PlayClip(4);
            GameMaster.GM.EndGame();
            GameMaster.doFlicker = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Move left and reset move timer
        if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A) && !GameMaster.GM.gamePaused){
            transform.position += new Vector3(-1, 0, 0);
            if (!isValid()) transform.position += new Vector3(1, 0, 0);
            else GameMaster.GM.PlayClip(1);
            timerMove = Time.time;
        }
        // Move right and reset move timer
        if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D) && !GameMaster.GM.gamePaused){
            transform.position += new Vector3(1, 0, 0);
            if (!isValid()) transform.position += new Vector3(-1, 0, 0);
            else GameMaster.GM.PlayClip(1);
            timerMove = Time.time;
        }
        // Soft drop and reset softDrop timer
        if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S) && !GameMaster.GM.gamePaused){
            transform.position += new Vector3(0, -1, 0);
            if (!isValid()){
                transform.position += new Vector3(0, 1, 0);
                Lock();
            }
            else GameMaster.GM.PlayClip(0);
            timerSoftDrop = Time.time;
            timerFall = Time.time;
            isSoftDropping = true;
        }
        // Spin unidirectionally and reset spin timer
        if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W) && !GameMaster.GM.gamePaused){
            Spin(-90);
            timerSpin = Time.time;
        }
        // Spin counter-clockwise and reset spin timer
        if (Input.GetKeyDown(KeyCode.Q) && !GameMaster.GM.gamePaused){
            Spin(90);
            timerSpin = Time.time;
        }
        // Spin clockwise and reset spin timer
        if (Input.GetKeyDown(KeyCode.E) && !GameMaster.GM.gamePaused){
            Spin(-90);
            timerSpin = Time.time;
        }
        //-------------------
        // Continuous move left
        if (Time.time - timerMove > moveDelay && (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A)) && !GameMaster.GM.gamePaused){
            transform.position += new Vector3(-1, 0, 0);
            if (!isValid()) transform.position += new Vector3(1, 0, 0);
            else GameMaster.GM.PlayClip(1);
            timerMove = Time.time;
        }
        // Continuous move right
        if (Time.time - timerMove > moveDelay && (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D)) && !GameMaster.GM.gamePaused){
            transform.position += new Vector3(1, 0, 0);
            if (!isValid()) transform.position += new Vector3(-1, 0, 0);
            else GameMaster.GM.PlayClip(1);
            timerMove = Time.time;
        }
        // Continuous unidirectional spin
        if (Time.time - timerSpin > spinDelay && (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W)) && !GameMaster.GM.gamePaused){
            Spin(-90);
        }
        // Continuous counter-clockwise spin
        if (Time.time - timerSpin > spinDelay && (Input.GetKey(KeyCode.Q)) && !GameMaster.GM.gamePaused){
            Spin(90);
        }
        // Continuous clockwise spin
        if (Time.time - timerSpin > spinDelay && (Input.GetKey(KeyCode.E)) && !GameMaster.GM.gamePaused){
            //Debug.Log("Elapsed time: "+(Time.time-elapsed));
            //elapsed = Time.time;
            Spin(-90);
        }
        // Continuous soft drop
        if (Time.time - timerSoftDrop > moveDelay && (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S)) && !GameMaster.GM.gamePaused){
            transform.position += new Vector3(0, -1, 0);
            if (isSoftDropping){
                GameMaster.GM.addToScore(1);
                isSoftDropping = false;
            }
            GameMaster.GM.addToScore(1);
            if (!isValid()){
                transform.position += new Vector3(0, 1, 0);
                GameMaster.GM.addToScore(-1);
                Lock();
            }
            else GameMaster.GM.PlayClip(0);
            timerSoftDrop = Time.time;
            timerFall = Time.time;
        }
        // Hard drop
        if (Input.GetKeyDown(KeyCode.Keypad0) || Input.GetKeyDown(KeyCode.Space) && !GameMaster.GM.gamePaused){
            int dropCount = -1;
            while (isValid()){
                transform.position += new Vector3(0, -1, 0);
                dropCount++;
            }
            transform.position += new Vector3(0, 1, 0);
            if (dropCount == 0) Lock();
            else{
                GameMaster.GM.addToScore(2*dropCount);
                GameMaster.GM.PlayClip(3);
            }
            timerFall = Time.time;
        }
        // Gravity
        if (fallSpeed!=0 && Time.time-timerFall > fallSpeed){
            transform.position += new Vector3(0, -1, 0);
            if (!isValid()){
                transform.position += new Vector3(0, 1, 0);
                Lock();
            }
            else GameMaster.GM.PlayClip(0);
            timerFall = Time.time;
        }
    }

    bool isValid(){
        foreach (Transform child in transform){
            int roundedX = Mathf.RoundToInt(child.transform.position.x);
            int roundedY = Mathf.RoundToInt(child.transform.position.y);
            if (roundedX<0 || roundedX>=width || roundedY<0 || grid[roundedX,roundedY]){
                return false;
            }
        }
        return true;
    }
    void Lock(){
        if (GameMaster.GM.debug) Debug.Log("DEBUG - Lock: Locking...");
        foreach (Transform child in transform){
            int roundedX = Mathf.RoundToInt(child.transform.position.x);
            int roundedY = Mathf.RoundToInt(child.transform.position.y);
            if (roundedY > 20) this.gameObject.SetActive(false);
            grid[roundedX, roundedY] = child;
            // grid[roundedX, roundedY] = true;
        }
        if (GameMaster.GM.debug) Debug.Log("DEBUG - Lock: Locked a block.");
        GameMaster.GM.PlayClip(5);

        while (LineCheck()) LineClear(toClear);
        this.enabled = false;
        //if (isGameEnd()) return;
        if (FindObjectOfType<Spawner>().next){
            if (lineClears > 0){
                GameMaster.GM.addLineClears(lineClears);
                GameMaster.GM.PlayClip(6);
                if (GameMaster.GM.info) Debug.Log("INFO - Lock: Cleared "+lineClears+" lines.");
            }
            if (!FindObjectOfType<Spawner>().isFirst) FindObjectOfType<Spawner>().next.transform.position
                = new Vector3(4, 20, 0)
                + FindObjectOfType<Spawner>().next.GetComponent<Tetramino>().spawnOffset;
            FindObjectOfType<Spawner>().next.GetComponent<Tetramino>().enabled = true;
            FindObjectOfType<Spawner>().next.GetComponent<Tetramino>().Start();
            FindObjectOfType<Spawner>().SpawnNext();
        }else{
            if (GameMaster.GM.debug) Debug.Log("DEBUG - Tetramino: ...Oh shit, sorry");
            this.gameObject.SetActive(false);
        }
    }
    bool LineCheck(){
        int count = 0;
        for (int j=0; j<height; j++){
            for (int i=0; i<width; i++){
                if (grid[i,j]) count++;
            }
            if (GameMaster.GM.debug) if (count>0) Debug.Log("DEBUG - LineCheck: Line "+j+" counts "+count+".");
            if (count == width){
                toClear = j;
                if (GameMaster.GM.info) Debug.Log("INFO - LineCheck: Marked "+j+" for clearing.");
                return true;
            }
            count = 0;
        }
        if (GameMaster.GM.info) Debug.Log("INFO - LineCheck: No lines to clear.");
        return false;
    }
    void LineClear(int line){
    #if NORMALCLEAR
        if (GameMaster.GM.info){
            Debug.Log("INFO - LineClear: Clearing "+line+".");
        }
        for (int i=0; i<width; i++){
            if (grid[i,line]) Destroy(grid[i,line].gameObject);
        }
        if (GameMaster.GM.debug){
            Debug.Log("DEBUG - LineClear: Cleared LINE "+line+".");
        }
        for (int j=line; j<height; j++){
            for (int i=0; i<width; i++){
                if (j+1<height){
                    grid[i,j] = grid[i,j+1];
                    if (grid[i,j+1]){
                        grid[i,j+1].transform.position += new Vector3(0, -1, 0);
                    }
                }else{
                    grid[i,j] = null;
                }
            }
        }
        toClear = -1;
        lineClears++;
    #endif
    #if FADECLEAR
        if (GameMaster.GM.info){
            Debug.Log("LineClear: Clearing "+line+".");
        }
        for (int i=0; i<width; i++){
            SpriteRenderer sprite;
            sprite = grid[i,line].GetComponent<SpriteRenderer>();
            sprite.color = Color.white;
        }
        for (int i=0; i<width; i++){
            if (grid[i,line]) Destroy(grid[i,line].gameObject);
        }
        for (int j=line; j<height; j++){
            for (int i=0; i<width; i++){
                if (j+1<height){
                    grid[i,j] = grid[i,j+1];
                    if (grid[i,j+1]){
                        grid[i,j+1].transform.position += new Vector3(0, -1, 0);
                    }
                }else{
                    grid[i,j] = null;
                }
            }
        }
        if (GameMaster.GM.info){ && DEBUG
            Debug.Log("LineClear: Cleared.");
        }
        toClear = -1;
        lineClears++;
    #endif
    }
    bool isGameEnd(){
        if (GameMaster.GM.debug) Debug.Log("DEBUG - isGameEnd: Checking...");
        int dropCount = 0;
        bool isInside = false;
        while (isValid()){
            transform.position += new Vector3(0, -1, 0);
            dropCount++;
        }
        foreach (Transform child in transform){
            int roundedX = Mathf.RoundToInt(child.transform.position.x);
            int roundedY = Mathf.RoundToInt(child.transform.position.y);
            if (roundedY<height){
                isInside = true;
                break;
            }
        }
        if (GameMaster.GM.debug) Debug.Log("DEBUG - isGameEnd: "+!isInside+".");
        transform.position += new Vector3(0, dropCount, 0);
        return !isInside;
    }
    void Spin(int angle){
        transform.RotateAround(transform.TransformPoint(rotationPoint), new Vector3(0, 0, 1), angle);
        if (!isValid()){
            if (GameMaster.GM.debug) Debug.Log("DEBUG - Spin: Trying move right.");
            transform.position += new Vector3(1, 0, 0);
            if (!isValid()){
                if (GameMaster.GM.debug) Debug.Log("DEBUG - Spin: Trying move left.");
                transform.position += new Vector3(-2, 0, 0);
                if (!isValid()){
                    if (GameMaster.GM.debug) Debug.Log("DEBUG - Spin: Moving back.");
                    transform.position += new Vector3(1, 0, 0);
                    transform.RotateAround(transform.TransformPoint(rotationPoint), new Vector3(0, 0, 1), -angle);
                }else GameMaster.GM.PlayClip(2);
            }else GameMaster.GM.PlayClip(2);
        }
        else{
            GameMaster.GM.PlayClip(2);
            if (GameMaster.GM.getSetCounterRotation()){
                if (GameMaster.GM.debug) Debug.Log("DEBUG - Spin: Applying counterrotation.");
                foreach (Transform child in transform)
                    child.RotateAround(new Vector3(child.transform.position.x, child.transform.position.y, 0), new Vector3(0, 0, 1), -angle);
            }
        }
        timerSpin = Time.time;
    }
}
