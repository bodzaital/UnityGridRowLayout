using UnityEngine;

[ExecuteAlways]
public class CSSGridRows : MonoBehaviour
{
    [Tooltip("Controls the size of the grid in fractional units.")]
    [Min(1f)]
    public float[] FractionalSizes;

    [Header("Padding")]
    public float PaddingTop;
    public float PaddingRight;
    public float PaddingBottom;
    public float PaddingLeft;

    [Header("Gap")]
    [Min(0f)]
    public float RowGap;

    /// <summary>
    /// Caching the child row <see cref="RectTransform"/>s.
    /// </summary>
    RectTransform[] rows;
    
    /// <summary>
    /// Caching the inventory panel <see cref="RectTransform"/>.
    /// </summary>
    RectTransform _rect;

    void Start()
    {
        UpdateUI(true);
    }

    void Update()
    {
        UpdateUI(!Application.isPlaying);
    }

    /// <summary>
    /// Updates the grid rows.
    /// </summary>
    /// <param name="cache">If true, creates the <see cref="RectTransform"/> caches.</param>
    void UpdateUI(bool cache = false)
    {
        if (cache)
        {
            rows = GetComponentsInChildren<RectTransform>();
            _rect = GetComponent<RectTransform>();
        }

        // Calculate the available size after subtracting the padding.
        float availHeight = _rect.sizeDelta.y - PaddingTop - PaddingBottom - (FractionalSizes.Length - 1) * RowGap;
        float availWidth = _rect.sizeDelta.x - PaddingLeft - PaddingRight;
        
        // Get the total fraction sum.
        float totalFrac = SumFractions();

        // Set up the previous row properties.
        float previousRowHeight = 0f;
        float previousRowPosY = -PaddingTop;
        float previousRowGap = 0f;

        // Loop through the rows.
        for (int i = 0; i < FractionalSizes.Length; i++)
        {
            float row = FractionalSizes[i];
            
            // Set the fractional row height.
            float rowHeight = availHeight * row / totalFrac;

            // Set the row position based on the previous row.
            float rowPosY = previousRowPosY - previousRowHeight - previousRowGap;

            // Save the previous row properties.
            previousRowHeight = rowHeight;
            previousRowPosY = rowPosY;
            previousRowGap = RowGap;

            // Make sure the anchors are correct.
            rows[i + 1].anchorMin = new Vector2(0, 1);
            rows[i + 1].anchorMax = new Vector2(0, 1);
            rows[i + 1].pivot = new Vector2(0, 1);

            // Set the position and size.
            rows[i + 1].anchoredPosition = new Vector2(PaddingLeft, rowPosY);
            rows[i + 1].sizeDelta = new Vector2(availWidth, rowHeight);
        }
    }

    /// <summary>
    /// Calculates the fractional sum of all rows.
    /// </summary>
    /// <returns></returns>
    float SumFractions()
    {
        float sum = 0;

        foreach (float row in FractionalSizes) sum += row;

        return sum;
    }
}
