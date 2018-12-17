using System;
using SyCoin.Models;

namespace SyCoin.Core.Exceptions
{
    public class NonUTXOException : ArgumentException
    {
        public NonUTXOException(TransactionInput nonUTXOInput)
        : base($"Output index {nonUTXOInput.PrevOutputIndex} from transaction {nonUTXOInput.TransactionHash} is not UTXO")
        { }
    }
}
