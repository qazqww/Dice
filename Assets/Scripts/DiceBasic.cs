﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DiceBasic : MonoBehaviour
{
    public static bool canChange = false;
    bool isChanged = false;
    
    Rigidbody rb;

    Die dice;
    Canvas canvas;
    Camera diceCam;
    GameObject eyeUI;
    GameObject eyeObj;
    Image eyeImg;
    Sprite[] dice_eye = new Sprite[6];

    GameObject walls;
    float elapsedTime = 0f;

    void Start()
    {
        diceCam = GameObject.Find("diceCamera").GetComponent<Camera>();
        rb = GetComponent<Rigidbody>();
        dice = GetComponent<Die>();
        canvas = GameObject.Find("Canvas").GetComponent<Canvas>();
        eyeUI = Resources.Load<GameObject>("Eye");

        walls = GameObject.Find("Dice-RollPlace").transform.Find("Walls").gameObject;
        walls.SetActive(true);

        for (int i = 0; i < dice_eye.Length; i++)
            dice_eye[i] = Resources.Load<Sprite>("Images/eye" + (i + 1));

        rb.AddForce(new Vector3(-500, 0, 0));
    }

    void Update()
    {
        elapsedTime += Time.deltaTime;

        if (!dice.rolling && !isChanged && elapsedTime > 0.33f) // 시간은 dice 누르자마자 주사위값 뜨는거 방지
        {
            if (dice.value >= 1 && dice.value <= 6)
            {
                canChange = true;
                DiceToUI();
                isChanged = true;
                elapsedTime = 0f;
            }
        }

        if (elapsedTime > 3.9f && elapsedTime < 4.15f) // 벽 끼임 방지
        {
            walls.SetActive(false);
            rb.AddForce(new Vector3(0, 20, 0));
        }
    }

    void DiceToUI()
    {
        eyeObj = Instantiate(eyeUI) as GameObject;
        eyeObj.transform.SetParent(canvas.transform);
        DiceUse newDice = eyeObj.GetComponent<DiceUse>();

        switch(dice.value) // 1,6 -> 1 / 2,5 -> 2 / 3,4 -> 3
        {
            case 1:
            case 6:
                newDice.Value = 1;
                break;
            case 2:
            case 5:
                newDice.Value = 2;
                break;
            case 3:
            case 4:
                newDice.Value = 3;
                break;
            default:
                return;
        }
        if (Character.itemOn == (int)ItemName.DiceUp)
        {
            Character.itemOn = -1;
            newDice.Value++;
        }
        newDice.SetDiceTemp = gameObject;
        newDice.Image.sprite = dice_eye[newDice.Value - 1];
        eyeObj.transform.position = diceCam.WorldToScreenPoint(transform.position);
        Board.diceUIs.Add(eyeObj);
    }
}
