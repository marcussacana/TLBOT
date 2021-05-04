using System;
using System.Linq;
using System.Security.Cryptography;

namespace SocketCommon
{

	static public class CryptoHelper
	{

		public static byte[] SeedKey = new byte[] { 0xE8, 0x44, 0xED, 0xE2, 0xA6, 0x8B, 0x49, 0x26, 0xE9, 0x6C, 0x55, 0xC7, 0x56, 0x94, 0xEF, 0xDA, 0xD6, 0x87, 0x34, 0xB4, 0x4A, 0x9F, 0x3D, 0xA2, 0xAD, 0x51, 0x06, 0xDB, 0x63, 0x9A, 0x94, 0x60 };
		public static byte[] IV = new byte[16];

		static CryptoHelper()
		{
			new Random().NextBytes(IV);
		}

		public static byte[] Decrypt(this byte[] Data, byte[] Key, byte[] IV)
		{
			using (var aesAlg = Aes.Create())
			{
				aesAlg.Mode = CipherMode.CBC;
				aesAlg.Padding = PaddingMode.PKCS7;

				var decryptor = aesAlg.CreateDecryptor(Key, IV);
				return decryptor.TransformFinalBlock(Data, 0, Data.Length);
			}
		}

		public static byte[] Encrypt(this byte[] Data, byte[] Key, byte[] IV)
		{
			using (var aesAlg = Aes.Create())
			{
				aesAlg.Mode = CipherMode.CBC;
				aesAlg.Padding = PaddingMode.PKCS7;

				var encryptor = aesAlg.CreateEncryptor(Key, IV);
				var encrypted = encryptor.TransformFinalBlock(Data, 0, Data.Length);

				return encrypted.ToArray();
			}
		}
	}
}
