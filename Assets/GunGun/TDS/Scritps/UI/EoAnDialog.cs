using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EoAnDialog : Dialog
{
    [SerializeField] private TextMeshProUGUI m_notificationText;
    private Coroutine hideCoroutine;



    public override void Show(bool isShow)
    {
        base.Show(isShow);
        ShowNotification();
        Time.timeScale = 0f;

    }
    public void UpCoid()
    {
        Prefs.coins += 5000;
        GUIManager.Ins.ShowCoinGUI(Prefs.coins);
        Close();
    }
    public void ShowNotification()
    {
        if (m_notificationText != null)
        {
            m_notificationText.text = $"Limit your clicking, my friend";
            m_notificationText.gameObject.SetActive(true);
            if (hideCoroutine != null)
            {
                StopCoroutine(hideCoroutine);
            } 
        }
    }
    public override void Close()
    {
        base.Close();
        Time.timeScale = 1f;
    }

}
