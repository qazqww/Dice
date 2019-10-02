using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DiceUse : MonoBehaviour, IDragHandler
{
    Character player;
    CharacterStatus playerStatus;
    Board board;
    Die dice;

    Canvas canvas;
    GraphicRaycaster gr;
    PointerEventData ped;
    Vector2 pos;
    
    Sprite[] dice_eye = new Sprite[6];
    GameObject dice_temp; // => 배열(리스트)로
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

        canvas = GameObject.Find("Canvas").GetComponent<Canvas>();
        gr = canvas.GetComponent<GraphicRaycaster>();
        ped = new PointerEventData(null);
        pos = GetComponent<RectTransform>().anchoredPosition;

        player = GameObject.Find("PlayerOne").GetComponent<Character>();
        playerStatus = player.GetComponent<CharacterStatus>();

        for (int i = 0; i < dice_eye.Length; i++)
            dice_eye[i] = Resources.Load<Sprite>("eye" + (i + 1));
    }

    void Update()
    {
        //RaycastHit hit;
        //Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        //if (Physics.Raycast(ray.origin, ray.direction, out hit))
        //{
        //    if (hit.transform.tag == "DiceUI")
        //    {
        //        Debug.Log("AA+");
        //        dice_temp.SetActive(false);
        //    }
        //}

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
                    if(obj.transform.tag == "DiceUI")
                    {
                        dice_temp.SetActive(false);
                    }
                }
            }
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position += (Vector3)eventData.delta;
        //pos = Input.mousePosition;
        Debug.Log("dragging");
    }

    //private void OnMouseDrag()
    //{
    //    pos = Input.mousePosition;
    //    Debug.Log("dragging");
    //}

    private void OnMouseUp()
    {/*
        ped.position = Input.mousePosition;
        List<RaycastResult> results = new List<RaycastResult>(); // 여기에 히트 된 개체 저장
        gr.Raycast(ped, results);
        if (results.Count != 0)
        {
            GameObject obj = results[0].gameObject;
            if (obj.transform != null) // 히트 된 오브젝트의 태그와 맞으면 실행
            {
                switch (obj.transform.name)
                {
                    case "Move":
                        player.GetMove(dice.value);
                        break;
                    case "HP+":
                        playerStatus.Hp = dice.value;
                        break;
                    case "ATK+":
                        playerStatus.Atk = dice.value;
                        break;
                    case "DEF+":
                        playerStatus.Def = dice.value;
                        break;
                    case "GOLD+":
                        playerStatus.Gold = dice.value;
                        break;
                    case "Fire":
                        break;
                    case "Water":
                        break;
                    case "Grass":
                        break;
                    default:
                        break;
                }

                board.dices.Remove(gameObject);
                Destroy(gameObject);
            }
        }*/
    }
}
