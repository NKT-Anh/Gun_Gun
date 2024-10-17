
using UnityEngine;

public class ItemHeath : Item
{
    public override void Trigger()
    {
        if(m_Player == null)
            return;
        m_Player.CurrentHP += m_Bonus;
        m_Player.CurrentHP = Mathf.Clamp(m_Player.CurrentHP, 0, m_Player.PlayerStats.hp);
        GUIManager.Ins.UpDateHpInfo(m_Player.CurrentHP,m_Player.PlayerStats.hp);
        AudioController.Ins.PlaySound(AudioController.Ins.healthPickup);
    }
}
