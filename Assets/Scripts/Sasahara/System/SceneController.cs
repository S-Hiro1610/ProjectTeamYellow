using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Cysharp.Threading.Tasks;

public static class SceneController
{
    #region property
    // プロパティを入れる。
    public static string CurrentScene => _currentSceneName;
    #endregion

    #region serialize
    // unity inpectorに表示したいものを記述。
    #endregion

    #region private
    // プライベートなメンバー変数。
    private static string _currentSceneName;
    [SerializeField]
    private static List<Scene> _sceneNameList = new List<Scene>();
    #endregion

    #region Constant
    // 定数をいれる。
    #endregion

    #region Event
    //  System.Action, System.Func などのデリゲートやコールバック関数をいれるところ。
    #endregion

    #region public method
    //　自身で作成したPublicな関数を入れる。
    /// <summary>アクティブなシーンを切り替える</summary>
    /// <param name="sceneName">シーン名</param>
    public static void ChangeActiveScene(string sceneName)
    {
        SceneManager.SetActiveScene(SceneManager.GetSceneByName(sceneName));
        _currentSceneName = sceneName;
    }

    /// <summary>シーンをロードする(非同期)</summary>
    /// <param name="sceneName">シーン名</param>
    public static void LoadScene(string sceneName)
    {
        SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
    }

    /// <summary>シーンをアンロードする(非同期)</summary>
    /// <param name="sceneName">シーン名</param>
    public static void UnloadScene(string sceneName)
    {
        SceneManager.UnloadSceneAsync(sceneName);
    }
    #endregion

    #region private method
    // 自身で作成したPrivateな関数を入れる。
    #endregion
}