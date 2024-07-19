using UnityEngine;
using System;
using Schnorrkel.Keys;
using System.Threading;
using Substrate.NetApi;
using Substrate.NetApi.Model.Extrinsics;
using Substrate.NetApi.Model.Rpc;
using Substrate.NetApi.Model.Types;
using Substrate.NetApi.Model.Types.Base;
using Substrate.NetApi.Model.Types.Primitive;
using Substrate.Vara.NET.NetApiExt.Generated.Storage;
using Substrate.Vara.NET.NetApiExt.Generated.Model.gear_core.ids;
using Substrate.Vara.NET.NetApiExt.Generated.Model.pallet_balances;
using VaraExt = Substrate.Vara.NET.NetApiExt.Generated;
using Substrate.Vara.NET.NetApiExt.Generated.Model.sp_core.crypto;




public class TransferAllowDeath : MonoBehaviour
{
    private VaraExt.SubstrateClientExt _clientvara;
    private string url;

    public static MiniSecret MiniSecretBob
    {
        get
        {
            var bytes = Utils.HexToByteArray("0x397aee0b14f2f82b2ff0b99d901c1e7a76dc80d65ac1a5f9c7e222b04cb1e973");
            if (bytes == null || bytes.Length == 0)
            {
                throw new ArgumentException("HexToByteArray returned null or empty array");
            }
            return new MiniSecret(bytes, ExpandMode.Ed25519);
        }
    }

    public static Account Bob
    {
        get
        {
            var secret = MiniSecretBob.ExpandToSecret().ToBytes();
            var publicKey = MiniSecretBob.GetPair().Public.Key;

            if (secret == null || secret.Length == 0)
            {
                throw new ArgumentException("ExpandToSecret returned null or empty array");
            }
            if (publicKey == null || publicKey.Length == 0)
            {
                throw new ArgumentException("GetPair().Public.Key returned null or empty array");
            }

            return Account.Build(KeyType.Sr25519, secret, publicKey);
        }
    }

    public static MiniSecret MiniSecretAlice
    {
        get
        {
            var bytes = Utils.HexToByteArray("0x496f9222372eca011351630ad276c7d44768a593cecea73685299e06acef8c0a");
            if (bytes == null || bytes.Length == 0)
            {
                throw new ArgumentException("HexToByteArray returned null or empty array");
            }
            return new MiniSecret(bytes, ExpandMode.Ed25519);
        }
    }

    public static Account Alice
    {
        get
        {
            var secret = MiniSecretAlice.ExpandToSecret().ToBytes();
            var publicKey = MiniSecretAlice.GetPair().Public.Key;

            if (secret == null || secret.Length == 0)
            {
                throw new ArgumentException("ExpandToSecret returned null or empty array");
            }
            if (publicKey == null || publicKey.Length == 0)
            {
                throw new ArgumentException("GetPair().Public.Key returned null or empty array");
            }

            return Account.Build(KeyType.Sr25519, secret, publicKey);
        }
    }


    // Start is called before the first frame update
    async void Start()
    {
        // Assign the test node URL to the variable url
        url = "wss://testnet.vara.network";

        // Initialize the Substrate client with the node URL and the default transaction payment method
        _clientvara = new VaraExt.SubstrateClientExt(new Uri(url), ChargeTransactionPayment.Default());

        // Connect the client to the node asynchronously
        await _clientvara.ConnectAsync();

        // Check if the client is initialized and connected
        if (_clientvara != null && _clientvara.IsConnected)
        {
            // Log a message indicating that the client is connected
            Debug.Log("Client is connected.");



            string bobPublicKeyHex = "397aee0b14f2f82b2ff0b99d901c1e7a76dc80d65ac1a5f9c7e222b04cb1e973";
            byte[] bobPublicKey = Utils.HexToByteArray(bobPublicKeyHex);
            var account32 = new AccountId32();
            account32.Create(bobPublicKey);
            var multiAddress = new VaraExt.Model.sp_runtime.multiaddress.EnumMultiAddress();
            multiAddress.Create(VaraExt.Model.sp_runtime.multiaddress.MultiAddress.Id, account32);


            var amount = new BaseCom<U128>(10000000000000);

            // Balance Calls
            var transferAllowDeath = BalancesCalls.TransferAllowDeath(multiAddress, amount);

            Debug.Log($"Extrinsic submitted : {transferAllowDeath}");
            Console.WriteLine($"Transaction : {transferAllowDeath}");


            // Enviar la transacción
            uint lifetime = 64; // Lifetime in blocks

            Hash extrinsic = await _clientvara.Author.SubmitExtrinsicAsync(transferAllowDeath, Alice, ChargeTransactionPayment.Default(), lifetime, CancellationToken.None);


            // Log the retrieved data to the debug console and the standard console
            Debug.Log($"Extrinsic: {extrinsic}");
        }
        else
        {
            // Log a message indicating that the client is not connected
            Debug.Log("Client is not connected.");
        }
    }

    
  static byte[] HexToByteArray(string hex)
  {
    int NumberChars = hex.Length;
    byte[] bytes = new byte[NumberChars / 2];
    for (int i = 0; i < NumberChars; i += 2)
    {
      bytes[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16);
    }
    return bytes;
  }

    void Update()
    {
        // You can add code here to be executed in each frame
    }
}