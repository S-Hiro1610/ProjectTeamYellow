using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class BgmPlayer : MonoBehaviour
{
    #region property
    // プロパティを入れる。
    #endregion

    #region serialize
    // unity inpectorに表示したいものを記述。
    [SerializeField] private List<AudioClip> BgmList;
    #endregion

    #region private
    // プライベートなメンバー変数。
    private static BgmPlayer _instance;
    private AudioSource _audioSource;
    private int _currentBgmIndex = -1;
    private readonly Dictionary<string, int> _bgmNameIndex = new Dictionary<string, int>();
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
        // Singleton
        if (_instance == null)
        {
            _instance = this;
        }
        else if (_instance != this)
        {
            Destroy(gameObject);
        }

        // AudioSource初期化
        _audioSource = gameObject.AddComponent<AudioSource>();
        _audioSource.clip = null;
        _audioSource.playOnAwake = false;
        _audioSource.loop = true;
        _audioSource.volume = 0.5f;

        // BGMの名前とインデックスを格納
        for (int i = 0;i < BgmList.Count;i++)
        {
            AudioClip audioclip = BgmList[i];
            if (!_bgmNameIndex.ContainsKey(audioclip.name))
            {
                _bgmNameIndex.Add(audioclip.name, i);
            }
        }
    }

    #endregion

    #region public method
    //　自身で作成したPublicな関数を入れる。

    /// <summary>
    /// BgmPlayerのインスタンス
    /// </summary>
    public static BgmPlayer Instance
    {
        get
        {
            return _instance;
        }
    }

    /// <summary>
    /// 再生中のBGM番号
    /// </summary>
    public int Index
    {
        get
        {
            return _currentBgmIndex;
        }
    }

    /// <summary>
    /// BGMの音量
    /// </summary>
    public float Volume
    {
        get
        {
            return _audioSource.volume;
        }
        set
        {
            _audioSource.volume = value;
        }
    }

    /// <summary>
    /// BGMを再生する
    /// </summary>
    /// <param name="Name">BGM名</param>
    public void Play(string Name)
    {
        if (!_bgmNameIndex.ContainsKey(Name))
        {
            Debug.Log("存在しないBGM名のため再生できません。");
            return;
        }
        Play(_bgmNameIndex[Name]);
    }

    /// <summary>
    /// BGMを再生する
    /// </summary>
    /// <param name="index">BGM番号</param>
    public void Play(int index)
    {
        // BGMリストが設定されていない場合
        if (!BgmList.Any())
        {
            Debug.Log("BGMListにAudioClipが登録されていないため再生できません。");
            return;
        }

        // 対象外のIndexを指定した場合
        if (index < 0 || BgmList.Count <= index )
        {
            Debug.Log("存在しないBGMインデックスを指定したため再生できません。");
            return;
        }

        // ポーズ中である場合は再開、そうでなければ新規再生
        if (_currentBgmIndex == index && !_audioSource.isPlaying)
        {
            _audioSource.Play();
        }
        else
        {
            // BGM再生中の場合は先に停止
            if (_audioSource.isPlaying)
            {
                _audioSource.Stop();
            }
            // 現在再生中のインデックスをカレントインデックスに格納し、BGMを再生
            _currentBgmIndex = index;
            AudioClip audiocip = BgmList[index];
            _audioSource.clip = audiocip;
            _audioSource.Play();
        }
    }

    /// <summary>
    /// 再生中のBGMをポーズする
    /// </summary>
    public void Pause()
    {
        // BGM再生中の場合はポーズ
        if (_audioSource.isPlaying)
        {
            _audioSource.Pause();
        }
    }

    /// <summary>
    /// 再生中のBGMを止める
    /// </summary>
    public void Stop()
    {
        // BGMを停止、現在のBGMインデックスを-1に設定
        _audioSource.Stop();
        _currentBgmIndex = -1;
    }

    #endregion

    #region private method
    // 自身で作成したPrivateな関数を入れる。
    #endregion
}
