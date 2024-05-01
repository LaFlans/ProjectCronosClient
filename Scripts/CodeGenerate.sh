#!/bin/bash -xeu
# 変数宣言が間違っていたらエラーにする(bash シェバンで検索！)
cd `dirname $0` && cd ../

dotnet mpc -i ./ProjectCronos/Assets/MasterData/TableDefines \
 -o ./ProjectCronos/Assets/MasterData/Generated/MessagePack

dotnet dotnet-mmgen -i ./ProjectCronos/Assets/MasterData/TableDefines \
 -o ./ProjectCronos/Assets/MasterData/Generated/MasterMemory \
 -c  \
 -n Generated

 C:\Users\shouy\Desktop\Projects\ProjectCronos\ProjectCronos\Assets\MasterData\TableDefines
 
// 手順(2024/05/01)
// 1.MasterMemory関連のコードを生成(このプロジェクトのディレクトリ位置から下記を記載)
dotnet-mmgen -i ./ProjectCronos/Assets/MasterData/TableDefines -o ./ProjectCronos/Assets/MasterData/Generated/MessagePack -n "Generated"
// 2.Unityの「ウィンドウ」→「MessagePack」→「Code Generator」を起動
// 3.-i input path(csproj or directory)の欄に「MasterData/TableDefines」を記載
// 4.-o output filepath(.cs) or directory(multiple)の欄に「MasterData/Generated/MessagePack」を記載