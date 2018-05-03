namespace KitsorLab.PushServer.PNS.ApplePush.Safari
{
	using KitsorLab.PushServer.PNS.ApplePush.Utils;
	using Microsoft.Extensions.Options;
	using Newtonsoft.Json;
	using Org.BouncyCastle.Asn1;
	using Org.BouncyCastle.Asn1.Cms;
	using Org.BouncyCastle.Asn1.X509;
	using Org.BouncyCastle.Cms;
	using System.Collections.Generic;
	using System.IO;
	using System.IO.Compression;
	using System.Linq;
	using System.Security.Cryptography;
	using System.Security.Cryptography.X509Certificates;
	using System.Text;
	using System.Threading.Tasks;

	public class PushPackage
	{
		private readonly PushPackageOptions _settings;
		private static readonly string _manifestFilename = "manifest.json";
		private static readonly string _signatureFilename = "signature";
		private static readonly string _oidSHA256 = "2.16.840.1.101.3.4.2.1";
		private static readonly string _oidPKCS7IdData = "1.2.840.113549.1.7.1";
		private static readonly string _oidPKCS7IdSignedData = "1.2.840.113549.1.7.2";

		public PushPackage(IOptions<PushPackageOptions> options)
		{
			_settings = options?.Value ?? throw new System.ArgumentNullException(nameof(options));
		}

		public void GenerateManifestJson()
		{
			IList<string> files = new List<string>
			{
				"website.json",
				"icon.iconset/icon_16x16.png",
				"icon.iconset/icon_16x16@2x.png",
				"icon.iconset/icon_32x32.png",
				"icon.iconset/icon_32x32@2x.png",
				"icon.iconset/icon_128x128.png",
				"icon.iconset/icon_128x128@2x.png",
			};

			string manifestFilename = "manifest.json";
			IDictionary<string, ManifestFileInfo> manifest = new Dictionary<string, ManifestFileInfo>();
			SHA512 mySHA256 = SHA512.Create();

			foreach (string filename in files)
			{
				using (FileStream stream = File.OpenRead(Path.Combine(_settings.PackagePath, filename)))
				{
					stream.Position = 0;
					byte[] hash = mySHA256.ComputeHash(stream);
					manifest.Add(filename, new ManifestFileInfo("sha512", hash));
				}
			}

			string manifestLocation = Path.Combine(_settings.PackagePath, manifestFilename);
			if (File.Exists(manifestLocation))
			{
				File.Delete(manifestLocation);
			}

			File.WriteAllBytes(manifestLocation, Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(manifest, Formatting.Indented)));
		}

		public void CreateSign()
		{
			byte[] data = File.ReadAllBytes(Path.Combine(_settings.PackagePath, _manifestFilename));

			X509Certificate2 certificate = new X509Certificate2(_settings.CertificatePath, _settings.CertificatePassword, X509KeyStorageFlags.Exportable);
			X509Certificate2 intermCertificate = new X509Certificate2(_settings.IntermCertificatePath);

			var privKey = SecurityUtils.GetRsaKeyPair(certificate.GetRSAPrivateKey()).Private;
			var cert = SecurityUtils.FromX509Certificate(certificate);
			var interm = SecurityUtils.FromX509Certificate(intermCertificate);

			CmsProcessableByteArray content = new CmsProcessableByteArray(data);
			CmsSignedDataGenerator generator = new CmsSignedDataGenerator();
			generator.AddSigner(privKey, cert, CmsSignedGenerator.EncryptionRsa, CmsSignedGenerator.DigestSha256);

			CmsSignedData signedContent = generator.Generate(content, false);
			var si = signedContent.GetSignerInfos();
			var signer = si.GetSigners().Cast<SignerInformation>().First();

			SignerInfo signerInfo = signer.ToSignerInfo();

			Asn1EncodableVector digestAlgorithmsVector = new Asn1EncodableVector();
			digestAlgorithmsVector.Add(new AlgorithmIdentifier(
				algorithm: new DerObjectIdentifier(_oidSHA256),
				parameters: DerNull.Instance));

			// Construct SignedData.encapContentInfo
			ContentInfo encapContentInfo = new ContentInfo(
					contentType: new DerObjectIdentifier(_oidPKCS7IdData),
					content: null);

			Asn1EncodableVector certificatesVector = new Asn1EncodableVector();
			certificatesVector.Add(X509CertificateStructure.GetInstance(Asn1Object.FromByteArray(cert.GetEncoded())));
			certificatesVector.Add(X509CertificateStructure.GetInstance(Asn1Object.FromByteArray(interm.GetEncoded())));

			// Construct SignedData.signerInfos
			Asn1EncodableVector signerInfosVector = new Asn1EncodableVector();
			signerInfosVector.Add(signerInfo.ToAsn1Object());

			// Construct SignedData
			SignedData signedData = new SignedData(
					digestAlgorithms: new DerSet(digestAlgorithmsVector),
					contentInfo: encapContentInfo,
					certificates: new BerSet(certificatesVector),
					crls: null,
					signerInfos: new DerSet(signerInfosVector));

			ContentInfo contentInfo = new ContentInfo(
					contentType: new DerObjectIdentifier(_oidPKCS7IdSignedData),
					content: signedData);

			File.WriteAllBytes(Path.Combine(_settings.PackagePath, _signatureFilename), contentInfo.GetDerEncoded());
		}

		/// <returns></returns>
		public async Task<byte[]> CompressPackage()
		{
			using (MemoryStream stream = new MemoryStream())
			{
				using (ZipArchive zip = new ZipArchive(stream, ZipArchiveMode.Create, true))
				{
					await ZipDirectory(_settings.PackagePath, string.Empty, zip);
				}
				return stream.ToArray();
			}
		}

		/// <param name="path"></param>
		/// <param name="prefix"></param>
		/// <param name="zip"></param>
		/// <returns></returns>
		private async Task ZipDirectory(string path, string prefix, ZipArchive zip)
		{
			foreach (string filepath in Directory.GetFiles(path))
			{
				string filename = Path.GetFileName(filepath);
				string zipFilepath = string.IsNullOrEmpty(prefix) ? filename : $"{prefix}/{filename}";
				ZipArchiveEntry entry = zip.CreateEntry(zipFilepath, CompressionLevel.NoCompression);

				using (var zipStream = entry.Open())
				{
					byte[] content = File.ReadAllBytes(filepath);
					await zipStream.WriteAsync(content, 0, content.Length);
				}
			}

			foreach (string directory in Directory.GetDirectories(path))
			{
				await ZipDirectory(directory, Path.Combine(prefix, Path.GetFileName(directory)), zip);
			}
		}

		private class ManifestFileInfo
		{
			[JsonProperty("hashType")]
			public string HashType { get; set; }
			[JsonProperty("hashValue")]
			public string HashValue { get; set; }

			public ManifestFileInfo(string hashType, byte[] hashValue)
			{
				HashType = hashType;
				HashValue = HashByteToHex(hashValue);
			}

			public string HashByteToHex(byte[] hashByte)
			{
				return string.Join("", hashByte.ToList().Select(x => string.Format("{0:x2}", x)));
			}
		}
	}
}
