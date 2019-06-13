using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseMove : MonoBehaviour
{
    // Player Move
    [Range(0.5f, 20f)]
    [SerializeField] private float SpeedHorizontal;
    [Range(0.5f, 35)]
    [SerializeField] private float SpeedVertical;
    [Space]
    [Range(0.5f, 4f)]
    [SerializeField] private float DistanceBetweenHorizontal;
    [Range(0.5f, 10f)]
    [SerializeField] private float DistanceBetweenVertical;
    [HideInInspector] public float DistanceBetweenHorizontalSave;
    [HideInInspector] public float DistanceBetweenVerticalSave;

    [HideInInspector] public bool CanUpDown = false;


    Rigidbody physic;
    //[SerializeField] private Transform MoveVector; 

    private bool PlayerChangePos = false;
    private int PlayerPositionX = 0;         // Left from Centr < 0 ||  Right from Centr > 0
    private int PlayerPositionY = 0;

    // Player Move Mode
    private Move _Move;


    // Mouse State
    private bool Click = false;
    private bool ClickMouseButtonDown = false;

    private Vector3 StartPlayerPos;


    // Mouse Position
    private Vector3 MouseClickPos;




    // Start is called before the first frame update
    void Awake()
    {
        DistanceBetweenHorizontalSave = DistanceBetweenHorizontal;
        DistanceBetweenVerticalSave = DistanceBetweenVertical;
        _Move = Move.Stay;
        StartPlayerPos = transform.position;

        physic = gameObject.GetComponent<Rigidbody>();
    }

    public void UpDown(bool Can)
    {
        CanUpDown = Can;
        if (!Can && transform.position.y >= 9) Fall();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0)) ClickMouseButtonDown = true;
        else if (Input.GetMouseButtonUp(0)) { ClickMouseButtonDown = false; Click = false; } 

        

        // Move When Mouse Button Down
        if (ClickMouseButtonDown)
        {
            if (!Click)
            {
                Click = true;
                MouseClickPos = Input.mousePosition;
            }

            if (CanUpDown) // Move Player Left - Right if Level 3 (LevelUpDown/ Two platforms)
            {
                if ((Input.mousePosition.y < MouseClickPos.y + 30 && Input.mousePosition.y > MouseClickPos.y - 30) && Input.mousePosition.x > MouseClickPos.x + 10 && PlayerPositionX < 1 && _Move == Move.Stay) // Move Right
                {
                    //print("Right");
                    _Move = Move.Right;
                    PlayerPositionX++;
                    MoveFunc();
                }
                else if ((Input.mousePosition.y < MouseClickPos.y + 30 && Input.mousePosition.y > MouseClickPos.y - 30) && Input.mousePosition.x < MouseClickPos.x - 10 && PlayerPositionX > -1 && _Move == Move.Stay) // Move Left
                {
                    _Move = Move.Left;
                    PlayerPositionX--;
                    MoveFunc();
                }
                else if (Input.mousePosition.y > MouseClickPos.y + 8 && PlayerPositionY < 1 && _Move == Move.Stay && CanUpDown) // Move Up
                {
                    physic.useGravity = false;
                    _Move = Move.Up;
                    PlayerPositionY++;
                    MoveFunc();
                }
                else if (Input.mousePosition.y < MouseClickPos.y - 8 && PlayerPositionY > 0 && _Move == Move.Stay && CanUpDown) // Move Down
                {
                    _Move = Move.Down;
                    PlayerPositionY--;
                    MoveFunc();
                }
            }
            else
            {
                if (Input.mousePosition.x > MouseClickPos.x + 10 && PlayerPositionX < 1 && _Move == Move.Stay) // Move Right
                {
                    //print("Right");
                    _Move = Move.Right;
                    PlayerPositionX++;
                    MoveFunc();
                }
                else if (Input.mousePosition.x < MouseClickPos.x - 10 && PlayerPositionX > -1 && _Move == Move.Stay) // Move Left
                {
                    _Move = Move.Left;
                    PlayerPositionX--;
                    MoveFunc();
                }
            }


        }

        if (PlayerChangePos)    ChangPos();
    }

    private void Fall()
    {
        _Move = Move.Down;
        PlayerPositionY--;
        MoveFunc();
    }

    private void MoveFunc()
    {
        Click = false;
        PlayerChangePos = true;
        ClickMouseButtonDown = false;
    }

    private void ChangPos()
    {
        switch (_Move)
        {
            case Move.Right:
                transform.Translate(transform.right * Time.deltaTime * SpeedHorizontal);
                HorizontalMove();
                break;
            case Move.Left:
                transform.Translate(transform.right * Time.deltaTime * -SpeedHorizontal);
                HorizontalMove();
                break;
            case Move.Up:
                transform.Translate(transform.up * Time.deltaTime * SpeedVertical);
                VerticalMove();
                break;
            case Move.Down:
                transform.Translate(transform.up * Time.deltaTime * -SpeedVertical);
                VerticalMove();
                break;
            default:
                break;
        }
    }

    private void HorizontalMove()
    {
        if (DistanceBetweenHorizontal > 0)
        {
            DistanceBetweenHorizontal -= (Time.deltaTime * SpeedHorizontal);
        }
        else
        {
            transform.position = new Vector3(StartPlayerPos.x - (-PlayerPositionX * DistanceBetweenHorizontalSave), transform.position.y, transform.position.z);

            PlayerChangePos = false;
            _Move = Move.Stay;
            DistanceBetweenHorizontal = DistanceBetweenHorizontalSave;
        }
    }


    private void VerticalMove()
    {
        if (DistanceBetweenVertical > 0)
        {
            DistanceBetweenVertical -= (Time.deltaTime * SpeedVertical);
        }
        else
        {
            transform.position = new Vector3(transform.position.x, StartPlayerPos.y - (-PlayerPositionY * DistanceBetweenVerticalSave), transform.position.z);

            if (_Move == Move.Down)
                physic.useGravity = true;

            PlayerChangePos = false;
            _Move = Move.Stay;
            DistanceBetweenVertical = DistanceBetweenVerticalSave;
        }
    }
}
