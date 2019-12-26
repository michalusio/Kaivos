using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class SellTextScript : MonoBehaviour
{
    private Text textComponent;
    private RectTransform rectComponent;
    private float alpha;

    void Start()
    {
        textComponent = GetComponent<Text>();
        rectComponent = GetComponent<RectTransform>();
        alpha = 1;
        textComponent.color = new Color(0.2f, 1, 0.2f, alpha);
    }

    // Update is called once per frame
    void Update()
    {
        alpha -= Time.deltaTime;
        if (alpha <= 0)
        {
            Destroy(gameObject);
        }
        rectComponent.anchoredPosition = rectComponent.anchoredPosition + new Vector2(0, -1);
        textComponent.color = new Color(0.2f, 1, 0.2f, alpha);
    }
}
