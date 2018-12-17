using System;
namespace SyCoin.Core.Exceptions
{
    public class InputNotEnoughTokenException : ArgumentException
    {
        public InputNotEnoughTokenException() : base("The transaction input doesnt have enough token for output")
        {
        }
    }
}
