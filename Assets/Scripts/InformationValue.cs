using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InformationValue : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _titleTxt;
    [SerializeField] private TextMeshProUGUI _valueTxt;

    public void InittilizerData(string titleName, string value)
    {
        _valueTxt.text = value;
        _titleTxt.text = titleName;
    }
}
