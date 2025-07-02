import sys;
import re;

def scan_file(file_path:str) -> int:
    empty_line = '^ *\\|[\\| \\t]+$'
    regex = re.compile(empty_line, re.MULTILINE | re.IGNORECASE)
    try:
        with open(file_path, 'r',encoding="utf8") as file:            
            content = file.read()
            results = regex.findall(content)
            if results:                
                print(f"Empty lines found in {file_path}:")
                lines = content.splitlines()
                line_number = -1
                for error in results:
                    line_number = lines.index(error, line_number+1)
                    print(f"  line {line_number}")
                return 1
            else:
                return 0
            
    except FileNotFoundError:
        print(f"The file {file_path} does not exist.")

if __name__ == "__main__":
    item = 0    
    args = sys.argv[1:]
    for arg in args:
        if arg.endswith('.md'):
            item |= scan_file(arg)
    sys.exit(item)
    