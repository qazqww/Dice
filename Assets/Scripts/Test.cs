using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Test : MonoBehaviour
{
    public Canvas mycanvas; // raycast가 될 캔버스
    GraphicRaycaster gr;
    PointerEventData ped;

    void Start()
    {
        gr = mycanvas.GetComponent<GraphicRaycaster>();
        ped = new PointerEventData(null);
    }

    void Update()
    {
        ped.position = Input.mousePosition;
        List<RaycastResult> results = new List<RaycastResult>(); // 여기에 히트 된 개체 저장
        gr.Raycast(ped, results);
        if (results.Count != 0)
        {
            GameObject obj = results[0].gameObject;
            if (obj.transform != null) // 히트 된 오브젝트의 태그와 맞으면 실행
            {
                Debug.Log("hit !");
            }
        }
    }
}
