using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    public enum SaveOrder
    {
        Jams,
        Player,
        Null
    }

    public string Directory { get { return m_Directory; } }

    private Dictionary<SaveOrder, ISaveable> m_Saveables = null;
    private string m_Directory;

    //Save Data
    private string m_SaveLocation;
    private string m_SaveName;
    private int m_LastSaved;

    public void Initialize()
    {
        m_Directory = Application.persistentDataPath + @"\Saves\";

        if (!System.IO.Directory.Exists(m_Directory))
        {
            System.IO.Directory.CreateDirectory(m_Directory);
        }

        m_Saveables = new Dictionary<SaveOrder, ISaveable>();
    }

    public void RegisterSaveable(SaveOrder order, ISaveable saveable)
    {
        m_Saveables.Add(order, saveable);
    }

    public void CreateNewSave(string saveName)
    {
        int fileIndex = 0;
        string saveLocation = "";
        while (true)
        {
            saveLocation = m_Directory + saveName + fileIndex.ToString() + ".sav";
            if (!System.IO.File.Exists(saveLocation))
            {
                break;
            }
            else
            {
                fileIndex++;
            }
        }

        System.IO.Directory.CreateDirectory(m_Directory);
        System.IO.FileStream fs = System.IO.File.Create(saveLocation);
        fs.Close();

        m_SaveLocation = saveLocation;
        m_SaveName = saveName;

        System.DateTime epochStart = new System.DateTime(1970, 1, 1, 0, 0, 0, System.DateTimeKind.Utc);
        m_LastSaved = (int)(System.DateTime.UtcNow - epochStart).TotalSeconds;

        foreach(KeyValuePair<SaveOrder, ISaveable> kvp in m_Saveables)
        {
            if (kvp.Key == SaveOrder.Null)
            {
                continue;
            }

            kvp.Value.SetSaveableData();
        }

        Save();
    }

    public void Save()
    {
        System.DateTime epochStart = new System.DateTime(1970, 1, 1, 0, 0, 0, System.DateTimeKind.Utc);
        m_LastSaved = (int)(System.DateTime.UtcNow - epochStart).TotalSeconds;

        System.IO.StreamWriter writer = new System.IO.StreamWriter(m_SaveLocation, false);

        m_LastSaved.ToString().AppendToStream(writer);
        m_SaveName.AppendToStream(writer, true);

        foreach (KeyValuePair<SaveOrder, ISaveable> kvp in m_Saveables)
        {
            kvp.Key.ToString().AppendToStream(writer, true);

            string value = "|";
            foreach(string str in kvp.Value.GetSaveableData())
            {
                value += str + "|";
            }

            value.AppendToStream(writer);
        }

        writer.Close();
    }

    public void Load(string saveLocation)
    {
        if (System.IO.File.Exists(saveLocation))
        {
            System.IO.StreamReader reader = new System.IO.StreamReader(saveLocation);

            m_LastSaved = int.Parse(reader.ReadLine());
            m_SaveName = reader.ReadLine();

            while (reader.Peek() >= 0)
            {
                string line = reader.ReadLine();

                int index = 0;
                int buffer = 0;
                SaveOrder key = SaveOrder.Null;
                List<string> values = new List<string>();
                bool first = false;

                index = line.IndexOf('|', index);
                while (index != -1)
                {
                    if (!first)
                    {
                        key = (SaveOrder)System.Enum.Parse(typeof(SaveOrder), line.Substring(buffer, index));
                        first = true;
                    }
                    else
                    {
                        values.Add(line.Substring(buffer, index - buffer));
                    }

                    buffer = index + 1;
                    index = line.IndexOf('|', buffer);
                }

                if (key != SaveOrder.Null)
                {
                    m_Saveables[key].SetSaveableData(values.ToArray());
                }
            }

            m_SaveLocation = saveLocation;

            reader.Close();
        }
    }

    public string[] ExtractLoadScreenData(string fileName)
    {
        List<string> result = new List<string>();
        List<string> content = new List<string>();

        if (System.IO.File.Exists(fileName))
        {
            System.IO.StreamReader reader = new System.IO.StreamReader(fileName);

            while (reader.Peek() >= 0)
            {
                content.Add(reader.ReadLine());
            }

            reader.Close();
        }

        if (content.Count == 0)
        {
            return null;
        }

        result.Add(content[0]); //Last Saved
        result.Add(content[1]); //Save Name

        for(int i = 2; i < content.Count; i++)
        {
            int index = content[i].IndexOf('|');
            int indexBuf = 0;

            if (index != -1 && content[i].Substring(0, index) == SaveOrder.Player.ToString())
            {
                indexBuf = index + 1;
                index = content[i].IndexOf('|', indexBuf);
                result.Add(content[i].Substring(indexBuf, index - indexBuf)); //Money

                indexBuf = index + 1;
                index = content[i].IndexOf('|', indexBuf);
                result.Add(content[i].Substring(indexBuf, index - indexBuf)); //Jam Count

                indexBuf = index + 1;
                index = content[i].IndexOf('|', indexBuf);
                result.Add(content[i].Substring(indexBuf, index - indexBuf)); //Jam Wins
            }
        }

        return result.ToArray();
    }
}
