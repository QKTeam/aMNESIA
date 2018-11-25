using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pao : MonoBehaviour {

    private float count;
    private bool speed;

	// Use this for initialization
	void Start () {
        count = 0;
        speed = true;
	}
	
	// Update is called once per frame
	void Update () {
        if (count <= 60 && count >=0)
        {
            if (speed)
            {
                count++;
            } 
            else
            {
                count--;
            }
        }
        else if (count > 60)
        {
            speed = false;
            count--;
        }
        else
        {
            count++;
            speed = true;
        }
        transform.localScale = new Vector3(1 + count / 150, 1 + count / 150, 0);
	}
}
