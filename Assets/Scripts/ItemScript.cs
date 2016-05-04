using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public enum ItemType
{
    PAINT,
    MAX,
    BOMB
}

public class ItemScript : MonoBehaviour
{

    public ItemType itemType;

    public int row;
    public int col;
    public int color;

    private Toggle toggle;

    // Use this for initialization
    void Start()
    {
        toggle = GetComponent<Toggle>();
        toggle.onValueChanged.AddListener(
            delegate { ToggleValueChangeListener(); });
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
            if (itemType == ItemType.PAINT)
            {
                GameController.instance.ProcessPaintCombo(row, col, color);
            }
            else if (itemType == ItemType.MAX)
            {
                GameController.instance.ProcessMaxCombo(row, col);
            }
            else if (itemType == ItemType.BOMB)
            {
                GameController.instance.ProcessBombCombo(row, col);
            }

            GameController.instance.squares.Remove(gameObject);
            Destroy(gameObject);
        }
        Debug.Log(gameObject.name + " " + toggle.isOn);
    }

}
