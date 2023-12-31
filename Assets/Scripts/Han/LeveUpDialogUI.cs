﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using System;

public class LeveUpDialogUI : MonoBehaviour
{

    #region property
    // プロパティを入れる。
    public Button[] unitButton;

    public Image[] unitIcon;

    public Text[] NearLVText;
    public Text[] NearHPText;
    public Text[] NearAtkText;
    public Text[] NearCostText;

    public Text[] FarLVText;
    public Text[] FarHPText;
    public Text[] FarAtkText;
    public Text[] FarCostText;

    public Text[] RangeLVText;
    public Text[] RangeHPText;
    public Text[] RangeAtkText;
    public Text[] RangeCostText;

    


    #endregion

    #region serialize
    // unity inpectorに表示したいものを記述。
    #endregion

    #region private
    // プライベートなメンバー変数。
    private enum PLAYER_UNIT_TYPE
    {
        TYPE_NEAR,
        TYPE_FAR,
        TYPE_RANGE
    }

    private CharactorBase[] unitInfo;



    #endregion

    #region Constant
    // 定数をいれる。
    #endregion

    #region Event
    //  System.Action, System.Func などのデリゲートやコールバック関数をいれるところ。
    #endregion

    #region unity methods
    //  Start, UpdateなどのUnityのイベント関数。
    private void Awake()
    {
        
        //GameManager.OnLeveUp.Subscribe(_ => ShowLevelUpDialog());

        for(int buttonCnt = 0;buttonCnt<unitButton.Length;buttonCnt++)
        {
            int n = buttonCnt;
            Button button = unitButton[buttonCnt];
            button.onClick.AddListener(() => SubButtonEvent(n, button));
        }
        
    }

    private void Start()
    {
        //PlayerUnitController.lv;
        //PlayerRangeUnitController.UnitInitilize(level);
        //PlayerAreaUnitController.UnitInitilize(level);

        SetLevelupStatus();
    }

    private void Update()
    {
        
    }
    #endregion

    private void OnEnable()
    {
        SetLevelupStatus();
    }

    private void UpdateText(Text[] text, string beforeStr,string afterStr)
    {
        text[0].text = beforeStr;
        text[1].text = afterStr;
    }

    #region public method
    //　自身で作成したPublicな関数を入れる。
    #endregion

    #region private method
    // 自身で作成したPrivateな関数を入れる。
    public void SubButtonEvent(int buttonCnt, Button button)
    {
        Debug.Log(buttonCnt);

        switch (buttonCnt)
        {
            case 0:
                Debug.Log("0");
                UnitManager.Instance.LevelUp(UnitType.Wall);
                UIManager.Instance.UpdateCardsText(UIManager.Instance.unitCardsInfoArray[0], UnitManager.Instance.PlayerPrefab[0].Level.ToString(), UnitManager.Instance.PlayerPrefab[0].Cost.ToString());
                gameObject.SetActive(false);
                break;
            case 1:
                UnitManager.Instance.LevelUp(UnitType.Range);
                UIManager.Instance.UpdateCardsText(UIManager.Instance.unitCardsInfoArray[1], UnitManager.Instance.PlayerPrefab[1].Level.ToString(), UnitManager.Instance.PlayerPrefab[1].Cost.ToString());
                gameObject.SetActive(false);
                break;
            case 2:
                UnitManager.Instance.LevelUp(UnitType.Area);
                UIManager.Instance.UpdateCardsText(UIManager.Instance.unitCardsInfoArray[2], UnitManager.Instance.PlayerPrefab[2].Level.ToString(), UnitManager.Instance.PlayerPrefab[2].Cost.ToString());
                gameObject.SetActive(false);
                break;
            default:
                break;
        }

        GameManager.Instance.TimerStart();
    }

