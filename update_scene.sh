#!/bin/bash
SCENE_FILE="Assets/Scenes/MainScene.unity"
MANAGER_DIR="Assets/Scripts/Managers"
SCENE_SETUP_GUID="5a9b3b8f8d7c64b4a9b3b8f8d7c6b4a9"

# Start with a high instance ID
ID=50000
GAMEOBJECT_ID=$ID; ID=$((ID+1))
TRANSFORM_ID=$ID; ID=$((ID+1))
SCENE_SETUP_COMPONENT_ID=$ID; ID=$((ID+1))

# Build component list
COMPONENT_LIST="  - component: {instanceID: $TRANSFORM_ID}\n  - component: {instanceID: $SCENE_SETUP_COMPONENT_ID}"
MANAGER_COMPONENTS=""

# Create MonoBehaviour for MainSceneSetup
SCENE_SETUP_YAML="--- !u!114 &$SCENE_SETUP_COMPONENT_ID\nMonoBehaviour:\n  m_ObjectHideFlags: 0\n  m_CorrespondingSourceObject: {instanceID: 0}\n  m_PrefabInstance: {instanceID: 0}\n  m_PrefabAsset: {instanceID: 0}\n  m_GameObject: {instanceID: $GAMEOBJECT_ID}\n  m_Enabled: 1\n  m_EditorHideFlags: 0\n  m_Script: {fileID: 11500000, guid: $SCENE_SETUP_GUID, type: 3}\n  m_Name: \n  m_EditorClassIdentifier: \n"

# Loop through managers
for f in $MANAGER_DIR/*.cs.meta; do
  GUID=$(grep guid "$f" | sed 's/guid: //')
  COMPONENT_ID=$ID; ID=$((ID+1))
  COMPONENT_LIST="$COMPONENT_LIST\n  - component: {instanceID: $COMPONENT_ID}"
  MANAGER_COMPONENTS="$MANAGER_COMPONENTS\n--- !u!114 &$COMPONENT_ID\nMonoBehaviour:\n  m_ObjectHideFlags: 0\n  m_CorrespondingSourceObject: {instanceID: 0}\n  m_PrefabInstance: {instanceID: 0}\n  m_PrefabAsset: {instanceID: 0}\n  m_GameObject: {instanceID: $GAMEOBJECT_ID}\n  m_Enabled: 1\n  m_EditorHideFlags: 0\n  m_Script: {fileID: 11500000, guid: $GUID, type: 3}\n  m_Name: \n  m_EditorClassIdentifier: \n"
done

# Create GameObject and Transform
GAMEOBJECT_YAML="--- !u!1 &$GAMEOBJECT_ID\nGameObject:\n  m_ObjectHideFlags: 0\n  m_CorrespondingSourceObject: {instanceID: 0}\n  m_PrefabInstance: {instanceID: 0}\n  m_PrefabAsset: {instanceID: 0}\n  serializedVersion: 6\n  m_Component:\n$COMPONENT_LIST\n  m_Layer: 0\n  m_Name: MainSceneSetup\n  m_TagString: Untagged\n  m_IsActive: 1\n  m_StaticEditorFlags: 0\n  m_IsActiveCheckEnabled: 0\n"
TRANSFORM_YAML="--- !u!4 &$TRANSFORM_ID\nTransform:\n  m_ObjectHideFlags: 0\n  m_CorrespondingSourceObject: {instanceID: 0}\n  m_PrefabInstance: {instanceID: 0}\n  m_PrefabAsset: {instanceID: 0}\n  m_LocalRotation: {x: 0, y: 0, z: 0, w: 1}\n  m_LocalPosition: {x: 0, y: 0, z: 0}\n  m_LocalScale: {x: 1, y: 1, z: 1}\n  m_Children: []\n  m_Father: {instanceID: 0}\n  m_RootOrder: 13\n"

# Append to scene file
echo -e "\n$GAMEOBJECT_YAML\n$TRANSFORM_YAML\n$SCENE_SETUP_YAML\n$MANAGER_COMPONENTS" >> $SCENE_FILE

echo "Scene updated successfully."
