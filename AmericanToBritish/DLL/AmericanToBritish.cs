﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace Nikse.SubtitleEdit.PluginLogic
{
    public class AmericanToBritishConverter
    {
        // built-in list
        private readonly List<Regex> _regexListBuiltIn = new List<Regex>();
        private readonly List<string> _replaceListBuitIn = new List<string>();

        // local names
        private readonly List<Regex> _regexListLocal = new List<Regex>();
        private readonly List<string> _replaceListLocal = new List<string>();

        public AmericanToBritishConverter()
        {
            LoadBuiltInNamesWords();
        }

        public AmericanToBritishConverter(string localListPath) : this()
        {
            LoadLocalWords(localListPath);
        }

        public string FixText(string s, ListType listType)
        {
            if (string.IsNullOrWhiteSpace(s))
                return s;

            switch (listType)
            {
                case ListType.BuiltIn:
                    for (int index = 0; index < _regexListBuiltIn.Count; index++)
                    {
                        var regex = _regexListBuiltIn[index];
                        if (regex.IsMatch(s))
                        {
                            s = regex.Replace(s, _replaceListBuitIn[index]);
                        }
                    }
                    break;
                case ListType.Local:
                    // Todo: make sure local-list is loaded!
                    for (int index = 0; index < _regexListLocal.Count; index++)
                    {
                        var regex = _regexListLocal[index];
                        if (regex.IsMatch(s))
                        {
                            s = regex.Replace(s, _replaceListLocal[index]);
                        }
                    }
                    break;
            }
            return FixMissChangedWord(s);
        }

        public void LoadBuiltInNamesWords()
        {
            if (_regexListBuiltIn.Count > 0 && _replaceListBuitIn.Count > 0)
                return;
            _regexListBuiltIn.Clear();
            _replaceListBuitIn.Clear();
            try
            {
                //Cursor = Cursors.WaitCursor;
                using (var stream = System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream("Nikse.SubtitleEdit.PluginLogic.WordList.xml"))
                {
                    if (stream != null)
                    {
                        using (var reader = new StreamReader(stream))
                        {
                            var xdoc = XDocument.Parse(reader.ReadToEnd());
                            foreach (XElement xElement in xdoc.Descendants("Word"))
                            {
                                string american = xElement.Attribute("us").Value;
                                string british = xElement.Attribute("br").Value;
                                if (!(string.IsNullOrWhiteSpace(american) || string.IsNullOrWhiteSpace(british)) && american != british)
                                {
                                    _regexListBuiltIn.Add(new Regex("\\b" + american + "\\b", RegexOptions.Compiled));
                                    _replaceListBuitIn.Add(british);

                                    _regexListBuiltIn.Add(new Regex("\\b" + american.ToUpperInvariant() + "\\b", RegexOptions.Compiled));
                                    _replaceListBuitIn.Add(british.ToUpperInvariant());

                                    if (american.Length > 1)
                                    {
                                        _regexListBuiltIn.Add(new Regex("\\b" + char.ToUpperInvariant(american[0]) + american.Substring(1) + "\\b", RegexOptions.Compiled));
                                        if (british.Length > 1)
                                            _replaceListBuitIn.Add(char.ToUpperInvariant(british[0]) + british.Substring(1));
                                        else
                                            _replaceListBuitIn.Add(british.ToUpper());
                                    }
                                }
                            }
                        }
                    }
                }
                /*
                AmericanToBritishConvert();
                if (listViewFixes.Items.Count > 0)
                {
                    listViewFixes.Items[0].Selected = true;
                    listViewFixes.Items[0].Focused = true;
                }
                listViewFixes.Select();
                listViewFixes.Focus();
                */
            }
            catch (Exception exception)
            {
                /*MessageBox.Show(exception.Message);*/
            }
            finally
            {
                /*Cursor = Cursors.Default;*/
            }
        }

        public void LoadLocalWords(string path)
        {
            if (!Path.IsPathRooted(path))
                return;

        }

        private string FixMissChangedWord(string s)
        {
            var idx = s.IndexOf("<font", StringComparison.OrdinalIgnoreCase);
            while (idx >= 0) // Fix colour => color
            {
                var endIdx = s.IndexOf('>', idx + 5);
                if (endIdx < 5)
                    break;
                var tag = s.Substring(idx, endIdx - idx);
                tag = tag.Replace("colour", "color");
                tag = tag.Replace("COLOUR", "COLOR");
                tag = tag.Replace("Colour", "Color");
                s = s.Remove(idx, endIdx - idx).Insert(idx, tag);
                idx = s.IndexOf("<font", endIdx + 1, StringComparison.OrdinalIgnoreCase);
            }
            return s;
        }
    }
}
