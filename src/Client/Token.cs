namespace AlicesWebsite.Client
{
    public class Token
    {
        public Token() : this (string.Empty, DateTime.MinValue)
        {
        }

        public Token(string value, DateTime expiry)
        {
            Value = value;
            Expiry = expiry;
        }

        public string Value { get; set; }

        public DateTime Expiry { get; set; }

        public bool IsExpired => Expiry < DateTime.UtcNow;

    }
}
