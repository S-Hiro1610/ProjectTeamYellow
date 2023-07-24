using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class AudioPlayer : MonoBehaviour
{
    #region property
    // プロパティを入れる。
    #endregion
    public List<AudioClip> BgmList => _bgmList;
    public List<AudioClip> SeList => _seList;
    public int MaxSePlaySoundCount => _maxSePlaySoundCount;
    #region serialize
    // unity inpectorに表示したいものを記述。
    [SerializeField] private List<AudioClip> _bgmList;
    [SerializeField] private List<AudioClip> _seList;
    [SerializeField] private int _maxSePlaySoundCount;
    #endregion

    #region private
    // プライベートなメンバー変数。
    private static AudioPlayer _instance;
    private AudioSource _bgmAudioSource;
    private AudioSource _seAudioSource;
    private int _currentBgmIndex = -1;
    private readonly Dictionary<string, int> _bgmNameIndex = new Dictionary<string, int>();
    private readonly Dictionary<string, int> _seNameIndex = new Dictionary<string, int>();
    private HashSet<AudioClip> _seAudioPlayList = new HashSet<AudioClip>();
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
        InitializeAudioSource(out _bgmAudioSource, true);
        InitializeAudioSource(out _seAudioSource, false);

        // BGMの名前とBGM番号を格納
        AudioNameList(_bgmList, _bgmNameIndex);
        AudioNameList(_seList, _seNameIndex);
    }

    private void Update()
    {
        // SE再生リストに何も入ってなければ実行しない
        if (!_seAudioPlayList.Any())
        {
            return;
        }

        // SE再生リスト内のAudioClipを順番に再生する
        foreach (AudioClip audio in _seAudioPlayList)
        {
            _seAudioSource.PlayOneShot(audio);
        }
        _seAudioPlayList.Clear();
    }

    #endregion

    #region public method
    //　自身で作成したPublicな関数を入れる。

    /// <summary>
    /// AudioPlayerのインスタンス
    /// </summary>
    public static AudioPlayer Instance
    {
        get
        {
            return _instance;
        }
    }

    /// <summary>
    /// 再生中のBGM番号を返す。再生中でない場合は-1を返す。
    /// </summary>
    public int BGMIndex
    {
        get
        {
            return _currentBgmIndex;
        }
    }

    /// <summary>
    /// BGMの音量
    /// </summary>
    public float BGMVolume
    {
        get
        {
            return _bgmAudioSource.volume;
        }
        set
        {
            _bgmAudioSource.volume = value;
        }
    }

    /// <summary>
    /// BGMを再生する
    /// </summary>
    /// <param name="Name">BGM名</param>
    public void BGMPlay(string Name)
    {
        if (!_bgmNameIndex.ContainsKey(Name))
        {
            Debug.Log("存在しないBGM名のため再生できません。");
            return;
        }
        BGMPlay(_bgmNameIndex[Name]);
    }

    /// <summary>
    /// BGMを再生する
    /// </summary>
    /// <param name="index">BGM番号</param>
    public void BGMPlay(int index)
    {
        // BGMリストが設定されていない場合
        if (!_bgmList.Any())
        {
            Debug.Log("BGMListにAudioClipが登録されていないため再生できません。");
            return;
        }

        // 対象外のIndexを指定した場合
        if (index < 0 || _bgmList.Count <= index)
        {
            Debug.Log("存在しないBGM番号を指定したため再生できません。");
            return;
        }

        // ポーズ中である場合は再開、そうでなければ新規再生
        if (_currentBgmIndex == index && !_bgmAudioSource.isPlaying)
        {
            _bgmAudioSource.Play();
        }
        else
        {
            // BGM再生中の場合は先に停止
            if (_bgmAudioSource.isPlaying)
            {
                _bgmAudioSource.Stop();
            }
            // BGM番号を格納し、BGMを再生
            _currentBgmIndex = index;
            AudioClip audiocip = _bgmList[index];
            _bgmAudioSource.clip = audiocip;
            _bgmAudioSource.Play();
        }
    }

    /// <summary>
    /// 再生中のBGMをポーズする
    /// </summary>
    public void BGMPause()
    {
        // BGM再生中の場合はポーズ
        if (_bgmAudioSource.isPlaying)
        {
            _bgmAudioSource.Pause();
        }
    }

    /// <summary>
    /// 再生中のBGMを止める
    /// </summary>
    public void BGMStop()
    {
        // BGMを停止、現在のBGM番号を-1に設定
        _bgmAudioSource.Stop();
        _currentBgmIndex = -1;
    }

    /// <summary>
    /// SEの音量
    /// </summary>
    public float SEVolume
    {
        get
        {
            return _seAudioSource.volume;
        }
        set
        {
            _seAudioSource.volume = value;
        }
    }

    /// <summary>
    /// SEを再生する
    /// </summary>
    /// <param name="Name">SE名</param>
    public void SEPlay(string Name)
    {
        if (!_seNameIndex.ContainsKey(Name))
        {
            Debug.Log("存在しないSE名のため再生できません。");
            return;
        }
        SEPlay(_seNameIndex[Name]);
    }

    /// <summary>
    /// SEを再生する
    /// </summary>
    /// <param name="index">SE番号</param>
    public void SEPlay(int index)
    {
        // SEリストが設定されていない場合
        if (!_seList.Any())
        {
            Debug.Log("SEListにAudioClipが登録されていないため再生できません。");
            return;
        }

        // 対象外のIndexを指定した場合
        if (index < 0 || _seList.Count <= index)
        {
            Debug.Log("存在しないSE番号を指定したため再生できません。");
            return;
        }

        // SE再生リストに追加
        AudioClip audiocip = _seList[index];
        if (_seAudioPlayList.Count < _maxSePlaySoundCount)
        {
            _seAudioPlayList.Add(audiocip);
        }
    }

    /// <summary>
    /// 再生中のSEを止める
    /// </summary>
    public void SEStop()
    {
        // SEを停止
        _seAudioSource.Stop();
    }

    #endregion

    #region private method
    // 自身で作成したPrivateな関数を入れる。

    /// <summary>
    /// AudioSourceの初期化
    /// </summary>
    /// <param name="audiosource"></param>
    /// <param name="loopflg"></param>
    private void InitializeAudioSource(out AudioSource audiosource, bool loopflg)
    {
        audiosource = gameObject.AddComponent<AudioSource>();
        audiosource.clip = null;
        audiosource.playOnAwake = false;
        audiosource.loop = loopflg;
        audiosource.volume = 0.5f;
    }

    /// <summary>
    /// Audioファイル名のリスト作成
    /// </summary>
    /// <param name="audioList"></param>
    /// <param name="nameList"></param>
    private void AudioNameList(List<AudioClip> audioList, Dictionary<string, int> nameList)
    {
        for (int i = 0; i < audioList.Count; i++)
        {
            AudioClip audioclip = audioList[i];
            if (!nameList.ContainsKey(audioclip.name))
            {
                nameList.Add(audioclip.name, i);
            }
        }
    }
    #endregion
}
