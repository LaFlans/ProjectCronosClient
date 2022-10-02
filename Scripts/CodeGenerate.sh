#!/bin/bash -xeu
# 変数宣言が間違っていたらエラーにする(bash シェバンで検索！)
cd `dirname $0` && cd ../

dotnet mpc -i ./ProjectCronos/Assets/MasterData/TableDefines \
 -o ./ProjectCronos/Assets/MasterData/Generated/MessagePack

dotnet dotnet-mmgen -i ./ProjectCronos/Assets/MasterData/TableDefines \
 -o ./ProjectCronos/Assets/MasterData/Generated/MasterMemory \
 -c  \
 -n Generated