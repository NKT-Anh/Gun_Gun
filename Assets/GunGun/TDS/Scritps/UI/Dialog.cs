using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Dialog : MonoBehaviour
{
    public Text titleTxt;
    public Text contetTxt;
    
    public virtual void Show(bool isShow)
    {
        gameObject.SetActive(isShow);
    }
    public virtual void UpdateDialog(string title, string content)
    {
        if (titleTxt != null) { titleTxt.text = title; }
        if (contetTxt != null) { contetTxt.text = content; }

    }
    public virtual void OnGUI()
    {

    }
    public virtual void Close()
    {
        Show(false);
    }
}
