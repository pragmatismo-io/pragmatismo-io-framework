Pragmatismo.io Methodology

Copyright (c) Pragmatismo.io. All rights reserved.                          
Licensed under the MIT license                                              

Powershell

#### 6.5.6.1 Tail no Windows

~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~ powershell
    Get-Content arquivo.log –wait
    Get-Content arquivo.log -wait | where { $_ -match "ERROR" }


#### 6.5.6.1 Mesclando arquivos texto

~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~ powershell
    Get-ChildItem -recurse -include "*.txt" | % { Get-Content $_ -ReadCount 0 | Add-Content .combined_files.txt }
