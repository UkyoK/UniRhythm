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

    SongInfo[] DisplaySongInfo = new SongInfo[_ArraySize];
    GameObject[] PanelList = new GameObject[_ArraySize];
    RectTransform[] RectList = new RectTransform[_ArraySize];
    TextMeshProUGUI[] TitleList = new TextMeshProUGUI[_ArraySize];
    TextMeshProUGUI[] ArtistList = new TextMeshProUGUI[_ArraySize];
    TextMeshProUGUI[] StartBPMList = new TextMeshProUGUI[_ArraySize];

    DG.Tweening.Sequence sequence;

    private int TopSong;

    private bool isScroll;
    private void ArrowScroll() { isScroll = true; }

    void Awake()
    {
        PanelList[0] = Panel0;
        PanelList[1] = Panel1;
        PanelList[2] = Panel2;
        PanelList[3] = Panel3;
        PanelList[4] = Panel4;
        PanelList[5] = Panel5;
        PanelList[6] = Panel6;

        for (int i = 0; i < _ArraySize; i++)
        {
            if (i == 0)
            {
                // 一番上のパネルには、リストの最後の要素を入れる
                DisplaySongInfo[i] = SongInfoLoader.Instance.SongInfoList[SongInfoLoader.Instance.SongInfoList.Count - 1];
            }
            else
            {
                DisplaySongInfo[i] = SongInfoLoader.Instance.SongInfoList[i - 1];
            }
            TopSong = SongInfoLoader.Instance.SongInfoList.Count - 1;

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

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.DownArrow) && isScroll)
        {
            ++TopSong;
            if (TopSong >= SongInfoLoader.Instance.SongInfoList.Count)
            {
                TopSong = 0;
            }

            UpdateSongList();
            SelectAnimationUp();
        }
        if (Input.GetKeyDown(KeyCode.UpArrow) && isScroll)
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
            SettingManager.Instance.LoadChartData(DisplaySongInfo[3].Title);
            SceneManager.LoadScene("TestScene");
        }

        int top = TopSong;
    }

    void UpdateSongList()
    {
        for (int i = 0; i < _ArraySize; ++i)
        {
            int songID = i + TopSong;
            if (songID > SongInfoLoader.Instance.SongInfoList.Count - 1)
            {
                songID -= SongInfoLoader.Instance.SongInfoList.Count;
            }

            DisplaySongInfo[i] = SongInfoLoader.Instance.SongInfoList[songID];

            TitleList[i].text = DisplaySongInfo[i].Title;
            ArtistList[i].text = DisplaySongInfo[i].Artist;
            StartBPMList[i].text = DisplaySongInfo[i].StartBPM.ToString();

            TitleList[i].rectTransform.sizeDelta = new Vector2(TitleList[i].preferredWidth, TitleList[i].rectTransform.sizeDelta.y);
            ArtistList[i].rectTransform.sizeDelta = new Vector2(ArtistList[i].preferredWidth, ArtistList[i].rectTransform.sizeDelta.y);
        }

        isScroll = false;
    }

    void SelectAnimationUp()
    {
        sequence.Kill(true);

        sequence = DOTween.Sequence()
            .Append(RectList[0].DOAnchorPos(new Vector2(-60, 286), 0.5f).From())
            .Join(RectList[1].DOAnchorPos(new Vector2(-30, 143), 0.5f).From())
            .Join(RectList[2].DOAnchorPos(new Vector2(0, 0), 0.5f).From())
            .Join(RectList[3].DOAnchorPos(new Vector2(-30, -143), 0.5f).From())
            .Join(RectList[4].DOAnchorPos(new Vector2(-60, -286), 0.5f).From())
            .Join(RectList[5].DOAnchorPos(new Vector2(-90, -429), 0.5f).From())
            .OnComplete(ArrowScroll);
    }

    void SelectAnimationDown()
    {
        sequence.Kill(true);

        sequence = DOTween.Sequence()
            .Append(RectList[1].DOAnchorPos(new Vector2(-90, 429), 0.5f).From())
            .Join(RectList[2].DOAnchorPos(new Vector2(-60, 286), 0.5f).From())
            .Join(RectList[3].DOAnchorPos(new Vector2(-30, 143), 0.5f).From())
            .Join(RectList[4].DOAnchorPos(new Vector2(0, 0), 0.5f).From())
            .Join(RectList[5].DOAnchorPos(new Vector2(-30, -143), 0.5f).From())
            .Join(RectList[6].DOAnchorPos(new Vector2(-60, -286), 0.5f).From())
            .OnComplete(ArrowScroll);
    }

}
