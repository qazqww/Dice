using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DiceUse : MonoBehaviour, IDragHandler, IPointerUpHandler, IPointerDownHandler
{
    Character player;
    CharacterStatus playerStatus;
    Board board;
    Die dice;
    GameObject resetPoint;

    Canvas canvas;
    GraphicRaycaster gr;
    PointerEventData ped;

    GameObject dice_temp; // 기존의 3d 주사위, => 배열(리스트)로
    public GameObject SetDiceTemp
    {
        set { dice_temp = value; }
    }

    int value;
    public int Value
    {
        get { return value; }
        set { this.value = value; }
    }

    int funcValue;
    public int FuncValue
    {
        get { return funcValue; }
    }

    void Start()
    {
        board = FindObjectOfType<Board>();
        dice = GetComponent<Die>();
        resetPoint = GameObject.Find("resetPoint");

        canvas = GameObject.Find("Canvas").GetComponent<Canvas>();
        gr = canvas.GetComponent<GraphicRaycaster>();
        ped = new PointerEventData(null);

        player = GameObject.Find("PlayerOne").GetComponent<Character>();
        playerStatus = player.GetComponent<CharacterStatus>();
    }

    void Update()
    {
        // 굴려진 주사위를 누를 때, 3d 주사위 비활성화
        if (Input.GetMouseButtonDown(0))
        {
            ped.position = Input.mousePosition;
            List<RaycastResult> results = new List<RaycastResult>(); // 여기에 히트 된 개체 저장
            gr.Raycast(ped, results);
            if (results.Count != 0)
            {
                GameObject obj = results[0].gameObject;
                if (obj.transform != null)
                {
                    if(obj.transform.tag == "DiceUI" && dice_temp != null)
                    {
                        board.dices.Remove(dice_temp);
                        dice_temp.SetActive(false);
                    }
                }
            }
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position += (Vector3)eventData.delta;
    }

    // 드래그 놓을 때
    public void OnPointerUp(PointerEventData eventData)
    {
        ped.position = Input.mousePosition;
        List<RaycastResult> results = new List<RaycastResult>(); // 여기에 히트 된 개체 저장
        gr.Raycast(ped, results);
        if (results.Count > 1)
        {
            GameObject obj = results[1].gameObject;
            if (obj.transform != null && obj.transform.tag == "DicePlace") // 히트 된 오브젝트의 태그와 맞으면 실행
            {
                // 주사위 위치를 잡아주는 코드
                transform.position = new Vector2(obj.transform.position.x, 128);

                switch(obj.transform.name)
                {
                    case "HP+":
                        funcValue = 0;
                        break;
                    case "ATK+":
                        funcValue = 1;
                        break;
                    case "DEF+":
                        funcValue = 2;
                        break;
                    case "Move":
                        funcValue = 3;
                        break;
                    case "GOLD+":
                        funcValue = 4;
                        break;
                }

                /*
                switch (obj.transform.name)
                {
                    case "Move":
                        player.GetMove(value);
                        Board.diceCount--;
                        Destroy(gameObject);
                        break;
                    case "HP+":
                        playerStatus.Hp = value;
                        Board.diceCount--;
                        Destroy(gameObject);
                        break;
                    case "ATK+":
                        playerStatus.Atk = value;
                        Board.diceCount--;
                        Destroy(gameObject);
                        break;
                    case "DEF+":
                        playerStatus.Def = value;
                        Board.diceCount--;
                        Destroy(gameObject);
                        break;
                    case "GOLD+":
                        playerStatus.Gold = 7-value;
                        Board.diceCount--;
                        Destroy(gameObject);
                        break;
                    default:
                        Debug.Log(obj.transform.name);
                        break;
                }
                */

                //board.dices.Remove(gameObject);                
            }
        }
        else if (results.Count == 1)
        {
            transform.position = Camera.main.WorldToScreenPoint(resetPoint.transform.position);
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {

    }

    public void DiceUsing(int func, int val)
    {
        switch (func)
        {
            case 0:
                playerStatus.Hp = val;
                Board.diceCount--;
                Destroy(gameObject);
                break;
            case 1:
                playerStatus.Atk = val;
                Board.diceCount--;
                Destroy(gameObject);
                break;
            case 2:
                playerStatus.Def = val;
                Board.diceCount--;
                Destroy(gameObject);
                break;
            case 3:
                player.GetMove(val);
                Board.diceCount--;
                Destroy(gameObject);
                break;
            case 4:
                playerStatus.Gold = 7 - val;
                Board.diceCount--;
                Destroy(gameObject);
                break;
        }
    }
}
