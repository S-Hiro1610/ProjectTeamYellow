using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager: MonoBehaviour
{
    #region property
    // プロパティを入れる。

    public static UIManager Instance;


    public int EnemyCnt = 0;
    public int EnemyMaxNum = 0;

    //Unit_Cards_Panel
    public GameObject UnitCardsPanel;
    public GameObject UnitCardPanel;

    //TopPanelText
    public Text PauseMenuUIText;//一時停止ボタン
    public Text WaveUIText;
    //BottomPlaneText
    public Text PowerUIText;


    public Image EnemyCntUIImage;

    //TopPanelText
    public string PauseMenuUIString;
    public string WaveCnt;
    //public string EnemyCntUICnt;
    //BottomPlaneText
    public string PowerUICnt;

    public Button MenuButton;

    public Dialogbox ExitDialogUI;//メインメニュー

    public CardInfo[] unitCardsInfoArray;

    public List<GameObject> cardGameObjcetList;

    //UnitCardsPanel
    private int unitCardsNum = 0;
    

    public float UnitCardsPanelSpacingW = 0;
    #endregion

    #region serialize
    // unity inpectorに表示したいものを記述。
    #endregion

    #region private
    // プライベートなメンバー変数。
    //TopPanelText
    private string _currentPauseMenuUIString;
    private string _currentWaveString;
    //private string _currentEnemyCntUIString;
    //BottomPlaneText
    private string _currentPowerUIString;

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
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        cardGameObjcetList = new List<GameObject>();

        _currentPauseMenuUIString = PauseMenuUIText.text;
        UpdateText(PauseMenuUIText, PauseMenuUIString);

        ExitDialogUI.OnOpenEvent.AddListener(ExitDialogUIIsOpen);
        ExitDialogUI.OnCloseEvent.AddListener(ExitDialogUIIsClose);


        _currentWaveString = WaveUIText.text;
        UpdateText(WaveUIText, WaveCnt);

        //_currentEnemyCntUIString = EnemyCntUIText.text;
        //UpdateText(EnemyCntUIText, EnemyCntUICnt);

        _currentPowerUIString = PowerUIText.text;
        UpdateText(PowerUIText, PowerUICnt);

        unitCardsNum = unitCardsInfoArray.Length;

        UpdateEnemyKillBar();

        GridLayoutGroup cardsGrop = UnitCardsPanel.GetComponent<GridLayoutGroup>();

        for (int cardCnt = 0; cardCnt < unitCardsNum; cardCnt++)
        {
            GameObject cardObj = Instantiate(UnitCardPanel, cardsGrop.transform);
            cardObj.name = "Card_" + cardCnt;
            unitCardsInfoArray[cardCnt].thisGameObjcet = cardObj;
            unitCardsInfoArray[cardCnt].cardContext = cardObj.GetComponent<Card>();

            UpdateCardsText(unitCardsInfoArray[cardCnt], unitCardsInfoArray[cardCnt].LVUIString, unitCardsInfoArray[cardCnt].costUIString);
            
            UpdateCardsCoolTime(unitCardsInfoArray[cardCnt].cardContext.coolTimePlane, unitCardsInfoArray[cardCnt].coolTime);
            //int index = cardCnt;
            //cardObj.GetComponent<Button>().onClick.AddListener(() => unitCardsInfoArray[index].cardContext.OnClick());
            cardGameObjcetList.Add(cardObj);
        }

        //System.Array.Copy(unitCardsInfoArray, _currentUnitCardsInfoArray, unitCardsInfoArray.Length);


        RectTransform cardRectTransform = UnitCardPanel.GetComponent<RectTransform>();
        cardsGrop.cellSize = cardRectTransform.sizeDelta;

        cardsGrop.spacing = new Vector2(UnitCardsPanelSpacingW, cardsGrop.spacing.y);

        LayoutRebuilder.ForceRebuildLayoutImmediate(UnitCardsPanel.transform as RectTransform);

        StartCoroutine(CheckSceneUIValuesChanged());
        StartCoroutine(CheckCardValuesChanged());

        StartCoroutine(CheckEnemyKillBarChanged());

        SceneManager.LoadSceneAsync("InputScene", LoadSceneMode.Additive);//InputSceneをロード
    }

    private void Update()
    {
        //foreach(var item in unitCardsInfoArray)
        //{
        //    Debug.Log(item.cardContext.name);
        //}
    }
    #endregion

    #region public method
    // 自身で作成したPublicな関数を入れる。
    #endregion

    #region private method
    // 自身で作成したPrivateな関数を入れる。

    private IEnumerator CheckSceneUIValuesChanged()
    {
        while(true)
        {
            string newPauseMenuUIString = PauseMenuUIString;
            if (newPauseMenuUIString != _currentPauseMenuUIString)
            {
                _currentPauseMenuUIString = newPauseMenuUIString;
                UpdateText(PauseMenuUIText, _currentPauseMenuUIString);
            }

            string newWaveString = WaveCnt;
            if (newWaveString != _currentWaveString)
            {
                _currentWaveString = newWaveString;
                UpdateText(WaveUIText, _currentWaveString);
            }

            //string newEnemyCntUIString = EnemyCntUICnt;
            //if (newEnemyCntUIString != _currentEnemyCntUIString)
            //{
            //    _currentEnemyCntUIString = newEnemyCntUIString;
            //    UpdateText(EnemyCntUIText, _currentEnemyCntUIString);
            //}

            string newPowerUIString = PowerUICnt;
            if (newPowerUIString != _currentPowerUIString)
            {
                _currentPowerUIString = newPowerUIString;
                UpdateText(PowerUIText, _currentPowerUIString);
            }

            yield return new WaitForSeconds(.1f);//呼び出しを頻繁し過ぎないように
        }
    }

    private IEnumerator CheckCardValuesChanged()
    {
        while (true)
        {
            for (int cardCnt = 0; cardCnt < unitCardsInfoArray.Length; cardCnt++)
            {
                UpdateCardsText(unitCardsInfoArray[cardCnt], unitCardsInfoArray[cardCnt].LVUIString, unitCardsInfoArray[cardCnt].costUIString);
            }

            yield return new WaitForSeconds(.1f);//呼び出しを頻繁し過ぎないように
        }
    }

    private IEnumerator CheckEnemyKillBarChanged()
    {
        while (true)
        {
            UpdateEnemyKillBar();

            yield return new WaitForSeconds(.1f);//呼び出しを頻繁し過ぎないように
        }
    }

    private void UpdateCardsCoolTime(Image image, float coolTime)
    {
        image.fillAmount = coolTime;
    }

    private void UpdateCardsText(CardInfo info,string LVStr,string costStr)
    {
        info.cardContext.LVUIText.text = LVStr;
        info.cardContext.costUIText.text = costStr;
    }
    private void UpdateText(Text text,object str)
    {
        text.text = (string)str;
    }

    private void UpdateEnemyKillBar()
    {
        float percent = (float)EnemyCnt / EnemyMaxNum;
        Mathf.Clamp01(percent);

        EnemyCntUIImage.fillAmount = percent;
    }

    private void ExitDialogUIIsOpen()
    {
        UpdateText(PauseMenuUIText, "II");
        Debug.Log("UIManager:ExitDialogUIIsOpen");
    }

    private void ExitDialogUIIsClose()
    {
        UpdateText(PauseMenuUIText, "▶");
        Debug.Log("UIManager:ExitDialogUIIsClose");
    }

    #endregion
}
