using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Shine.Common
{
    public enum NowJudgement
    {
        None,
        Perfect,
        Great_Fast,
        Great_Late,
        Miss_Fast,
        Miss_Late,
        Miss_Pass,
    }

    public struct Note
    {
        public int lane;
        public float justTime;
        public bool isJudged;
        public NowJudgement judgement;

        public Note(int lane, float justTime, bool isJudged, NowJudgement judgement)
        {
            this.lane = lane;
            this.justTime = justTime;
            this.isJudged = isJudged;
            this.judgement = judgement;
        }
    }

    public enum MusicInfo
    {
        Title,
        Artist,
        StartBPM,
        Offset,
        FolderName,
        EasyLevel,
        NormalLevel,
        ExpertLevel,
        MasterLevel,
        MAX
    }

    public enum Difficulty
    {
        Easy,
        Normal,
        Expert,
        Master,
    }

    public struct SongInfo
    {
        public string Title;
        public string Artist;
        public float StartBPM;
        public float Offset;
        public string FolderName;
        public int EasyLevel;
        public int NormalLevel;
        public int ExpertLevel;
        public int MasterLevel;
    }

    /// <summary>
    /// 譜面情報の分類
    /// </summary>
    public enum ChartType
    {
        Info,
        Measure,
        Lane,
        Body,
    }

    public enum ChartInfoType
    {
        Note,
        MeasureChange,
        BPMChange,
        SceneChange,
    }

    /// <summary>
    /// ノーツの種類
    /// </summary>
    public enum NoteType
    {
        Empty,
        Hit,
        //Hold_Start,
        //Hold_End,
    }

    /// <summary>
    /// 楽曲データ情報
    /// </summary>
    public enum MusicDataType
    {
        Title,
        Artist,
        BPM,
        Offset,
        Path,
    }
}

namespace Shine.Json
{
    [Serializable]
    public class Level
    {
        public int Easy;
        public int Normal;
        public int Expert;
        public int Master;
    }

    [Serializable]
    public class MusicData
    {
        public string Title;
        public string Artist;
        public string StartBPM;
        public string Offset;
        public string FolderName;
        public Level Levels;
    }

    [Serializable]
    public class Data
    {
        public MusicData[] MusicDatas;
    }
}