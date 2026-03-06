
using UnityEngine;
using System.Collections.Generic;

public class EventCharacterManager : Singleton<EventCharacterManager>
{
    private List<EventCharacterData> activeEventCharacters = new List<EventCharacterData>();

    public void StartEvent(List<EventCharacterData> eventCharacters)
    {
        activeEventCharacters = eventCharacters;
        Debug.Log("Event started with " + eventCharacters.Count + " characters.");
        // In a real game, you would also set the event end time.
    }

    public void EndEvent()
    {
        activeEventCharacters.Clear();
        Debug.Log("Event ended.");

        // Convert any remaining event currency
        EventCurrencyManager.Instance.ConvertEventCurrencyToCoins();
    }

    public List<EventCharacterData> GetActiveEventCharacters()
    {
        return activeEventCharacters;
    }

    public EventCharacterData GetEventCharacter(string characterID)
    {
        return activeEventCharacters.Find(c => c.characterID == characterID);
    }

    public void ApplyCharacterAbility(string characterID)
    {
        EventCharacterData character = GetEventCharacter(characterID);
        if (character != null)
        {
            // Example of applying a passive ability.
            // This would need to be connected to the relevant game systems.
            Debug.Log($"Applying ability for {character.characterName}: {character.specialAbilityDescription}");
        }
    }
}
