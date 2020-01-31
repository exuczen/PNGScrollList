using MustHave.Utilities;
using UnityEngine;
using UnityEngine.UI;

public class MainCanvas : MonoBehaviour
{
    [SerializeField] private PNGScrollList _pngScrollList = default;
    [SerializeField] private InfoPanel _infoPanel = default;
    [SerializeField] private Button _infoButton = default;
    [SerializeField] private Button _refreshButton = default;

    private void Awake()
    {
        _infoButton.onClick.AddListener(() => {
            _infoPanel.SetActive(!_infoPanel.IsActive);
        });
        _refreshButton.onClick.AddListener(() => {
            _pngScrollList.RefreshList(OnRefreshScrollListResult);
        });
    }

    private void Start()
    {
        this.StartCoroutineActionAfterFrames(() => {
            _pngScrollList.RefreshList(OnRefreshScrollListResult);
        }, 1);

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }
    }

    private void OnRefreshScrollListResult(bool result)
    {
        _infoPanel.SetActive(!result);
    }
}
