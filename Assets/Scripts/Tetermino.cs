using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tetermino : MonoBehaviour
{

    float fall = 0f;

    public float fallSpeed = 1;

    public bool allowRotation = true;
    public bool limitRotation = false;

    private float continuousVerticalSpeed = 0.05f;
    private float continuousHorizontalSpeed = 0.1f;
    private float verticalTimer = 0;
    private float horizantalTimer = 0;
    private float buttonDownWaitMax = 0.2f;
    private float buttonDownWaitTimer = 0;
    private bool movedImmediateHorizontal = false;
    private bool movedImmediateVertical = false;


    private int touchSesitivityHorisontal = 8;
    private int touchSesitivityVertical = 4;
    Vector2 preciousUnitPosition = Vector2.zero;
    Vector2 direction = Vector2.zero;
    bool moved = false;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        CheckUserInput();
    }


    void CheckUserInput()
    {
#if UNITY_ANDROID

        if (Input.touchCount > 0)
        {
            Touch t = Input.GetTouch(0);

            if (t.phase == TouchPhase.Began)
            {
                preciousUnitPosition = new Vector2(t.position.x, t.position.y);
            }
            else if (t.phase == TouchPhase.Moved)
            {
                Vector2 touchDeltaPosition = t.deltaPosition;
                direction = touchDeltaPosition.normalized;

                if (Mathf.Abs(t.position.x - preciousUnitPosition.x) >= touchSesitivityHorisontal && direction.x < 0 && t.deltaPosition.y > -10 && t.deltaPosition.y < 10)
                {
                    MoveLeft();
                    preciousUnitPosition = t.position;
                    moved = true;
                }
                else if (Mathf.Abs(t.position.x - preciousUnitPosition.x) >= touchSesitivityHorisontal && direction.x > 0 && t.deltaPosition.y > -10 && t.deltaPosition.y < 10)
                {
                    MoveRight();
                    preciousUnitPosition = t.position;
                    moved = true;
                }
                else if (Mathf.Abs(t.position.y - preciousUnitPosition.y) >= touchSesitivityVertical && direction.y < 0 && t.deltaPosition.x > -+10 && t.deltaPosition.x < 10)
                {
                    MoveDown();
                    preciousUnitPosition = t.position;
                    moved = true;
                }
            }
            else if (t.phase == TouchPhase.Ended)
            {
                if (!moved && t.position.x > Screen.width / 4)
                {
                    Rotate();
                }
                moved = false;
            }

        }

        if (Time.time - fall >= fallSpeed)
        {
            MoveDown();
        }
#elif UNITY_STANDALONE

        if (Input.GetKeyUp(KeyCode.LeftArrow) || Input.GetKeyUp(KeyCode.RightArrow) || Input.GetKeyUp(KeyCode.DownArrow))
        {
            horizantalTimer = 0;
            verticalTimer = 0;
            movedImmediateHorizontal = false;
            movedImmediateVertical = false;
        }



        if (Input.GetKey(KeyCode.LeftArrow))
        {
            MoveLeft();
        }

        else if (Input.GetKey(KeyCode.RightArrow))
        {
            MoveRight();


        }

        else if (Input.GetKeyUp(KeyCode.UpArrow))
        {
            Rotate();
        }

        else if (Input.GetKey(KeyCode.DownArrow) || Time.time - fall >= fallSpeed)
        {
            MoveDown();
        }
#endif
    }
    bool CheckIsValidPosition()
    {
        foreach (Transform mino in transform)
        {
            Vector2 pos = FindObjectOfType<Game>().Round(mino.position);
            if (!FindObjectOfType<Game>().CheckIsInsideGrid(pos))
            {
                return false;
            }
            if (FindObjectOfType<Game>().GetTransformAtGridPosition(pos) != null && FindObjectOfType<Game>().GetTransformAtGridPosition(pos).parent != transform)
            {
                return false;
            }
        }
        return true;
    }


    void MoveRight()
    {
        if (movedImmediateHorizontal)
        {
            if (buttonDownWaitTimer < buttonDownWaitMax)
            {
                buttonDownWaitTimer += Time.deltaTime;
                return;
            }
            if (horizantalTimer < continuousHorizontalSpeed)
            {
                horizantalTimer += Time.deltaTime;
                return;
            }
        }
        else { movedImmediateHorizontal = true; }
        horizantalTimer = 0;
        transform.position += new Vector3(1, 0, 0);
        if (CheckIsValidPosition())
        {
            FindObjectOfType<Game>().UpdateGrid(this);
        }
        else { transform.position += new Vector3(-1, 0, 0); }

    }
    void MoveLeft()
    {
        if (movedImmediateHorizontal)
        {
            if (buttonDownWaitTimer < buttonDownWaitMax)
            {
                buttonDownWaitTimer += Time.deltaTime;
                return;
            }
            if (horizantalTimer < continuousHorizontalSpeed)
            {
                horizantalTimer += Time.deltaTime;
                return;
            }
        }
        else { movedImmediateHorizontal = true; }
        horizantalTimer = 0;
        transform.position += new Vector3(-1, 0, 0);
        if (CheckIsValidPosition())
        {
            FindObjectOfType<Game>().UpdateGrid(this);
        }
        else { transform.position += new Vector3(1, 0, 0); }
    }
    void MoveDown()
    {
        if (movedImmediateVertical)
        {


            if (buttonDownWaitTimer < buttonDownWaitMax)
            {
                buttonDownWaitTimer += Time.deltaTime;
                return;
            }
            if (verticalTimer < continuousVerticalSpeed)
            {
                verticalTimer += Time.deltaTime;
                return;
            }
        }
        else
        {
            movedImmediateVertical = true;
        }
        verticalTimer = 0;
        transform.position += new Vector3(0, -1, 0);

        if (CheckIsValidPosition())
        {
            FindObjectOfType<Game>().UpdateGrid(this);
        }
        else
        {
            transform.position += new Vector3(0, 1, 0);
            FindObjectOfType<Game>().DeleteRow();

            if (FindObjectOfType<Game>().CheckIsAboveGrid(this))
            {
                FindObjectOfType<Game>().GameOver();
            }


            enabled = false;
            FindObjectOfType<Game>().SpawnNextTetromino();
        }

        fall = Time.time;
    }
    void Rotate()
    {
        if (allowRotation)
        {
            if (limitRotation)
            {
                if (transform.rotation.eulerAngles.z >= 90)
                {
                    transform.Rotate(0, 0, -90);
                }
                else
                {
                    transform.Rotate(0, 0, 90);
                }
            }
            else
            {
                transform.Rotate(0, 0, 90);
            }


        }
        if (CheckIsValidPosition())
        {
            FindObjectOfType<Game>().UpdateGrid(this);
        }
        else
        {
            if (limitRotation)
            {
                if (transform.rotation.eulerAngles.z >= 90)
                {
                    transform.Rotate(0, 0, -90);
                }
                else
                {
                    transform.Rotate(0, 0, 90);
                }
            }
            else
            {
                transform.Rotate(0, 0, -90);
            }
        }
    }

}

