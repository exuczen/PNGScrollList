using UnityEngine;
using UnityEngine.UI;
using MustHave.Utilities;

public abstract class ListItem : MonoBehaviour
{
    [SerializeField] private Image _image = default;
    [SerializeField] private Image _background = default;
    [SerializeField] private Animator _progressSpinner = default;

    protected void LoadImage(string url, MonoBehaviour context)
    {
        ImageDownloader.DownloadInto(context, url, (uri, texture, sprite) => {
            _image.sprite = sprite;
            _image.gameObject.SetActive(true);
            _background.enabled = false;
            _progressSpinner.gameObject.SetActive(false);
        });
    }
}
