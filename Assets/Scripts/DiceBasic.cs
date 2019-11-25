using System.Collections;
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
    Camera diceCamera;
    GameObject eyeUI;
    GameObject eyeObj;
    Image eyeImg;
    Sprite[] dice_eye = new Sprite[6];

    float elapsedTime = 0f;

    void Start()
    {
        diceCamera = GameObject.Find("diceCamera").GetComponent<Camera>();
        rb = GetComponent<Rigidbody>();
        dice = GetComponent<Die>();
        canvas = GameObject.Find("Canvas").GetComponent<Canvas>();
        eyeUI = Resources.Load<GameObject>("Eye");

        for (int i = 0; i < dice_eye.Length; i++)
            dice_eye[i] = Resources.Load<Sprite>("Images/eye" + (i + 1));

        rb.AddForce(new Vector3(-500, 0, 0));
    }

    void Update()
    {
        elapsedTime += Time.deltaTime;

        if (!dice.rolling && !isChanged && elapsedTime > 0.33f) // 시간은 dice 누르자마자 주사위값 뜨는거 방지
        {
            if (dice.value >= 1 || dice.value <= 6)
            {
                canChange = true;
                //Board.diceCount++;
                DiceToUI();
                isChanged = true;
                elapsedTime = 0f;
            }
        }
    }

    void DiceToUI()
    {
        eyeObj = Instantiate(eyeUI) as GameObject;
        eyeObj.transform.SetParent(canvas.transform);
        DiceUse newDice = eyeObj.GetComponent<DiceUse>();
        eyeImg = newDice.GetComponent<Image>();

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
        }
        if (Character.itemOn == (int)ItemName.DiceUp)
        {
            Character.itemOn = -1;
            newDice.Value++;
        }
        //newDice.Value = dice.value;
        newDice.SetDiceTemp = gameObject;
        eyeImg.sprite = dice_eye[newDice.Value - 1];
        eyeObj.transform.position = diceCamera.WorldToScreenPoint(transform.position);
        Board.diceUIs.Add(eyeObj);
    }
}
