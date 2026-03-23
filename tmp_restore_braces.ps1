
$scripts = Get-ChildItem -Path "Assets\Scripts" -Filter "*.cs" -Recurse

foreach ($file in $scripts) {
    $content = Get-Content $file.FullName
    $newContent = @()
    $fixed = $false
    
    # Heuristic: Add { after lines ending in ) or class/struct/enum definitions that don't have {
    # and aren't followed by a { line.
    
    for ($i = 0; $i -lt $content.Count; $i++) {
        $line = $content[$i]
        $newContent += $line
        
        $trimmed = $line.Trim()
        
        # Check if this line looks like a signature that NEEDS a {
        # Patterns: 
        # 1. public/private/protected class/struct/interface/enum ... (no { at end)
        # 2. public/private/protected [type] Method(...) (no ; or { at end)
        # 3. if/foreach/for/while (...) (no ; or { at end)
        # 4. protected override void Awake() ...
        
        $isSignature = $false
        if ($trimmed -match '^(public|private|protected|internal|static|override|virtual).*' -and 
            $trimmed -notmatch '[;\{]$') {
            
            # Additional check to exclude fields/properties
            if ($trimmed -match '(class|struct|interface|enum|void|int|string|bool|float|Task)' -or 
                $trimmed -match '\(.*\)$') {
                $isSignature = $true
            }
        }
        
        if ($isSignature) {
            # Check next non-empty line
            $nextIdx = $i + 1
            while ($nextIdx -lt $content.Count -and [string]::IsNullOrWhiteSpace($content[$nextIdx])) {
                $nextIdx++
            }
            
            if ($nextIdx -lt $content.Count) {
                if ($content[$nextIdx].Trim() -ne '{') {
                    $newContent += "    {"
                    $fixed = $true
                }
            } else {
                # End of file after signature? needs {
                $newContent += "    {"
                $fixed = $true
            }
        }
    }
    
    if ($fixed) {
        $newContent | Set-Content $file.FullName
        Write-Host "Restored Braces in: $($file.FullName)"
    }
}
