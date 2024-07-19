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
using VaraExt = Substrate.Vara.NET.NetApiExt.Generated;




public class SendMessage : MonoBehaviour
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

            // Creando el ProgramId (destination)
           // var destination = new ProgramId();
           // destination.Create("0x619701ff1f3e041069e70726679cef949cd86e826a20dcbe1bd9c8077c37c01d");
            var payload = new BaseVec<U8>(new U8[0]);
            var gas_limit = new U64(1000000);
            var value = new U128(10000000000000);
            var keep_alive = new Bool(true);

            // Llamando a GearCalls.SendMessage
           // var sendMessage = GearCalls.SendMessage(destination, payload, gas_limit, value, keep_alive);

            // Debug.Log($"Extrinsic submitted : {sendMessage}");
            //Console.WriteLine($"Transaction : {sendMessage}");


            // Enviar la transacción
            uint lifetime = 64; // Lifetime in blocks
            //Hash extrinsic = await _clientvara.Author.SubmitExtrinsicAsync(sendMessage, Alice, ChargeTransactionPayment.Default(), lifetime, CancellationToken.None);


            // Log the retrieved data to the debug console and the standard console
            //Debug.Log($"Extrinsic: {extrinsic}");
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