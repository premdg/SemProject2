using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class PanelAndTextAnimation : MonoBehaviour
{
    public RectTransform panel; // Panel to animate
    public Text messageText;    // UI Text to animate (TextMeshPro can also be used)
    
    void Start()
    {
        // Start with the panel off-screen
        panel.anchoredPosition = new Vector2(0, 1000);

        // Animate the panel falling
        AnimatePanel(() =>
        {
            // Once the panel animation is complete, animate the text
            AnimateText();
        });
    }

    void AnimatePanel(TweenCallback onComplete)
    {
        panel.DOAnchorPos(Vector2.zero, 0.8f).SetEase(Ease.OutBounce).OnComplete(onComplete);
    }

    void AnimateText()
    {
        // Start with text invisible
        messageText.color = new Color(messageText.color.r, messageText.color.g, messageText.color.b, 0);

        // Fade in the text
        messageText.DOFade(1f, 0.5f).SetEase(Ease.InOutQuad);

        // Add scaling effect for emphasis
        messageText.transform.localScale = Vector3.zero;
        messageText.transform.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutElastic);
    }
}
