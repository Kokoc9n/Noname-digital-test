using Core;
using Core.UI;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
public class SetCategoryButton : ButtonClickHandler
{
    [SerializeField] string category;
    [SerializeField] Color highlighColor;
    [SerializeField] Color defaultColor;
    private Image image;
    private bool toggle;
    public void UnToggle()
    {
        if (toggle)
        {
            toggle = !toggle;
            image.color = defaultColor;
        }
    }
    private void Awake()
    {
        image = GetComponent<Image>();
    }
    public override void OnButtonClicked()
    {
        toggle = !toggle;
        if (toggle)
        {
            image.color = highlighColor;
            ModsDisplayManager.Instance.PopulateCards(category);
            // Cringe alert.
            FindObjectsOfType<SetCategoryButton>().Where(_ => _.transform != transform).ToList()
                .ForEach(_ => _.UnToggle());
        }
        else
        {
            image.color = defaultColor;
            ModsDisplayManager.Instance.PopulateCards("");
        }
    }
}
