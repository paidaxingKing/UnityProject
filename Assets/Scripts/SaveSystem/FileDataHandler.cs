using System;
using System.IO;
using UnityEngine;

public class FileDataHandler
{
    private string fullPath;//游戏数据存储位置
    private bool encryptData;//encrypt加密
    private string codeword = "paidaxing";

    public FileDataHandler(string dataDirPath, string dataFileName,bool encryptData)
    {
        fullPath = Path.Combine(dataDirPath, dataFileName);//注意参数不能写反，先是目录名再是文件名
        this.encryptData = encryptData;
        Debug.Log(fullPath);
    }

    public void SaveData(GameData gameData)
    {
        try
        {
            Directory.CreateDirectory(Path.GetDirectoryName(fullPath));//创建目录

            string dataToSave = JsonUtility.ToJson(gameData, true);//将以实例类型保存的数据转化为json形式的字符串

            if (encryptData)
            {
                dataToSave = EncryptDecrypt(dataToSave);
            }    

            using (FileStream stream = new FileStream(fullPath, FileMode.Create))//using表示离开代码块后自动关闭文件流
            {//创建或打开文件
                using (StreamWriter write = new StreamWriter(stream))//写入流
                {//写入文件
                    write.WriteLine(dataToSave);
                }
            }
        }

        catch (Exception e)
        {
            Debug.LogError("Error On tring to save data to file: " + fullPath + "\n" + e);
        }
    }

    public GameData LoadData()
    {
        GameData loadData = null;

        if (File.Exists(fullPath))
        {
            try
            {
                string dataToLoad = "";

                using (FileStream stream = new FileStream(fullPath, FileMode.Open))//打开文件流
                {
                    using (StreamReader reader = new StreamReader(stream))//读取文件流
                    {
                        dataToLoad = reader.ReadToEnd();//将文件流的信息以字符串形式提取保存
                    }
                }

                if (encryptData)
                {
                    dataToLoad = EncryptDecrypt(dataToLoad);
                }

                loadData = JsonUtility.FromJson<GameData>(dataToLoad);
            }

            catch(Exception e)
            {
                Debug.LogError("Error on trying to load data from: " + fullPath + "\n" + e);
            }
        }
        return loadData;
    }

    public void Delete()
    {
        if(File.Exists(fullPath))
        {
            File.Delete(fullPath);
        }
    }

    private string EncryptDecrypt(string data)//加密和解密,使用一次加密，使用两次解密，恢复原文
    {
        string modifiedData = "";

        for (int i = 0; i < data.Length; i++)
        {
            modifiedData += (char)(data[i] ^ codeword[i % codeword.Length]);
        }

        return modifiedData;
    }
}
