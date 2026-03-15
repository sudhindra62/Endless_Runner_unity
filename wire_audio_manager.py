
import yaml

SCENE_FILE = 'Assets/Scenes/MainScene.unity'

with open(SCENE_FILE, 'r') as f:
    content = f.read()

yaml_docs = content.split('--- !u!')

# Find the AudioManager MonoBehaviour
for i, doc in enumerate(yaml_docs):
    if 'MonoBehaviour' in doc and 'm_Script: {fileID: 11500000, guid: d2b12a7f5b0f54b4ab8a7b3b3a7b5d1f, type: 3}' in doc: # GUID for AudioManager
        # Add the references to the AudioSource components
        yaml_docs[i] = doc.replace('m_Name: ','musicSource: {fileID: 60002}\n  sfxSource: {fileID: 60005}\n  m_Name: ')
        break

# Join the YAML documents back together
new_content = '--- !u!'.join(yaml_docs)

with open(SCENE_FILE, 'w') as f:
    f.write(new_content)

print('AudioManager wired successfully.')
