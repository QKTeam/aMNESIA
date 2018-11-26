using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class test : MonoBehaviour {

    public Button btn;
    public Canvas canvas;

	// Use this for initialization
	void Start () {
        btn.onClick.AddListener(delegate ()
        {
            this.Btn_Test();
        });
	}
	
	void Btn_Test()
    {
        canvas.gameObject.SetActive(false);
        canvas.transform.GetChild(1).gameObject.SetActive(false);
        canvas.transform.GetChild(2).gameObject.SetActive(false);
        canvas.transform.GetChild(3).gameObject.SetActive(false);
        btn.gameObject.SetActive(false);
    }
}
