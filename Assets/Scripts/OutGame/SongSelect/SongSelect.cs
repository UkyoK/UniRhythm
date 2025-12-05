using DG.Tweening;
using Shine.Common;
using TMPro;
using UnityEngine;

public class SongSelect : MonoBehaviour
{
    public static SongSelect Instance;

    [SerializeField] GameObject Panel0;
    [SerializeField] GameObject Panel1;
    [SerializeField] GameObject Panel2;
    [SerializeField] GameObject Panel3;
    [SerializeField] GameObject Panel4;
    [SerializeField] GameObject Panel5;
    [SerializeField] GameObject Panel6;

    private const int _ArraySize = 7;
    public int _CenterSongID = 3;

    public SelectState SongSelectState;

    /// <summary>
    /// 表示する曲データ配列
    /// </summary>
    SongInfo[] DisplaySongInfo = new SongInfo[_ArraySize];
    public SongInfo CenterSongInfo = new SongInfo();

    /// <summary>
    /// 表示用パネルオブジェクト
    /// </summary>
    GameObject[] PanelList = new GameObject[_ArraySize];

    /// <summary>
    /// 表示用パネルRectTransform
    /// </summary>
    RectTransform[] RectList = new RectTransform[_ArraySize];

    /// <summary>
    /// 表示用タイトル
    /// </summary>
    TextMeshProUGUI[] TitleList = new TextMeshProUGUI[_ArraySize];

    /// <summary>
    /// 表示用アーティスト
    /// </summary>
    TextMeshProUGUI[] ArtistList = new TextMeshProUGUI[_ArraySize];

    /// <summary>
    /// 表示用BPM
    /// </summary>
    TextMeshProUGUI[] StartBPMList = new TextMeshProUGUI[_ArraySize];

    private Sequence sequence;

    /// <summary>
    /// DOTween動作時間
    /// </summary>
    private const float _duration = 0.25f;

    /// <summary>
    /// パネル0に表示される曲のID
    /// </summary>
    public int TopSong;

    /// <summary>
    /// スクロール処理を行うかどうか
    /// </summary>
    private bool isScroll;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        // パネルの配列にそれぞれのパネルを格納
        PanelList[0] = Panel0;
        PanelList[1] = Panel1;
        PanelList[2] = Panel2;
        PanelList[3] = Panel3;
        PanelList[4] = Panel4;
        PanelList[5] = Panel5;
        PanelList[6] = Panel6;

        // 表示する分の曲データを取得、表示
        int songID = SettingManager.Instance.TopSongID;
        for (int i = 0; i < _ArraySize; ++i, ++songID)
        {
            if (songID >= SongInfoLoader.Instance.SongInfoList.Count)
            {
                songID = 0;
            }

            if (i == 0)
            {
                // 一番上のパネルには、リストの最後の要素を入れる
                DisplaySongInfo[i] = SongInfoLoader.Instance.SongInfoList[songID];
            }
            else
            {
                // 残りは先頭から順に入れる
                DisplaySongInfo[i] = SongInfoLoader.Instance.SongInfoList[songID];
            }

            RectList[i] = PanelList[i].GetComponent<RectTransform>();

            TitleList[i] = PanelList[i].transform.Find("TitlePanel/Title").gameObject.GetComponent<TextMeshProUGUI>();
            ArtistList[i] = PanelList[i].transform.Find("ArtistPanel/Artist").gameObject.GetComponent<TextMeshProUGUI>();
            StartBPMList[i] = PanelList[i].transform.Find("BPMPanel/StartBPM").gameObject.GetComponent<TextMeshProUGUI>();

            TitleList[i].text = DisplaySongInfo[i].Title;
            ArtistList[i].text = DisplaySongInfo[i].Artist;
            StartBPMList[i].text = DisplaySongInfo[i].StartBPM.ToString();

            CenterSongInfo = DisplaySongInfo[_CenterSongID];
        }

        Debug.Log("表示用データ読み込み完了");

