using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GunStatUI : MonoBehaviour
{
    [SerializeField] private Text labelTxt;
    [SerializeField] private Text valueTxt;
    [SerializeField] private Text updateValueTxt;
    public void UpdateGun( string label, string value, string updateValue)
    {
        if (label != null) labelTxt.text = label;
        if(value != null) valueTxt.text = value;
        if(updateValue != null) updateValueTxt.text = updateValue;
    }
}
