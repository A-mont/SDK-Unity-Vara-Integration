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

using Unity.Collections.LowLevel.Unsafe;
using Substrate.Vara.NET.NetApiExt.Generated.Model.sp_core.crypto;
using UnityEditor;
using Substrate.Vara.NET.NetApiExt.Generated.Model.frame_support.traits.tokens.misc;

public class SendExtrinsic : MonoBehaviour
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
    // URL del nodo de Substrate
    string url = "wss://testnet.vara.network";

    // Initialize the Substrate client with the node URL and the default transaction payment method
    _clientvara = new VaraExt.SubstrateClientExt(new Uri(url), ChargeTransactionPayment.Default());

    // Conectar al nodo
    await _clientvara.ConnectAsync();

    // Crear una cuenta a partir de Alice
    Account accountAlice = Alice;
    if (accountAlice == null)
    {
      Debug.LogError("Failed to create account for Alice.");
      return;
    }

    Debug.Log($"Alice Address: {accountAlice}");
    Console.WriteLine($"Alice Address: {accountAlice}");

    Account bobAccount = Bob;
    Debug.Log($"Bob Address: {bobAccount}");
    Console.WriteLine($"Bob Address: {bobAccount}");

  
    string bobPublicKeyHex = "397aee0b14f2f82b2ff0b99d901c1e7a76dc80d65ac1a5f9c7e222b04cb1e973";
    byte[] bobPublicKey = HexToByteArray(bobPublicKeyHex);
    var account32 = new AccountId32();
    account32.Create(bobPublicKey);
    var multiAddress = new VaraExt.Model.sp_runtime.multiaddress.EnumMultiAddress();
    multiAddress.Create(VaraExt.Model.sp_runtime.multiaddress.MultiAddress.Id, account32);

    // Mostrar la salida
    // Imprimir la clave pública de Bob
    Debug.Log($"Soy Bob: {account32}");
    Console.WriteLine($"Soy Bob: {account32}");
    Debug.Log($"MultiAddress: {multiAddress}");
    Debug.Log($"MultiAddress Bytes: {BitConverter.ToString(multiAddress.Bytes)}");
    Debug.Log($"MultiAddress Type Size: {multiAddress.TypeSize}");
    Console.WriteLine($"MultiAddress: {multiAddress}");
    Console.WriteLine($"MultiAddress Bytes: {BitConverter.ToString(multiAddress.Bytes)}");
    Console.WriteLine($"MultiAddress Type Size: {multiAddress.TypeSize}");

    var amount = new BaseCom<U128>(10000000000000);
    Debug.Log($"Soy bob: {multiAddress}");
    Console.WriteLine($"sendMessageMethod: {multiAddress}");

    Bool keepAlive = new Bool(true);

    //string ExecuteInherentParams = GearStorage.ExecuteInherentParams();
     // Crear una instancia de GearStorage
    //GearStorage gearStorage = new GearStorage(_clientvara);


   
    // Balance Calls
    var transferKeepAlive = BalancesCalls.TransferKeepAlive(multiAddress, amount);
    var transfer = BalancesCalls.Transfer(multiAddress, amount);
    var transferAllowDeath = BalancesCalls.TransferAllowDeath(multiAddress, amount);
    var transferAll = BalancesCalls.TransferAll(multiAddress, keepAlive);
     // Verificar saldo
    AccountId32 mybalance = new AccountId32();
    mybalance.Create("0xe4fa3b466792dcd7e58f5d8d49bc4631b5eec3a9ebe48ffe79f859dadf76cb71");
   //var balance = await _clientvara.BalancesStorage.Account(mybalance,"0x0195e429a647919909d9a1ba3944a5f4a773fd3c1eca1266e9ef313c6bec0c9d",CancellationToken.None);
    // var accountParams = BalancesStorage.AccountParams(mybalance)
    
     //Debug.Log($"ExecuteInherentParams: { ExecuteInherentParams}");
    //Console.WriteLine($"ExecuteInherentParams {  ExecuteInherentParams}");




    var account = BalancesStorage.AccountDefault();
    var blocknumberDefault = GearStorage.BlockNumberDefault();
    var MemoryPageStorageDefault = GearProgramStorage.MemoryPageStorageDefault();

    Debug.Log($"MemoryPageStorageDefault(): {MemoryPageStorageDefault}");
    Console.WriteLine($"MemoryPageStorageDefault {MemoryPageStorageDefault}");

    try
    {
      // Validación de parámetros
      if (accountAlice == null) throw new ArgumentNullException(nameof(accountAlice), "accountAlice cannot be null");
      if (transferKeepAlive == null) throw new ArgumentNullException(nameof(transferKeepAlive), "transferKeepAlive cannot be null");

      // Imprimir las variables en la consola
      Debug.Log($"accountAlice: {accountAlice}");
      Debug.Log($"transferKeepAlive: {transferKeepAlive}");
      Console.WriteLine($"accountAlice: {accountAlice}");
      Console.WriteLine($"transferKeepAlive: {transferKeepAlive}");

       // Creando el ProgramId (destination)
      //var destination = new ProgramId(); // Ejemplo de un ID de programa
      //destination.Create("0x619701ff1f3e041069e70726679cef949cd86e826a20dcbe1bd9c8077c37c01d");
      //var payload = new BaseVec<U8>(new U8[0]);
      //var gas_limit = new U64(1000000);
      //var value = new U128(10000000000000); // 1 token, dependiendo de la denominación de tu red
                                                 // Creando el keep_alive (Bool)
      var keep_alive = new Bool(true);
      // Llamando a GearCalls.SendMessage
      // var sendMessage = GearCalls.SendMessage(destination, payload, gas_limit, value, keep_alive);

      // Debug.Log($"Transaction submitted : {sendMessage}");
      // Console.WriteLine($"Transaction : {sendMessage}");



      // Enviar la transacción
      uint lifetime = 64; // Lifetime in blocks
      Hash extrinsic = await _clientvara.Author.SubmitExtrinsicAsync(transferKeepAlive, accountAlice, ChargeTransactionPayment.Default(), lifetime, CancellationToken.None);

      //string extrinsicAndSubscription = await _clientvara.Author.SubmitAndWatchExtrinsicAsync(callback, transferKeepAlive, aliceAccount, ChargeTransactionPayment.Default(), nonce, CancellationToken.None);
     
      Debug.Log($"Transaction submitted successfully. Block hash: {extrinsic}");
      Console.WriteLine($"Transaction submitted successfully. Block hash: {extrinsic}");
    }
    catch (Exception e)
    {
      Debug.LogError($"Error: {e.Message}");
      Console.WriteLine($"Error: {e.Message}");
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

  // Update is called once per frame
  void Update()
  {
  }
}
