using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QualItem : MonoBehaviour
{

    public static int index;

    Toggle tog;
    public void SelectThis()
    {
        if (tog.isOn)
        {
            tog.Select();
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        tog = GetComponent<Toggle>();

    }

    // Update is called once per frame
    void Update()
    {
        string str = gameObject.name;

        int i = str.IndexOf(' ');
        int j = str.IndexOf(':');

        string s = str.Substring(i + 1, j - i - 1);

        //Debug.Log("\\" + s + "/");
        if (int.Parse(s) == index)
        {
            tog.Select();
        }
    }

}
