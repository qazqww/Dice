using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DiceUse : MonoBehaviour, IDragHandler, IPointerUpHandler, IPointerDownHandler
{
    Camera diceCam;
    Character player;
    CharacterStatus playerStatus;
    Board board;
    Die dice;
    GameObject resetPoint;
    Image myImage;
    Image image;
    public Image Image
    {
        get { return image; }
        set { image = value; }
    }

    Vector3 oldPos = Vector3.zero;
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

    int funcValue = -1;
    public int FuncValue
    {
        get { return funcValue; }
    }

    void Awake()
    {
        diceCam = GameObject.Find("diceCamera").GetComponent<Camera>();
        board = FindObjectOfType<Board>();
        dice = GetComponent<Die>();
        resetPoint = GameObject.Find("resetPoint");
        myImage = GetComponent<Image>();
        image = transform.Find("Image").GetComponent<Image>();

        canvas = GameObject.Find("Canvas").GetComponent<Canvas>();
        gr = canvas.GetComponent<GraphicRaycaster>();
        ped = new PointerEventData(null);
        
        player = GameObject.Find("PlayerOne").GetComponent<Character>();
        playerStatus = player.GetComponent<CharacterStatus>();
    }

    void Update()
    {
        if (image.sprite == null)
            Destroy(gameObject);

        if (funcValue > 0)
            myImage.enabled = true;
        else
            myImage.enabled = false;

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
                    if(obj.transform.tag == "DiceEyeUI")
                    {
                        if (dice_temp != null)
                        {
                            board.dices.Remove(dice_temp);
                            dice_temp.SetActive(false);
                        }
                        oldPos = transform.position;
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
        List<RaycastResult> results = new List<RaycastResult>();
        gr.Raycast(ped, results);
        if (results.Count > 1)
        {
            GameObject obj = results[results.Count - 1].gameObject;     // obj에 일단 슬롯을 담음
            if (obj.transform != null)
            {
                if (obj.transform.tag == "DicePlace")
                {
                    // 다른 주사위가 존재하는 곳인지 판정
                    DiceUse oldDice = null;
                    for (int i = 0; i < results.Count; i++)
                    {
                        if (results[i].gameObject.tag == "DiceUI" && results[i].gameObject != gameObject)
                            oldDice = results[i].gameObject.GetComponent<DiceUse>();
                    }

                    if (oldDice != null)
                    {
                        transform.position = oldDice.transform.position;
                        oldDice.transform.position = oldPos;
                        Swap(ref oldDice.funcValue, ref funcValue);
                    }
                    else
                    {
                        transform.position = new Vector2(obj.transform.position.x, 220); // 주사위 위치
                        AudioManager.Instance.PlayUISound(SoundType.diceput);

                        switch (obj.transform.name)
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
                                if (!Board.moveLocked)
                                    funcValue = 3;
                                else
                                    funcValue = 5;
                                break;
                            case "GOLD+":
                                funcValue = 4;
                                break;
                            default:
                                funcValue = -1;
                                break;
                        }
                    }
                }                
                else // DicePlace가 아닌 다른 UI에 주사위를 놓는 경우
                {
                    transform.position = diceCam.WorldToScreenPoint(resetPoint.transform.position);
                    funcValue = -1;
                }
            }
        }
        else if (results.Count == 1) // UI가 아닌 곳에 
        {
            transform.position = diceCam.WorldToScreenPoint(resetPoint.transform.position);
            if(funcValue >= 0)
                Board.diceFunc[funcValue] = 0;
            funcValue = -1;
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {

    }

    void Swap<T>(ref T LSide, ref T RSide)
    {
        T Temp = LSide;
        LSide = RSide;
        RSide = Temp;
    }
}
