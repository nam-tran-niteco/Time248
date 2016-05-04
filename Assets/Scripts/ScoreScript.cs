using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ScoreScript : MonoBehaviour
{

    Text scoreText;

    // Use this for initialization
    void Start()
    {
        scoreText = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        scoreText.text = "High Score: " + PlayerPrefs.GetInt("High Score") + "\n" +
            "Score: " + GameController.instance.score + "  "
            + "Combos: " + GameController.instance.comboCount;
    }
}
