using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIstorage : MonoBehaviour
{
    public ScrollRect scrollRect;
    public int totalPages = 3; // total number of pages
    private float[] pagePositions;

    void Start()
    {
        pagePositions = new float[totalPages];
        float step = 1f / (totalPages - 1);

        for (int i = 0; i < totalPages; i++)
        {
            pagePositions[i] = i * step;
        }
    }

    public void ScrollToPage(int pageIndex)
    {
        if (pageIndex >= 0 && pageIndex < totalPages)
        {
            scrollRect.horizontalNormalizedPosition = pagePositions[pageIndex];
        }
    }
}
