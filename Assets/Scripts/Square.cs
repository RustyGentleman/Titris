using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Square : MonoBehaviour
{
    // public static int height = 20;
    // public static int width = 10;
    // public Vector3 rotationPoint;
    //public int[] Pos = new int[] {0,0,0};
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        // if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.RightArrow)){
        //     Pos[0] = Mathf.RoundToInt(transform.position.x);
        //     Pos[1] = Mathf.RoundToInt(transform.position.y);
        //     Pos[2] = Mathf.RoundToInt(transform.position.z);
        // }
    }

    /*public void CounterRotate(){
        this.transform.RotateAround(new Vector3(0, 0, 0), new Vector3(0, 0, 1), -90);
    }*/

    /*bool isValid(){
        int roundedX = Mathf.RoundToInt(this.transform.position.x);
        int roundedY = Mathf.RoundToInt(this.transform.position.y);

        if (roundedX<0 || roundedX>=width || roundedY<0 || roundedY>=height){
            return false;
        }
        else return true;
    }*/
}
