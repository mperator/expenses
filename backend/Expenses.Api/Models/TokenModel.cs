using System;

namespace Expenses.Api.Models
{
    public class TokenModel
    {
        public string TokenType { get; set; }
        public string AccessToken { get; set; }
        public DateTime ExpiryDate { get; set; }
        
        public string RefreshToken { get; set; }
        public DateTime RefreshTokenExpires { get; set; }
    }
}