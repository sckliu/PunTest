using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
        if (partyImage[2].sprite != null)
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
            if (index == 2)
            {
                characterClone[index].transform.localScale = new Vector3(3f, 3f, 0);
            }
            else if (index == 3)
            {
                characterClone[index].transform.localScale = new Vector3(3f, 3f, 0);
            }
            else
            {
                characterClone[index].transform.localScale = new Vector3(12.5f, 12.5f, 0);
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
        if (characterImageNumber == 1)
        {
            characterName.text = "兔子";
            characterInfo.text = "回復型角色,攻擊很弱";
        }
        else if (characterImageNumber == 2)
        {
            characterName.text = "狐狸";
            characterInfo.text = "遠程型角色,使用火焰攻擊敵人";
        }
        else if (characterImageNumber == 3)
        {
            characterName.text = "狼";
            characterInfo.text = "";
        }
        else if (characterImageNumber == 4)
        {
            characterName.text = "老虎";
            characterInfo.text = "";
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
            StaticVar.AddVar("Role1");
            confirmSelected = false;

        }
        else if (partyImage[1].sprite == null && confirmSelected)
        {
            partyImage[1].sprite = partyImages[characterImageNumber];
            StaticVar.AddVar("Role2");
            confirmSelected = false;
        }
        else if (partyImage[2].sprite == null && confirmSelected)
        {
            partyImage[2].sprite = partyImages[characterImageNumber];
            StaticVar.AddVar("Role3");
            confirmSelected = false;
        }
        else if (partyImage[3].sprite == null && confirmSelected)
        {
            partyImage[3].sprite = partyImages[characterImageNumber];
            StaticVar.AddVar("Role4");
            confirmSelected = false;
        }
    }
    #endregion
    /// <summary>
    /// 重選按鈕
    /// </summary>
    public void ResetButton()
    {
        Application.LoadLevel("ChooseCharacter");
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
        Application.LoadLevel("Menu");
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
        Application.LoadLevel("Lobby");
    }
}
