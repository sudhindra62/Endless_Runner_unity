#!/bin/bash
cd Assets/Scripts/Managers
for f in *.cs; do
    if [ ! -f "$f.meta" ]; then
        guid=$(cat /proc/sys/kernel/random/uuid | sed 's/-//g')
        echo "fileFormatVersion: 2" > "$f.meta"
        echo "guid: $guid" >> "$f.meta"
        echo "MonoImporter:" >> "$f.meta"
        echo "  externalObjects: {}" >> "$f.meta"
        echo "  serializedVersion: 2" >> "$f.meta"
        echo "  defaultReferences: []" >> "$f.meta"
        echo "  executionOrder: 0" >> "$f.meta"
        echo "  icon: {instanceID: 0}" >> "$f.meta"
        echo "  userData: " >> "$f.meta"
        echo "  assetBundleName: " >> "$f.meta"
        echo "  assetBundleVariant: " >> "$f.meta"
    fi
done