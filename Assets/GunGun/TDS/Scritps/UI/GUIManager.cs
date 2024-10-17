using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using static UnityEditor.Progress;

public class GUIManager : Singleton<GUIManager>
{
    [SerializeField] private GameObject m_homeGUI;
    [SerializeField] private GameObject m_gameGUI;

    [SerializeField] private Transform m_lifeGrid;
    [SerializeField] private GameObject m_lifeIconPref;

    [SerializeField] private ImageFilled m_levelProgressBar;
    [SerializeField] private ImageFilled m_hpProgressBar;

    [SerializeField] private Text m_level;
    [SerializeField] private Text m_hp;
    [SerializeField] private TextMeshProUGUI m_coin;
    [SerializeField] private Text m_xp;
    [SerializeField] private Text m_reloadState;

    [SerializeField] private Dialog m_gunUpgradeDialog;
    [SerializeField] private Dialog m_gameoverDialog;
    [SerializeField] private Dialog eoannhabn;


    private Dialog m_activeDialog;

    public Dialog ActiveDialog { get => m_activeDialog; private set => m_activeDialog = value; }

    protected override void Awake()
    {
        MakeSingleton(false);
    }
    public void ShowHomeGUI(bool isShow)
    {
        m_homeGUI.SetActive(isShow);
        m_gameGUI.SetActive(!isShow);
    }
    public void ShowGameGUI(bool isShow) {
        m_gameGUI.SetActive(isShow);
        m_homeGUI.SetActive(!isShow);
    }
    public void ShowDialog(Dialog dialog)
    {
        if (dialog == null) return;
        ActiveDialog = dialog;
        ActiveDialog.Show(true);

    }
    public void ShowGunUpgradeDialog()
    {
        ShowDialog(m_gunUpgradeDialog);
    }
    public void Showeoannhabn()
    {
        ShowDialog(eoannhabn);
    }
    public void ShowGameOverDialog()
    {
        ShowDialog(m_gameoverDialog);
    }
    public void UpdateLife(int life)
    {
        if (m_lifeGrid == null || m_lifeIconPref == null) return;
        foreach (Transform item in m_lifeGrid)
        {
            Destroy(item.gameObject);
        }
        for (int i = 0; i <= life; i++)
        {
            Instantiate(m_lifeIconPref, m_lifeGrid);
        }
    }
    public void UpDateLevelInfo(int curLevel, float curXP, float levelXPUp)
    {
        m_levelProgressBar?.UpdateValue(curXP, levelXPUp);
        if (m_level != null)
        {
            m_level.text = $"LEVEL {curLevel.ToString("00")}";
        } if (m_xp != null)
        {
            m_xp.text = $"{curXP.ToString("00")}/{levelXPUp.ToString("00")}";
            // m_xp.text = $"{Mathf.RoundToInt(curXP)}/{Mathf.RoundToInt(levelXPUp)}";

        }
    }
    public void UpDateHpInfo(float curHp, float maxHp)
    {
        m_hpProgressBar?.UpdateValue(curHp, maxHp);
        if (m_hp != null) {
            m_hp.text = $"{Mathf.RoundToInt(curHp)}/{Mathf.RoundToInt(maxHp)}";
        }

    }
    public void ShowCoinGUI(int coin)
    {
        if (m_coin.text != null)
        {
            m_coin.text = coin.ToString("n0");
        }
    }
    public void ShowReloadStateGUI(bool isShow)
    {
        if (m_reloadState.text != null)
            m_reloadState.text = "Reloading......";
            m_reloadState.gameObject.SetActive(isShow);
    }


    public void UpdateUI(PlayerStats playerStats)
    {
        if (playerStats == null) return;

        m_level.text = $"Level: {playerStats.level}";
        m_hp.text = $"{playerStats.hp}/{playerStats.hp}";
        m_xp.text = $"{playerStats.xp}/{playerStats.leverUpXP}";
    }





}
