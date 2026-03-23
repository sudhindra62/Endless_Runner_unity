import sys
import re

def extract_latest_errors(log_path, out_path):
    error_pattern = re.compile(r'(Assets\\[^:]+\.cs\(\d+,\d+\):\s*error\s*CS\d+:\s*.*)')
    
    with open(log_path, 'r', encoding='utf-8', errors='ignore') as f:
        lines = f.readlines()
        
    latest_errors = set()
    found_errors = False
    
    # Read backwards
    for line in reversed(lines):
        match = error_pattern.search(line)
        if match:
            found_errors = True
            latest_errors.add(match.group(1).strip())
        elif found_errors and "compilation" in line.lower():
            # If we were collecting errors and reached a "compilation" start/end log, we stop.
            # Unity logs things like "Reloading assemblies" or "compiling" before errors.
            if len(latest_errors) > 0:
                break
                
    with open(out_path, 'w', encoding='utf-8') as f:
        for err in sorted(list(latest_errors)):
            f.write(err + "\n")

if __name__ == '__main__':
    extract_latest_errors(sys.argv[1], sys.argv[2])
