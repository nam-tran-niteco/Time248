using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class HighScoreScript : MonoBehaviour {

	public Text highScoreText;

	// Use this for initialization
	void Start () {
		highScoreText.text = PlayerPrefs.GetInt ("High Score") + "";
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
