using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.Security.Policy;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Net;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text.Json.Serialization;
using static SciChain.Block;
using System.Net.Sockets;
namespace SciChain
{
    public class Block
    {
        public class Transaction
        {
            public enum Type
            {
                transaction,
                blockreward,
                registration,
            }
            public string FromAddress { get; set; }
            public string ToAddress { get; set; }
            public string PublicKey { get; set; }
            public decimal Amount { get; set; }
            public string Signature { get; set; }
            public Type TransactionType { get; set; }
            public Transaction(Type t, string fromAddress, string toAddress, decimal amount)
            {
                FromAddress = fromAddress;
                ToAddress = toAddress;
                Amount = amount;
            }
            public void SignTransaction(RSAParameters privateKey, string fromAddress)
            {
                var dataToSign = fromAddress + ToAddress + Amount.ToString();
                using (var rsa = new RSACryptoServiceProvider())
                {
                    rsa.ImportParameters(privateKey);
                    var dataToSignBytes = Encoding.UTF8.GetBytes(dataToSign);
                    var hasher = new SHA256Managed();
                    var hashedData = hasher.ComputeHash(dataToSignBytes);
                    Signature = Convert.ToBase64String(rsa.SignData(hashedData, CryptoConfig.MapNameToOID("SHA256")));
                }
            }
        }
        public int Index { get; set; } // Position of the block on the chain
        public DateTime TimeStamp { get; set; } // When the block was created
        public string PreviousHash { get; set; } // The hash of the previous block
        public IList<Transaction> Transactions { get; set; }
        public string Hash { get; set; } // The block's hash

        public Block(DateTime timeStamp, string previousHash, IList<Transaction> transactions)
        {
            Index = 0;
            TimeStamp = timeStamp;
            PreviousHash = previousHash;
            Transactions = transactions;
            Hash = CalculateHash();
        }

