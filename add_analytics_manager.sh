
#!/bin/bash
SCENE_FILE="Assets/Scenes/MainScene.unity"

# --- AnalyticsManager --- 
ANALYTICS_GO_ID=70000
ANALYTICS_TRANSFORM_ID=70001
ANALYTICS_MONOBEHAVIOUR_ID=70002
ANALYTICS_MANAGER_GUID=a7b8c9d0e1f2a3b4c5d6e7f8a9b0c1d2

ANALYTICS_GAMEOBJECT_YAML="--- !u!1 &$ANALYTICS_GO_ID\nGameObject:\n  m_ObjectHideFlags: 0\n  m_CorrespondingSourceObject: {instanceID: 0}\n  m_PrefabInstance: {instanceID: 0}\n  m_PrefabAsset: {instanceID: 0}\n  serializedVersion: 6\n  m_Component:\n  - component: {instanceID: $ANALYTICS_TRANSFORM_ID}\n  - component: {instanceID: $ANALYTICS_MONOBEHAVIOUR_ID}\n  m_Layer: 0\n  m_Name: AnalyticsManager\n  m_TagString: Untagged\n  m_IsActive: 1\n  m_StaticEditorFlags: 0\n  m_IsActiveCheckEnabled: 0\n"

ANALYTICS_TRANSFORM_YAML="--- !u!4 &$ANALYTICS_TRANSFORM_ID\nTransform:\n  m_ObjectHideFlags: 0\n  m_CorrespondingSourceObject: {instanceID: 0}\n  m_PrefabInstance: {instanceID: 0}\n  m_PrefabAsset: {instanceID: 0}\n  m_LocalRotation: {x: 0, y: 0, z: 0, w: 1}\n  m_LocalPosition: {x: 0, y: 0, z: 0}\n  m_LocalScale: {x: 1, y: 1, z: 1}\n  m_Children: []\n  m_Father: {instanceID: 0}\n  m_RootOrder: 16\n"

ANALYTICS_MONOBEHAVIOUR_YAML="--- !u!114 &$ANALYTICS_MONOBEHAVIOUR_ID\nMonoBehaviour:\n  m_ObjectHideFlags: 0\n  m_CorrespondingSourceObject: {instanceID: 0}\n  m_PrefabInstance: {instanceID: 0}\n  m_PrefabAsset: {instanceID: 0}\n  m_GameObject: {instanceID: $ANALYTICS_GO_ID}\n  m_Enabled: 1\n  m_EditorHideFlags: 0\n  m_Script: {fileID: 11500000, guid: $ANALYTICS_MANAGER_GUID, type: 3}\n  m_Name: \n  m_EditorClassIdentifier: \n"

# Append to scene file
echo -e "\n$ANALYTICS_GAMEOBJECT_YAML\n$ANALYTICS_TRANSFORM_YAML\n$ANALYTICS_MONOBEHAVIOUR_YAML" >> $SCENE_FILE

echo "AnalyticsManager GameObject added successfully."
