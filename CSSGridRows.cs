using UnityEngine;

[ExecuteAlways]
public class CSSGridRows : MonoBehaviour
{
    [Tooltip("Controls the size of the grid in fractional units.")]
    [Min(1f)]
    public float[] FlexValues;

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
            int i = 0;
            rows = new RectTransform[transform.childCount];

            foreach (Transform child in transform)
            {
                rows[i++] = child.GetComponent<RectTransform>();
            }

            _rect = GetComponent<RectTransform>();
        }

        // Calculate the available size after subtracting the padding.
        float availHeight = _rect.sizeDelta.y - PaddingTop - PaddingBottom - (FlexValues.Length - 1) * RowGap;
        float availWidth = _rect.sizeDelta.x - PaddingLeft - PaddingRight;
        
        // Get the total fraction sum.
        float totalFrac = SumFractions();

        // Set up the previous row properties.
        float previousRowHeight = 0f;
        float previousRowPosY = -PaddingTop;
        float previousRowGap = 0f;

        // Loop through the rows.
        for (int i = 0; i < FlexValues.Length; i++)
        {
            float row = FlexValues[i];
            
            // Set the fractional row height.
            float rowHeight = availHeight * row / totalFrac;

            // Set the row position based on the previous row.
            float rowPosY = previousRowPosY - previousRowHeight - previousRowGap;

            // Save the previous row properties.
            previousRowHeight = rowHeight;
            previousRowPosY = rowPosY;
            previousRowGap = RowGap;

            // Make sure the anchors are correct.
            rows[i].anchorMin = new Vector2(0, 1);
            rows[i].anchorMax = new Vector2(0, 1);
            rows[i].pivot = new Vector2(0, 1);

            // Set the position and size.
            rows[i].anchoredPosition = new Vector2(PaddingLeft, rowPosY);
            rows[i].sizeDelta = new Vector2(availWidth, rowHeight);
        }
    }

    /// <summary>
    /// Calculates the fractional sum of all rows.
    /// </summary>
    /// <returns></returns>
    float SumFractions()
    {
        float sum = 0;

        foreach (float row in FlexValues) sum += row;

        return sum;
    }
}
