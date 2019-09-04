using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CharacterButtons : MonoBehaviour
{
    [Header("角色立繪替換")]
    public GameObject[] characters;
    //選擇角色的複製物件生成點
    public GameObject spawnPoint;
    [Header("角色按鈕")]
    public Toggle[] characterToggle;
    [Header("角色按鈕選擇文字")]
    public Text[] selected;
    [Header("隊伍頭像格")]
    public Image[] partyImage;
    [Header("隊伍角色頭像替換格")]
    public Sprite[] partyImages;
    [Header("立繪編號")]
    public int characterImageNumber = 0;
    [Header("角色名稱")]
    public Text characterName;
    [Header("角色資訊")]
    public Text characterInfo;

    [Header("進入大廳按鈕")]
    public Button nextBtn;

    public GameObject backBG;

    //選擇角色的複製物件
    GameObject[] characterClone = new GameObject[8];

    //選取確認
    bool confirmSelected = false;

    private void Update()
    {
        //確認是否選滿角色才開放下一頁
        if (partyImage[5].sprite != null)
        {
            nextBtn.interactable = true;
        }
    }

    #region 選擇按鈕
    /// <summary>
    /// 選擇按鈕
    /// </summary>
    public void CharacterSelectButton()
    {
        if (characterImageNumber == 1)
        {
            selected[0].text = "已選擇";
            selected[0].fontSize = 40;
            selected[0].color = Color.yellow;
            characterToggle[0].interactable = false;
            PartySelect();
        }
        else if (characterImageNumber == 2)
        {
            selected[1].text = "已選擇";
            selected[1].fontSize = 40;
            selected[1].color = Color.yellow;
            characterToggle[1].interactable = false;
            PartySelect();
        }
        else if (characterImageNumber == 3)
        {
            selected[2].text = "已選擇";
            selected[2].fontSize = 40;
            selected[2].color = Color.yellow;
            characterToggle[2].interactable = false;
            PartySelect();
        }
        else if (characterImageNumber == 4)
        {
            selected[3].text = "已選擇";
            selected[3].fontSize = 40;
            selected[3].color = Color.yellow;
            characterToggle[3].interactable = false;
            PartySelect();
        }
        else if (characterImageNumber == 5)
        {
            selected[4].text = "已選擇";
            selected[4].fontSize = 40;
            selected[4].color = Color.yellow;
            characterToggle[4].interactable = false;
            PartySelect();
        }
        else if (characterImageNumber == 6)
        {
            selected[5].text = "已選擇";
            selected[5].fontSize = 40;
            selected[5].color = Color.yellow;
            characterToggle[5].interactable = false;
            PartySelect();
        }
        else if (characterImageNumber == 7)
        {
            selected[6].text = "已選擇";
            selected[6].fontSize = 40;
            selected[6].color = Color.yellow;
            characterToggle[6].interactable = false;
            PartySelect();
        }
        else if (characterImageNumber == 8)
        {
            selected[7].text = "已選擇";
            selected[7].fontSize = 40;
            selected[7].color = Color.yellow;
            characterToggle[7].interactable = false;
            PartySelect();
        }
    }
    #endregion
    #region 角色按鈕
    /// <summary>
    /// 角色按鈕
    /// </summary>
    /// <param name="index">角色陣列編號</param>
    public void CharacterButton(int index)
    {
        if (characterToggle[index].isOn == true)
        {
            characterToggle[index].interactable = false;
            characterImageNumber = index + 1;
            //GameObject character = Instantiate(characters[index], spawnPoint.transform.position, spawnPoint.transform.rotation)as GameObject;
            //選取角色的複製物件儲存到陣列中
            characterClone[index] = Instantiate(characters[index], spawnPoint.transform.position, spawnPoint.transform.rotation) as GameObject;
            //調整複製物件的大小
            switch (index)
            {
                case 1:
                case 2:
                case 3:
                    characterClone[index].transform.localScale = new Vector3(3f, 3f, 0);
                    break;
                case 0:
                case 6:
                    characterClone[index].transform.localScale = new Vector3(12.5f, 12.5f, 0);
                    break;
                default:
                    characterClone[index].transform.localScale = new Vector3(3f, 3f, 0);
                    break;

            }
            confirmSelected = true;
        }
        else if (characterToggle[index].isOn == false && confirmSelected)
        {
            characterToggle[index].interactable = true;
            //將複製物件保存在臨時引用中
            GameObject tempCharacter = characterClone[index];
            //把複製陣列清空
            characterClone[index] = null;
            //刪除臨時引用中保存的複製物件
            Destroy(tempCharacter);
        }
        else if (!confirmSelected)
        {
            //將複製物件保存在臨時引用中
            GameObject tempCharacter = characterClone[index];
            //把複製陣列清空
            characterClone[index] = null;
            //刪除臨時引用中保存的複製物件
            Destroy(tempCharacter);
        }
    }
    #endregion
    #region 顯示角色資訊
    /// <summary>
    /// 角色資訊
    /// </summary>
    public void CharacterInfo()
    {
        switch (characterImageNumber) {
            case 1:
                characterName.text = "狐狸";
                characterInfo.text = "遠程型角色,使用火焰攻擊敵人";
                break;
            case 2:
                characterName.text = "狼";
                characterInfo.text = "近戰型角色。以劍攻擊、以盾防禦，不易受到傷害。";
                break;
            case 3:
                characterName.text = "虎";
                characterInfo.text = "近戰型角色。以雙刀攻擊，移動快速、攻擊快速，防禦較差。";
                break;
            case 4:
                characterName.text = "鷹";
                characterInfo.text = "遠程型角色。以弓箭攻擊，有機會擊中對手要害，防禦較差。";
                break;
            case 5:
                characterName.text = "獅";
                characterInfo.text = "近戰型角色。以槍攻擊，有機會擊中對手要害。";
                break;
            case 6:
                characterName.text = "龍";
                characterInfo.text = "遠程型角色。以杖攻擊、能發動大範圍魔法攻擊對手。血量很低。";
                break;
            case 7:
                characterName.text = "兔子";
                characterInfo.text = "遠程型角色。攻擊很弱";
                //characterInfo.text = "回復型角色,攻擊很弱";
                break;
            case 8:
                characterName.text = "熊";
                characterInfo.text = "近戰型角色。以指虎攻擊，移動較慢、攻擊快速，防禦、血量較優秀。";
                break;
        }
    }
    #endregion
    #region 選擇到隊伍區
    /// <summary>
    /// 選擇到隊伍區
    /// </summary>
    public void PartySelect()
    {
        if (partyImage[0].sprite == null)
        {
            partyImage[0].sprite = partyImages[characterImageNumber];
            StaticVar.AddVar("Role" + characterImageNumber);
            confirmSelected = false;

        }
        else if (partyImage[1].sprite == null && confirmSelected)
        {
            partyImage[1].sprite = partyImages[characterImageNumber];
            StaticVar.AddVar("Role" + characterImageNumber);
            confirmSelected = false;
        }
        else if (partyImage[2].sprite == null && confirmSelected)
        {
            partyImage[2].sprite = partyImages[characterImageNumber];
            StaticVar.AddVar("Role" + characterImageNumber);
            confirmSelected = false;
        }
        else if (partyImage[3].sprite == null && confirmSelected)
        {
            partyImage[3].sprite = partyImages[characterImageNumber];
            StaticVar.AddVar("Role"+ characterImageNumber);
            confirmSelected = false;
        }
        else if (partyImage[4].sprite == null && confirmSelected)
        {
            partyImage[4].sprite = partyImages[characterImageNumber];
            StaticVar.AddVar("Role" + characterImageNumber);
            confirmSelected = false;
        }
        else if (partyImage[5].sprite == null && confirmSelected)
        {
            partyImage[5].sprite = partyImages[characterImageNumber];
            StaticVar.AddVar("Role" + characterImageNumber);
            confirmSelected = false;
        }
        else if (partyImage[6].sprite == null && confirmSelected)
        {
            partyImage[6].sprite = partyImages[characterImageNumber];
            StaticVar.AddVar("Role" + characterImageNumber);
            confirmSelected = false;
        }
        else if (partyImage[7].sprite == null && confirmSelected)
        {
            partyImage[7].sprite = partyImages[characterImageNumber];
            StaticVar.AddVar("Role" + characterImageNumber);
            confirmSelected = false;
        }
    }
    #endregion
    /// <summary>
    /// 重選按鈕
    /// </summary>
    public void ResetButton()
    {
        SceneManager.LoadScene("ChooseCharacter");
        //Application.LoadLevel("ChooseCharacter");
    }
    /// <summary>
    /// 返回按鈕
    /// </summary>
    public void BackButton()
    {
        backBG.SetActive(true);
    }
    public void BackButton2()
    {
        SceneManager.LoadScene("Menu");
        //Application.LoadLevel("Menu");
    }
    public void BackButton3()
    {
        backBG.SetActive(false);
    }
    /// <summary>
    /// 下一頁按鈕
    /// </summary>
    public void NextButton()
    {
        GameObject[] PlayerTarget = GameObject.Find("Canvas").gameObject.scene.GetRootGameObjects();
        foreach (GameObject monster in PlayerTarget)
        {
            Destroy(monster);
        }
        SceneManager.LoadScene("Lobby");
        //Application.LoadLevel("Lobby");
    }
}
