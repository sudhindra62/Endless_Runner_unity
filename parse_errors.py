import re
import sys

def parse_errors(log_path, out_path):
    error_pattern = re.compile(r'(Assets\\[^:]+\.cs\(\d+,\d+\):\s*error\s*CS\d+:\s*.*)')
    unique_errors = set()
    
    with open(log_path, 'r', encoding='utf-8', errors='ignore') as f:
        for line in f:
            match = error_pattern.search(line)
            if match:
                unique_errors.add(match.group(1).strip())
                
    with open(out_path, 'w', encoding='utf-8') as f:
        for err in sorted(list(unique_errors)):
            f.write(err + "\n")
            
if __name__ == '__main__':
    parse_errors(sys.argv[1], sys.argv[2])
