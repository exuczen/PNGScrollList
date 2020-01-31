using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using MustHave.Utilities;

[RequireComponent(typeof(ScrollRect))]
public abstract class ScrollList<T> : UIBehaviour where T : ListItem
{
    [SerializeField] protected RectTransform _listItemSlotPrefab = default;
    [SerializeField] protected T _listItemPrefab = default;

    protected HashSet<Transform> _itemsInView = new HashSet<Transform>();
    protected ScrollRect _scrollRect = default;
    protected RectTransform _viewport = default;
    protected RectTransform _content = default;
    protected float _itemHeight = default;
    protected int _viewportItemsCount = default;
    protected MonoBehaviour _parentContext = default;

    protected abstract T CreateListItemAtIndex(int index, Transform slot);

    public abstract void RefreshList(Action<bool> onResult);

    protected override void Awake()
    {
        _scrollRect = GetComponent<ScrollRect>();
        _viewport = _scrollRect.viewport;
        _content = _scrollRect.content;
        _content.DestroyAllChildren();
        _scrollRect.onValueChanged.AddListener(ShowItemsInViewport);
        _itemHeight = _listItemSlotPrefab.rect.height;
    }

    protected override void OnRectTransformDimensionsChange()
    {
        SetViewportItemsCount();
    }

    protected void SetViewportItemsCount()
    {
        if (_viewport && _viewport.rect.height > 0f)
        {
            _viewportItemsCount = (int)((_viewport.rect.height + _itemHeight - 1) / _itemHeight);
        }
    }

    protected void ShowItemsInViewport(Vector2 position)
    {
        foreach (var item in _itemsInView)
        {
            item.gameObject.SetActive(false);
        }
        _itemsInView.Clear();

        int begIndex = (int)((1f - position.y) * (_content.rect.height - _viewport.rect.height) / _itemHeight);
        begIndex = Mathf.Clamp(begIndex - 1, 0, _content.childCount);
        int endIndex = Mathf.Min(begIndex + 2 + _viewportItemsCount, _content.childCount);
        for (int i = begIndex; i < endIndex; i++)
        {
            Transform slot = _content.GetChild(i);
            Transform item = null;
            if (slot.childCount == 0)
            {
                item = CreateListItemAtIndex(i, slot).transform;
            }
            else
            {
                item = slot.GetChild(0);
                item.gameObject.SetActive(true);
            }
            _itemsInView.Add(item);
        }
    }
}
