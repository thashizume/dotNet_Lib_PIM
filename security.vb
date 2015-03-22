
Imports System.IO
Imports System.Text
Imports System.Security.Cryptography

Public Class security

    ' 128bit(16byte)のIV（初期ベクタ）とKey（暗号キー）
    Private Const AesIV As String = "!QAZ2WSX#EDC4RFV"

    'Private AesIV As String

    Private Const AesKey As String = "5TGB&YHN7UJM(IK<"

    Public Sub New()
        'AesIV = Left((System.Guid.NewGuid.ToString).Replace("-", ""), 16).ToUpper

    End Sub


    ''' <summary>
    ''' 文字列をAESで暗号化
    ''' </summary>
    Public Function Encrypt(text As String) As String
        ' AES暗号化サービスプロバイダ
        Dim aes As New AesCryptoServiceProvider()
        aes.BlockSize = 128
        aes.KeySize = 128
        aes.IV = Encoding.UTF8.GetBytes(AesIV)
        aes.Key = Encoding.UTF8.GetBytes(AesKey)
        aes.Mode = CipherMode.CBC
        aes.Padding = PaddingMode.PKCS7

        ' 文字列をバイト型配列に変換
        Dim src As Byte() = Encoding.Unicode.GetBytes(text)

        ' 暗号化する
        Using enc As ICryptoTransform = aes.CreateEncryptor()
            Dim dest As Byte() = enc.TransformFinalBlock(src, 0, src.Length)

            ' バイト型配列からBase64形式の文字列に変換
            Return Convert.ToBase64String(dest)
        End Using
    End Function

    ''' <summary>
    ''' 文字列をAESで復号化
    ''' </summary>
    Public Function Decrypt(text As String) As String
        ' AES暗号化サービスプロバイダ
        Dim aes As New AesCryptoServiceProvider()
        aes.BlockSize = 128
        aes.KeySize = 128
        aes.IV = Encoding.UTF8.GetBytes(AesIV)
        aes.Key = Encoding.UTF8.GetBytes(AesKey)
        aes.Mode = CipherMode.CBC
        aes.Padding = PaddingMode.PKCS7

        ' Base64形式の文字列からバイト型配列に変換
        Dim src As Byte() = System.Convert.FromBase64String(text)

        ' 複号化する
        Using dec As ICryptoTransform = aes.CreateDecryptor()
            Dim dest As Byte() = dec.TransformFinalBlock(src, 0, src.Length)
            Return Encoding.Unicode.GetString(dest)
        End Using
    End Function

End Class
