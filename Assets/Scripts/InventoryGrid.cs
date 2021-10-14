using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class InventoryGrid : MonoBehaviour
{
    private GridLayoutGroup grid;
    private RectTransform rectTransform;

    [Range(1, 10)]
    public int childrenPerRow = 10;
    [Range(1, 10)]
    public int childrenPerColumn = 1;

    void OnEnable()
    {
        ResizeToFit();
    }

    public void ResizeToFit()
    {
        grid = gameObject.GetComponent<GridLayoutGroup>();
        rectTransform = gameObject.GetComponent<RectTransform>();

        int childCount = transform.childCount;

        float gridWidth = rectTransform.rect.width;
        float gridHeight = rectTransform.rect.height;

        float xSpacing = grid.spacing.x;
        float ySpacing = grid.spacing.y;

        float newCellWidth = (gridWidth - (childrenPerRow - 1) * xSpacing) / childrenPerRow;
        float newCellHeight = (gridHeight - (childrenPerColumn - 1) * ySpacing) / childrenPerColumn;

        float newCellSize = Mathf.Max(newCellWidth, newCellHeight);
        //float newCellSize = newCellWidth;

        grid.cellSize = new Vector2(newCellSize, newCellSize);
        //grid.cellSize = new Vector2(newCellSize, 50);
        grid.constraintCount = childrenPerRow;

        //rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, (newCellSize * childrenPerRow) + xSpacing * (childrenPerRow - 1));
    }
}
