using MustHave.Utilities;
using System;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class PNGListItem : ListItem
{
    [SerializeField] private Text _fileNameText = default;
    [SerializeField] private Text _fileTimeText = default;

    private string GetFormattedTimeSinceFileCreation(FileInfo fileInfo)
    {
        //var debugDate = new DateTime(1970, 5, 1, 8, 30, 52);
        //TimeSpan interval = DateTime.Now.Subtract(fileInfo.CreationTime);
        //TimeSpan interval = DateTime.Now - debugDate;
        TimeSpan interval = DateTime.Now - fileInfo.CreationTime;
        string intervalFormat = "hh\\:mm\\:ss\\.fff";
        if (interval.Days > 0)
        {
            intervalFormat = "d\\." + intervalFormat;
        }
        return interval.ToString(intervalFormat);
    }

    private void SetFileDescription(FileInfo fileInfo)
    {
        _fileNameText.text = fileInfo.Name;
        _fileTimeText.text = GetFormattedTimeSinceFileCreation(fileInfo);
    }

    public PNGListItem CreateInstance(FileInfo fileInfo, Transform slot, MonoBehaviour context)
    {
        PNGListItem item = Instantiate(this, slot);
        (item.transform as RectTransform).FillParent();
        item.SetFileDescription(fileInfo);
        item.LoadImage("file://" + fileInfo.FullName, context);
        return item;
    }
}
