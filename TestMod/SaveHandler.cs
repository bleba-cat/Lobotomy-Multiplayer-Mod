using ICSharpCode.SharpZipLib.Zip;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace TestMod
{
    public class SaveHandler
    {
        static readonly List<string> files = new List<string>(){
            Path.Combine(Application.persistentDataPath, "etc170808.dat"),
            Path.Combine(Application.persistentDataPath, "saveData170808.dat"),
            Path.Combine(Application.persistentDataPath, "saveGlobal170808.dat"),
            Path.Combine(Application.persistentDataPath, "saveUnlimitV5170808.dat"),
            Path.Combine(Application.persistentDataPath, "creatureData")
        };
        public static (byte[], int) GetSave()
        {
            byte[] buffer = new byte[4096];
            using (MemoryStream memoryStream = new MemoryStream(131072))
            {
                memoryStream.WriteByte((byte)((int)PacketType.Save & 0xFF));
                memoryStream.WriteByte((byte)(((int)PacketType.Save >> 8) & 0xFF));
                memoryStream.WriteByte((byte)(((int)PacketType.Save >> 16) & 0xFF));
                memoryStream.WriteByte((byte)(((int)PacketType.Save >> 24) & 0xFF));
                using (ZipOutputStream zipStream = new ZipOutputStream(memoryStream))
                {
                    zipStream.IsStreamOwner = false;
                    zipStream.SetLevel(3);
                    foreach (string file in files)
                    {
                        if (File.Exists(file))
                        {
                            string relativePath = file.Substring(Application.persistentDataPath.Length + 1);
                            ZipEntry entry = new ZipEntry(ZipEntry.CleanName(relativePath)) { DateTime = File.GetLastWriteTime(file) };
                            zipStream.PutNextEntry(entry);
                            using (FileStream fs = File.OpenRead(file))
                            {
                                int sourceBytes;
                                while ((sourceBytes = fs.Read(buffer, 0, buffer.Length)) > 0)
                                {
                                    zipStream.Write(buffer, 0, sourceBytes);
                                }
                            }
                            zipStream.CloseEntry();
                        }
                        else if (Directory.Exists(file))
                        {
                            foreach (string fileInDirectory in Directory.GetFiles(file, "*.dat"))
                            {
                                string relativePath = fileInDirectory.Substring(Application.persistentDataPath.Length + 1);
                                ZipEntry entry = new ZipEntry(ZipEntry.CleanName(relativePath)) { DateTime = File.GetLastWriteTime(fileInDirectory) };
                                zipStream.PutNextEntry(entry);
                                using (FileStream fs = File.OpenRead(fileInDirectory))
                                {
                                    int sourceBytes;
                                    while ((sourceBytes = fs.Read(buffer, 0, buffer.Length)) > 0)
                                    {
                                        zipStream.Write(buffer, 0, sourceBytes);
                                    }
                                }
                                zipStream.CloseEntry();
                            }
                        }
                    }
                    zipStream.Finish();
                }
                return (memoryStream.GetBuffer(), (int)memoryStream.Length);
            }
        }
        public static void SetSave(byte[] save)
        {
            string backupPath = Path.Combine(Application.persistentDataPath, "backup");
            try
            {
                if (Directory.Exists(backupPath)) Directory.Delete(backupPath, true);
                Directory.CreateDirectory(backupPath);
                foreach (string file in files)
                {
                    if (File.Exists(file))
                    {
                        File.Copy(file, Path.Combine(backupPath, Path.GetFileName(file)));
                    }
                    else if (Directory.Exists(file))
                    {
                        string directoryPath = Path.Combine(backupPath, file.Substring(Application.persistentDataPath.Length + 1));
                        Directory.CreateDirectory(directoryPath);
                        foreach (string fileInDirectory in Directory.GetFiles(file, "*.dat"))
                        {
                            File.Copy(fileInDirectory, Path.Combine(directoryPath, Path.GetFileName(fileInDirectory)));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Directory.Delete(backupPath, true);
                File.WriteAllText(Application.dataPath + "/BaseMods/Test_MOD/Bleba_Log.txt", ex.ToString());
                return;
            }
            foreach (string file in files)
            {
                if (File.Exists(file))
                {
                    File.Delete(file);
                }
                else if (Directory.Exists(file))
                {
                    Directory.Delete(file, true);
                }
            }
            byte[] buffer = new byte[4096];
            const long MaxSingleFileSize = 200 * 1024;
            const long MaxTotalSize = 150 * 1024 * 1024;
            long totalUncompressedBytes = 0;
            using (MemoryStream memoryStream = new MemoryStream(save))
            {
                try
                {
                    string fullAllowedDir = Path.GetFullPath(Application.persistentDataPath);
                    using (ZipInputStream zipStream = new ZipInputStream(memoryStream))
                    {
                        ZipEntry entry;
                        while ((entry = zipStream.GetNextEntry()) != null)
                        {
                            if (entry.IsDirectory) continue;
                            string cleanedEntryName = ZipEntry.CleanName(entry.Name);
                            string targetPath = Path.Combine(Application.persistentDataPath, cleanedEntryName);
                            string fullTargetPath = Path.GetFullPath(targetPath);
                            if (!files.Exists(p => Path.GetFullPath(p) == fullTargetPath) && !files.Exists(p => Path.GetFullPath(p) == Path.GetDirectoryName(fullTargetPath)))
                            {
                                File.WriteAllText(Application.dataPath + "/BaseMods/Test_MOD/Bleba_Log.txt", "Путь файла не соответствует ожидаемым. Возможно, хост прислал заражённый .zip");
                                return;
                            }
                            if (Path.GetExtension(fullTargetPath) != ".dat")
                            {
                                File.WriteAllText(Application.dataPath + "/BaseMods/Test_MOD/Bleba_Log.txt", "Формат одного из файлов не .dat. Возможно, хост прислал заражённый .zip");
                                return;
                            }
                            string directoryName = Path.GetDirectoryName(targetPath);
                            if (!string.IsNullOrEmpty(directoryName))
                            {
                                Directory.CreateDirectory(directoryName);
                            }
                            long currentFileBytes = 0;
                            using (FileStream fs = File.Create(targetPath))
                            {
                                int sourceBytes;
                                while ((sourceBytes = zipStream.Read(buffer, 0, buffer.Length)) > 0)
                                {
                                    currentFileBytes += sourceBytes;
                                    totalUncompressedBytes += sourceBytes;
                                    if (currentFileBytes > MaxSingleFileSize)
                                    {
                                        File.WriteAllText(Application.dataPath + "/BaseMods/Test_MOD/Bleba_Log.txt", "Один из файлов слишком большой. Возможно, хост прислал заражённый .zip");
                                        return;
                                    }
                                    if (totalUncompressedBytes > MaxTotalSize)
                                    {
                                        File.WriteAllText(Application.dataPath + "/BaseMods/Test_MOD/Bleba_Log.txt", "Размер сохранения слишком большой. Возможно, хост прислал заражённый .zip");
                                        fs.Close();
                                        return;
                                    }
                                    fs.Write(buffer, 0, sourceBytes);
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    SetOldSave();
                    File.WriteAllText(Application.dataPath + "/BaseMods/Test_MOD/Bleba_Log.txt", "Ошибка при обработке архива папки: " + ex.Message);
                    return;
                }
            }
        }
        public static void SetOldSave()
        {
            try
            {
                string backupPath = Path.Combine(Application.persistentDataPath, "backup");
                string targetCreatureData = Path.Combine(Application.persistentDataPath, "creatureData");
                if (Directory.Exists(backupPath))
                {
                    if (Directory.Exists(targetCreatureData)) Directory.Delete(targetCreatureData, true);
                    foreach (string file in Directory.GetFiles(backupPath, "*.dat"))
                    {
                        string destPath = Path.Combine(Application.persistentDataPath, Path.GetFileName(file));
                        File.Copy(file, destPath, true);
                    }
                    string creatureData = Path.Combine(backupPath, "creatureData");
                    if (Directory.Exists(creatureData))
                    {
                        Directory.CreateDirectory(targetCreatureData);
                        foreach (string data in Directory.GetFiles(creatureData, "*.dat"))
                        {
                            string destPath = Path.Combine(targetCreatureData, Path.GetFileName(data));
                            File.Copy(data, destPath, true);
                        }
                    }
                    Directory.Delete(backupPath, true);
                }
            }
            catch (Exception ex)
            {
                File.WriteAllText(Application.dataPath + "/BaseMods/Test_MOD/Bleba_Log.txt", ex.ToString());
            }
        }
    }
}
