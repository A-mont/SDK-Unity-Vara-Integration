
using System;
using System.Net.WebSockets;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.Threading;
using Newtonsoft.Json.Linq;
using Serilog;
using SerilogTraceListener;
using StreamJsonRpc;
using Substrate.NetApi.Exceptions;
using Substrate.NetApi.Model.Extrinsics;
using Substrate.NetApi.Model.Meta;
using Substrate.NetApi.Model.Rpc;
using Substrate.NetApi.Model.Types;
using Substrate.NetApi.Model.Types.Base;
using Substrate.NetApi.Model.Types.Metadata;
using Substrate.NetApi.Model.Types.Primitive;
using Substrate.NetApi.Modules;
using Substrate.NetApi.TypeConverters;
using Substrate.NetApi.Model.Extrinsics;
using System;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using Substrate.NetApi.Model.Rpc;
using VaraExt = Substrate.Vara.NET.NetApiExt.Generated;
using PolkadotExt = Substrate.Vara.NET.NetApiExt.Generated;



public class ReadState : MonoBehaviour
{
    private VaraExt.SubstrateClientExt _clientvara;
    private string url;

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

            // Example of querying contract storage
            string programID = "..."; // Replace with your contract address
            string metadata = "..."; // Replace with your specific storage key in hex format
            string blockHash = null; // Use null for the latest state


           

        }
        else
        {
            // Log a message indicating that the client is not connected
            Debug.Log("Client is not connected.");
        }
    }


    public virtual async Task<T> GetContractStateAsync<T>(string parameters, string blockhash, CancellationToken token) where T : IType, new()
    {
        string text = await _clientvara.InvokeAsync<string>("gear_readState", new object[2] { parameters, blockhash }, token);
        if (text == null || text.Length == 0)
        {
            return default(T);
        }

        T result = new T();
        result.Create(text);
        return result;
    }


       

    void Update()
    {
        // You can add code here to be executed in each frame
    }
}