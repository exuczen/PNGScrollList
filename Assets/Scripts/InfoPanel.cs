using UnityEngine;
using UnityEngine.UI;

public class InfoPanel : MonoBehaviour
{
    [SerializeField] private Button _dismissButton = default;
    [SerializeField] private Text _text = default;

    private const string TEXT = "Place your png files in\n\"{0}\"\nfolder.\n\nTap to continue";

    private void Awake()
    {
        _text.text = string.Format(TEXT, AppData.PicturesFolderPath);
        _dismissButton.onClick.AddListener(() => {
            gameObject.SetActive(false);
        });
    }

    public void SetActive(bool active)
    {
        gameObject.SetActive(active);
    }

    public bool IsActive => gameObject.activeSelf;
}
