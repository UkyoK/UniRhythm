using DG.Tweening;
using Shine.Common;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SongSelect : MonoBehaviour
{
    [SerializeField] GameObject Panel0;
    [SerializeField] GameObject Panel1;
    [SerializeField] GameObject Panel2;
    [SerializeField] GameObject Panel3;
    [SerializeField] GameObject Panel4;
    [SerializeField] GameObject Panel5;
    [SerializeField] GameObject Panel6;

    private const int _ArraySize = 7;
    private const int _CenterSongID = 3;

    /// <summary>
    /// 表示する曲データ配列
    /// </summary>
    SongInfo[] DisplaySongInfo = new SongInfo[_ArraySize];

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
    private int TopSong;

    /// <summary>
    /// スクロール処理を行うかどうか
    /// </summary>
    private bool isScroll;

    void Awake()
    {
        PanelList[0] = Panel0;
        PanelList[1] = Panel1;
        PanelList[2] = Panel2;
        PanelList[3] = Panel3;
        PanelList[4] = Panel4;
        PanelList[5] = Panel5;
        PanelList[6] = Panel6;

        TopSong = SongInfoLoader.Instance.SongInfoList.Count - 1 + SettingManager.Instance.TopSongID;

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
        }

        Debug.Log("表示用データ読み込み完了");

        isScroll = true;
    }

    void Start()
    {
        Fade.Instance.FadeIn();
        TopSong = SettingManager.Instance.TopSongID - 1;
        if (TopSong < 0)
        {
            TopSong = SongInfoLoader.Instance.SongInfoList.Count - 1;
        }
    }

    void Update()
    {
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
        if (Input.GetKeyDown(KeyCode.Return) && isScroll)
        {
            SettingManager.Instance.SetDefaultSetting();
            SettingManager.Instance.LoadChartData(DisplaySongInfo[_CenterSongID].Title);
            SettingManager.Instance.TopSongID = TopSong + 1;
            Fade.Instance.FadeOut("InGameScene");
        }

    }

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

        isScroll = false;
    }

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
