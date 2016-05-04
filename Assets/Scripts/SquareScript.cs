using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SquareScript : MonoBehaviour
{

    public int row;
    public int col;

    public int value;
    public int color;

    private Toggle toggle;
    private Image img;

    private float startAnimationTime;
    private float animationTime = 0.8f;
    private bool allowAnimation;

    // Use this for initialization
    void Start()
    {
        toggle = GetComponent<Toggle>();
        toggle.onValueChanged.AddListener(
            delegate { ToggleValueChangeListener(); });
        img = GetComponent<Image>();
    }

    public void SetValue(int val)
    {
        value = val;
        GetComponentInChildren<Text>().text = value.ToString();
    }

    public void SetColor(int color)
    {
        if (color == 0)
        {
            GetComponent<Image>().color = Color.white;
        }
        else
        if (color == 1)
        {
            GetComponent<Image>().color = Color.red;
        }
        else
        if (color == 2)
        {
            GetComponent<Image>().color = Color.green;
        }
        else
        if (color == 3)
        {
            GetComponent<Image>().color = Color.yellow;
        }
        else
        {
            Debug.Log("Undefined Color!");
        }
        this.color = color;
    }

    void ToggleValueChangeListener()
    {
        if (toggle.isOn)
        {
            if (GameController.instance.firstPress == null)
            {
                GameController.instance.firstPress = gameObject;
            }
            else
            {
                GameController.instance.secondPress = gameObject;
            }
        }
        else
        {
            GameController.instance.firstPress = null;
        }
        //Debug.Log(toggle.isOn);
    }

    void Update()
    {
        if (allowAnimation)
        {
            PlayAnimation();
        }
    }

    public void StartEffect()
    {
        allowAnimation = true;
        startAnimationTime = Time.time;
    }

    public void PlayAnimation()
    {
        Color temp = new Color(img.color.r, img.color.g, img.color.b);
        temp.a = (Random.Range(1, 3) == 1) ? 0.5f : 0.7f;
        img.color = temp;

        if (Time.time - startAnimationTime > animationTime)
        {
            //img.color = Color.white;
            SetColor(color);
            allowAnimation = false;
        }
    }

}
