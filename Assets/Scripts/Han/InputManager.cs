using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UniRx;

public class InputManager : MonoBehaviour
{
    #region property
    // プロパティを入れる。

    protected static InputManager Instance;

    public enum ControlType { MouseAndKeyboard, TouchScreen }

    /*動的入力モードを入り変え：
     * {
         * if (Input.GetKeyDown(KeyCode.T))
            {
                InputManager.Instance.CurrentControlScheme = InputManager.ControlType.TouchScreen;
            }
        }
     */
    public ControlType nowControlType = ControlType.MouseAndKeyboard;

    /*Click()使用する場合:
     * {
         *  if (InputManager.Instance != null && InputManager.Instance.Click())
            {
                // クリックされた
                // Click後の処理
            }
        }
     */
    #endregion

    #region serialize
    // unity inpectorに表示したいものを記述。
    #endregion

    #region private
    // プライベートなメンバー変数。
    private Button menuButton;

    private Dialogbox exitDialogUI;

    private List<GameObject> cardGameObjcetList;

    private int playerPower = 0;
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

        if (GameManager.Instance != null)
        {
            GameManager.Instance.PowerUI.Subscribe(count => { playerPower = count; });//配置にかかるコストのリソース
        }
    }

    private void Start()
    {
        menuButton = UIManager.Instance.MenuButton;
        exitDialogUI = UIManager.Instance.ExitDialogUI;

        menuButton.onClick.AddListener(() => OnClickMenuButton());
        exitDialogUI.OnOpenEvent.AddListener(ExitDialogUIIsOpen);
        exitDialogUI.OnCloseEvent.AddListener(ExitDialogUIIsClose);
        cardGameObjcetList = UIManager.Instance.cardGameObjcetList;

        foreach (GameObject cardObj in cardGameObjcetList)
        {
            Card cardContext = cardObj.GetComponent<Card>();
            cardObj.GetComponent<Button>().onClick.AddListener(() => cardContext.OnClick());
        }

    }

    private void Update()
    {
        if (InputManager.Instance != null && InputManager.Instance.Click())
        {
            if (EventSystem.current.IsPointerOverGameObject())
            {
                return;
            }

            //UIManager.Instance.cardGameObjcetList

            int selectCnt = 0;
            Card selectCard = null;
            GameObject cardObj = null;
            foreach (var item in UIManager.Instance.cardGameObjcetList)
            {
                Card card = item.GetComponent<Card>();
                if (card.SelectMode.Value == SELSCT_MODE.SELECT_MOD_SELECT)
                {
                    selectCnt++;
                    selectCard = card;
                    cardObj = item;
                }
            }

            if (selectCnt == 0) goto ENDSELECT;


            if (GameManager.Instance == null) goto ENDSELECT;

            int cost = int.Parse(cardObj.GetComponent<Card>().costUIText.text);

            if (cost > playerPower) goto ENDSELECT;

            GameManager.Instance.PowerUI.Value -= cost;


            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit[] hits = Physics.RaycastAll(ray);
            //hits = hits.Where(hit => hit.collider.gameObject.tag != "UI").ToArray();
            float minDistance = Mathf.Infinity;
            RaycastHit? closestHit = null;

            foreach (var hit in hits)
            {
                StageBlock groundBlock = null;
                if (!hit.transform.TryGetComponent(out groundBlock)) continue;
                if (hit.distance < minDistance)
                {
                    minDistance = hit.distance;
                    closestHit = hit;
                }
            }
            //Debug.Log("OK0");
            if (closestHit != null)
            {
                StageBlock groundBlock = null;
                Debug.Log(closestHit.Value.transform.name);
                closestHit.Value.transform.TryGetComponent(out groundBlock);
                //Debug.Log("OK1");
                if (groundBlock == null) goto ENDSELECT;
                //Debug.Log("OK2");
                if (groundBlock.isPlayerUnitSet == false) goto ENDSELECT;
                //Debug.Log("OK3");


                UnitManager.Instance.UnitCreate(selectCard.type, new Vector3(closestHit.Value.transform.position.x, 0, closestHit.Value.transform.position.z));

                //Debug.Log("Hit " + closestHit.Value.transform.name);
            }

        ENDSELECT:
            if(selectCard != null)
            selectCard.SelectMode.Value = SELSCT_MODE.SELECT_MOD_NO;
        }
    }
    #endregion

    #region public method
    //　自身で作成したPublicな関数を入れる。

    public bool Click()
    {
        switch (nowControlType)
        {
            case ControlType.MouseAndKeyboard:
                return Input.GetMouseButtonDown(0);
            case ControlType.TouchScreen:
                //if(Input.touchCount > 0)の場合、タッチ入力あり。touchCountの戻り値はタッチ入力の数を返すこと。
                //Input.GetTouch(0)のパラメータは0番のindexを返すこと、つまり最初フレームのタッチ入力情報。
                //.phaseは今の段階を返すこと、段階は：Began(タッチ開始)、Moved(タッチ移動)、Stationary(タッチまま移動しない)、Ended(タッチ終了)
                //
                /*
                 * もしInput.touchCount > 0の条件を確認せずに、Input.GetTouch(0).phase == TouchPhase.Beganの条件だけを使用すると、
                 * タッチ入力がない場合、Input.GetTouch(0)メソッドを呼び出すとIndexOutOfRangeException例外がスローされます。
                 * これは、タッチ入力がない場合、Input.GetTouch(0)メソッドが存在しないタッチ入力にアクセスしようとして例外が発生するためです。

                 * もしInput.GetTouch(0).phase == TouchPhase.Beganの条件を確認せずに、Input.touchCount > 0の条件だけを使用すると、
                 * タッチ入力がある限り、タッチ入力がどのフェーズにあるかに関係なく、条件はtrueを返します。
                 * つまり、タッチ入力が終了しているか移動中であっても、条件はtrueを返します。

                 * したがって、新しいタッチ入力を検出するためには、
                 * Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Beganの条件を同時に使用するのが最善です。
                 * これにより、新しいタッチ入力がある場合にのみ条件がtrueを返し、存在しないタッチ入力へのアクセスによる例外が回避されます。
                 */
                return Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began;
            default:
                return false;
        }
    }
    #endregion

    #region private method
    // 自身で作成したPrivateな関数を入れる。
    private void OnClickMenuButton()
    {
        exitDialogUI.SetActive(true);
    }

    private void ExitDialogUIIsOpen()
    {
        Debug.Log("InputManager:ExitDialogUIIsOpen");
    }

    private void ExitDialogUIIsClose()
    {
        Debug.Log("InputManager:ExitDialogUIIsClose");
    }
    #endregion
}
