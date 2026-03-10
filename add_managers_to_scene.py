
import os
import uuid
import re

def get_guid(meta_file_path):
    if not os.path.exists(meta_file_path):
        new_guid = uuid.uuid4().hex
        with open(meta_file_path, "w") as f:
            f.write(f"fileFormatVersion: 2\nguid: {new_guid}\n")
        return new_guid
    with open(meta_file_path, "r") as f:
        for line in f:
            if "guid" in line:
                return line.split(":")[1].strip()
    return None

def get_existing_guids_in_scene(scene_path):
    guids = set()
    if os.path.exists(scene_path):
        with open(scene_path, "r") as f:
            content = f.read()
            found_guids = re.findall(r"guid: (\w+)", content)
            guids.update(found_guids)
    return guids

def add_managers_to_scene():
    managers_path = "Assets/Scripts/Managers"
    scene_path = "Assets/Scenes/MainScene.unity"
    
    manager_scripts = [f for f in os.listdir(managers_path) if f.endswith(".cs")]
    
    existing_guids = get_existing_guids_in_scene(scene_path)
    
    with open(scene_path, "a") as scene_file:
        file_id_counter = 20000 
        for script in manager_scripts:
            meta_path = os.path.join(managers_path, script + ".meta")
            guid = get_guid(meta_path)
            
            if guid not in existing_guids:
                game_object_id = file_id_counter
                transform_id = file_id_counter + 1
                monobehaviour_id = file_id_counter + 2
                file_id_counter += 3
                
                manager_name = script.replace(".cs", "")

                scene_file.write(f"""
---
!u!1 &{game_object_id}
GameObject:
  m_ObjectHideFlags: 0
  m_CorrespondingSourceObject: {{instanceID: 0}}
  m_PrefabInstance: {{instanceID: 0}}
  m_PrefabAsset: {{instanceID: 0}}
  serializedVersion: 6
  m_Component:
  - component: {{instanceID: {transform_id}}}
  - component: {{instanceID: {monobehaviour_id}}}
  m_Layer: 0
  m_Name: {manager_name}
  m_TagString: Untagged
  m_IsActive: 1
  m_StaticEditorFlags: 0
  m_IsActiveCheckEnabled: 0
---
!u!4 &{transform_id}
Transform:
  m_ObjectHideFlags: 0
  m_CorrespondingSourceObject: {{instanceID: 0}}
  m_PrefabInstance: {{instanceID: 0}}
  m_PrefabAsset: {{instanceID: 0}}
  m_LocalRotation: {{x: 0, y: 0, z: 0, w: 1}}
  m_LocalPosition: {{x: 0, y: 0, z: 0}}
  m_LocalScale: {{x: 1, y: 1, z: 1}}
  m_Children: []
  m_Father: {{instanceID: 0}}
  m_RootOrder: {file_id_counter}
  m_LocalEulerAnglesHint: {{x: 0, y: 0, z: 0}}
---
!u!114 &{monobehaviour_id}
MonoBehaviour:
  m_ObjectHideFlags: 0
  m_CorrespondingSourceObject: {{instanceID: 0}}
  m_PrefabInstance: {{instanceID: 0}}
  m_PrefabAsset: {{instanceID: 0}}
  m_GameObject: {{instanceID: {game_object_id}}}
  m_Enabled: 1
  m_EditorHideFlags: 0
  m_Script: {{fileID: 11500000, guid: {guid}, type: 3}}
  m_Name: 
  m_EditorClassIdentifier: 
""")

if __name__ == "__main__":
    add_managers_to_scene()
