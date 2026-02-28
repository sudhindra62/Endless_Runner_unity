using UnityEngine;
using System.Collections.Generic;

public class SaveManager : MonoBehaviour
{
    public PlayerController playerController;
    public InventoryManager inventoryManager;
    public QuestManager questManager;
    public ItemDatabase itemDatabase;
    public QuestDatabase questDatabase;

    public void SaveGame()
    {
        GameData data = new GameData();

        // Player Data
        data.playerPosition = new float[3];
        data.playerPosition[0] = playerController.transform.position.x;
        data.playerPosition[1] = playerController.transform.position.y;
        data.playerPosition[2] = playerController.transform.position.z;
        data.playerHealth = playerController.health;

        // Inventory Data
        data.inventory = new List<ItemData>();
        foreach (Item item in inventoryManager.items)
        {
            data.inventory.Add(new ItemData(item));
        }

        // Quest Data
        data.quests = new List<QuestData>();
        foreach (Quest quest in questManager.activeQuests)
        {
            data.quests.Add(new QuestData(quest));
        }

        string json = JsonUtility.ToJson(data);
        string checksum = Checksum.Calculate(json);
        SaveSystem.SaveGame(json, checksum);
    }

    public void LoadGame()
    {
        string json;
        string checksum;
        SaveSystem.LoadGame(out json, out checksum);

        if (string.IsNullOrEmpty(json) || string.IsNullOrEmpty(checksum))
        {
            return;
        }

        if (Checksum.Calculate(json) != checksum)
        {
            Debug.LogError("Save file has been tampered with!");
            return;
        }

        GameData data = JsonUtility.FromJson<GameData>(json);

        if (data != null)
        {
            // Player Data
            Vector3 playerPosition = new Vector3(data.playerPosition[0], data.playerPosition[1], data.playerPosition[2]);
            playerController.transform.position = playerPosition;
            playerController.health = data.playerHealth;

            // Inventory Data
            inventoryManager.items.Clear();
            foreach (ItemData itemData in data.inventory)
            {
                Item item = itemDatabase.GetItem(itemData.itemName);
                if (item != null)
                {
                    item.stackSize = itemData.stackSize;
                    inventoryManager.items.Add(item);
                }
            }

            // Quest Data
            questManager.activeQuests.Clear();
            foreach (QuestData questData in data.quests)
            {
                Quest quest = questDatabase.GetQuest(questData.questTitle);
                if (quest != null)
                {
                    questManager.activeQuests.Add(quest);
                    for (int i = 0; i < questData.objectives.Count; i++)
                    {
                        QuestObjectiveData objectiveData = questData.objectives[i];
                        QuestObjective objective = quest.objectives[i];

                        objective.isComplete = objectiveData.isComplete;
                        if (objective is KillObjective killObjective && objectiveData is KillObjectiveData killObjectiveData)
                        {
                            killObjective.amountKilled = killObjectiveData.amountKilled;
                        }
                    }
                }
            }
        }
    }
}
