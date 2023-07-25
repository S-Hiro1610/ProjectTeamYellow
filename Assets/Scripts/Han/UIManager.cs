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

    //Unit_Cards_Panel
    public GameObject UnitCardsPanel;
    public GameObject UnitCardPanel;
    public List<GameObject> UnitCardPrefab;

    //TopPanelText
    public Text PauseMenuUIText;//一時停止ボタン
    public Text WaveUIText;
    //BottomPlaneText
    public Text PowerUIText;


    public Image EnemyCntUIImage;

    //TopPanelText
    
    public int WaveMaxNum = 0;          // 現在の wave の敵総数
    public int RemainingEnemies = 0;    // 現在の wave の敵残数

    public Button MenuButton;

    public Button UnitQuitButton;//退場ボタン

    public Button OptionMenuButton;

    public Dialogbox ExitDialogUI;//メインメニュー

    public Dialogbox ExitDialogOptionUI;//オプション(音量調整)

    public GameObject LeveUpDialogUI;

    public CardInfo[] unitCardsInfoArray;

    public List<GameObject> cardGameObjcetList;

    //UnitCardsPanel
    private int unitCardsNum = 0;

    private string PauseMenuUIString;

    public float UnitCardsPanelSpacingW = 0;
    #endregion

    #region serialize
    // unity inpectorに表示したいものを記述。

    [SerializeField]
    private GameObject _titleWindow;
    [SerializeField]
    private GameObject _gameOverWindow;
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
            WaveManager.Instance.WaveEnemyCount.Subscribe(_ => { 
                WaveMaxNum = _;   // 現在のWaveの敵の総数の購読★
                UpdateText(WaveUIText, $"{RemainingEnemies} / {WaveMaxNum}");});   // 残敵数表示の更新★
        }

        if (GameManager.Instance != null)
        {
            GameManager.Instance.EnemyCount.Subscribe(_ => { 
                EnemyCnt = _;              // 現在のWaveで倒した敵の数★
                UpdateEnemyKillBar((float)EnemyCnt / (float)GameManager.Instance.LevelUpList[GameManager.Instance.LevelUpIndex]);    // ゲージの更新も連動★
            });

            GameManager.Instance.RemainingEnemies.Subscribe(_ => {
                RemainingEnemies = _;
                UpdateText(WaveUIText, $"{RemainingEnemies} / {WaveMaxNum}");        // 残敵数表示の更新★
            });  // 残敵数の更新

            GameManager.Instance.Resouce.Subscribe(_ => UpdateText(PowerUIText, _.ToString()));//配置にかかるコストのリソース★

            GameManager.Instance.OnLevelUp.Subscribe(_ => LeveUpDialogUI.SetActive(true));
            //GameManager.Instance.UnitCardsInfoArray.Subscribe(array => { unitCardsInfoArray = array; });
            //GameManager.Instance.EnemyALLCount.Subscribe(allCnt => { EnemyMaxNum = allCnt; });

        }
    }
    

    private void Start()
    {
        WaveMaxNum = 0;         // 現在の wave の敵総数★
        RemainingEnemies = 0;   // 現在の wave の敵残数★

        cardGameObjcetList = new List<GameObject>();

        PauseMenuUIString = "▶";
        UpdateText(PauseMenuUIText, PauseMenuUIString);
        UpdateText(WaveUIText, $"{WaveMaxNum - EnemyCnt} / {WaveMaxNum}");

        ExitDialogUI.OnSubWindowEvent.AddListener(OpenDialogOptionMenu);

        // UnitManager から Playerユニットの種類数を読み出して、UI 下部のCard欄に表示する

        unitCardsNum = UnitManager.Instance.PlayerPrefab.Count;

        GridLayoutGroup cardsGrop = UnitCardsPanel.GetComponent<GridLayoutGroup>();

        for (int cardCnt = 0; cardCnt < unitCardsNum; cardCnt++)
        {
            //GameObject cardObj = Instantiate(UnitCardPanel, cardsGrop.transform);
            GameObject cardObj = Instantiate(UnitCardPrefab[cardCnt], cardsGrop.transform);
            cardObj.name = "Card_" + cardCnt;
            unitCardsInfoArray[cardCnt].thisGameObjcet = cardObj;       // 未使用？
            unitCardsInfoArray[cardCnt].cardContext = cardObj.GetComponent<Card>();
            cardObj.GetComponent<Card>().type = (UnitType)cardCnt;

            unitCardsInfoArray[cardCnt].LVUIString = UnitManager.Instance.PlayerPrefab[cardCnt].Level.ToString();   // UnitManagerのレベル値をカードに表示
            unitCardsInfoArray[cardCnt].costUIString = UnitManager.Instance.PlayerPrefab[cardCnt].Cost.ToString();  // UnitManagerのコスト値をカードに表示

            UpdateCardsText(unitCardsInfoArray[cardCnt], unitCardsInfoArray[cardCnt].LVUIString, unitCardsInfoArray[cardCnt].costUIString);
            
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

        RectTransform cardRectTransform = UnitCardPanel.GetComponent<RectTransform>();
        cardsGrop.cellSize = cardRectTransform.sizeDelta;

        cardsGrop.spacing = new Vector2(UnitCardsPanelSpacingW, cardsGrop.spacing.y);

        LayoutRebuilder.ForceRebuildLayoutImmediate(UnitCardsPanel.transform as RectTransform);

        StartCoroutine(CheckUnitCardSelectMode());

        SceneManager.LoadSceneAsync("InputScene", LoadSceneMode.Additive);//InputSceneをロード
    }

    private void Update()
    {
    }
    #endregion

    #region public method
    // 自身で作成したPublicな関数を入れる。
    #endregion

    #region private method
    // 自身で作成したPrivateな関数を入れる。

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

    private void UpdateCardsText(CardInfo info,string LVStr,string costStr)
    {
        info.cardContext.LVUIText.text = LVStr;
        info.cardContext.costUIText.text = costStr;
    }
    private void UpdateText(Text text,object str)
    {
        text.text = (string)str;
    }

    private void UpdateEnemyKillBar(float percent)
    {
        //float percent = (float)EnemyCnt / EnemyMaxNum;
        
        //if (EnemyMaxNum == 0) percent = 0;
        //Mathf.Clamp01(percent);

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

    private void OpenDialogOptionMenu(Dialogbox.DIALOG_TYPE dialogType,Button button)
    {
        //Debug.Log(dialogType);
        button.transform.parent.parent.gameObject.SetActive(false);
        ExitDialogOptionUI.SetActive(true,false);
    }
    #endregion
}
