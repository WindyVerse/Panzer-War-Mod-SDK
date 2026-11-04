using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using CustomAttributes;
using ShanghaiWindy.Core;
using ShanghaiWindy.Editor;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Tools.Editor
{
    [Serializable]
    public class RuntimeLibVersionGmCommand : AbstractGmCommand
    {
        public override string GetName()
        {
            return ELK.Cat_DataMigration + "编辑器工具文件版本查看 - Runtime Lib Version";
        }

        [ReadOnly] [TableList] [LocalizedLabel("文件版本 - File Version")]
        public List<DllMD5Item> dllMD5List;

        public RuntimeLibVersionGmCommand()
        {
            dllMD5List = new List<DllMD5Item>();
            RefreshDllMD5List();
        }

        public override void OnImGUI()
        {
            GUILayout.Label($"Free Supported Version: {GameDataManager.BUILD_VERSION}");
            GUILayout.Label($"DE Supported Version: {GameDataManager.BUILD_DE_VERSION}");
            
            base.OnImGUI();
        }


        [Button("刷新 - Refresh", ButtonSizes.Large)]
        private void RefreshDllMD5List()
        {
            dllMD5List.Clear();

            var dllFolderPath = Application.dataPath + "/../Packages/com.shanghaiwindy.middlelayer/Runtime";
            var dllFiles = Directory.GetFiles(dllFolderPath, "*.dll");

            foreach (var dllFile in dllFiles)
            {
                var fileName = Path.GetFileName(dllFile);

                var md5Hash = CalculateMD5(dllFile);
                dllMD5List.Add(new DllMD5Item { DllName = fileName, Hash = md5Hash });
            }
        }

        private string CalculateMD5(string filePath)
        {
            using (var md5 = MD5.Create())
            {
                using (var stream = File.OpenRead(filePath))
                {
                    var hash = md5.ComputeHash(stream);
                    return BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();
                }
            }
        }
    }

    [Serializable]
    public class DllMD5Item
    {
        [TableColumnWidth(120)] public string DllName;

        [TableColumnWidth(300)] public string Hash;
    }
}