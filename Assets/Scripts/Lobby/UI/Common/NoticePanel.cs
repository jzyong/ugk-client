using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NoticePanel : MonoBehaviour
{

    [SerializeField] private Button confirm;

    [SerializeField] private TMP_Text message;
    
    // Start is called before the first frame update
    void Start()
    {
        confirm.onClick.AddListener(Hide);
    }


    public void Show(String msg)
    {
        message.text = msg;
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
