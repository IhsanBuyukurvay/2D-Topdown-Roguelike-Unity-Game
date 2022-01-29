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
        //max levela ulaþtýmý silah kontrol et
        if (weaponPrices.Count <= weapon.weaponLevel)
            return false;
        //eðer elinde yeteri kadar coin varsa silahý satýn al true
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

            if (r == xpTable.Count) // MAX oldu sadece r döndür.
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

        //Change Player Skin (burdaki fonksiyon sonra yazýlacak veya olmayabilir eklemek için dönüþ
        coins = int.Parse(data[1]);
        experience = int.Parse(data[2]);
        player.SetLevel(GetCurrentLevel());
        //Weapon Level Levellarda ilerlendikçe veya camavar öldürdükçe weapon spriteý deðiþtirilebilir eklemek için dönüþ
        weapon.SetWeaponLevel(int.Parse(data[3]));

        //Spawn Point ayarlamamýz gerekti yoksa scene deðiþtirirken random yerlere atýyor nedeni bilinmeyen bir þekilde
        GameObject.Find("Player").transform.position = GameObject.Find("SpawnPoint").transform.position;
    }
}
