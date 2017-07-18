﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test
{
    class Program
    {
        static void PrintType<T>(T first,T second)
        {
            Console.WriteLine(typeof(T));
        }
        static void Main(string[] args)
        {
           // UseGeneric();

            //Signature();

            //PrintX509();

            VerifyWith256();

            
           Console.ReadLine();
        }

        private static void VerifyWith256()
        {
            X509Encyption.X509Encryptioner.UseRSA256();
        }

        private static void PrintX509()
        {
            string pubKeyFile = @"G:\MyCode\CSharp\20170418\X509Encyption\publickey.cer";
            string pwd = "";
            //string pubKeyFile = @"G:\MyCode\CSharp\20170418\X509Encyption\public_privatekey.pfx";
            //string pwd = "abc123";
            X509Encyption.X509Encryptioner.GetInfoFromX509(pubKeyFile, pwd);
        }

        private static void Signature()
        {
            string signedstr = X509Encyption.X509Encryptioner.Signature();
            Console.WriteLine(signedstr + Environment.NewLine);
            X509Encyption.X509Encryptioner.VerifySignature(signedstr);
        }

        private static void UseGeneric()
        {
            PrintType(1, new object());
            PrintType<object>(1, new object());

            PrintType(1, 2);
            PrintType<int>(1, 2);
        }

        
       

        
       
    }   
}
