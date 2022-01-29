using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterMenu : MonoBehaviour
{
    public Text levelText, hitpointText, coinsText, upgradeCostText, xptext;

    public Image WeaponSprite;
    public RectTransform xpBar;

    public void OnClickUpgrade()
    {
        if (GameManager.instance.TryUpgradeWeapon())
            UpdateMenu();
        //gamemanagerdan upgrade almamýz lazým
    }

    //Karakter bilgisi için bilgi
    public void UpdateMenu()
    {
        //weapon
        WeaponSprite.sprite = GameManager.instance.weaponStripes[GameManager.instance.weapon.weaponLevel];
        if (GameManager.instance.weapon.weaponLevel == GameManager.instance.weaponPrices.Count-1)
            upgradeCostText.text = "MAX";
        else
        upgradeCostText.text = GameManager.instance.weaponPrices[GameManager.instance.weapon.weaponLevel].ToString();

        //hitpoint
        levelText.text = GameManager.instance.GetCurrentLevel().ToString();
        coinsText.text = GameManager.instance.coins.ToString();
        //hitpointText.text = "Eklenmedi!";
        hitpointText.text = GameManager.instance.player.hitPoint.ToString();

        int currLevel = GameManager.instance.GetCurrentLevel();

        // Tecrübe Çubuðu
        if (currLevel == GameManager.instance.xpTable.Count)
        {
            xptext.text = GameManager.instance.experience.ToString() + " Total EXP Points ";
            xpBar.localScale = Vector3.one;
        }
        else
        {
            int prevLevelXp = GameManager.instance.GetXpToLevel(currLevel - 1);
            int currLevelXp = GameManager.instance.GetXpToLevel(currLevel);

            int diff = currLevelXp - prevLevelXp;
            int currXpIntoLevel = GameManager.instance.experience - prevLevelXp;

            float completionRatio = (float)currXpIntoLevel / (float)diff;
            xpBar.localScale = new Vector3(completionRatio, 1, 1);
            xptext.text = currXpIntoLevel.ToString() + " / " + diff;

        }
    }
}
