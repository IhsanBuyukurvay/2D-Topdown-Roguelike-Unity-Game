using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    private void Awake()
    {
        if (GameManager.instance != null)
        {
            Destroy(gameObject);
            Destroy(player.gameObject);
            Destroy(floatingTextManager.gameObject);
            return;
        }
        
        instance = this;
        SceneManager.sceneLoaded += LoadState;
         
    }

    //Resources
    public List<Sprite> playerSprites;
    public List<Sprite> weaponStripes;
    public List<int> weaponPrices;
    public List<int> xpTable;

    //references
    public PlayerMove player;
    public Weapon weapon;
    public Animator deathMenuAnim;
    public FloatingTextManager floatingTextManager;
    public RectTransform hitpointBar;

    //logic
    public int coins;
    public int experience;

    //Floatingobject
    public void ShowText(string msg, int fontSize, Color color, Vector3 position, Vector3 motion, float duration)
    {
        floatingTextManager.Show(msg, fontSize, color, position, motion, duration);
    }

    public bool TryUpgradeWeapon()
    {
        //max levela ula�t�m� silah kontrol et
        if (weaponPrices.Count <= weapon.weaponLevel)
            return false;
        //e�er elinde yeteri kadar coin varsa silah� sat�n al true
        if(coins>= weaponPrices[weapon.weaponLevel])
        {
            coins -= weaponPrices[weapon.weaponLevel];
            weapon.UpgradeWeapon();
            return true;
        }
        
        //coin yoksa bb
        return false;
    }

    //Healthbar
    public void OnHitpointChange()
    {
        float ratio = (float)player.hitPoint / (float)player.maxHitpoint;
        hitpointBar.localScale = new Vector3(ratio, 1, 1);
    }

    
    //Experience

    

    public int GetCurrentLevel()
    {
        int r = 0;
        int add = 0;

        while(experience >= add)
        {
            add += xpTable[r];
            r++;

            if (r == xpTable.Count) // MAX oldu sadece r d�nd�r.
                return r;
        }

        return r;
    }
    
    //xp al lvl atla
    public int GetXpToLevel(int level)
    {
        int r = 0;
        int xp = 0;

        while( r < level)
        {
            xp += xpTable[r];
            r++;
        }

        return xp;
    }

    public void GrantXp(int xp)
    {
        int currLevel = GetCurrentLevel();
        experience += xp;
        if (currLevel < GetCurrentLevel())
            OnLevelUp();
    }

    public void OnLevelUp()
    {
        ShowText("Level Up!", 45, Color.yellow, player.transform.position, transform.up * 35, 1.50f);
        Debug.Log("LevelUp");
        player.OnLevelUp();
        OnHitpointChange();
    }

    //Respawn
    public void Respawn()
    {
        deathMenuAnim.SetTrigger("Hide");
        UnityEngine.SceneManagement.SceneManager.LoadScene("Level 01");
        player.transform.position = GameObject.Find("SpawnPoint").transform.position;
        player.Respawn();
    }

    


    //Saving Progress    
    public void SaveState()
    {
        string s = "";
        s += "0" + "|";
        s += coins.ToString() + "|";
        s += experience.ToString() + "|";
        s += weapon.weaponLevel.ToString(); ;

        PlayerPrefs.SetString("SaveState", s);

        Debug.Log("Save State");
        
    }
    public void LoadState(Scene s ,LoadSceneMode mode)
    {
        Debug.Log("Load State");

        if (PlayerPrefs.HasKey("SaveState"))
            return;
        
        string[] data = PlayerPrefs.GetString("SaveState").Split('|');

        //Change Player Skin (burdaki fonksiyon sonra yaz�lacak veya olmayabilir eklemek i�in d�n��
        coins = int.Parse(data[1]);
        experience = int.Parse(data[2]);
        player.SetLevel(GetCurrentLevel());
        //Weapon Level Levellarda ilerlendik�e veya camavar �ld�rd�k�e weapon sprite� de�i�tirilebilir eklemek i�in d�n��
        weapon.SetWeaponLevel(int.Parse(data[3]));

        //Spawn Point ayarlamam�z gerekti yoksa scene de�i�tirirken random yerlere at�yor nedeni bilinmeyen bir �ekilde
        GameObject.Find("Player").transform.position = GameObject.Find("SpawnPoint").transform.position;
    }
}
