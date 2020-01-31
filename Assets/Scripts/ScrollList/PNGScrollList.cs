using MustHave.Utilities;
using System;
using System.IO;
using UnityEngine;

public class PNGScrollList : ScrollList<PNGListItem>
{
#if UNITY_EDITOR
    [SerializeField, Tooltip("Copy png files from StreamingAssets on start in Editor.")]
    private bool _copyFilesFromStreamingAssetsOnStartInEditor = default;
#endif
    private FileInfo[] _files = default;

    protected override void Start()
    {
#if UNITY_EDITOR
        if (_copyFilesFromStreamingAssetsOnStartInEditor)
        {
            FileManager.CopyDirectory(AppData.StreamingAssetsPicturesFolderPath, AppData.PicturesFolderPath, FileManager.PNG_FILE_PATTERN, false);
        }
#endif
        _parentContext = GetComponentInParent<MainCanvas>();
    }

    protected override PNGListItem CreateListItemAtIndex(int index, Transform slot)
    {
        return _listItemPrefab.CreateInstance(_files[index], slot, _parentContext);
    }

    public override void RefreshList(Action<bool> onResult)
    {
        _parentContext.StopAllCoroutines();
        if (_viewportItemsCount == 0)
        {
            SetViewportItemsCount();
        }
        _content.DestroyAllChildren();

        string folderPath = AppData.PicturesFolderPath;
        if (Directory.Exists(folderPath))
        {
            _files = FileManager.GetFilesFromFolder(folderPath, FileManager.PNG_FILE_PATTERN);
            foreach (FileInfo file in _files)
            {
                Instantiate(_listItemSlotPrefab, _content);
            }
            ShowItemsInViewport(_scrollRect.normalizedPosition);
            onResult?.Invoke(_files.Length > 0);
        }
        else
        {
            Directory.CreateDirectory(folderPath);
#if UNITY_EDITOR
            UnityEditor.AssetDatabase.Refresh();
#endif
            onResult?.Invoke(false);
        }
    }
}
