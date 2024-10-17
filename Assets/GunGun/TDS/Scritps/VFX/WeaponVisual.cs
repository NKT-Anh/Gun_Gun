using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Weapon))]
public class WeaponVisual : MonoBehaviour
{
    [SerializeField] private AudioClip m_shoting;
    [SerializeField] private AudioClip m_reload;

    private Weapon Weapon;
    private void Awake()
    {
        Weapon = GetComponent<Weapon>();

    }
    public void OnShot()
    {
        AudioController.Ins.PlaySound(m_shoting);
        CineController.Ins.ShakeTrigger();

    }
    public void OnReload()
    {
        GUIManager.Ins.ShowReloadStateGUI(true);
        

    }
    public void OnReloadDone()
    {
        GUIManager.Ins.ShowReloadStateGUI(false);
        AudioController.Ins.PlaySound(m_reload);
    }
}
