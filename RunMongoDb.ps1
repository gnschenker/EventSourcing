New-Item .\logs\mongodb -type directory -force
New-Item .\data\mongodb -type directory -force
$path = Get-Location
    echo "systemLog:" > .\tools\MongoDb\mongod.cfg
    echo "   destination: file" >> .\tools\MongoDb\mongod.cfg
    echo "   path: $path\logs\mongodb\mongod.log" >> .\tools\MongoDb\mongod.cfg
    echo "storage:" >> .\tools\\MongoDb\mongod.cfg
    echo "   dbPath: $path\data\mongodb" >> .\tools\MongoDb\mongod.cfg
    &"$path\tools\mongodb\mongod.exe" --config=$path\tools\mongodb\mongod.cfg