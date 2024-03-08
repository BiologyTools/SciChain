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
namespace SciChain
{
    public class Block
    {
        public int Index { get; set; } // Position of the block on the chain
        public DateTime TimeStamp { get; set; } // When the block was created
        public string PreviousHash { get; set; } // The hash of the previous block
        public string Data { get; set; } // Your data (e.g., transaction details)
        public string Hash { get; set; } // The block's hash

        public Block(DateTime timeStamp, string previousHash, string data)
        {
            Index = 0;
            TimeStamp = timeStamp;
            PreviousHash = previousHash;
            Data = data;
            Hash = CalculateHash();
        }

        public string CalculateHash()
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                string rawData = $"{TimeStamp}-{PreviousHash ?? ""}-{Data}";
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(rawData));
                return BitConverter.ToString(bytes).Replace("-", "").ToLowerInvariant();
            }
        }
    }
    public class Blockchain
    {
        public IList<Block> Chain { set; get; }

        public Blockchain()
        {
            InitializeChain();
            AddGenesisBlock();
        }

        private void InitializeChain()
        {
            Chain = new List<Block>();
        }

        private void AddGenesisBlock()
        {
            Chain.Add(CreateGenesisBlock());
        }

        private Block CreateGenesisBlock()
        {
            return new Block(DateTime.Now, null, "{}");
        }

        public Block GetLatestBlock()
        {
            return Chain[Chain.Count - 1];
        }

        public void AddBlock(Block block)
        {
            Block latestBlock = GetLatestBlock();
            block.Index = latestBlock.Index + 1;
            block.PreviousHash = latestBlock.Hash;
            block.Hash = block.CalculateHash();
            Chain.Add(block);
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
    }
    public class OrcidAuthentication
    {
        private readonly string clientId;
        private readonly string clientSecret;
        private readonly HttpClient httpClient;

        public OrcidAuthentication(string clientId, string clientSecret)
        {
            this.clientId = clientId;
            this.clientSecret = clientSecret;
            httpClient = new HttpClient();
        }

        public async Task<bool> AuthenticateOrcidId(string orcidId, string accessToken)
        {
            // URL for ORCID public API; adjust as needed for your use case
                var url = $"https://pub.orcid.org/v3.0/{orcidId}/person";

            // Add the access token in the Authorization header
            httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);

            try
            {
                var response = await httpClient.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    // Assuming you have a simple way to validate the content. In real scenarios, you would parse this content,
                    // and validate the ORCID iD matches the expected user details.
                    return true; // Simplified for illustration. Actual implementation should validate the response content.
                }
            }
            catch (HttpRequestException e)
            {
                // Handle exceptions or errors in communication with the ORCID API
                Console.WriteLine($"Error contacting ORCID API: {e.Message}");
            }

            return false;
        }
    }
}
