namespace TravelShare.Models.Expenses
{
    public class Payment
    {
        private Payment()
        {
        }

        public double Amount { get; private set; }
        public string Currency { get; private set; }
        public string CardNumber { get; private set; }
        public string Expiry { get; private set; }
        public string Cvv { get; private set; } 

        public class Builder
        {
            private Payment _payment = new();
            public Builder SetAmount(double amount)
            {
                _payment.Amount = amount;
                return this;
            }

            public Builder SetCurrency(string currency)
            {
                if (currency.Length == 3)
                    _payment.Currency = currency.ToUpper();
                return this;
            }

            public Builder SetCardNumber(string number)
            {
                if (number.Length == 16)
                    _payment.CardNumber = number.Replace(" ","");
                return this;
            }

            public Builder SetCvv(string number)
            {
                if (number.Length == 3)
                    _payment.Cvv = number;
                return this;
            }

            public Builder SetExpiry(string monthYear)
            {
                string[] details = monthYear.Split("/");
                bool succMonth = int.TryParse(details.First(), out int month);
                bool succYear = int.TryParse(details.Last(), out int year);
                if (!succMonth || !succYear)
                    return this;
                var currentDate = DateTime.UtcNow;

                if(currentDate.Month <= month && currentDate.Year <= year)
                    _payment.Expiry = monthYear;

                return this;
            }

            public Payment Build()
            {
                var buildPaymentCopy = _payment;
                _payment = new Payment();
                return buildPaymentCopy;
            }
        }
    }
}