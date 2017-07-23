using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography.Pkcs;

namespace X509Encyption
{
    public class X509Encryptioner
    {
        #region standard method to sign and verify 
        //http://blog.csdn.net/besley/article/details/7918787
        public static string Signature(string hashal = "SHA1")
        {
            string docFile = @"G:\MyCode\CSharp\20170418\X509Encyption\testSendFile.txt";
            byte[] hashBytes;
            using (FileStream fs = new FileStream(docFile, FileMode.Open))
            {
                //计算报文的Hash值  
                HashAlgorithm hashAlgorithm = HashAlgorithm.Create(hashal);
                hashBytes = hashAlgorithm.ComputeHash(fs);
                fs.Close();
            }

            //Bob用自己(发送方)的私钥对报文摘要进行签名  
            //包含私钥文件的rsa算法初始化  
            string priKeyFile = @"G:\MyCode\CSharp\20170418\X509Encyption\public_privatekey.pfx";
            X509Certificate2 certEncrypt = new X509Certificate2(priKeyFile, "abc123");
            byte[] signedHashBytes;
            using (RSACryptoServiceProvider rsaProviderEncrypt = (RSACryptoServiceProvider)certEncrypt.PrivateKey)
            {
                //给报文摘要进行签名  
                RSAPKCS1SignatureFormatter signFormatter = new RSAPKCS1SignatureFormatter(rsaProviderEncrypt);
                signFormatter.SetHashAlgorithm(hashal);
                signedHashBytes = signFormatter.CreateSignature(hashBytes);
                rsaProviderEncrypt.Dispose();
            }

            return Convert.ToBase64String(signedHashBytes);
        }

        public static bool VerifySignature(string digest, string hashal = "SHA1")
        {
            byte[] signedHashBytes = Convert.FromBase64String(digest);

            string docFileNew = @"G:\MyCode\CSharp\20170418\X509Encyption\testSendFile.txt";
            byte[] hashBytesNew;
            using (FileStream fs = new FileStream(docFileNew, FileMode.Open))
            {
                //计算报文的Hash值  
                HashAlgorithm hashAlgorithm = HashAlgorithm.Create(hashal);
                hashBytesNew = hashAlgorithm.ComputeHash(fs);
                fs.Close();
            }

            //由证书公钥文件验证  
            //初始化rsa算法  
            string pubKeyFile = @"G:\MyCode\CSharp\20170418\X509Encyption\publickey.cer";
            X509Certificate2 cerDecrypt = new X509Certificate2(pubKeyFile);
            bool isOk = false;
            using (RSACryptoServiceProvider rsaProviderDecrypt = (RSACryptoServiceProvider)cerDecrypt.PublicKey.Key)
            {
                //验证报文摘要和签名是否匹配  
                RSAPKCS1SignatureDeformatter deFormatter = new RSAPKCS1SignatureDeformatter(rsaProviderDecrypt);
                deFormatter.SetHashAlgorithm(hashal);
                isOk = deFormatter.VerifySignature(hashBytesNew, signedHashBytes);
                rsaProviderDecrypt.Dispose();
            }

            Console.WriteLine("Verify result: " + isOk);
            return isOk;
        }
        #endregion 

        public static void GetInfoFromX509(string path, string pwd)
        {
            X509Certificate2 x509 = new X509Certificate2(path, pwd);

            //Print to console information contained in the certificate.
            Console.WriteLine("{0}Subject: {1}{0}", Environment.NewLine, x509.Subject);
            Console.WriteLine("{0}Issuer: {1}{0}", Environment.NewLine, x509.Issuer);
            Console.WriteLine("{0}Version: {1}{0}", Environment.NewLine, x509.Version);
            Console.WriteLine("{0}Valid Date: {1}{0}", Environment.NewLine, x509.NotBefore);
            Console.WriteLine("{0}Expiry Date: {1}{0}", Environment.NewLine, x509.NotAfter);
            Console.WriteLine("{0}Thumbprint: {1}{0}", Environment.NewLine, x509.Thumbprint);
            Console.WriteLine("{0}Serial Number: {1}{0}", Environment.NewLine, x509.SerialNumber);
            Console.WriteLine("{0}Friendly Name: {1}{0}", Environment.NewLine, x509.PublicKey.Oid.FriendlyName);
            Console.WriteLine("{0}Public Key Format: {1}{0}", Environment.NewLine, x509.PublicKey.EncodedKeyValue.Format(true));
            Console.WriteLine("{0}Raw Data Length: {1}{0}", Environment.NewLine, x509.RawData.Length);
            Console.WriteLine("{0}Certificate to string: {1}{0}", Environment.NewLine, x509.ToString(true));

            Console.WriteLine("{0}Certificate to XML String: {1}{0}", Environment.NewLine, x509.PublicKey.Key.ToXmlString(false));
        }

