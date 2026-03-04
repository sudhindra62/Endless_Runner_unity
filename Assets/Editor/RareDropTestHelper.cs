
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEditor.SceneManagement;

public class RareDropTestHelper
{
    [MenuItem("Rare Drop System/Setup Test Scene")]
    public static void SetupTestScene()
    {
        // --- Create Core GameObjects ---
        GameObject gameManagerObj = CreateManagerObject("GameManager", typeof(GameManager), typeof(RunSessionData));
        
        GameObject managers = new GameObject("--- MANAGERS ---");
        
        GameObject rareDropEngineObj = CreateManagerObject("RareDropEngine", typeof(RareDropEngine));
        rareDropEngineObj.transform.SetParent(managers.transform);

        GameObject liveEventManagerObj = CreateManagerObject("LiveEventManager", typeof(LiveEventManager));
        liveEventManagerObj.transform.SetParent(managers.transform);

        GameObject leagueManagerObj = CreateManagerObject("LeagueManager", typeof(LeagueManager));
        leagueManagerObj.transform.SetParent(managers.transform);

        GameObject rewardManagerObj = CreateManagerObject("RewardManager", typeof(RewardManager));
        rewardManagerObj.transform.SetParent(managers.transform);
        
        GameObject pityCounterManagerObj = CreateManagerObject("PityCounterManager", typeof(PityCounterManager));
        pityCounterManagerObj.transform.SetParent(managers.transform);

        GameObject dropIntegrityValidatorObj = CreateManagerObject("DropIntegrityValidator", typeof(DropIntegrityValidator));
        dropIntegrityValidatorObj.transform.SetParent(managers.transform);
        
        GameObject shardInventoryManagerObj = CreateManagerObject("ShardInventoryManager", typeof(ShardInventoryManager));
        shardInventoryManagerObj.transform.SetParent(managers.transform);
        
        GameObject skinManagerObj = CreateManagerObject("SkinManager", typeof(SkinManager));
        skinManagerObj.transform.SetParent(managers.transform);

        GameObject cosmeticEffectManagerObj = CreateManagerObject("CosmeticEffectManager", typeof(CosmeticEffectManager));
        cosmeticEffectManagerObj.transform.SetParent(managers.transform);

        // --- Configure Dependencies ---
        RareDropEngine rareDropEngine = rareDropEngineObj.GetComponent<RareDropEngine>();
        
        // Create and assign DropTableRegistry
        DropTableRegistry dropTableRegistry = ScriptableObject.CreateInstance<DropTableRegistry>();
        AssetDatabase.CreateAsset(dropTableRegistry, "Assets/Resources/DefaultDropTable.asset");
        
        // Populate the drop table with some example data
        RareDropProfileData commonProfile = CreateDropProfile("Common", 0.8f, 0);
        RareDropProfileData rareProfile = CreateDropProfile("Rare", 0.15f, 20);
        RareDropProfileData epicProfile = CreateDropProfile("Epic", 0.04f, 50);
        RareDropProfileData legendaryProfile = CreateDropProfile("Legendary", 0.01f, 150);

        dropTableRegistry.dropTables = new System.Collections.Generic.List<DropTableRegistry.DropTable>
        {
            new DropTableRegistry.DropTable
            {
                name = "Default",
                items = new System.Collections.Generic.List<DropTableRegistry.DropItem>
                {
                    new DropTableRegistry.DropItem { itemID = "SHARD_SWORD_COMMON", rarityProfile = commonProfile, weight = 70 },
                    new DropTableRegistry.DropItem { itemID = "SKIN_ROOKIE_ARMOR", rarityProfile = commonProfile, weight = 30 },
                    new DropTableRegistry.DropItem { itemID = "SHARD_SWORD_RARE", rarityProfile = rareProfile, weight = 60 },
                    new DropTableRegistry.DropItem { itemID = "EFFECT_RARE_TRAIL", rarityProfile = rareProfile, weight = 40 },
                    new DropTableRegistry.DropItem { itemID = "SHARD_SWORD_EPIC", rarityProfile = epicProfile, weight = 80 },
                    new DropTableRegistry.DropItem { itemID = "SKIN_VETERAN_ARMOR", rarityProfile = epicProfile, weight = 20 },
                    new DropTableRegistry.DropItem { itemID = "LEGENDARY_SWORD_OF_DOOM", rarityProfile = legendaryProfile, weight = 100 }
                }
            }
        };
        EditorUtility.SetDirty(dropTableRegistry);

        // Create and assign RunSessionData
        RunSessionData runData = ScriptableObject.CreateInstance<RunSessionData>();
        AssetDatabase.CreateAsset(runData, "Assets/Resources/DefaultRunData.asset");
        runData.distance = 1500;
        runData.styleScore = 7500;
        runData.comboPeak = 150;
        runData.riskLaneUsage = 20;
        runData.duration = 180;
        runData.score = 100000;
        runData.reviveCount = 0;
        EditorUtility.SetDirty(runData);

        // Use reflection to set private fields on RareDropEngine
        SetPrivateField(rareDropEngine, "dropTableRegistry", dropTableRegistry);
        SetPrivateField(rareDropEngine, "pityCounterManager", pityCounterManagerObj.GetComponent<PityCounterManager>());
        SetPrivateField(rareDropEngine, "dropIntegrityValidator", dropIntegrityValidatorObj.GetComponent<DropIntegrityValidator>());
        SetPrivateField(rareDropEngine, "runSessionData", runData);

        // --- Create UI ---
        GameObject canvasObj = new GameObject("Canvas", typeof(Canvas), typeof(CanvasScaler), typeof(GraphicRaycaster));
        canvasObj.GetComponent<Canvas>().renderMode = RenderMode.ScreenSpaceOverlay;

        GameObject buttonObj = new GameObject("EvaluateDropButton", typeof(RectTransform), typeof(Button), typeof(Image));
        buttonObj.transform.SetParent(canvasObj.transform);
        RectTransform buttonRect = buttonObj.GetComponent<RectTransform>();
        buttonRect.anchoredPosition = new Vector2(0, 0);
        buttonRect.sizeDelta = new Vector2(160, 30);

        GameObject textObj = new GameObject("Text", typeof(RectTransform), typeof(Text));
        textObj.transform.SetParent(buttonObj.transform);
        Text buttonText = textObj.GetComponent<Text>();
        buttonText.text = "Evaluate Drop";
        buttonText.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
        buttonText.alignment = TextAnchor.MiddleCenter;
        buttonText.color = Color.black;
        RectTransform textRect = textObj.GetComponent<RectTransform>();
        textRect.anchorMin = Vector2.zero;
        textRect.anchorMax = Vector2.one;
        textRect.sizeDelta = Vector2.zero;

        // Add listener to the button
        buttonObj.GetComponent<Button>().onClick.AddListener(() => {
            if (Application.isPlaying)
            {
                rareDropEngine.EvaluateDrop();
            }
            else
            {
                Debug.LogWarning("The 'Evaluate Drop' button only works in Play Mode.");
            }
        });

        EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
        Debug.Log("Test scene setup complete! Open 'Assets/Scenes/RareDropTestScene.unity', press Play, and click the 'Evaluate Drop' button.");
    }
    
    private static RareDropProfileData CreateDropProfile(string name, float chance, int pity) {
        RareDropProfileData profile = ScriptableObject.CreateInstance<RareDropProfileData>();
        profile.rarityName = name;
        profile.baseDropChance = chance;
        profile.pityThreshold = pity;
        AssetDatabase.CreateAsset(profile, $"Assets/Resources/Rarity_{name}.asset");
        return profile;
    }

    private static GameObject CreateManagerObject(string name, params System.Type[] components)
    {
        GameObject obj = GameObject.Find(name);
        if (obj == null)
        {
            obj = new GameObject(name);
            foreach(var component in components)
            {
                obj.AddComponent(component);
            }
        }
        return obj;
    }
    
    private static void SetPrivateField(object obj, string fieldName, object value)
    {
        System.Type type = obj.GetType();
        System.Reflection.FieldInfo field = type.GetField(fieldName, System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        if (field != null)
        {
            field.SetValue(obj, value);
        }
        else
        {
            Debug.LogError($"Field '{fieldName}' not found on object of type '{type.Name}'.");
        }
    }
}
