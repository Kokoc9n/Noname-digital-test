using Core.UI;
using Newtonsoft.Json;
using Plugins.Dropbox;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Core
{
    public class ModsDisplayManager : MonoBehaviour
    {
        public static ModsDisplayManager Instance;
        [SerializeField] GameObject cardPrefab;
        [SerializeField] TMP_InputField searchField;
        private Root root;
        private string lastCategory;
        private Dictionary<Mod, int> valuePairs = new();

        private void Awake()
        {
            if (Instance == null) Instance = this;
        }
        private void OnEnable()
        {
            searchField.onValueChanged.AddListener((s) => Search(s));
        }
        private void OnDisable()
        {
            searchField.onValueChanged.RemoveAllListeners();
        }
        public void Search(string input)
        {
            valuePairs.Clear();
            foreach (var m in root.mods)
            {
                valuePairs[m] = LevenshteinDistance(root.mods.Find(_ => _.title == m.title).title, searchField.text);
            }
            valuePairs = valuePairs.OrderBy(_ => _.Value).ToDictionary(_ => _.Key, _ => _.Value);

            foreach (var mod in valuePairs)
            {
                Debug.Log(mod.Key.title);
            }
            PopulateCards(lastCategory);
        }

        private int LevenshteinDistance(string source, string target)
        {
            if (string.IsNullOrEmpty(source))
            {
                if (string.IsNullOrEmpty(target)) return 0;
                return target.Length;
            }
            if (string.IsNullOrEmpty(target)) return source.Length;

            if (source.Length > target.Length)
            {
                var temp = target;
                target = source;
                source = temp;
            }

            var m = target.Length;
            var n = source.Length;
            var distance = new int[2, m + 1];
            // Initialize the distance 'matrix'
            for (var j = 1; j <= m; j++) distance[0, j] = j;

            var currentRow = 0;
            for (var i = 1; i <= n; ++i)
            {
                currentRow = i & 1;
                distance[currentRow, 0] = i;
                var previousRow = currentRow ^ 1;
                for (var j = 1; j <= m; j++)
                {
                    var cost = target[j - 1] == source[i - 1] ? 0 : 1;
                    distance[currentRow, j] = Mathf.Min(Mathf.Min(
                        distance[previousRow, j] + 1,
                        distance[currentRow, j - 1] + 1),
                        distance[previousRow, j - 1] + cost);
                }
            }
            return distance[currentRow, m];
        }
        public void PopulateCards(string category = "")
        {
            lastCategory = category;
            // Smart way to do it - create a pool of gameobjects but I have no time left ¯\_UwU_/¯
            // And the lag when deleting and instantiating 10*4 objects is so noticeable :(
            foreach (Transform child in transform)
            {
                Destroy(child.gameObject);
            }
            foreach (var card in valuePairs.Keys)
            {
                if (card.category != category && category != "") continue;
                var imagePath = Application.persistentDataPath + "/" + card.preview_path;
                var instance = Instantiate(cardPrefab, transform);
                instance.transform.GetChild(0).GetComponent<TMP_Text>().text = card.title;
                instance.transform.GetChild(1).GetComponent<TMP_Text>().text = card.description;
                //instance.transform.GetChild(2).GetChild(0).GetComponent<Image>().sprite = LoadSprite(imagePath);
                instance.transform.GetChild(3).GetComponent<DownloadFileButton>().SetFilePath(card.file_path);

                // Load sprite asynchronously
                LoadSpriteAsync(instance.transform.GetChild(2).GetChild(0).GetComponent<Image>(), card.preview_path);
            }
        }
        private async void LoadSpriteAsync(Image image, string previewPath)
        {
            var imagePath = Application.persistentDataPath + "/" + previewPath;
            var sprite = await LoadSpriteAsync(imagePath);
            if (sprite != null)
                image.sprite = sprite;
            else
            {
                // Handle sprite loading failure
                Debug.LogError("Failed to download preview image: " + imagePath);
            }
        }
        private async Task<Sprite> LoadSpriteAsync(string imagePath)
        {
            return await LoadSprite(imagePath);
        }
        public async void OnModsTabEnter()
        {
            var path = Path.Combine(Application.persistentDataPath, "mods.json");
            root = JsonConvert.DeserializeObject<Root>(File.ReadAllText(path));
            valuePairs = root.mods.ToDictionary(_ => _, _ => -1);
            PopulateCards();
            var downloadTask = DownloadPreviews();
            while (!downloadTask.IsCompleted)
            {
                await Task.Yield();
            }

            if (!downloadTask.IsCompletedSuccessfully)
                // Bad connection warning.
                CanvasManager.StackPage(typeof(ConnectionLostPage));
        }
        private async Task DownloadPreviews()
        {
            foreach (var mod in root.mods)
            {
                await DropboxHelper.DownloadAndSaveFile(mod.preview_path.TrimStart('/'));
            }
        }
        private async Task<Sprite> LoadSprite(string path)
        {
            if (string.IsNullOrEmpty(path)) return null;
            while (File.Exists(path) == false) await Task.Yield();
            if (File.Exists(path))
            {
                byte[] bytes = File.ReadAllBytes(path);
                var texture = new Texture2D(1024, 512, TextureFormat.RGB24, false);
                texture.filterMode = FilterMode.Bilinear;
                texture.LoadImage(bytes);
                var sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.0f));

                return sprite;
            }
            return null;
        }
    }
}