using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine;

public class GameAttribute : MonoBehaviour {

    public int coin;
    public int multiply = 1;
    public static GameAttribute instance;

	public int life = 1;

	public Text Text_Coin; 

	// Use this for initialization
	void Start () {
        coin = 0;
        instance = this;
	}
	
	// Update is called once per frame
	void Update () {
		Text_Coin.text = coin.ToString ();
	}

    public void AddCoin(int coinNUmber)
    {
        coin += multiply * coinNUmber;

    }
}
