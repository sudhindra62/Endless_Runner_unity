
import re
import sys

guid_map = {}
try:
    lines = sys.stdin.read().strip().split('\n')
    if len(lines) < 2:
        print("No meta files found or input is empty.")
        sys.exit(0)

    i = 0
    while i < len(lines):
        filepath = lines[i]
        if not filepath.endswith(".meta"):
            # This can happen if a line in the file content contains "guid:"
            # and our processing of lines gets misaligned.
            # Let's try to find the next .meta file path.
            i += 1
            continue

        if i + 1 >= len(lines):
            break

        guid_line = lines[i+1]
        match = re.search(r'guid: (\w+)', guid_line)
        if match:
            guid = match.group(1)
            if guid in guid_map:
                guid_map[guid].append(filepath)
            else:
                guid_map[guid] = [filepath]
            i += 2
        else:
            # This is also for robustness, if a .meta file doesn't have a guid for some reason.
            i += 1


except Exception as e:
    print(f"An error occurred: {e}")
    sys.exit(1)

duplicates = {guid: paths for guid, paths in guid_map.items() if len(paths) > 1}

if duplicates:
    print("Duplicate GUIDs found:")
    for guid, paths in duplicates.items():
        print(f"  GUID: {guid}")
        for path in paths:
            print(f"    - {path}")
else:
    print("No duplicate GUIDs found.")
