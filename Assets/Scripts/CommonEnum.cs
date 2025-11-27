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

        public Note(int lane, float justTime)
        {
            this.lane = lane;
            this.justTime = justTime;
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

    /// <summary>
    /// 譜面コマンド
    /// </summary>
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