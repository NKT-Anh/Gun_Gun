using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemLife : Item
{
    public override void Trigger()
    {
        GameManager.Ins.CurrenLife += m_Bonus;
        GUIManager.Ins.UpdateLife(GameManager.Ins.CurrenLife);
        AudioController.Ins.PlaySound(AudioController.Ins.lifePickup);
    }
}
