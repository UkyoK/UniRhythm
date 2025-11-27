using Shine.Common;
using Shine.Json;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class MusicDataCreater : EditorWindow
{
    /// <summary>
    /// 表示用MusicData変数
    /// </summary>
    [SerializeField]
    List<MusicData> MusicDatas;

    /// <summary>
    /// リスト用スクロール値
    /// </summary>
    private Vector2 ListScrollPos = Vector2.zero;

    /// <summary>
    /// プレビュー用スクロール値
    /// </summary>
    private Vector2 PreviewScrollPos = Vector2.zero;

    const string _path = "/StreamingAssets/MusicDatas";

    string InputPath;
    string OutputPath;

    Data data = new Data();
    string json;
    string outputText;

    [MenuItem("UniRhythm/MusicDataCreater", priority = 50)]
    private static void ShowWindow()
    {
        var window = GetWindow<MusicDataCreater>();
        window.titleContent = new GUIContent("MusicDataCreater");
        window.Show();
    }

    private void OnGUI()
    {
        // ファイルを開く
        if (GUILayout.Button("Jsonファイルを開く"))
        {
            InputPath = EditorUtility.OpenFilePanel("Select Json File", Application.dataPath + _path, "json");
            LoadJson();
        }
        if (GUILayout.Button("csvから開く"))
        {
            InputPath = EditorUtility.OpenFilePanel("Select Json File", Application.dataPath + _path, "csv");
            LoadCSV();
        }

        // データ編集部分
        ListScrollPos = GUILayout.BeginScrollView(ListScrollPos, false, true);
        var so = new SerializedObject(this);
        so.Update();
        EditorGUILayout.PropertyField(so.FindProperty("MusicDatas"), true);
        so.ApplyModifiedProperties();
        GUILayout.EndScrollView();

        // プレビュー
        if (GUILayout.Button("プレビュー更新"))
        {
            data.MusicDatas = MusicDatas.ToArray();
            outputText = JsonUtility.ToJson(data, true);
        }

        PreviewScrollPos = GUILayout.BeginScrollView(PreviewScrollPos, false, true, GUILayout.Height(150.0f));
        using (new EditorGUI.DisabledScope(true))
        {
            outputText = EditorGUILayout.TextArea(outputText);
        }
        GUILayout.EndScrollView();

        // ファイルを保存
        if (GUILayout.Button("Jsonファイルを保存"))
        {
            data.MusicDatas = MusicDatas.ToArray();
            outputText = JsonUtility.ToJson(data, true);

            OutputPath = EditorUtility.SaveFilePanel("Save Asset", Application.dataPath + _path, "music_datas", "json");
            if (string.IsNullOrEmpty(OutputPath))
            {
                return;
            }

            File.WriteAllText(OutputPath, outputText);
        }
    }

    void LoadJson()
    {
        if (!File.Exists(InputPath))
        {
            Debug.LogError("楽曲一覧データが見つかりませんでした(json)");
            return;
        }

        json = File.ReadAllText(InputPath);
        data = JsonUtility.FromJson<Data>(json);
        MusicDatas = new List<MusicData>(data.MusicDatas);
    }

    void LoadCSV()
    {
        if (!File.Exists(InputPath))
        {
            Debug.LogError("楽曲一覧データが見つかりませんでした(csv)");
            return;
        }

        MusicDatas.Clear();

        FileStream fs = new FileStream(InputPath, FileMode.Open, FileAccess.Read);
        StreamReader sr = new StreamReader(fs);

        string checkLine = sr.ReadLine();
        string[] checkSprit = checkLine.Split(',');
        for (int i = 0; i < (int)MusicInfo.MAX; ++i)
        {
            MusicInfo info = (MusicInfo)Enum.ToObject(typeof(MusicInfo), i);

            if (checkSprit[i] != info.ToString())
            {
                Debug.LogError("楽曲一覧データの形式が間違っています(csv)");
                return;
            }
        }

        while (sr.Peek() != -1)
        {
            string line = sr.ReadLine();
            string[] split = line.Split(',');

            MusicData musicData = new MusicData();
            musicData.Levels = new Level();

            musicData.Title = split[(int)MusicInfo.Title];
            musicData.Artist = split[(int)MusicInfo.Artist];
            musicData.StartBPM = split[(int)MusicInfo.StartBPM];
            musicData.Offset = split[(int)MusicInfo.Offset];
            musicData.FolderName = split[(int)MusicInfo.FolderName];
            musicData.Levels.Easy = int.Parse(split[(int)MusicInfo.EasyLevel]);
            musicData.Levels.Normal = int.Parse(split[(int)MusicInfo.NormalLevel]);
            musicData.Levels.Expert = int.Parse(split[(int)MusicInfo.ExpertLevel]);
            musicData.Levels.Master = int.Parse(split[(int)MusicInfo.MasterLevel]);

            // csvで文字列に含まれるカンマは全角で入力するので、半角に変換
            musicData.Title = musicData.Title.Replace("，", ",");
            musicData.Artist = musicData.Artist.Replace("，", ",");

            MusicDatas.Add(musicData);
        }

    }

}
