using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Board : MonoBehaviour
{
    // static 고려해보기
    public List<GameObject> dices = new List<GameObject>();
    private string galleryDie = "d6-red";
    bool diceActive = false;

    [HideInInspector]
    public int score = 0;
    public Text scoreText;

    GameObject spawnPoint = null;

    public GameObject canvas;
    GameObject[] dice_eye = new GameObject[6];
    Image eyeImg;

    void Start()
    {
        spawnPoint = GameObject.Find("spawnPoint");
        for(int i=0; i<dice_eye.Length; i++)
        {
            dice_eye[i] = Resources.Load<GameObject>("eye" + i);
        }
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.transform.tag == "Dice")
                {
                    Die dice = hit.transform.GetComponent<Die>();
                    if (!dice.rolling)
                        Debug.Log(dice.value);
                }
            }
        }
    }

    private Vector3 Force()
    {
        Vector3 rollTarget = Vector3.zero + new Vector3(2 + 7 * Random.value, .5F + 4 * Random.value, -2 - 3 * Random.value);
        return Vector3.Lerp(spawnPoint.transform.position, rollTarget, 1).normalized * (-35 - Random.value * 20);
    }

    // 주사위를 굴리는 코드
    public void UpdateRoll()
    {
        if (CheckRolling())
            return;

        DiceUse.canDrag = false;
        Dice.Clear();
        string[] a = galleryDie.Split('-');
        Dice.Roll("2" + a[0], galleryDie, spawnPoint.transform.position, Force());

        //else if(Input.GetKeyDown(KeyCode.R))
        //{
        //    int sum = 0;
        //    for (int i = 0; i < dices.Count; i++)
        //        sum += dices[i].GetComponent<Die>().value;

        //    Debug.Log(sum);
        //}
    }

    // 주사위가 구르고 있는지 return해주는 코드
    public bool CheckRolling()
    {
        for (int i = 0; i < dices.Count; i++)
        {
            if (dices[i].GetComponent<Die>().rolling)
                return true;
        }
        return false;
    }
    
    public void DiceToUI(int eyes)
    {
        // 이미지가 없다면 만들고
        // 마우스 포지션으로 갖다놓음
        
        // switch case로 눈 개수 받고
        // 게임오브젝트에 눈 개수에 맞는 Image 넣어줌
        // 게임오브젝트 생성하여 넘겨줌

        // 요소 - 이미지를 넣은 빈 게임오브젝트(반환), 이미지가 담긴 이미지 배열

        //var testImg = Instantiate(test) as GameObject;
        //testImg.transform.SetParent(canvas.transform, false);
    }
}
