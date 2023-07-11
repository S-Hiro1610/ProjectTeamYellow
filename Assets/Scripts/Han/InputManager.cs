using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InputManager: MonoBehaviour
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
        menuButton = UIManager.Instance.MenuButton;
        exitDialogUI = UIManager.Instance.ExitDialogUI;

        menuButton.onClick.AddListener(() => OnClickMenuButton());
        exitDialogUI.OnOpenEvent.AddListener(ExitDialogUIIsOpen);
        exitDialogUI.OnCloseEvent.AddListener(ExitDialogUIIsClose);
        cardGameObjcetList = UIManager.Instance.cardGameObjcetList;

        foreach(GameObject cardObj in cardGameObjcetList)
        {
            Card cardContext = cardObj.GetComponent<Card>();
            cardObj.GetComponent<Button>().onClick.AddListener(() => cardContext.OnClick());
        }

    }

    private void Update()
    {

    }
    #endregion

    #region public method
    //　自身で作成したPublicな関数を入れる。

    public bool Click()
    {
        switch(nowControlType)
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
        Debug.Log("ExitDialogUIIsOpen");
    }

    private void ExitDialogUIIsClose()
    {
        Debug.Log("ExitDialogUIIsClose");
    }
    #endregion
}
