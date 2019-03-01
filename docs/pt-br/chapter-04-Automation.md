# Scripting


## VBA


## Powershell

### Tail no Windows

```powershell
    Get-Content arquivo.log –wait
    Get-Content arquivo.log -wait | where { $_ -match "ERROR" }
``` 

### Mesclando arquivos texto

```powershell
    Get-ChildItem -recurse -include "*.txt" | % { Get-Content $_ -ReadCount 0 | Add-Content .combined_files.txt }
```

## Azure Command Line Tools

bash
az login
az account set  --subscription "fd4ba5aa-c5be-424d-a3f8-42eb6a756001"
