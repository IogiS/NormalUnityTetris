using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Game : MonoBehaviour
{

    public static int gridWidth = 10;
    public static int gridHeight = 20;

    public static Transform[,] grid = new Transform[gridWidth, gridHeight];



    public int scoreOneLine = 40;
    public int scoreTwoLine = 100;
    public int scoreThreeLine = 300;
    public int scoreFourLine = 1200;

    public Text score_text;
    private int currentScore = 0;

    private int numberOfRowsThisTurn = 0;

    private GameObject previewTetromino;
    private GameObject nextTetromino;
    private bool gameStarted = false;

    private Vector2 previewTetrominoPosition = new Vector2(-6.5f, 15);


    // Start is called before the first frame update
    void Start()
    {
        SpawnNextTetromino();
    }

    void Update()
    {
        if (score_text != null)
        {
            UpdateScore();
            UpdateUI();
        }
        
    }


    public void UpdateUI()
    {
        score_text.text = currentScore.ToString();
    }


    public void UpdateScore()
    {
        if (numberOfRowsThisTurn > 0)
        {
            if (numberOfRowsThisTurn == 1)
            {
                ClearedOneLine();
            }
            else if (numberOfRowsThisTurn == 2)
            {
                ClearedTwoLine();
            }
            else if (numberOfRowsThisTurn == 3)
            {
                ClearedThreeLine();
            }
            else if (numberOfRowsThisTurn == 4)
            {
                ClearedFourLine();
            }
            numberOfRowsThisTurn = 0;
        }
    }

    void ClearedOneLine()
    {
        currentScore += scoreOneLine;
    }
    void ClearedTwoLine()
    {
        currentScore += scoreTwoLine;
    }
    void ClearedThreeLine()
    {
        currentScore += scoreThreeLine;
    }
    void ClearedFourLine()
    {
        currentScore += scoreFourLine;
    }

    public bool CheckIsAboveGrid (Tetermino tetermino)
    {
        for (int x = 0; x < gridWidth; ++x)
        {
            foreach (Transform mino in tetermino.transform)
            {
                Vector2 pos = Round(mino.position);
                if(pos.y > gridHeight - 1)
                {
                    return true;
                }
            }
        }
        return false;

    }



     public bool IsFullRowAt(int y)
    {
        for (int x = 0; x < gridWidth; ++x)
        {
            if (grid[x,y] == null)
            {
                return false;
            }
        }

        numberOfRowsThisTurn++;
        return true;
    }

    public void deleteMinoAt(int y)
    {
        for (int x = 0; x < gridWidth; ++x)
        {
            Destroy(grid[x, y].gameObject);
            grid[x, y] = null;
        }
    }

    public void MoveRowDown(int y)
    {
        for (int x = 0; x < gridWidth; ++x)
        {
            if (grid[x, y] != null)
            {
                grid[x, y - 1] = grid[x, y];
                grid[x, y] = null;
                grid[x, y - 1].position += new Vector3(0, -1, 0);
            }
        }
    }


    public void  MoveAllRowsDown(int y)
    {
        for (int i = y; i < gridHeight; ++i)
        {
            MoveRowDown(i);
        }
    }


    public void DeleteRow()
    {
        for (int y = 0; y < gridHeight; ++y)
        {
            if (IsFullRowAt(y))
            {
                deleteMinoAt(y);
                MoveAllRowsDown(y + 1);
                --y;
            }
        }
    }


    public void UpdateGrid(Tetermino tetermino) {

        for (int y = 0; y < gridHeight; ++y)
        {
            for (int x = 0; x < gridWidth; ++x)
            {
                if (grid[x,y]!= null)
                {
                    if (grid[x, y].parent == tetermino.transform)
                    {
                        grid[x, y] = null;
                    }
                }
            }
        }

        foreach (Transform mino in tetermino.transform)
        {
            Vector2 pos = Round(mino.position);
            if (pos.y < gridHeight)
            {
                grid[(int)pos.x, (int)pos.y] = mino;
            }
        }
    }


    public Transform GetTransformAtGridPosition(Vector2 pos)
    {

        if (pos.y >gridHeight - 1)
        {
            return null;
        }
        else
        {
            return grid[(int)pos.x, (int)pos.y];
        }
    }


    public void SpawnNextTetromino()
    {

        if (!gameStarted)
        {
            gameStarted = true;

            nextTetromino = (GameObject)Instantiate(Resources.Load(GetRandomTetermino(), typeof(GameObject)), new Vector2(5.0f, 20.0f), Quaternion.identity);
            previewTetromino = (GameObject)Instantiate(Resources.Load(GetRandomTetermino(), typeof(GameObject)), previewTetrominoPosition, Quaternion.identity);
            previewTetromino.GetComponent<Tetermino>().enabled = false;

        }
        else
        {
            previewTetromino.transform.localPosition = new Vector2(5.0f, 20.0f);
            nextTetromino = previewTetromino;
            nextTetromino.GetComponent<Tetermino>().enabled = true;

            previewTetromino = (GameObject)Instantiate(Resources.Load(GetRandomTetermino(), typeof(GameObject)), previewTetrominoPosition, Quaternion.identity);
            previewTetromino.GetComponent<Tetermino>().enabled = false;
        }
         
    }
    public bool CheckIsInsideGrid (Vector2 pos)
    {
        return ((int)pos.x >= 0 && (int)pos.x < gridWidth && (int)pos.y >= 0);
    }
    public Vector2 Round(Vector2 pos)
    {
        return new Vector2(Mathf.Round(pos.x), Mathf.Round(pos.y));
    }

    string GetRandomTetermino()
    {
        int randomTetermino = Random.Range(1, 8);

        string randomTetrominoName = "Preferbs/Tetermino_T";


        switch (randomTetermino)
        {
            case 1:
                randomTetrominoName = "Preferbs/Tetermino_T";
                break;
            case 2:
                randomTetrominoName  ="Preferbs/Tetermino_Long";
                break;
            case 3:
                randomTetrominoName ="Preferbs/Tetermino_Square";
                break;
            case 4:
                randomTetrominoName ="Preferbs/Tetermino_J";
                break;
            case 5:
                randomTetrominoName = "Preferbs/Tetermino_L";
                break;
            case 6:
                randomTetrominoName = "Preferbs/Tetermino_S";
                break;
            case 7:
                randomTetrominoName = "Preferbs/Tetermino_Z";
                break;

        }
        return randomTetrominoName;
    }


    public void GameOver() => SceneManager.LoadScene("GameOver");
}
