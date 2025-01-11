using NUnit.Framework;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TilesSorting
{
    private List<SpriteRenderer> mSortIndices = new List<SpriteRenderer>();

    public TilesSorting() {  }

    public void CLear()
    {
        mSortIndices.Clear();
    }

    public void Add(SpriteRenderer renderer)
    {
        mSortIndices.Add(renderer);
        SetRenderOrder(renderer, mSortIndices.Count);
    }

    public void Remove(SpriteRenderer renderer)
    {
        if (renderer == null || renderer.gameObject == null)
        {
            return;
        }

        mSortIndices.Remove(renderer);
        for (int i = 0; i < mSortIndices.Count; i++)
            SetRenderOrder(mSortIndices[i], i+1);
    }

    public void BringToTop(SpriteRenderer renderer)
    {
        if (renderer == null || renderer.gameObject == null)
        {
            return;
        }

        Remove(renderer);
        Add(renderer);
    }

    public void SendToBottom(SpriteRenderer renderer)
    {
        if (renderer == null || renderer.gameObject == null)
        {
            return;
        }

        Remove(renderer);
        mSortIndices.Insert(0, renderer);
        SetRenderOrder(renderer, 1);
        for (int i = 1; i < mSortIndices.Count; i++)
            SetRenderOrder(mSortIndices[i], i + 1);
    }

    private void SetRenderOrder(SpriteRenderer renderer, int index)
    {
        if (renderer == null || renderer.gameObject == null)
        {
            return;
        }

        renderer.sortingOrder = index;
        Vector3 p =renderer.transform.position;
        p.z = -index / 10.0f;
        renderer.transform.position = p;
    }
}
