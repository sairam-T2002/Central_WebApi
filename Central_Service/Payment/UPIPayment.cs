namespace Central_Service.Payment
{
    public class UPIPayment: IPaymentDTO
    {
        public string Upi_Id {  get; set; }
        public string Account_Number {  get; set; }
        public string Bank_Name {  get; set; }
    }
}
