using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelItem : Item
{
    public override void Trigger()
    {
       
        if (m_Player == null)
            return;
        m_Player.PlayerStats.xp += m_Bonus;
        GUIManager.Ins.UpDateLevelInfo(m_Player.PlayerStats.level, m_Player.PlayerStats.xp, m_Player.PlayerStats.leverUpXP);
        AudioController.Ins.PlaySound(AudioController.Ins.levelUp);
    }
}
