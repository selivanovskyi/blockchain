using BlockchainProject.model;
using Nethereum.Hex.HexConvertors;
using Nethereum.Hex.HexTypes;
using Nethereum.RPC.Eth.DTOs;
using Nethereum.Web3;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace BlockchainProject
{
    class BlockchainExplorer
    {
        
        private static Web3 web3 = new Web3();
        private static string abi = "[{\"constant\":false,\"inputs\":[{\"name\":\"spender\",\"type\":\"address\"},{\"name\":\"value\",\"type\":\"uint256\"}],\"name\":\"approve\",\"outputs\":[{\"name\":\"\",\"type\":\"bool\"}],\"payable\":false,\"type\":\"function\"},{\"constant\":true,\"inputs\":[],\"name\":\"totalSupply\",\"outputs\":[{\"name\":\"\",\"type\":\"uint256\"}],\"payable\":false,\"type\":\"function\"},{\"constant\":false,\"inputs\":[{\"name\":\"from\",\"type\":\"address\"},{\"name\":\"to\",\"type\":\"address\"},{\"name\":\"value\",\"type\":\"uint256\"}],\"name\":\"transferFrom\",\"outputs\":[{\"name\":\"\",\"type\":\"bool\"}],\"payable\":false,\"type\":\"function\"},{\"constant\":true,\"inputs\":[{\"name\":\"who\",\"type\":\"address\"}],\"name\":\"balanceOf\",\"outputs\":[{\"name\":\"\",\"type\":\"uint256\"}],\"payable\":false,\"type\":\"function\"},{\"constant\":false,\"inputs\":[{\"name\":\"to\",\"type\":\"address\"},{\"name\":\"value\",\"type\":\"uint256\"}],\"name\":\"transfer\",\"outputs\":[{\"name\":\"\",\"type\":\"bool\"}],\"payable\":false,\"type\":\"function\"},{\"constant\":false,\"inputs\":[{\"name\":\"spender\",\"type\":\"address\"},{\"name\":\"value\",\"type\":\"uint256\"},{\"name\":\"extraData\",\"type\":\"bytes\"}],\"name\":\"approveAndCall\",\"outputs\":[{\"name\":\"\",\"type\":\"bool\"}],\"payable\":false,\"type\":\"function\"},{\"constant\":true,\"inputs\":[{\"name\":\"owner\",\"type\":\"address\"},{\"name\":\"spender\",\"type\":\"address\"}],\"name\":\"allowance\",\"outputs\":[{\"name\":\"\",\"type\":\"uint256\"}],\"payable\":false,\"type\":\"function\"},{\"anonymous\":false,\"inputs\":[{\"indexed\":true,\"name\":\"owner\",\"type\":\"address\"},{\"indexed\":true,\"name\":\"spender\",\"type\":\"address\"},{\"indexed\":false,\"name\":\"value\",\"type\":\"uint256\"}],\"name\":\"Approval\",\"type\":\"event\"},{\"anonymous\":false,\"inputs\":[{\"indexed\":true,\"name\":\"from\",\"type\":\"address\"},{\"indexed\":true,\"name\":\"to\",\"type\":\"address\"},{\"indexed\":false,\"name\":\"value\",\"type\":\"uint256\"}],\"name\":\"Transfer\",\"type\":\"event\"}]\r\n";


        static Dictionary<string, string> listContract = new Dictionary<string, string>()
        {
            {"EOS","0x86fa049857e0209aa7d9e616f7eb3b3b78ecfdb0" },
            {"TRX","0xf230b790e05390fc8295f4d3f60332c93bed42e2" },
            {"BNB","0xB8c77482e45F1F44dE1745F52C74426C631bDD52" },
            {"VEN","0xd850942ef8811f2a866692a623011bde52a462c1" },
            {"OMG","0xd26114cd6EE289AccF82350c8d8487fedB8A0C07" },
            {"ICX","0xb5a5f22694352c15b00323844ad545abb2b11028" },
            {"WTC","0xb7cb1c96db6b22b0d3d9536e0108d062bd488f74" },
            {"ZIL","0x05f4a42e251f2d52b8ed15e9fedaacfcef1fad27" },
            {"AE","0x5ca9a71b1d01849c0a95490cc00559717fcf0d1d" },
            {"BTM", "0xcb97e65f07da24d46bcdd078ebebd7c6e6e3d750"}
        };

        static void GetTransact(string account, int startBlock, int endBlock)
        {
            for (int i = startBlock; i <= endBlock; i++)
            {
                TransactionReceipt var = new TransactionReceipt();
                var block = web3.Eth.Blocks.GetBlockWithTransactionsByNumber.SendRequestAsync(new HexBigInteger(i))
                    .Result;
                if (block != null && block.Transactions != null)
                {
                    block.Transactions.ToList().ForEach(x =>
                    {
                        if (string.Equals(x.From, account, StringComparison.CurrentCultureIgnoreCase))
                        {
                            Console.WriteLine($"TxHash - {x.TransactionHash} \n" +
                                              $"To - {x.To}\n" +
                                              $"From - {x.From}\n" +
                                              $"Value - {x.Value.Value}\n" +
                                              $"Input : \n {decodeInput(x.Input)}");
                        }
                    });
                }
            }
        }

        static string decodeInput(string input)
        {
            HexBigIntegerBigEndianConvertor a = new HexBigIntegerBigEndianConvertor();
            //cut off method name 
            input = input.Substring(10);
            //get value         
            var value = a.ConvertFromHex(input.Substring(input.Length / 2));
            // get address
            var address = a.ConvertToHex(a.ConvertFromHex("0x" + input.Substring(0, input.Length / 2)));

            return $"Value -  {value} \n" +
                   $"To - {address}";
        }




        /// <summary>
        /// get current balance eth
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        public static BigInteger BalanceETH(string account)
        {
            var currentBalance = web3.Eth.GetBalance.SendRequestAsync(account).Result;
            return currentBalance.Value;
        }


        /// <summary>
        /// get token erc20
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        /// 
        public static IList<Token> BalanceToken(string account)
        {
            List<Token> balanceToken = new List<Token>();
            foreach (var keyValuePair in listContract)
            {
                var cont = web3.Eth.GetContract(abi, keyValuePair.Value);
                var eth = cont.GetFunction("balanceOf");
                var balance = eth.CallAsync<System.Numerics.BigInteger>(account).Result;
                balanceToken.Add(new Token(keyValuePair.Key, balance));
            }
            return balanceToken;
        }



        //static void Main()
        //{

        //    var account = "0x3f5CE5FBFe3E9af3971dD833D26bA9b5C936f0bE";
        //    //get current balance


        //    //list with ERC20 tokens and their quantity
        //    foreach (var keyValuePair in listContract)
        //    {
        //        var cont = web3.Eth.GetContract(abi, keyValuePair.Value);
        //        var eth = cont.GetFunction("balanceOf");
        //        //var data = eth.CreateCallInput(account);
        //        var balance = eth.CallAsync<System.Numerics.BigInteger>(account).Result;
        //        Console.WriteLine($"Token name {keyValuePair.Key} and quantity is {balance}");
        //    }
        //    Console.WriteLine();
        //    //get token transactions between two blocks
        //    GetTransact(account, 5850910, 5850910);
        //    Console.ReadKey();
        //}







    }
}
