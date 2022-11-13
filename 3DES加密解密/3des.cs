// See https://aka.ms/new-console-template for more information
using System.Net;
using System.Reflection.PortableExecutable;
using System;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Security.Cryptography;

EncryptHelper helper = new EncryptHelper("123456", "111");
string oldValue = "13800138000";
string newValue = helper.Encrypt(oldValue);
Console.WriteLine("加密：" + newValue);
Console.WriteLine("解密：" + helper.Decrypt(newValue));
Console.ReadLine();

/// <summary>
/// 加解密类
/// </summary>
public class EncryptHelper
{
    public string Key { get; set; }
    public string IV { get; set; }
    //构造一个对称算法
    private SymmetricAlgorithm mCSP = TripleDES.Create();

    /// <summary>
    /// 
    /// </summary>
    /// <param name="key">密钥，必须32位</param>
    /// <param name="iv">向量，必须是12个字符</param>
    public EncryptHelper(string key, string iv)
    {
        this.Key = this.md5(key);
        this.IV = this.md5(iv).Substring(0, 11) + "=";
    }

    /// <summary>
    /// 字符串的加密
    /// </summary>
    /// <param name="Value">要加密的字符串</param>
    /// <returns>加密后的字符串</returns>
    public string Encrypt(string Value)
    {
        try
        {
            ICryptoTransform ct;
            MemoryStream ms;
            CryptoStream cs;
            byte[] byt;
            mCSP.Key = Convert.FromBase64String(this.Key);
            mCSP.IV = Convert.FromBase64String(this.IV);
            ct = mCSP.CreateEncryptor(mCSP.Key, mCSP.IV);//创建加密对象
            byt = Encoding.UTF8.GetBytes(Value);
            ms = new MemoryStream();
            cs = new CryptoStream(ms, ct, CryptoStreamMode.Write);
            cs.Write(byt, 0, byt.Length);
            cs.FlushFinalBlock();
            cs.Close();

            return Convert.ToBase64String(ms.ToArray());
        }
        catch (Exception ex)
        {
            //MessageBox.Show(ex.Message, "出现异常", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return ("Error in Encrypting " + ex.Message);
        }
    }

    /// <summary>
    /// 解密字符串
    /// </summary>
    /// <param name="Value">加密后的字符串</param>
    /// <returns>解密后的字符串</returns>
    public string Decrypt(string Value)
    {
        try
        {
            ICryptoTransform ct;//加密转换运算
            MemoryStream ms;//内存流
            CryptoStream cs;//数据流连接到数据加密转换的流
            byte[] byt;
            mCSP.Key = Convert.FromBase64String(this.Key);
            mCSP.IV = Convert.FromBase64String(this.IV);
            ct = mCSP.CreateDecryptor(mCSP.Key, mCSP.IV);//创建对称解密对象
            byt = Convert.FromBase64String(Value);
            ms = new MemoryStream();
            cs = new CryptoStream(ms, ct, CryptoStreamMode.Write);
            cs.Write(byt, 0, byt.Length);
            cs.FlushFinalBlock();
            cs.Close();

            return Encoding.UTF8.GetString(ms.ToArray());
        }
        catch (Exception ex)
        {
            //MessageBox.Show(ex.Message, "出现异常", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return ("Error in Decrypting " + ex.Message);
        }
    }

    public string md5(string str)
    {
        byte[] buffer = Encoding.UTF8.GetBytes(str);
        using (MD5 md5 = MD5.Create())
        {
            byte[] md5Bytes = md5.ComputeHash(buffer);
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < md5Bytes.Length; i++)
            {
                sb.Append(md5Bytes[i].ToString("x2")); // X2时，生成字母大写MD5
            }
            return sb.ToString();
        }
    }
}