        SongSelectState = SelectState.SongSelect;
        isScroll = true;
    }

    void Start()
    {
        Fade.Instance.FadeIn();

        // 先頭の曲番号を取得
        TopSong = SettingManager.Instance.TopSongID - 1;
        if (TopSong < 0)
        {
            // 0未満だったらリストの最後の曲番号に補正
            TopSong = SongInfoLoader.Instance.SongInfoList.Count - 1;
        }

        MySoundManager.Instance.PlaySongSelectVoice(SongSelectState);
    }

    async void Update()
    {
        if (SongSelectState != SelectState.SongSelect)
        {
            // 楽曲選択状態でないなら、処理をしない
            return;
        }

        // 下の曲に移動
        if (Input.GetKey(KeyCode.DownArrow) && isScroll)
        {
            ++TopSong;
            if (TopSong >= SongInfoLoader.Instance.SongInfoList.Count)
            {
                TopSong = 0;
            }

            UpdateSongList();
            SelectAnimationUp();
        }

        // 上の曲に移動
        if (Input.GetKey(KeyCode.UpArrow) && isScroll)
        {
            --TopSong;
            if (TopSong < 0)
            {
                TopSong = SongInfoLoader.Instance.SongInfoList.Count - 1;
            }

            UpdateSongList();
            SelectAnimationDown();
        }

        // 難易度を選ぶ
        if (Input.GetKeyDown(KeyCode.Return) && isScroll)
        {
            SongSelectState = SelectState.LevelSelect;
            MySoundManager.Instance.PlaySongSelectVoice(SongSelectState);
            await LevelSelect.Instance.LevelSelectStep();
        }

    }

    /// <summary>
    /// 表示曲一覧を更新
    /// </summary>
    void UpdateSongList()
    {
        int songID = TopSong + 1;
        SettingManager.Instance.TopSongID = TopSong;

        for (int i = 0; i < _ArraySize; ++i, ++songID)
        {
            if (songID >= SongInfoLoader.Instance.SongInfoList.Count)
            {
                songID = 0;
            }

            DisplaySongInfo[i] = SongInfoLoader.Instance.SongInfoList[songID];

            TitleList[i].text = DisplaySongInfo[i].Title;
            ArtistList[i].text = DisplaySongInfo[i].Artist;
            StartBPMList[i].text = DisplaySongInfo[i].StartBPM.ToString();
        }

        CenterSongInfo = DisplaySongInfo[_CenterSongID];
        isScroll = false;
    }

    /// <summary>
    /// 曲一覧を1つ上に移動
    /// </summary>
    async void SelectAnimationUp()
    {
        await DOTween.Sequence()
            .Append(RectList[0].DOAnchorPos(new Vector2(-60, 286), _duration).From())
            .Join(RectList[1].DOAnchorPos(new Vector2(-30, 143), _duration).From())
            .Join(RectList[2].DOAnchorPos(new Vector2(0, 0), _duration).From())
            .Join(RectList[3].DOAnchorPos(new Vector2(-30, -143), _duration).From())
            .Join(RectList[4].DOAnchorPos(new Vector2(-60, -286), _duration).From())
            .Join(RectList[5].DOAnchorPos(new Vector2(-90, -429), _duration).From())
            .AsyncWaitForCompletion();

        isScroll = true;
    }

    /// <summary>
    /// 曲一覧を1つ下に移動
    /// </summary>
    async void SelectAnimationDown()
    {
        await DOTween.Sequence()
            .Append(RectList[1].DOAnchorPos(new Vector2(-90, 429), _duration).From())
            .Join(RectList[2].DOAnchorPos(new Vector2(-60, 286), _duration).From())
            .Join(RectList[3].DOAnchorPos(new Vector2(-30, 143), _duration).From())
            .Join(RectList[4].DOAnchorPos(new Vector2(0, 0), _duration).From())
            .Join(RectList[5].DOAnchorPos(new Vector2(-30, -143), _duration).From())
            .Join(RectList[6].DOAnchorPos(new Vector2(-60, -286), _duration).From())
            .AsyncWaitForCompletion();

        isScroll = true;
    }

}
