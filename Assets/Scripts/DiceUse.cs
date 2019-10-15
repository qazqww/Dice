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
        set { this.value = value; }
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
            if (obj.transform != null) // 히트 된 오브젝트의 태그와 맞으면 실행
            {
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
}
