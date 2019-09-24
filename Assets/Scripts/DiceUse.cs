﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DiceUse : MonoBehaviour
{
    public static bool canDrag = false;
    Character player;
    Board board;
    Die dice;

    Rigidbody rb;

    Canvas canvas;
    GraphicRaycaster gr;
    PointerEventData ped;

    void Start()
    {
        board = FindObjectOfType<Board>();
        dice = GetComponent<Die>();
        rb = transform.GetComponent<Rigidbody>();

        canvas = GameObject.Find("Canvas").GetComponent<Canvas>();
        gr = canvas.GetComponent<GraphicRaycaster>();
        ped = new PointerEventData(null);
        
        player = GameObject.Find("PlayerOne").GetComponent<Character>();
    }

    void Update()
    {
        if (!board.CheckRolling())
            canDrag = true;
    }

    private void OnMouseDrag()
    {
        if (canDrag)
        {
            rb.constraints = RigidbodyConstraints.FreezeRotation;

            Vector3 mousePos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10);
            transform.position = Camera.main.ScreenToWorldPoint(mousePos);
        }
    }

    private void OnMouseUp()
    {
        rb.constraints = RigidbodyConstraints.None;

        ped.position = Input.mousePosition;
        List<RaycastResult> results = new List<RaycastResult>(); // 여기에 히트 된 개체 저장
        gr.Raycast(ped, results);
        if (results.Count != 0)
        {
            GameObject obj = results[0].gameObject;
            if (obj.transform != null) // 히트 된 오브젝트의 태그와 맞으면 실행
            {
                Debug.Log(obj.transform.name);
                Text text = obj.GetComponentInChildren<Text>();
                text.text += "짠";

                switch(obj.transform.name)
                {
                    case "Move":
                        board.dices.Remove(gameObject);
                        Destroy(gameObject);
                        player.GetMove(dice.value);
                        break;
                }
            }
        }
    }
    /*
    private void OnCollisionEnter(Collision coll)
    {
        if(coll.transform.tag == "Score")
        {
            board.score += dice.value;
            board.dices.Remove(gameObject);
            Destroy(gameObject);
        }

        else if (coll.transform.tag == "CharMove")
        {
            int moveCount = dice.value;
            board.dices.Remove(gameObject);
            Destroy(gameObject);
            //player.GetMove(moveCount);
        }
    }
    */
}
