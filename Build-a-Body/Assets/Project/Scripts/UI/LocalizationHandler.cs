using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace ProjectHKU.UI
{
    public static class LocalizationHandler
    {
        static string SPLIT_RE = @",(?=(?:[^""]*""[^""]*"")*(?![^""]*""))";
        static string LINE_SPLIT_RE = @"\r\n|\n\r|\n|\r";
        static char[] TRIM_CHARS = { '\"' };

        public static Dictionary<string, string> Load(TextAsset sourceFile){
            Dictionary<string, string> localization = new Dictionary<string, string>();
            var lines = Regex.Split(sourceFile.text, LINE_SPLIT_RE);
            if (lines.Length <= 1) {
                Debug.LogError("localization file empty");
                return localization;
            }
            for(int i = 0; i < lines.Length; i++){
                string line = lines[i];
                var values = Regex.Split(line, SPLIT_RE);
                if(values.Length < 2 || values[0] == "") {
                    if (i != lines.Length-1) Debug.LogError($"localization file line {i+1} not right!");
                    continue;
                }
                var key = values[0];
                var value = values[1].TrimStart(TRIM_CHARS).TrimEnd(TRIM_CHARS).Replace("\\", "").Replace("\"\"", "\"");
                // special exceptions to make localization more easier 
                value = value.Replace("{endline}", "\n");
                localization.Add(key, value);
            }
            return localization;
        }
    }
}
