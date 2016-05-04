using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{

    // Singleton instance
    public static GameController instance;

    // Configure values
    public int startColors = 2;
    public float playingTime = 15;
    public float remainingTime;
    public int score = 0;

    // Constants
    public const int BOARD_SIZE = 5;
    public const int MAX_VALUE = 32;
    public const float SPAWN_TIME = 0.5f;
    public const int MAX_COLOR = 3;

    public GameObject squarePrefab;
    public GameObject paintItemPrefab;
    public GameObject maxItemPrefab;
    public GameObject bombItemPrefab;
    public GameObject gameOverPopup;
    public Vector2[,] slots;

    public List<GameObject> squares;

    public GameObject firstPress;
    public GameObject secondPress;

    private Transform mainCanvas;
    private bool playable;
    public int comboCount;
    public int combos;

    // Use this for initialization
    void Start()
    {
        instance = this;

        slots = new Vector2[BOARD_SIZE, BOARD_SIZE];
        squares = new List<GameObject>();
        mainCanvas = GameObject.FindGameObjectWithTag("Main Canvas").transform;
        remainingTime = playingTime;
        playable = true;
        comboCount = 0;
        combos = 0;

        //Time.timeScale = 0.3f;

        FillSlotsArray();
        FillPlayingBoard();
    }

    void FillSlotsArray()
    {
        int row;
        int col;
        string searchString = "";
        for (int i = 0; i < BOARD_SIZE; i++)
        {
            for (int j = 0; j < BOARD_SIZE; j++)
            {
                row = i + 1; col = j + 1;
                searchString = "Slot " + row.ToString() + "-" + col.ToString();
                slots[i, j] = GameObject.Find(searchString).GetComponent<RectTransform>().position;
            }
        }
    }

    void FillPlayingBoard()
    {
        for (int i = 0; i < BOARD_SIZE; i++)
        {
            for (int j = 0; j < BOARD_SIZE; j++)
            {
                int tempColor = Random.Range(1, startColors + 1);
                SpawnSquare(i, j, 2, tempColor);
            }
        }
    }

    bool AreTwoSquaresSame(int value1, int color1,
        int value2, int color2)
    {
        if ((color1 == color2) &&
            (value1 == value2))
            return true;

        return false;
    }

    void SpawnSquare(int row, int col, int value, int eColor)
    {
        GameObject temp = (GameObject)Instantiate(squarePrefab, slots[row, col], squarePrefab.transform.rotation);
        temp.GetComponent<SquareScript>().SetColor(eColor);
        temp.GetComponent<SquareScript>().SetValue(value);
        temp.GetComponent<SquareScript>().row = row;
        temp.GetComponent<SquareScript>().col = col;
        temp.transform.SetParent(mainCanvas);
        temp.transform.localScale = new Vector3(1, 1, 1);
        squares.Add(temp);
    }

    // Update is called once per frame
    void Update()
    {
        if (playable)
        {
            ProcessClickEvents();
            UpdateTime();
        }
    }

    void UpdateTime()
    {
        remainingTime -= Time.deltaTime;

        if (remainingTime <= 0)
        {
            playable = false;
            remainingTime = 0;
            //Time.timeScale = 0;
            if (score > PlayerPrefs.GetInt("High Score"))
            {
                PlayerPrefs.SetInt("High Score", score);
            }

            gameOverPopup.transform.SetAsLastSibling();
            gameOverPopup.SetActive(true);
        }
    }

    void ProcessClickEvents()
    {
        if (firstPress != null && secondPress != null)
        {
            int color1, color2;
            int value1, value2;
            value1 = firstPress.GetComponent<SquareScript>().value;
            color1 = firstPress.GetComponent<SquareScript>().color;
            value2 = secondPress.GetComponent<SquareScript>().value;
            color2 = secondPress.GetComponent<SquareScript>().color;

            if (AreTwoSquaresSame(value1, color1, value2, color2))
            {
                //Debug.Log("Two Square Is Same");
                ProcessClickOnTwoSameSquares();
            }
            else
            {
                //Debug.Log("Two Square Is Not Same");
                ProcessClickOnTwoDifferentSquares();
            }

            if (firstPress != null)
            {
                firstPress.GetComponent<Toggle>().isOn = false;
                firstPress = null;
            }
            if (secondPress != null)
            {
                secondPress.GetComponent<Toggle>().isOn = false;
                secondPress = null;
            }
        }
    }

    void ProcessClickOnTwoSameSquares()
    {
        comboCount++;
        if (comboCount == 10)
        {
            //ProcessPaintCombo();
            PlacePaintItem();
        }
        else if (comboCount == 20)
        {
            //ProcessMaxCombo();
            PlaceMaxItem();
        }
        else if (comboCount == 30)
        {
            //ProcessBombCombo();
            PlaceBombItem();
        }
        else
        {
            ProcessNonCombo();
        }
    }

    void PlacePaintItem()
    {
        SquareScript secondPressSquareScript = secondPress.GetComponent<SquareScript>();
        int row = secondPressSquareScript.row;
        int col = secondPressSquareScript.col;
        int color = secondPressSquareScript.color;

        score += secondPressSquareScript.value * 2;

        int index = squares.IndexOf(secondPress.gameObject);
        squares.RemoveAt(index);

        GameObject temp = (GameObject)Instantiate(paintItemPrefab, slots[row, col], squarePrefab.transform.rotation);
        temp.GetComponent<ItemScript>().SetColor(color);
        temp.GetComponent<ItemScript>().row = row;
        temp.GetComponent<ItemScript>().col = col;
        temp.transform.SetParent(mainCanvas);
        temp.transform.localScale = new Vector3(1, 1, 1);
        squares.Add(temp);

        Destroy(secondPress.gameObject);

        StartCoroutine(SpawnRandomSquare(firstPress.GetComponent<SquareScript>().row,
            firstPress.GetComponent<SquareScript>().col));
        squares.Remove(firstPress.gameObject);
        Destroy(firstPress.gameObject);
    }

    void PlaceMaxItem()
    {
        SquareScript secondPressSquareScript = secondPress.GetComponent<SquareScript>();
        int row = secondPressSquareScript.row;
        int col = secondPressSquareScript.col;
        int color = secondPressSquareScript.color;

        score += secondPressSquareScript.value * 2;

        int index = squares.IndexOf(secondPress.gameObject);
        squares.RemoveAt(index);

        GameObject temp = (GameObject)Instantiate(maxItemPrefab, slots[row, col], squarePrefab.transform.rotation);
        temp.GetComponent<ItemScript>().SetColor(0);
        temp.GetComponent<ItemScript>().row = row;
        temp.GetComponent<ItemScript>().col = col;
        temp.transform.SetParent(mainCanvas);
        temp.transform.localScale = new Vector3(1, 1, 1);
        squares.Add(temp);

        Destroy(secondPress.gameObject);

        StartCoroutine(SpawnRandomSquare(firstPress.GetComponent<SquareScript>().row,
            firstPress.GetComponent<SquareScript>().col));
        squares.Remove(firstPress.gameObject);
        Destroy(firstPress.gameObject);
    }

    void PlaceBombItem()
    {
        SquareScript secondPressSquareScript = secondPress.GetComponent<SquareScript>();
        int row = secondPressSquareScript.row;
        int col = secondPressSquareScript.col;
        int color = secondPressSquareScript.color;

        score += secondPressSquareScript.value * 2;

        int index = squares.IndexOf(secondPress.gameObject);
        squares.RemoveAt(index);

        GameObject temp = (GameObject)Instantiate(bombItemPrefab, slots[row, col], squarePrefab.transform.rotation);
        temp.GetComponent<ItemScript>().SetColor(0);
        temp.GetComponent<ItemScript>().row = row;
        temp.GetComponent<ItemScript>().col = col;
        temp.transform.SetParent(mainCanvas);
        temp.transform.localScale = new Vector3(1, 1, 1);
        squares.Add(temp);

        Destroy(secondPress.gameObject);

        StartCoroutine(SpawnRandomSquare(firstPress.GetComponent<SquareScript>().row,
            firstPress.GetComponent<SquareScript>().col));
        squares.Remove(firstPress.gameObject);
        Destroy(firstPress.gameObject);
    }

    SquareScript FindSquareByCoordinate(int row, int col)
    {
        for (int i = 0; i < squares.Count; i++)
        {
            SquareScript square = squares[i].GetComponent<SquareScript>();
            if (square != null)
            {
                if (square.row == row && square.col == col)
                {
                    return square;
                }
            }
        }

        return null;
    }

    public void ProcessPaintCombo(int row, int col, int color)
    {
        combos++;
        remainingTime += 3.5f;

        if (remainingTime > playingTime)
            remainingTime = playingTime;

        int minRow, maxRow, minCol, maxCol;
        minRow = minCol = 0;
        maxRow = maxCol = BOARD_SIZE - 1;

        for (int i = minRow; i <= maxRow; i++)
        {
            for (int j = minCol; j <= maxCol; j++)
            {
                SquareScript square = FindSquareByCoordinate(i, j);
                if (square != null)
                {
                    square.SetColor(color);
                    //square.SetValue(maxValue);
                }
            }
        }

        StartCoroutine(SpawnRandomSquare(row, col));
    }

    public void ProcessMaxCombo(int row, int col)
    {
        combos++;
        remainingTime += 3.5f;

        if (remainingTime > playingTime)
            remainingTime = playingTime;

        int minRow, maxRow, minCol, maxCol;
        minRow = row - 1;
        maxRow = row + 1;
        minCol = col - 1;
        maxCol = col + 1;
        if (minRow < 0)
            minRow = 0;
        if (maxRow >= BOARD_SIZE)
            maxRow = BOARD_SIZE;
        if (minCol < 0)
            minCol = 0;
        if (maxCol >= BOARD_SIZE)
            maxCol = BOARD_SIZE;

        for (int i = minRow; i <= maxRow; i++)
        {
            for (int j = minCol; j <= maxCol; j++)
            {
                SquareScript square = FindSquareByCoordinate(i, j);
                if (square != null)
                {
                    square.SetValue(MAX_VALUE);
                }
            }
        }

        StartCoroutine(SpawnRandomSquare(row, col));
    }

    public void ProcessBombCombo(int row, int col)
    {
        combos++;
        comboCount = 0;
        remainingTime += 5.0f;

        if (remainingTime > playingTime)
            remainingTime = playingTime;

        int minRow, maxRow, minCol, maxCol;
        minRow = row - 2;
        maxRow = row + 2;
        minCol = col - 2;
        maxCol = col + 2;
        if (minRow < 0)
            minRow = 0;
        if (maxRow >= BOARD_SIZE)
            maxRow = BOARD_SIZE - 1;
        if (minCol < 0)
            minCol = 0;
        if (maxCol >= BOARD_SIZE)
            maxCol = BOARD_SIZE - 1;

        for (int i = minRow; i <= maxRow; i++)
        {
            for (int j = minCol; j <= maxCol; j++)
            {
                if (i != row || j != col)
                {
                    SquareScript square = FindSquareByCoordinate(i, j);
                    if (square == null)
                        continue;
                    score += square.value * 8;
                    StartCoroutine(SpawnRandomSquare(square.row, square.col));
                    int squareIndex = squares.IndexOf(square.gameObject);
                    squares.RemoveAt(squareIndex);
                    Destroy(square.gameObject);
                }
            }
        }

        StartCoroutine(SpawnRandomSquare(row, col));
    }

    void ProcessNonCombo()
    {
        SquareScript secondPressSquareScript = secondPress.GetComponent<SquareScript>();
        if (secondPressSquareScript.value != MAX_VALUE)
        {
            score += secondPressSquareScript.value * 2;
            remainingTime += 0.5f;

            if (remainingTime > playingTime)
                remainingTime = playingTime;

            int firstPressIndex = squares.IndexOf(firstPress.gameObject);
            squares.RemoveAt(firstPressIndex);
            //squares.Remove(firstPress.gameObject);

            int firstRow = firstPress.GetComponent<SquareScript>().row;
            int firstCol = firstPress.GetComponent<SquareScript>().col;
            StartCoroutine(SpawnRandomSquare(firstRow, firstCol));

            secondPressSquareScript.SetValue(secondPressSquareScript.value * 2);
            Destroy(firstPress);
        }
        else
        {
            score += secondPressSquareScript.value * 5;
            remainingTime += 1.0f;

            if (remainingTime > playingTime)
                remainingTime = playingTime;

            int firstPressIndex = squares.IndexOf(firstPress.gameObject);
            squares.RemoveAt(firstPressIndex);
            //squares.Remove(firstPress.gameObject);

            int firstRow = firstPress.GetComponent<SquareScript>().row;
            int firstCol = firstPress.GetComponent<SquareScript>().col;
            StartCoroutine(SpawnRandomSquare(firstRow, firstCol));

            int secondPressIndex = squares.IndexOf(secondPress.gameObject);
            squares.RemoveAt(secondPressIndex);
            //squares.Remove(firstPress.gameObject);

            int secondRow = secondPress.GetComponent<SquareScript>().row;
            int secondCol = secondPress.GetComponent<SquareScript>().col;
            StartCoroutine(SpawnRandomSquare(secondRow, secondCol));

            Destroy(firstPress); Destroy(secondPress);
        }
    }

    IEnumerator SpawnRandomSquare(int row, int col)
    {
        yield return new WaitForSeconds(1.0f);
        if (playable)
        {
            if (score > 1500)
            {
                SpawnSquare(row, col, 2, Random.Range(1, MAX_COLOR + 1));
            }
            else
            {
                SpawnSquare(row, col, 2, Random.Range(1, startColors + 1));
            }
        }
    }

    void ProcessClickOnTwoDifferentSquares()
    {
        remainingTime -= 3.0f;
        comboCount = 0;
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene("Main");
    }


}
