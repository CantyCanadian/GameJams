using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class StringExtention
{
    public static void AppendToStream(this string str, System.IO.StreamWriter stream, bool newLine = false)
    {
        if (str == string.Empty)
        {
            return;
        }

        if (newLine)
        {
            stream.Write(System.Environment.NewLine);
        }

        stream.Write(str);
    }
}
