using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerVisual : ActorVisual
{
    [SerializeField] private GameObject m_deathVfx;
    private Player m_player;
    private PlayerStats stats;
    private void Start()
    {
        m_player = (Player)m_actor;
        stats = m_player.PlayerStats;

    }
    public override void OnTakeDamage()
    {
        base.OnTakeDamage();
        GUIManager.Ins.UpDateHpInfo(m_actor.CurrentHP, m_actor.stastData.hp);
    }
    public void OnLostLife()
     {
        if(m_player == null) return;
        AudioController.Ins.PlaySound(AudioController.Ins.lostLife);
        GUIManager.Ins.UpdateLife(GameManager.Ins.CurrenLife);
        GUIManager.Ins.UpDateHpInfo(m_player.CurrentHP, stats.hp);
    }
    public void OnDead()
    {
        if (m_deathVfx)
        {
            Instantiate(m_deathVfx,transform.position,Quaternion.identity);
        }
        AudioController.Ins.PlaySound(AudioController.Ins.playerDeath);
        GUIManager.Ins.ShowGameOverDialog();
    }
    public void OnAddxp()
    {
        if (stats == null) return;

        GUIManager.Ins.UpDateLevelInfo(stats.level,stats.xp,stats.leverUpXP);
    }
    public void LevelUp()
    {
        AudioController.Ins.PlaySound(AudioController.Ins.levelUp);
    }

}
