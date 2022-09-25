using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;

namespace ProjectCronos
{
    class TestGenerator
    {
        [MenuItem("Cronos/CodeGeneratorTest")]
        static void CodeGenerator()
        {
            UnityEngine.Debug.Log("コード生成テスト");
        }
    }
}
