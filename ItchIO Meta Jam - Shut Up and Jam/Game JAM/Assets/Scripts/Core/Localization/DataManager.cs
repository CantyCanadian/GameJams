using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    public bool OutputLog = false;

    public int FilesPerFrame = 2;

    public string DataFile;

    public string[] GameplayLocFiles;
    public string[] CharacterLocFiles;
    public string[] CharacterGenFiles;
    public string[] ItemLocFiles;

    private float m_LoadingPercentage = 0.0f;

    //Data
    private Dictionary<string, string> m_Data;

    //All locs
    private Dictionary<string, string> m_GameplayLoc;
    private Dictionary<string, string> m_CharacterLoc;
    private Dictionary<string, string> m_SingularNameLoc;
    private Dictionary<string, string> m_PluralNameLoc;

    public void Initialize()
    {
        m_Data = new Dictionary<string, string>();

        m_GameplayLoc = new Dictionary<string, string>();
        m_CharacterLoc = new Dictionary<string, string>();
        m_SingularNameLoc = new Dictionary<string, string>();
        m_PluralNameLoc = new Dictionary<string, string>();
    }

    public void LoadEarlyData()
    {
        m_GameplayLoc.Add("EARLY_PLAYENGLISH", "I play in English.");
        m_GameplayLoc.Add("EARLY_PLAYFRENCH", "Je joue en Français.");

        m_GameplayLoc.Add("EARLY_LOADENGLISH", "Loading");
        m_GameplayLoc.Add("EARLY_LOADFRENCH", "Chargement");
    }

    public IEnumerator LoadData()
    {
        m_LoadingPercentage = 0.0f;

        int files = 0;

        int fileCount = 1 + GameplayLocFiles.Length + CharacterLocFiles.Length + CharacterGenFiles.Length + ItemLocFiles.Length;
        float percentPerFile = 1.0f / fileCount;

        string language = GameManager.Instance.Settings.Language;

        LoadHardCodedData(language);

        if (DataFile == string.Empty)
        {
            Debug.LogError("DataManager : No Data file found. Skipping data loading.");
        }
        else
        {
            m_Data = ParsingUtil.LoadCSV_IDColumn(DataFile, 1);

            //GAMEPLAY LOC
            int singleColumn = int.Parse(m_Data["LOCALIZATION_COLUMN_" + language + "_SINGLE"]);

            foreach (string filename in GameplayLocFiles)
            {
                Dictionary<string, string> locs = ParsingUtil.LoadCSV_IDColumn(filename, singleColumn);
                m_GameplayLoc.Append(locs);
                m_LoadingPercentage += percentPerFile;

                if (OutputLog)
                {
                    foreach(KeyValuePair<string, string> kvp in locs)
                    {
                        Debug.Log("Key [" + kvp.Key + "] - Value [" + kvp.Value + "]");
                    }
                }

                files++;
                if (files >= FilesPerFrame)
                {
                    files = 0;
                    yield return null;
                }

                m_LoadingPercentage += percentPerFile;
            }

            //CHARACTER LOC
            foreach (string filename in CharacterLocFiles)
            {
                Dictionary<string, string> locs = ParsingUtil.LoadCSV_IDColumn(filename, singleColumn);
                m_CharacterLoc.Append(locs);
                m_LoadingPercentage += percentPerFile;

                if (OutputLog)
                {
                    foreach (KeyValuePair<string, string> kvp in locs)
                    {
                        Debug.Log("Key [" + kvp.Key + "] - Value [" + kvp.Value + "]");
                    }
                }

                files++;
                if (files >= FilesPerFrame)
                {
                    files = 0;
                    yield return null;
                }

                m_LoadingPercentage += percentPerFile;
            }

            //CHARACTER GEN
            foreach (string filename in CharacterGenFiles)
            {
                Dictionary<string, string> locs = ParsingUtil.LoadCSV_IDColumn(filename, 1);
                m_CharacterLoc.Append(locs);
                m_LoadingPercentage += percentPerFile;

                if (OutputLog)
                {
                    foreach (KeyValuePair<string, string> kvp in locs)
                    {
                        Debug.Log("Key [" + kvp.Key + "] - Value [" + kvp.Value + "]");
                    }
                }

                files++;
                if (files >= FilesPerFrame)
                {
                    files = 0;
                    yield return null;
                }

                m_LoadingPercentage += percentPerFile;
            }

            //ITEM NAME LOC
            int singularColumn = int.Parse(m_Data["LOCALIZATION_COLUMN_" + language]);
            int pluralColumn = int.Parse(m_Data["LOCALIZATION_COLUMN_" + language + "_PLURAL"]);

            foreach (string filename in ItemLocFiles)
            {
                List<List<string>> locs = ParsingUtil.LoadCSV_IDMultipleColumn(filename, new int[] { singularColumn, pluralColumn });

                foreach (List<string> list in locs)
                {
                    m_SingularNameLoc.Add(list[0], list[1]);
                    m_PluralNameLoc.Add(list[0], list[2]);
                }

                if (OutputLog)
                {
                    foreach (List<string> ls in locs)
                    {
                        Debug.Log("Key [" + ls[0] + "] - ValueS [" + ls[1] + "] - ValueP [" + ls[2] + "]");
                    }
                }

                m_LoadingPercentage += percentPerFile;

                files++;
                if (files >= FilesPerFrame)
                {
                    files = 0;
                    yield return null;
                }
            }
        }

        m_LoadingPercentage = 1.0f;
    }

    public string GetData(string id)
    {
        if (m_Data.ContainsKey(id))
        {
            return m_Data[id];
        }
        else
        {
            Debug.Assert(true, "Couldn't get data from data file.");
            return "";
        }
    }

    public string GetGameplayLoc(string id)
    {
        if (m_GameplayLoc.ContainsKey(id))
        {
            return m_GameplayLoc[id];
        }
        else
        {
            return m_GameplayLoc["DEBUG_ERRORGP"];
        }
    }

    public string GetCharacterLoc(string id)
    {
        if (m_CharacterLoc.ContainsKey(id))
        {
            return m_CharacterLoc[id];
        }
        else
        {
            return m_CharacterLoc["DEBUG_ERRORCHAR"];
        }
    }

    public string GetSingularItemLoc(string id)
    {
        if (m_SingularNameLoc.ContainsKey(id))
        {
            return m_SingularNameLoc[id];
        }
        else
        {
            return m_SingularNameLoc["DEBUG_ERRORNAMES"];
        }
    }

    public string GetPluralItemLoc(string id)
    {
        if (m_PluralNameLoc.ContainsKey(id))
        {
            return m_PluralNameLoc[id];
        }
        else
        {
            return m_PluralNameLoc["DEBUG_ERRORNAMEP"];
        }
    }

    public float GetLoadingScreenPercentage()
    {
        return m_LoadingPercentage;
    }

    private void LoadHardCodedData(string language)
    {
        switch (language)
        {
            case "ENGLISH":
                m_GameplayLoc.Add("DEBUG_ERRORGP", "Loc Error");
                m_CharacterLoc.Add("DEBUG_ERRORCHAR", "Loc Error");
                m_SingularNameLoc.Add("DEBUG_ERRORNAMES", "Loc Error");
                m_PluralNameLoc.Add("DEBUG_ERRORNAMEP", "Loc Error");
                break;

            case "FRENCH":
                m_GameplayLoc.Add("DEBUG_ERRORGP", "Erreur de Loc");
                m_CharacterLoc.Add("DEBUG_ERRORCHAR", "Erreur de Loc");
                m_SingularNameLoc.Add("DEBUG_ERRORNAMES", "Erreur de Loc");
                m_PluralNameLoc.Add("DEBUG_ERRORNAMEP", "Erreur de Loc");
                break;
        }
    }
}