    private void SetLevelupStatus()
    {
        const int unitNum = 3;

        for (int unitCnt = 0; unitCnt < unitNum; unitCnt++)
        {
            UnitProperty unitInfo = UnitManager.Instance.PlayerPrefab[unitCnt];

            GameObject obj = unitInfo.Unit;
            PlayerUnitController pu = obj.GetComponent<PlayerUnitController>();
            PlayerRangeUnitController pr = obj.GetComponent<PlayerRangeUnitController>();
            PlayerAreaUnitController pa = obj.GetComponent<PlayerAreaUnitController>();

            int lv, hp, atk, cost;

            if (pu != null)
            {
                //PlayerUnitController
                lv = unitInfo.Level;
                hp = pu.BaseHp;
                cost = unitInfo.Cost;
                atk = pu.BasePower;
                if (lv > 1)
                {
                    hp = pu.BaseHp + (lv * 5);
                    atk = lv + pu.BasePower;
                }

                int nextLV = lv + 1;
                string nowLVStr = "LV." + Convert.ToString(lv);
                string nextLVStr = "LV." + Convert.ToString(nextLV);
                UpdateText(NearLVText, nowLVStr, nextLVStr);

                int nextHP = pu.BaseHp + ((lv + 1) * 5);
                string nowHPStr = "HP:" + Convert.ToString(hp);
                string nextHPStr = Convert.ToString(nextHP);
                UpdateText(NearHPText, nowHPStr, nextHPStr);

                int nextAtk = lv + 1 + pu.BasePower;//LV+atk
                string nowAtkStr = "Atk:" + Convert.ToString(atk);
                string nextAtkStr = Convert.ToString(nextAtk);
                UpdateText(NearAtkText, nowAtkStr, nextAtkStr);

                int nextCost = (int)(unitInfo.Cost * 1.5f);
                string nowCostStr = "Cost:" + Convert.ToString(cost);
                string nextCostStr = Convert.ToString(nextCost);
                UpdateText(NearCostText, nowCostStr, nextCostStr);

            }
            else if (pr != null)
            {
                //PlayerRangeUnitController
                lv = unitInfo.Level;
                hp = pr.BaseHp;
                cost = unitInfo.Cost;
                atk = pr.BasePower;
                if (lv > 1)
                {
                    hp = pr.BaseHp + (lv * 5);
                    atk = lv + pr.BasePower;
                }

                int nextLV = lv + 1;
                string nowLVStr = "LV." + Convert.ToString(lv);
                string nextLVStr = "LV." + Convert.ToString(nextLV);
                UpdateText(RangeLVText, nowLVStr, nextLVStr);

                int nextHP = pr.BaseHp + ((lv + 1) * 5);//Lv*5
                string nowHPStr = "HP:" + Convert.ToString(hp);
                string nextHPStr = Convert.ToString(nextHP);
                UpdateText(RangeHPText, nowHPStr, nextHPStr);

                int nextAtk = lv + 1 + pr.BasePower; ;//LV+atk
                string nowAtkStr = "Atk:" + Convert.ToString(atk);
                string nextAtkStr = Convert.ToString(nextAtk);
                UpdateText(RangeAtkText, nowAtkStr, nextAtkStr);

                int nextCost = (int)(unitInfo.Cost * 1.5f);
                string nowCostStr = "Cost:" + Convert.ToString(cost);
                string nextCostStr = Convert.ToString(nextCost);
                UpdateText(RangeCostText, nowCostStr, nextCostStr);
            }
            else if (pa != null)
            {
                //PlayerAreaUnitController
                lv = unitInfo.Level;
                hp = pa.BaseHp + (lv * 5);
                cost = unitInfo.Cost;
                atk = pa.BasePower;
                if (lv > 1)
                {
                    hp = pa.BaseHp + (lv * 5);
                    atk = lv + pa.BasePower;
                }

                int nextLV = lv + 1;
                string nowLVStr = "LV." + Convert.ToString(lv);
                string nextLVStr = "LV." + Convert.ToString(nextLV);
                UpdateText(FarLVText, nowLVStr, nextLVStr);

                int nextHP = pa.BaseHp + ((lv + 1) * 5);//Lv*5
                string nowHPStr = "HP:" + Convert.ToString(hp);
                string nextHPStr = Convert.ToString(nextHP);
                UpdateText(FarHPText, nowHPStr, nextHPStr);

                int nextAtk = lv + 1 + pa.BasePower; ;//LV+atk
                string nowAtkStr = "Atk:" + Convert.ToString(atk);
                string nextAtkStr = Convert.ToString(nextAtk);
                UpdateText(FarAtkText, nowAtkStr, nextAtkStr);

                int nextCost = (int)(unitInfo.Cost * 1.5f);
                string nowCostStr = "Cost:" + Convert.ToString(cost);
                string nextCostStr = Convert.ToString(nextCost);
                UpdateText(FarCostText, nowCostStr, nextCostStr);
            }

        }
    }

    #endregion
}
