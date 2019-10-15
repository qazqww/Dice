using System.Collections;
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
            Board.diceCount++;
            DiceToUI();
            isChanged = true;
        }
    }

    void DiceToUI()
    {
        eyeObj = Instantiate(eyeUI) as GameObject;
        eyeObj.transform.SetParent(canvas.transform);
        DiceUse newDice = eyeObj.GetComponent<DiceUse>();
        eyeImg = newDice.GetComponent<Image>();
        newDice.Value = dice.value;
        newDice.SetDiceTemp = gameObject;
        eyeImg.sprite = dice_eye[dice.value - 1];
        eyeObj.transform.position = Camera.main.WorldToScreenPoint(transform.position);
    }
}