        //https://stackoverflow.com/questions/7444586/how-can-i-sign-a-file-using-rsa-and-sha256-with-net
        public static void UseRSA256()
        {
            byte[] certificate = File.ReadAllBytes(@"G:\MyCode\CSharp\20170418\X509Encyption\public_privatekey.pfx");
            X509Certificate2 cert2 = new X509Certificate2(certificate, "abc123", X509KeyStorageFlags.Exportable);
            string stringToBeSigned = "This is a string to be signed";
            SHA256Managed shHash = new SHA256Managed();
            //byte[] computedHash = shHash.ComputeHash(Encoding.Default.GetBytes(stringToBeSigned));
            byte[] computedHash = shHash.ComputeHash(Encoding.UTF8.GetBytes(stringToBeSigned));

            var certifiedRSACryptoServiceProvider = cert2.PrivateKey as RSACryptoServiceProvider;
            RSACryptoServiceProvider defaultRSACryptoServiceProvider = new RSACryptoServiceProvider();
            defaultRSACryptoServiceProvider.ImportParameters(certifiedRSACryptoServiceProvider.ExportParameters(true));
            byte[] signedHashValue = defaultRSACryptoServiceProvider.SignData(computedHash, "SHA256");
            string signature = Convert.ToBase64String(signedHashValue);
            Console.WriteLine("Signature : {0}", signature);

            RSACryptoServiceProvider publicCertifiedRSACryptoServiceProvider = cert2.PublicKey.Key as RSACryptoServiceProvider;
            bool verify = publicCertifiedRSACryptoServiceProvider.VerifyData(computedHash, "SHA256", signedHashValue);
            Console.WriteLine("Verification result : {0}", verify);
        }


        //https://stackoverflow.com/questions/7444586/how-can-i-sign-a-file-using-rsa-and-sha256-with-net
        public static void UserRS1256Version2()
        {
            //X509KeyStorageFlags.Exportable -> this is the key , below we will use the new Created RSACrytoServiceProvider to sign the name.
            X509Certificate2 privateCert = new X509Certificate2(@"G:\MyCode\CSharp\20170418\X509Encyption\public_privatekey.pfx", "abc123", X509KeyStorageFlags.Exportable);

            // This instance can not sign and verify with SHA256:
            RSACryptoServiceProvider privateKey = (RSACryptoServiceProvider)privateCert.PrivateKey;

            // This one can:
            RSACryptoServiceProvider privateKey1 = new RSACryptoServiceProvider();
            privateKey1.ImportParameters(privateKey.ExportParameters(true));

            byte[] data = Encoding.UTF8.GetBytes("Data to be signed");

            byte[] signature = privateKey1.SignData(data, "SHA256");
            Console.WriteLine("Value: " + Convert.ToBase64String(signature));

            bool isValid = privateKey1.VerifyData(data, "SHA256", signature);
            Console.WriteLine("" + isValid);
        }


        #region sign with pkcs7
        //https://stackoverflow.com/questions/3576066/c-sharp-pkcs-signatures
        public static string Sign(string st)
        {
            X509Certificate2 certificate = new X509Certificate2(@"G:\MyCode\CSharp\20170418\X509Encyption\public_privatekey.pfx", "abc123");

            byte[] data = Encoding.UTF8.GetBytes(st);
            // setup the data to sign
            ContentInfo content = new ContentInfo(data);
            SignedCms signedCms = new SignedCms(content, false);
            CmsSigner signer = new CmsSigner(SubjectIdentifierType.IssuerAndSerialNumber, certificate);

            // create the signature
            signedCms.ComputeSignature(signer);
            string ret = Convert.ToBase64String(signedCms.Encode());
            Console.WriteLine(ret);
            return ret;
        }

        //https://blogs.msdn.microsoft.com/shawnfa/2006/02/27/enveloped-pkcs-7-signatures/
        public static bool Verify(string str,string txt)
        {
            byte[] signature=Convert.FromBase64String(str);
            X509Certificate2 certificate = new X509Certificate2(@"G:\MyCode\CSharp\20170418\X509Encyption\publickey.cer");

            ContentInfo content = new ContentInfo(Encoding.UTF8.GetBytes(txt));
            // decode the signature
            SignedCms verifyCms = new SignedCms(content,true);
            //SignedCms verifyCms = new SignedCms();
            verifyCms.Decode(signature);
            
            // verify it
            try
            {
                verifyCms.CheckSignature(new X509Certificate2Collection(certificate), false);
                //verifyCms.CheckSignature(new X509Certificate2Collection(certificate), true);
                Console.WriteLine(verifyCms.ContentInfo.Content);
                return true;
            }
            catch (CryptographicException)
            {
                return false;
            }
        }
        #endregion


        //Utf-8; sha256[SHA1,md5] -> algorithm
        public static string GetDecodeBase64String(string text, string hashal = "Sha256")
        {
            //SHA256Managed hashstring = new SHA256Managed();
            //byte[] bytes = Encoding.UTF8.GetBytes(text);
            //HashAlgorithm hashAlgorithm = HashAlgorithm.Create(hashal);
            //byte[] hashBytes = hashAlgorithm.ComputeHash(bytes);
            //return Convert.ToBase64String(hashBytes);

            byte[] bytes = Encoding.UTF8.GetBytes(text);
            return Convert.ToBase64String(bytes);
        }
    }
}
