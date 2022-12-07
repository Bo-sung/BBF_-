using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

public class TranslateManager 
{
    private static TranslateManager instance;
    public static TranslateManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new TranslateManager();
            }

            return instance;
        }
    }
    public Dictionary<string, string> dic_korean = new Dictionary<string, string>();
    public Dictionary<string, string> dic_english = new Dictionary<string, string>();
    public Dictionary<string, string> dic_japanese = new Dictionary<string, string>();
    public Dictionary<string, string> dic_french = new Dictionary<string, string>();
    public Dictionary<string, string> dic_russian = new Dictionary<string, string>();
    public Dictionary<string, string> dic_portuguese = new Dictionary<string, string>();
    public Dictionary<string, string> dic_german = new Dictionary<string, string>();
    public Dictionary<string, string> dic_spanish = new Dictionary<string, string>();
    public Dictionary<string, string> dic_thai = new Dictionary<string, string>();
    public Dictionary<string, string> dic_italian = new Dictionary<string, string>();
    public Dictionary<string, string> dic_indonesio = new Dictionary<string, string>();
    public Dictionary<string, string> dic_vietnamese = new Dictionary<string, string>();
    public Dictionary<string, string> dic_simpleChinese = new Dictionary<string, string>();
    public Dictionary<string, string> dic_traditionChinese = new Dictionary<string, string>();
    public static int LANGUAGE_TYPE_MAX;
    private Dictionary<LANGUAGE_TYPE, Dictionary<string, string>> dicTotalLanguege = new Dictionary<LANGUAGE_TYPE, Dictionary<string, string>>();
    private Dictionary<string, string> dic_language;

    private Font m_defaultFont;
    private Font m_thaiFont;
    private LANGUAGE_TYPE m_current;
    private bool m_changeFont;

    public void Init()
    {
        LANGUAGE_TYPE_MAX = System.Enum.GetValues(typeof(LANGUAGE_TYPE)).Length;
        m_changeFont = false;
        m_defaultFont = Resources.Load<Font>("GodoB");
        m_thaiFont = Resources.Load<Font>("Pridi-Regular");
        LoadData();
    }

    private void LoadData()
    {
        string languageData = Resources.Load<TextAsset>("Table/translate").text;
        TableLanguageData[] arrayTranslateData = Util.JsonNewtonsoft.DeserializeObject<TableLanguageData[]>(languageData);

        for (int i = 0; i < LANGUAGE_TYPE_MAX; i++)
        {
            dicTotalLanguege[(LANGUAGE_TYPE)i] = new Dictionary<string, string>();
        }
        
        for (int i = 0, count = arrayTranslateData.Length; i < count; i++)
        {
            dicTotalLanguege[LANGUAGE_TYPE.KO][arrayTranslateData[i].Item] = arrayTranslateData[i].KO;
            dicTotalLanguege[LANGUAGE_TYPE.EN][arrayTranslateData[i].Item] = arrayTranslateData[i].EN;
            dicTotalLanguege[LANGUAGE_TYPE.TC][arrayTranslateData[i].Item] = arrayTranslateData[i].TC;
            dicTotalLanguege[LANGUAGE_TYPE.SC][arrayTranslateData[i].Item] = arrayTranslateData[i].SC;
            dicTotalLanguege[LANGUAGE_TYPE.JA][arrayTranslateData[i].Item] = arrayTranslateData[i].JA;
            dicTotalLanguege[LANGUAGE_TYPE.DE][arrayTranslateData[i].Item] = arrayTranslateData[i].DE;
            dicTotalLanguege[LANGUAGE_TYPE.ES][arrayTranslateData[i].Item] = arrayTranslateData[i].ES;
            dicTotalLanguege[LANGUAGE_TYPE.FR][arrayTranslateData[i].Item] = arrayTranslateData[i].FR;
            dicTotalLanguege[LANGUAGE_TYPE.RU][arrayTranslateData[i].Item] = arrayTranslateData[i].RU;
            dicTotalLanguege[LANGUAGE_TYPE.ID][arrayTranslateData[i].Item] = arrayTranslateData[i].ID;
            dicTotalLanguege[LANGUAGE_TYPE.AR][arrayTranslateData[i].Item] = arrayTranslateData[i].AR;
            dicTotalLanguege[LANGUAGE_TYPE.MS][arrayTranslateData[i].Item] = arrayTranslateData[i].MS;
            dicTotalLanguege[LANGUAGE_TYPE.TH][arrayTranslateData[i].Item] = arrayTranslateData[i].TH;
            dicTotalLanguege[LANGUAGE_TYPE.PT][arrayTranslateData[i].Item] = arrayTranslateData[i].PT;
            dicTotalLanguege[LANGUAGE_TYPE.TR][arrayTranslateData[i].Item] = arrayTranslateData[i].TR;
            dicTotalLanguege[LANGUAGE_TYPE.IT][arrayTranslateData[i].Item] = arrayTranslateData[i].IT;
        }
    }

    public void SetChangeFont(bool isEnable)
    {
        m_changeFont = isEnable;
    }
    
    public void SetLanguageData(LANGUAGE_TYPE language)
    {
        m_current = language;
        dic_language = dicTotalLanguege[m_current];
    }

    public string GetTranslateText(string key)
    {
        if (string.IsNullOrEmpty(key) || dic_language == null || !dic_language.ContainsKey(key))
        {
            return string.Format("No Key_{0}", key);
        }
        else
        {
            return dic_language[key];
        }
    }

    public void ChangeFont(UnityEngine.UI.Text text)
    {
        if (m_changeFont && m_current == LANGUAGE_TYPE.TH+ 1)
        {
            text.font = m_defaultFont;
        }
        else if (m_current == LANGUAGE_TYPE.TH)
        {
            text.font = m_thaiFont;
        }
    }
}
