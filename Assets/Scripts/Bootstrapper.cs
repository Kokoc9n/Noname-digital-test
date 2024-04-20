using UnityEngine;
using Plugins.Dropbox;
using Core.UI;
using System.Threading.Tasks;
using UnityEngine.Networking;
using System.Threading;
using System;

namespace Core
{
    public static class Bootstrapper
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        public static async void Execute()
        {
            Application.targetFrameRate = 60;
            await DropboxHelper.Initialize();
            
            IProgress<float> progressBar = new Progress<float>(percent =>
            {
                ProgressBar.Instance.Image.fillAmount = percent;
            });
            await DropboxHelper.DownloadFilesInFolder("", progressBar, () => ProgressBar.Instance.gameObject.SetActive(false));
            LoadingCurtain.Instance.Hide();
            await Task.Delay(1000);
            CanvasManager.StackPage(typeof(BottomTabBar));
            var connectionCheck = ConnectionCheck();

            Application.quitting += tokenSource.Cancel;
            tokenSource = new();
        }
        private static CancellationTokenSource tokenSource = new();
        private static async Task ConnectionCheck()
        {
            UnityWebRequest ping;
            while(tokenSource.IsCancellationRequested == false)
            {
                ping = new("https://api.dropbox.com");
                ping.timeout = 5;
                ping.SendWebRequest();

                while (!ping.isDone)
                {
                    await Task.Yield();
                }
                if (ping.result != UnityWebRequest.Result.Success)
                {
                    CanvasManager.StackPage(typeof(ConnectionLostPage));
                    break;
                }
                await Task.Delay(5000);
            }
        }
    }
}