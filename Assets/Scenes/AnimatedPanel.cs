using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;


public class AnimatePanel : MonoBehaviour
{
    public RectTransform panel; // Assign your panel's RectTransform
    public Image panelImage;    // Assign the Image component of the panel

    void Start()
    {
        AnimatePanelBackground();
    }

    // Animate the panel to slide and fade in
    public void AnimatePanelBackground()
    {
        // Initial setup for panel: Place it above the screen and make it invisible
        Vector2 offScreenPosition = new Vector2(panel.anchoredPosition.x, Screen.height);
        panel.anchoredPosition = offScreenPosition;
        panelImage.color = new Color(panelImage.color.r, panelImage.color.g, panelImage.color.b, 0);

        // Animate panel to slide in and fade in
        panel.DOAnchorPos(Vector2.zero, 1.0f).SetEase(Ease.OutBounce); // Slide to center
        panelImage.DOFade(1.0f, 1.0f);                                // Fade to full opacity
    }

    // Optional: Animate the panel to slide and fade out
    public void HidePanel()
    {
        Vector2 offScreenPosition = new Vector2(panel.anchoredPosition.x, Screen.height);
        panel.DOAnchorPos(offScreenPosition, 1.0f).SetEase(Ease.InBack); // Slide out of screen
        panelImage.DOFade(0.0f, 1.0f);                                  // Fade to invisible
    }
}
