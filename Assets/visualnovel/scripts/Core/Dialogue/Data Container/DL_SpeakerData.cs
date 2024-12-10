using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;

namespace DIALOGUE
{
    public class DL_SpeakerData
    {
        public string name, castName;
        public Vector2 castPosition;
        public string rawData { get; private set; } = string.Empty;

        //the name to be displayed in dialogue box showing who is speaking
        public string displayName => castName != string.Empty ? castName : name;
        public List<(int layer, string expression)> CastExpression { get; set; }

        public bool isCastingName => castName != string.Empty;
        public bool isCastingPosition = false;
        public bool isCastingExpressions => CastExpression.Count > 0;

        public bool makeCharacterEnter = false;

        private const string NAME_CAST_ID = " as ";
        private const string POS_CAST_ID = " at ";
        private const string EXP_CAST_ID = " [";
        private const char AXISDELIMETER_ID = ':';
        private const char EXPRESSION_LAYER_JOINER = ',';
        private const char EXPRESSION_LAYER_DELIMETER = ':';

        private const string ENTER_KEYWORD = "enter ";

        private string ProcessKeywords(string rawSpeaker)
        {
            if(rawSpeaker.StartsWith(ENTER_KEYWORD))
            {
                rawSpeaker = rawSpeaker.Substring(ENTER_KEYWORD.Length);
                makeCharacterEnter = true;
            }
            return rawSpeaker;
        }

        public DL_SpeakerData(string rawSpeaker)
        {
            rawData = rawSpeaker;
            rawSpeaker = ProcessKeywords(rawSpeaker);

            string pattern = @$"{NAME_CAST_ID}|{POS_CAST_ID}|{EXP_CAST_ID.Insert(EXP_CAST_ID.Length - 1, @"\")}";
            MatchCollection matches = Regex.Matches(rawSpeaker, pattern);

            //populate this data to avoid null references to value
            castName = "";
            castPosition = Vector2.zero;
            CastExpression = new List<(int layer, string expression)>();

            //no matches mean this entire line is the speaker name
            if (matches.Count == 0)
            {
                name = rawSpeaker;
                return;
            }

            int index = matches[0].Index;
            name = rawSpeaker.Substring(0, index);

            for (int i = 0; i < matches.Count; i++)
            {
                Match match = matches[i];
                int startIndex = 0, endIndex = 0;

                if (match.Value == NAME_CAST_ID)
                {
                    startIndex = match.Index + NAME_CAST_ID.Length;
                    endIndex = i < matches.Count - 1 ? matches[i + 1].Index : rawSpeaker.Length;
                    castName = rawSpeaker.Substring(startIndex, endIndex - startIndex);
                }
                else if (match.Value == POS_CAST_ID)
                {
                    isCastingPosition = true;
                    startIndex = match.Index + POS_CAST_ID.Length;
                    endIndex = i < matches.Count - 1 ? matches[i + 1].Index : rawSpeaker.Length;
                    string castPos = rawSpeaker.Substring(startIndex, endIndex - startIndex);

                    string[] axis = castPos.Split(AXISDELIMETER_ID, System.StringSplitOptions.RemoveEmptyEntries);

                    float.TryParse(axis[0], NumberStyles.Float, CultureInfo.InvariantCulture, out castPosition.x);
                    if (axis.Length > 1)
                        float.TryParse(axis[1], NumberStyles.Float, CultureInfo.InvariantCulture, out castPosition.y);
                }
                else if (match.Value == EXP_CAST_ID)
                {
                    startIndex = match.Index + EXP_CAST_ID.Length;
                    endIndex = i < matches.Count - 1 ? matches[i + 1].Index : rawSpeaker.Length;
                    string castExp = rawSpeaker.Substring(startIndex, endIndex - startIndex - 1);

                    CastExpression = castExp.Split(EXPRESSION_LAYER_JOINER)
                    .Select(x =>
                    {
                        var parts = x.Trim().Split(EXPRESSION_LAYER_DELIMETER);

                        if (parts.Length == 2)
                            return (int.Parse(parts[0]), parts[1]);
                        else
                            return (0, parts[0]);
                    }).ToList();
                }
            }
        }

    }
}