﻿using SeSkydriveSave;
using System.Windows.Forms;

namespace Nikse.SubtitleEdit.PluginLogic
{
    public class SeSkydriveSave : IPlugin // dll file name must "<classname>.dll" - e.g. "SyncViaOtherSubtitle.dll"
    {
        string IPlugin.Name
        {
            get { return "Save to Skydrive"; }
        }

        string IPlugin.Text
        {
            get { return "Save subtitle to Skydrive..."; } // text in interface
        }

        decimal IPlugin.Version
        {
            get { return 0.2M; }
        }

        string IPlugin.Description
        {
            get { return "Save subtitle from Skydrive"; }
        }

        string IPlugin.ActionType // Can be one of these: file, tool, sync, translate, spellcheck
        {
            get { return "file"; }
        }

        string IPlugin.Shortcut
        {
            get { return string.Empty; }
        }

        string IPlugin.DoAction(Form parentForm, string subtitle, double frameRate, string listViewLineSeparatorString, string subtitleFileName, string videoFileName, string rawText)
        {
            subtitleFileName = subtitleFileName.Trim();
            if (!string.IsNullOrEmpty(subtitleFileName))
            {
                var form = new PluginForm((this as IPlugin).Name, (this as IPlugin).Description, subtitleFileName, rawText);
                if (form.ShowDialog(parentForm) == DialogResult.OK)
                {
                }
            }
            else
            {
                MessageBox.Show("No subtitle loaded", parentForm.Text,
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            return string.Empty;
        }
    }
}