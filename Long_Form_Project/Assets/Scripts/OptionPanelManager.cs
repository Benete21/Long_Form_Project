using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class OptionPanelManager : MonoBehaviour
{
    [Header("Images")]
    public GameObject keyboardImage;
    public GameObject controllerImage;

    [Header("Buttons")]
    public Button keyboardTabButton;
    public Button controllerTabButton;
    public Button backButton;

    [Header("Scroll & Navigation")]
    public ScrollRect scrollRect;
    public float scrollSpeed = 0.3f;
    public GameObject firstSelected;

    private void OnEnable()
    {
        // Default tab
        ShowKeyboardTab();

        // Ensure controller starts on first selectable UI element
        if (firstSelected != null)
            EventSystem.current.SetSelectedGameObject(firstSelected);

        // Reset scroll
        if (scrollRect != null)
            scrollRect.verticalNormalizedPosition = 1f;
    }

    private void Update()
    {
        HandleScrolling();
        HandleBackButton();
    }

    private void HandleScrolling()
    {
        if (scrollRect == null) return;

        // Get input from new Input System or old axis
        float scrollInput = 0f;
        if (Gamepad.current != null)
            scrollInput = -Gamepad.current.leftStick.ReadValue().y; // inverted for natural direction
        else
            scrollInput = -Input.GetAxis("Vertical");

        if (Mathf.Abs(scrollInput) > 0.1f)
        {
            scrollRect.verticalNormalizedPosition =
                Mathf.Clamp01(scrollRect.verticalNormalizedPosition + scrollInput * scrollSpeed * Time.unscaledDeltaTime);
        }
    }

    private void HandleBackButton()
    {
        if (Gamepad.current != null && Gamepad.current.buttonEast.wasPressedThisFrame) // Circle / B
        {
            backButton.onClick.Invoke();
        }
        else if (Keyboard.current != null && Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            backButton.onClick.Invoke();
        }
    }

    public void ShowKeyboardTab()
    {
        keyboardImage.SetActive(true);
        controllerImage.SetActive(false);
        HighlightTab(keyboardTabButton);
        ResetScroll();
    }

    public void ShowControllerTab()
    {
        keyboardImage.SetActive(false);
        controllerImage.SetActive(true);
        HighlightTab(controllerTabButton);
        ResetScroll();
    }

    private void HighlightTab(Button activeButton)
    {
        // Optional visual feedback
        ColorBlock activeColors = activeButton.colors;
        activeColors.normalColor = new Color(1f, 0.8f, 0.2f); // highlight yellow
        activeButton.colors = activeColors;
    }

    private void ResetScroll()
    {
        if (scrollRect != null)
            scrollRect.verticalNormalizedPosition = 1f;
    }
}
