using UnityEngine;
using UnityEngine.UI;

public class CharacterSelector : MonoBehaviour
{
    public CommunicatorSO communicatorSO; // Reference to the CommunicatorSO
    public int characterIndex; // Index of the character to select
    public Button selectButton; // Reference to the UI button

    private void Start()
    {
        // Add listener to the button to call SelectCharacter when clicked
        selectButton.onClick.AddListener(SelectCharacter);
    }

    // Method to update the character index and spawn the character
    private void SelectCharacter()
    {
        // Update the character index in the CommunicatorSO
        communicatorSO.characterIndex = characterIndex;

        // Log the selected character index for debugging
        Debug.Log("Selected Character Index: " + communicatorSO.characterIndex);

        // Optionally, you can call a method to spawn the character here
        SpawnCharacter();
    }

    // Method to spawn the character based on the updated index
    private void SpawnCharacter()
    {
        // Assuming you have a reference to the character selection script
        characterSelection characterSelectionScript = FindObjectOfType<characterSelection>();
        if (characterSelectionScript != null)
        {
            characterSelectionScript.Awake(); // Call the Awake method to spawn the character
        }
    }
}