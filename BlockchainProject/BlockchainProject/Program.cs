using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Nethereum;
using Nethereum.ABI;
using Nethereum.ABI.Decoders;
using Nethereum.ABI.Model;
using Nethereum.Hex.HexConvertors;
using Nethereum.Hex.HexTypes;
using Nethereum.RPC.Eth.DTOs;
using Nethereum.RPC.TransactionManagers;
using Nethereum.StandardTokenEIP20;
using Nethereum.RPC.Web3;
using Nethereum.Util;
using Nethereum.Web3;
using Nethereum.Web3.Accounts.Managed;

namespace BlockchainProject
{
    class Program
    {
        static string account = "0x3f5CE5FBFe3E9af3971dD833D26bA9b5C936f0bE";

        static void Main(string[] args)
        {
            Console.WriteLine($"Account balance - {BlockchainExplorer.BalanceETH(account)}");
            Console.WriteLine();
            Console.WriteLine("Token balances : ");
            var token = BlockchainExplorer.BalanceToken(account);
            foreach (var tn in token)
            {
                Console.WriteLine("name:" + tn.Name + ",  balance: " + tn.Balance);
            }

            var s =
                "0xa9059cbb00000000000000000000000067fa2c06c9c6d4332f330e14a66bdf1873ef3d2b0000000000000000000000000000000000000000000000000de0b6b3a7640000"
                    .Length;
            Console.WriteLine();
            Console.WriteLine("Transactions");
            var transacts = BlockchainExplorer.GetTransactons(account, 40);
            //get token transfers
            var trans = BlockchainExplorer.GetTokenTransfersForAcc(account, 100);
            Console.ReadLine();
        }
    }
}