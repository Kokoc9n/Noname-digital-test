using Plugins.Dropbox;
using System;
using UnityEngine;

namespace Core.UI
{
    public class DownloadFileButton : ButtonClickHandler
    {
        private string filePath;
        public void SetFilePath(string path) => filePath = path;

        public async override void OnButtonClicked()
        {
            ProgressBar.Instance.Image.fillAmount = 0;
            ProgressBar.Instance.gameObject.SetActive(true);
            IProgress<float> progressBar = new Progress<float>(percent =>
            {
                ProgressBar.Instance.Image.fillAmount = percent;
            });
            await DropboxHelper.DownloadAndSaveFile(filePath.TrimStart('/'), progressBar, () => ProgressBar.Instance.gameObject.SetActive(false));

            new NativeShare().AddFile(Application.persistentDataPath + filePath)
            .SetCallback((result, shareTarget) => Debug.Log("Share result: " + result + ", selected app: " + shareTarget))
            .Share();
        }
    }
}