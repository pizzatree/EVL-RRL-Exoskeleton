// Uses the StandaloneFileBrowser plugin to open environment's file browser
// Stores location of last opened directory to PlayerPrefs

// If file IO is not working, try changing environmentSlash to one appropriate to your environment
// '\\' should work on Windows. The plugin can work on WebGL with additional considerations.

using System;
using SFB;
using UnityEngine;

namespace UI
{
    public class FileOpener : MonoBehaviour
    {
        public event Action<string> OnNewFileOpened;

        [SerializeField] private TMPro.TMP_Text filenameText;
        [SerializeField] private char environmentSlash = '\\';
        
        private string lastDirectory;

        private void Start()
        {
            lastDirectory = (PlayerPrefs.HasKey("LastDirectoryOpened"))
                ? PlayerPrefs.GetString("LastDirectoryOpened")
                : "";
        }
        
        public string[] OpenVSKPanel()
            => StandaloneFileBrowser.OpenFilePanel("Open Connections", 
                                                        lastDirectory, 
                                                        "vsk", 
                                                        false);
        public void OpenCSVPanel()
            => StandaloneFileBrowser.OpenFilePanelAsync("Open Movement", 
                                                        lastDirectory, 
                                                        "csv", 
                                                        false, 
                                                        OpenCSV);

        private void OpenCSV(string[] paths)
        {
            if(paths.Length.Equals(0))
            {
                filenameText.text = $"<color=\"red\">IO Error</color>";
                return;
            }
            
            var url = paths[0];
            OnNewFileOpened?.Invoke(url);

            var lastSlashLoc = url.LastIndexOf(environmentSlash);
            filenameText.text = url.Substring(lastSlashLoc + 1);

            lastDirectory = url.Substring(0, lastSlashLoc);
            PlayerPrefs.SetString("LastDirectoryOpened", lastDirectory);   
        }
    }
}