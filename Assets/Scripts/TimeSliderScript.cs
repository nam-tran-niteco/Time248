using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TimeSliderScript : MonoBehaviour
{

    Text remainingTimeText;
    Image sliderImage;

    // Use this for initialization
    void Start()
    {
        sliderImage = GetComponent<Image>();
        remainingTimeText = GetComponentInChildren<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        float ratio = GameController.instance.remainingTime /
            GameController.instance.playingTime;
        sliderImage.fillAmount = ratio;
        remainingTimeText.text = (int)GameController.instance.remainingTime + " s";
    }

}