        public string CalculateHash()
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                string rawData = $"{TimeStamp}-{PreviousHash ?? ""}-{JsonConvert.SerializeObject(Transactions)}";
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(rawData));
                return BitConverter.ToString(bytes).Replace("-", "").ToLowerInvariant();
            }
        }
    }
    public class Blockchain
    {
        public int currentHeight = 0;
        public List<Transaction> PendingTransactions = new List<Transaction>();
        public IList<Block> Chain { set; get; }
        //Total supply is calculated to be one coin per person.
        public const decimal totalSupply = 9900000000 + treasury;
        public const decimal treasury = 100000000;
        //We will use 10 billion people as our population
        public const long population = 10000000000;
        //With the treasury we can give each user 0.01 coins.
        public const decimal gift = 0.01M;
        public decimal currentSupply = 0;
        public const decimal miningReward = 10;
        public const int port = 1234;
        public Blockchain()
        {
            InitializeChain();
            AddGenesisBlock();
        }
        string dir = System.IO.Path.GetDirectoryName(Environment.ProcessPath);
        public List<Peer> Peers { set; get; } = new List<Peer>();

        private void InitializeChain()
        {
            Settings.Load();
            string h = Settings.GetSettings("Height");
            if(h!="")
            currentHeight = int.Parse(h);
            Chain = new List<Block>();
            Directory.CreateDirectory(dir + "/Blocks");
        }

        public void AddGenesisBlock()
        {
            Chain.Add(CreateGenesisBlock());
        }

        private Block CreateGenesisBlock()
        {
            return new Block(DateTime.Now, null, null);
        }

        public Block GetLatestBlock()
        {
            return Chain[Chain.Count - 1];
        }

        public decimal GetBalance(string address)
        {
            decimal balance = 0;

            foreach (var block in Chain)
            {
                foreach (var trans in block.Transactions)
                {
                    if (trans.FromAddress == address)
                    {
                        balance -= trans.Amount;
                    }

                    if (trans.ToAddress == address)
                    {
                        balance += trans.Amount;
                    }
                }
            }

            return balance;
        }

        public void AddBlock(Block block)
        {
            Block latestBlock = GetLatestBlock();
            block.Index = latestBlock.Index + 1;
            block.PreviousHash = latestBlock.Hash;
            block.Hash = block.CalculateHash();
            Chain.Add(block);
            BroadcastNewBlock(block);
        }

        public bool ProcessTransaction(Transaction transaction)
        {
            if (!VerifyTransaction(transaction))
            {
                Console.WriteLine("Transaction failed: Not a valid transaction.");
                return false;
            }

            if (transaction.TransactionType == Transaction.Type.transaction)
            {
                // Proceed with adding the transaction
                if (transaction.FromAddress != null)
                {
                    var senderBalance = GetBalance(transaction.FromAddress);

                    if (senderBalance < transaction.Amount)
                    {
                        Console.WriteLine("Transaction failed: Not enough balance.");
                        return false;
                    }
                }
            }
            else if (transaction.TransactionType == Transaction.Type.registration)
            {
                //We will register this user and associate their ORCID ID with their public key (RSAParameters) 
                //If this is a new user we will send them the gift transaction from the treasury.

            }
            PendingTransactions.Add(transaction);
            return true;
        }
        public bool VerifyTransaction(Transaction transaction)
        {
            using (var rsa = new RSACryptoServiceProvider())
            {
                try
                {
                    rsa.ImportParameters(RSA.StringToRSAParameters(transaction.PublicKey));
                    var dataToVerify = transaction.FromAddress + transaction.ToAddress + transaction.Amount.ToString();
                    var dataToVerifyBytes = Encoding.UTF8.GetBytes(dataToVerify);
                    var hasher = new SHA256Managed();
                    var hashedData = hasher.ComputeHash(dataToVerifyBytes);
                    return rsa.VerifyData(hashedData, CryptoConfig.MapNameToOID("SHA256"), Convert.FromBase64String(transaction.Signature));
                }
                catch
                {
                    return false;
                }
            }
        }

        #region Server
        public class Peer
        {
            public TcpClient Client { get; set; }
            public string Address { get; set; }
            public int Port { get; set; }

            public Peer(TcpClient client, string address, int port)
            {
                Client = client;
                Address = address;
                Port = port;
            }
        }
        public class Node
        {
            public string Address { get; set; } // Could be IP address, domain name, etc.
            public int Port { get; set; }

            // Additional node-specific information (e.g., public key)
        }

        public class Message
        {
            public string Type { get; set; } // E.g., "NewBlock", "NewTransaction", etc.
            public string Content { get; set; } // The actual message content, likely serialized data
        }

        public void StartServer()
        {
            TcpListener server = new TcpListener(IPAddress.Any, port);

            server.Start();
            Console.WriteLine("Server has started on 127.0.0.1:" + port + ".");

            while (true)
            {
                TcpClient client = server.AcceptTcpClient();

                // Handle the client in a new thread
                Thread clientThread = new Thread(new ParameterizedThreadStart(HandleClient));
                clientThread.Start(client);
            }
        }

        public void HandleClient(object clientObj)
        {
            var client = clientObj as TcpClient;
            if (client == null) return;

            try
            {
                var stream = client.GetStream();
                var reader = new StreamReader(stream, Encoding.UTF8);

                // Continuously listen for messages
                while (true)
                {
                    var messageString = reader.ReadLine();
                    if (messageString == null) break; // Client has disconnected

                    // Deserialize the message
                    var message = JsonConvert.DeserializeObject<Message>(messageString);
                    if (message == null) continue;

                    // Process the message based on its type
                    ProcessMessage(message);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                // Handle error (e.g., logging, cleanup, etc.)
            }
            finally
            {
                client.Close();
            }
        }

        private void ProcessMessage(Message message)
        {
            // Implement message processing logic here
            // For example, handling "NewBlock" messages:
            if (message.Type == "NewBlock")
            {
                var block = JsonConvert.DeserializeObject<Block>(message.Content);
                AddBlock(block);
            }
            // Handle other message types as necessary
        }

        public void ConnectToPeer(string address)
        {
            try
            {
                TcpClient client = new TcpClient(address, port);
                Peers.Add(new Peer(client, address, port));
            }
            catch (Exception e)
            {
                Console.WriteLine("Error connecting to peer: " + e.Message);
            }
        }
        public void BroadcastNewBlock(Block block)
        {
            var message = new Message
            {
                Type = "NewBlock",
                Content = JsonConvert.SerializeObject(block)
            };

            var messageString = JsonConvert.SerializeObject(message);
            byte[] messageBytes = Encoding.ASCII.GetBytes(messageString);

            foreach (var peer in Peers)
            {
                try
                {
                    NetworkStream stream = peer.Client.GetStream();
                    stream.Write(messageBytes, 0, messageBytes.Length);
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Error sending message to peer {peer.Address}:{peer.Port} - {e.Message}");
                    // Handle error (e.g., remove peer from list)
                }
            }
        }

        #endregion

        public void MineBlock(string minerAddress)
        {
            List<Transaction> transactions = new List<Transaction>();
            foreach (var transaction in PendingTransactions)
            {
                if(VerifyTransaction(transaction))
                    transactions.Add(transaction);
            }
            var block = new Block(DateTime.Now, GetLatestBlock().Hash, transactions);
            AddBlock(block);
            // Reset the pending transactions and send mining reward
            PendingTransactions = new List<Transaction>();
            ProcessTransaction(new Transaction(Transaction.Type.blockreward,null, minerAddress, miningReward));
            if(currentSupply < totalSupply)
            currentSupply += miningReward;
        }

        public class Wallet
        {
            public RSAParameters PublicKey { get; private set; }
            public RSAParameters PrivateKey { get; private set; }

            public Wallet()
            {
                using (var rsa = new RSACryptoServiceProvider(2048)) // 2048-bit key size
                {
                    PublicKey = rsa.ExportParameters(false); // Export the public key
                    PrivateKey = rsa.ExportParameters(true); // Export the private key
                }
            }
        }

        public bool IsValid()
        {
            for (int i = 1; i < Chain.Count; i++)
            {
                Block currentBlock = Chain[i];
                Block previousBlock = Chain[i - 1];

                if (currentBlock.Hash != currentBlock.CalculateHash())
                {
                    return false;
                }

                if (currentBlock.PreviousHash != previousBlock.Hash)
                {
                    return false;
                }
            }
            return true;
        }

        public void Save()
        {
            Settings.AddSettings("Height",currentHeight.ToString());
            Settings.Save();
            foreach (var block in Chain)
            {
                string f = dir + "/Blocks/" + block.Index + ".json";
                if(!File.Exists(f))
                {
                    File.WriteAllText(f, JsonConvert.SerializeObject(block));
                }
            }
        }

        public void Load()
        {
            foreach (var f in Directory.GetFiles(dir + "/Blocks/"))
            {
                var json = File.ReadAllText(f);
                Block b = JsonConvert.DeserializeObject<Block>(json);
                Chain.Add(b);
            }
        }

    }

    public static class RSA
    {
        //Note we only store the public key parts of the RSA Parameters on purpose.
        public static string RSAParametersToString(RSAParameters parameters)
        {
            // RSAParameters contains fields that are byte arrays and cannot be directly serialized by JsonConvert,
            // so we create a serializable object to hold the data.
            var paramsToSerialize = new
            {
                Modulus = parameters.Modulus != null ? Convert.ToBase64String(parameters.Modulus) : null,
                Exponent = parameters.Exponent != null ? Convert.ToBase64String(parameters.Exponent) : null,
                //P = parameters.P != null ? Convert.ToBase64String(parameters.P) : null,
                //Q = parameters.Q != null ? Convert.ToBase64String(parameters.Q) : null,
                //DP = parameters.DP != null ? Convert.ToBase64String(parameters.DP) : null,
                //DQ = parameters.DQ != null ? Convert.ToBase64String(parameters.DQ) : null,
                //InverseQ = parameters.InverseQ != null ? Convert.ToBase64String(parameters.InverseQ) : null,
                //D = parameters.D != null ? Convert.ToBase64String(parameters.D) : null
            };

            return JsonConvert.SerializeObject(paramsToSerialize);
        }
        public static RSAParameters StringToRSAParameters(string jsonString)
        {
            var paramsFromJson = JsonConvert.DeserializeObject<dynamic>(jsonString);

            return new RSAParameters
            {
                Modulus = paramsFromJson.Modulus != null ? Convert.FromBase64String(paramsFromJson.Modulus.ToString()) : null,
                Exponent = paramsFromJson.Exponent != null ? Convert.FromBase64String(paramsFromJson.Exponent.ToString()) : null,
                //P = paramsFromJson.P != null ? Convert.FromBase64String(paramsFromJson.P.ToString()) : null,
                //Q = paramsFromJson.Q != null ? Convert.FromBase64String(paramsFromJson.Q.ToString()) : null,
                //DP = paramsFromJson.DP != null ? Convert.FromBase64String(paramsFromJson.DP.ToString()) : null,
                //DQ = paramsFromJson.DQ != null ? Convert.FromBase64String(paramsFromJson.DQ.ToString()) : null,
                //InverseQ = paramsFromJson.InverseQ != null ? Convert.FromBase64String(paramsFromJson.InverseQ.ToString()) : null,
                //D = paramsFromJson.D != null ? Convert.FromBase64String(paramsFromJson.D.ToString()) : null
            };
        }
    }

    public static class OrcidAuthentication
    {
        private static readonly string clientId = "APP-PCZPI3V579SL36TV";
        private static readonly string clientSecret = "6d00747a-ae0f-4567-bade-5bfa359bc75a";
        private static readonly HttpClient httpClient = new HttpClient();
        public static async Task<OAuthTokenResponse> GetAccessToken(string authorizationCode)
        {
            var httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri("https://orcid.org/");
            var tokenEndpoint = "https://orcid.org/oauth/token";
            var postData = new Dictionary<string, string>
            {
                { "client_id", clientId },
                { "client_secret", clientSecret },
                { "grant_type", "authorization_code" },
                { "redirect_uri", "http://127.0.0.1:8000" },
                { "code", authorizationCode }
            };

            var content = new FormUrlEncodedContent(postData);
            var response = await httpClient.PostAsync(tokenEndpoint, content);
            var responseString = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                var tokenResponse = JsonConvert.DeserializeObject<OAuthTokenResponse>(responseString);
                return tokenResponse; // This is the access token you'll use in subsequent requests
            }

            throw new Exception("Failed to obtain access token.");
        }

        // Assume OAuthTokenResponse is a class that matches the JSON structure of ORCID's response
        public class OAuthTokenResponse
        {
            [JsonProperty("access_token")]
            public string AccessToken { get; set; }
            [JsonProperty("bearer")]
            public string Bearer { get; set; }
            [JsonProperty("refresh_token")]
            public string RefreshToken { get; set; }
            [JsonProperty("expires_in")]
            public string Expiry { get; set; }
            [JsonProperty("scope")]
            public string Scope { get; set; }
            [JsonProperty("name")]
            public string Name { get; set; }
            [JsonProperty("orcid")]
            public string ORCID { get; set; }
            // Include other fields as necessary
        }
    }
    public static class OAuthHelper
    {
        private static HttpListener httpListener;
        private static string authorizationCode;

        public static async Task<string> StartListenerAsync()
        {
            string redirectUri = "http://127.0.0.1:8000/";
            httpListener = new HttpListener();
            httpListener.Prefixes.Add(redirectUri);
            httpListener.Start();

            // Open the user's browser and direct them to the authorization URL
            string authorizationUrl = $"https://orcid.org/oauth/authorize?client_id=APP-PCZPI3V579SL36TV&response_type=code&scope=/authenticate&redirect_uri=http://127.0.0.1:8000";
            OpenUrl(authorizationUrl);
            // Wait for the authorization response
            var context = httpListener.GetContext();
            var request = context.Request;

            // Extract the authorization code from the request
            string responseString = request.QueryString["code"];
            authorizationCode = responseString; // This is the authorization code

            // You can now send an HTTP response to the browser to close the window or display a message
            var response = context.Response;
            string responseHtml = "<html><head><meta http-equiv='Content-Type' content='text/html; charset=utf-8'></head><body>Please return to the application.</body></html>";
            byte[] buffer = System.Text.Encoding.UTF8.GetBytes(responseHtml);
            response.ContentLength64 = buffer.Length;
            var responseOutput = response.OutputStream;
            Task responseTask = responseOutput.WriteAsync(buffer, 0, buffer.Length).ContinueWith((task) =>
            {
                responseOutput.Close();
                httpListener.Stop();
            });

            return authorizationCode;
        }
        private static void OpenUrl(string url)
        {
            try
            {
                Process.Start(url);
            }
            catch
            {
                // hack because of this: https://github.com/dotnet/corefx/issues/10361
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
                    Process.Start(new ProcessStartInfo(url) { UseShellExecute = true });
                }
                else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                {
                    Process.Start("xdg-open", url);
                }
                else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                {
                    Process.Start("open", url);
                }
                else
                {
                    throw;
                }
            }
        }
    }
}
