using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_coin : Item
{
    public override void Trigger()
    {
        Prefs.coins += m_Bonus;
        GUIManager.Ins.ShowCoinGUI(Prefs.coins);
        AudioController.Ins.PlaySound(AudioController.Ins.coinPickup);
    }

}
