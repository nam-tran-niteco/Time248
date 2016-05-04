using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class ButtonController : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	public void NewGame()
	{
		SceneManager.LoadScene ("Main");
	}

	public void HighScore()
	{
		SceneManager.LoadScene ("HighScore");
	}

	public void Exit()
	{
		Application.Quit ();
	}

	public void Back()
	{
		SceneManager.LoadScene ("Menu");
	}
}
