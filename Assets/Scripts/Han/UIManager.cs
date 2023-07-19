using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UniRx;

public class UIManager: MonoBehaviour
{
    #region property
    // プロパティを入れる。

    public static UIManager Instance;

    public ReactiveProperty<SELSCT_MODE> SelectMode => selectMode;
    public int EnemyCnt = 0;//1wave残機数
    public int EnemyMaxNum = 0;//１Waveの敵ユニットの総数

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
    
    //public string WaveCnt;
    public int WaveCnt = 0; //現在のwave index
    public int WaveMaxNum = 0;//総wave数
    //public string EnemyCntUICnt;
    //BottomPlaneText
    //public string PowerUICnt;
    public int PowerUI;

    public Button MenuButton;

    public Button UnitQuitButton;//退場ボタン

    public Dialogbox ExitDialogUI;//メインメニュー

    public CardInfo[] unitCardsInfoArray;

    public List<GameObject> cardGameObjcetList;

    //UnitCardsPanel
    private int unitCardsNum = 0;

    private string PauseMenuUIString;

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

    private ReactiveProperty<SELSCT_MODE> selectMode = new ReactiveProperty<SELSCT_MODE>(SELSCT_MODE.SELECT_MOD_NO);

    //private List<CardInfo> cardsInfoArray;

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

        if(WaveManager.Instance != null)
        {
            WaveManager.Instance.WaveCount.Subscribe(count => { WaveCnt = count; });//現在のwave index
            WaveManager.Instance.WaveEnemyCount.Subscribe(allCnt => { WaveMaxNum = allCnt; });//総wave数
        }

        if (GameManager.Instance != null)
        {
            GameManager.Instance.EnemyCount.Subscribe(count => { EnemyCnt = count; });//1wave倒した敵機数
            GameManager.Instance.EnemyALLCount.Subscribe(allCnt => { EnemyMaxNum = allCnt; });//全て敵の数
            GameManager.Instance.PowerUI.Subscribe(count => { PowerUI = count; });//配置にかかるコストのリソース
            GameManager.Instance.UnitCardsInfoArray.Subscribe(array => { unitCardsInfoArray = array; });
        }
    }
    

    private void Start()
    {
        cardGameObjcetList = new List<GameObject>();

        PauseMenuUIString = "▶";
        _currentPauseMenuUIString = PauseMenuUIText.text;
        UpdateText(PauseMenuUIText, PauseMenuUIString);

        ExitDialogUI.OnOpenEvent.AddListener(ExitDialogUIIsOpen);
        ExitDialogUI.OnCloseEvent.AddListener(ExitDialogUIIsClose);


        _currentWaveString = WaveCnt + "/" + WaveMaxNum;
        UpdateText(WaveUIText, WaveCnt + "/" + WaveMaxNum);

        //_currentEnemyCntUIString = EnemyCntUIText.text;
        //UpdateText(EnemyCntUIText, EnemyCntUICnt);

        _currentPowerUIString = PowerUI.ToString();
        UpdateText(PowerUIText, PowerUI.ToString());

        unitCardsNum = unitCardsInfoArray.Length;

        UpdateEnemyKillBar();

        GridLayoutGroup cardsGrop = UnitCardsPanel.GetComponent<GridLayoutGroup>();

        for (int cardCnt = 0; cardCnt < unitCardsNum; cardCnt++)
        {
            GameObject cardObj = Instantiate(UnitCardPanel, cardsGrop.transform);
            cardObj.name = "Card_" + cardCnt;
            unitCardsInfoArray[cardCnt].thisGameObjcet = cardObj;
            unitCardsInfoArray[cardCnt].cardContext = cardObj.GetComponent<Card>();
            cardObj.GetComponent<Card>().type = (UnitType)cardCnt;

            UpdateCardsText(unitCardsInfoArray[cardCnt], unitCardsInfoArray[cardCnt].LVUIString, unitCardsInfoArray[cardCnt].costUIString);
            
            UpdateCardsCoolTime(unitCardsInfoArray[cardCnt].cardContext.coolTimePlane, unitCardsInfoArray[cardCnt].coolTime);
            //int index = cardCnt;
            //cardObj.GetComponent<Button>().onClick.AddListener(() => unitCardsInfoArray[cardCnt].cardContext.OnClick());
            int index = cardCnt;
            cardObj.GetComponent<Button>().onClick.AddListener(() => {
                unitCardsInfoArray[index].cardContext.OnClick();
                unitCardsInfoArray[index].cardContext.SelectMode.Value = SELSCT_MODE.SELECT_MOD_SELECT;
                for (int i = 0; i < unitCardsNum; i++)
                {
                    if (i != index)
                    {
                        unitCardsInfoArray[i].cardContext.SelectMode.Value = SELSCT_MODE.SELECT_MOD_NO;
                    }
                }
            });
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

        StartCoroutine(CheckUnitCardSelectMode());

        SceneManager.LoadSceneAsync("InputScene", LoadSceneMode.Additive);//InputSceneをロード
    }

    private void Update()
    {
        //string[] str = new string[3];
        //int scnt = 0;
        //foreach (var item in cardGameObjcetList)
        //{
        //    str[scnt] = item.GetComponent<Card>().SelectMode.Value.ToString();
        //    scnt++;
        //}

        //Debug.Log("b1=>" + str[0] + ",b2=>" + str[1] + ",b2=>" + str[2]);
        //Debug.Log("selectMode=>"+selectMode.Value);
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

            string newWaveString = WaveCnt + "/" + WaveMaxNum;
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

            string newPowerUIString = PowerUI.ToString();
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

    private IEnumerator CheckUnitCardSelectMode()
    {
        while (true)
        {
            foreach (var item in cardGameObjcetList)
            {
                if (item.GetComponent<Card>().SelectMode.Value != SELSCT_MODE.SELECT_MOD_NO)
                {
                    SelectMode.Value = SELSCT_MODE.SELECT_MOD_SELECT;
                    break;
                }
                SelectMode.Value = SELSCT_MODE.SELECT_MOD_NO;
            }
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
