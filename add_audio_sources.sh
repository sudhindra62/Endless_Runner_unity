
#!/bin/bash
SCENE_FILE="Assets/Scenes/MainScene.unity"

# --- Music Source ---
MUSIC_GO_ID=60000
MUSIC_TRANSFORM_ID=60001
MUSIC_AUDIOSOURCE_ID=60002

MUSIC_GAMEOBJECT_YAML="--- !u!1 &$MUSIC_GO_ID\nGameObject:\n  m_ObjectHideFlags: 0\n  m_CorrespondingSourceObject: {instanceID: 0}\n  m_PrefabInstance: {instanceID: 0}\n  m_PrefabAsset: {instanceID: 0}\n  serializedVersion: 6\n  m_Component:\n  - component: {instanceID: $MUSIC_TRANSFORM_ID}\n  - component: {instanceID: $MUSIC_AUDIOSOURCE_ID}\n  m_Layer: 0\n  m_Name: MusicSource\n  m_TagString: Untagged\n  m_IsActive: 1\n  m_StaticEditorFlags: 0\n  m_IsActiveCheckEnabled: 0\n"
MUSIC_TRANSFORM_YAML="--- !u!4 &$MUSIC_TRANSFORM_ID\nTransform:\n  m_ObjectHideFlags: 0\n  m_CorrespondingSourceObject: {instanceID: 0}\n  m_PrefabInstance: {instanceID: 0}\n  m_PrefabAsset: {instanceID: 0}\n  m_LocalRotation: {x: 0, y: 0, z: 0, w: 1}\n  m_LocalPosition: {x: 0, y: 0, z: 0}\n  m_LocalScale: {x: 1, y: 1, z: 1}\n  m_Children: []\n  m_Father: {instanceID: 0}\n  m_RootOrder: 14\n"
MUSIC_AUDIOSOURCE_YAML="--- !u!81 &$MUSIC_AUDIOSOURCE_ID\nAudioSource:\n  m_ObjectHideFlags: 0\n  m_CorrespondingSourceObject: {instanceID: 0}\n  m_PrefabInstance: {instanceID: 0}\n  m_PrefabAsset: {instanceID: 0}\n  m_GameObject: {instanceID: $MUSIC_GO_ID}\n  m_Enabled: 1\n  m_AudioClip: {fileID: 0}\n  m_PlayOnAwake: 1\n  m_Volume: 1\n  m_Pitch: 1\n  Loop: 1\n"

# --- SFX Source ---
SFX_GO_ID=60003
SFX_TRANSFORM_ID=60004
SFX_AUDIOSOURCE_ID=60005

SFX_GAMEOBJECT_YAML="--- !u!1 &$SFX_GO_ID\nGameObject:\n  m_ObjectHideFlags: 0\n  m_CorrespondingSourceObject: {instanceID: 0}\n  m_PrefabInstance: {instanceID: 0}\n  m_PrefabAsset: {instanceID: 0}\n  serializedVersion: 6\n  m_Component:\n  - component: {instanceID: $SFX_TRANSFORM_ID}\n  - component: {instanceID: $SFX_AUDIOSOURCE_ID}\n  m_Layer: 0\n  m_Name: SFXSource\n  m_TagString: Untagged\n  m_IsActive: 1\n  m_StaticEditorFlags: 0\n  m_IsActiveCheckEnabled: 0\n"
SFX_TRANSFORM_YAML="--- !u!4 &$SFX_TRANSFORM_ID\nTransform:\n  m_ObjectHideFlags: 0\n  m_CorrespondingSourceObject: {instanceID: 0}\n  m_PrefabInstance: {instanceID: 0}\n  m_PrefabAsset: {instanceID: 0}\n  m_LocalRotation: {x: 0, y: 0, z: 0, w: 1}\n  m_LocalPosition: {x: 0, y: 0, z: 0}\n  m_LocalScale: {x: 1, y: 1, z: 1}\n  m_Children: []\n  m_Father: {instanceID: 0}\n  m_RootOrder: 15\n"
SFX_AUDIOSOURCE_YAML="--- !u!81 &$SFX_AUDIOSOURCE_ID\nAudioSource:\n  m_ObjectHideFlags: 0\n  m_CorrespondingSourceObject: {instanceID: 0}\n  m_PrefabInstance: {instanceID: 0}\n  m_PrefabAsset: {instanceID: 0}\n  m_GameObject: {instanceID: $SFX_GO_ID}\n  m_Enabled: 1\n  m_AudioClip: {fileID: 0}\n  m_PlayOnAwake: 0\n  m_Volume: 1\n  m_Pitch: 1\n  Loop: 0\n"

# Append to scene file
echo -e "\n$MUSIC_GAMEOBJECT_YAML\n$MUSIC_TRANSFORM_YAML\n$MUSIC_AUDIOSOURCE_YAML\n$SFX_GAMEOBJECT_YAML\n$SFX_TRANSFORM_YAML\n$SFX_AUDIOSOURCE_YAML" >> $SCENE_FILE

echo "AudioSource GameObjects added successfully."
