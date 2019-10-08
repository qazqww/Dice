﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DiceBasic : MonoBehaviour
{
    public static bool canChange = false;
    bool isChanged = false;

    Die dice;
    Canvas canvas;
    GameObject eyeUI;
    GameObject eyeObj;
    Image eyeImg;
    Sprite[] dice_eye = new Sprite[6];

    void Start()
    {
        dice = GetComponent<Die>();

        canvas = GameObject.Find("Canvas").GetComponent<Canvas>();

        eyeUI = Resources.Load<GameObject>("Eye");

        for (int i = 0; i < dice_eye.Length; i++)
            dice_eye[i] = Resources.Load<Sprite>("eye" + (i + 1));
    }

    void Update()
    {
        if (!dice.rolling && !isChanged)
        {
            canChange = true;
            DiceToUI();
            isChanged = true;
        }
    }

    void DiceToUI()
    {
        eyeObj = Instantiate(eyeUI) as GameObject;
        eyeObj.transform.SetParent(canvas.transform);
        //eyeImg = eyeObj.GetComponent<Image>();
        //eyeImg.sprite = dice_eye[dice.value - 1];
        DiceUse newDice = eyeObj.GetComponent<DiceUse>();
        eyeImg = newDice.GetComponent<Image>();
        newDice.Value = dice.value;
        newDice.SetDiceTemp = gameObject;
        eyeImg.sprite = dice_eye[dice.value - 1];
        eyeObj.transform.position = Camera.main.WorldToScreenPoint(transform.position);

        //gameObject.SetActive(false);
    }

    /*
    private void OnMouseDrag()
    {
        if (canDrag)
        {
            rb.constraints = RigidbodyConstraints.FreezeRotation;

            Vector3 mousePos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10);
            transform.position = Camera.main.ScreenToWorldPoint(mousePos);

            if (!isDragging)
            {
                board.DiceToUI(dice.value);
                //dice.gameObject.SetActive(false);
                isDragging = true;
            }

            board.DiceUIMove();
        }
    }*/

        /*
    private void OnMouseUp()
    {
        rb.constraints = RigidbodyConstraints.None;
        isDragging = false;
        //board.DiceUIRemove();
        //dice.gameObject.SetActive(true);

        ped.position = Input.mousePosition;
        List<RaycastResult> results = new List<RaycastResult>(); // 여기에 히트 된 개체 저장
        gr.Raycast(ped, results);
        if (results.Count != 0)
        {
            GameObject obj = results[0].gameObject;
            if (obj.transform != null) // 히트 된 오브젝트의 태그와 맞으면 실행
            {
                switch(obj.transform.name)
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
        }
    }
    */
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
