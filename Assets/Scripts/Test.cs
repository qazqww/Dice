using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Linq;

public class Test : MonoBehaviour
{
    Dictionary<int, string> place = new Dictionary<int, string>();

    void Start()
    {
        place.Add(10, "ten");
        place.Add(20, "2ten");
        place.Add(30, "3ten");

        Debug.Log(place.ElementAt(0).Key);
        Debug.Log(place.ElementAt(1).Key);
        Debug.Log(place.ElementAt(2).Key);
    }

    void Update()
    {

    }
}
