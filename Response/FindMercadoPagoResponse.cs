namespace LojaVirtual.Response;

// Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class AdditionalInfo
    {
        public object authentication_code { get; set; }
        public object? available_balance { get; set; }
        public string ip_address { get; set; }
        public List<Item> items { get; set; }
        public object? nsu_processadora { get; set; }
    }

    public class ApplicationData
    {
        public string name { get; set; }
        public string version { get; set; }
    }

    public class BankInfo
    {
        public Collector collector { get; set; }
        public object is_same_bank_account_owner { get; set; }
        public object origin_bank_id { get; set; }
        public object origin_wallet_id { get; set; }
        public Payer payer { get; set; }
    }

    public class BusinessInfo
    {
        public string sub_unit { get; set; }
        public string unit { get; set; }
    }

    public class Card
    {
    }

    public class Collector
    {
        public string account_holder_name { get; set; }
        public object account_id { get; set; }
        public object long_name { get; set; }
        public object transfer_account_id { get; set; }
    }

    public class Identification
    {
        public object number { get; set; }
        public object type { get; set; }
    }

    public class InfringementNotification
    {
        public object status { get; set; }
        public object type { get; set; }
    }

    public class Item
    {
        public object? category_id { get; set; }
        public object description { get; set; }
        public object? id { get; set; }
        public object picture_url { get; set; }
        public string quantity { get; set; }
        public string title { get; set; }
        public double unit_price { get; set; }
    }

    public class Metadata
    {
    }

    public class Order
    {
        public string id { get; set; }
        public string type { get; set; }
    }

    public class Payer
    {
        public object email { get; set; }
        public object entity_type { get; set; }
        public object first_name { get; set; }
        public string id { get; set; }
        public Identification identification { get; set; }
        public object last_name { get; set; }
        public object operator_id { get; set; }
        public Phone phone { get; set; }
        public object type { get; set; }
        public object account_id { get; set; }
        public object external_account_id { get; set; }
        public object long_name { get; set; }
    }

    public class Phone
    {
        public object area_code { get; set; }
        public object extension { get; set; }
        public object number { get; set; }
    }

    public class PointOfInteraction
    {
        public ApplicationData application_data { get; set; }
        public BusinessInfo business_info { get; set; }
        public TransactionData transaction_data { get; set; }
        public string type { get; set; }
    }

    public class FindMercadoPagoResponse
    {
        public List<object> acquirer_reconciliation { get; set; }
        public AdditionalInfo additional_info { get; set; }
        public object? authorization_code { get; set; }
        public bool binary_mode { get; set; }
        public object? brand_id { get; set; }
        public string build_version { get; set; }
        public object? call_for_authorize_id { get; set; }
        public object callback_url { get; set; }
        public bool captured { get; set; }
        public Card card { get; set; }
        public List<object> charges_details { get; set; }
        public int collector_id { get; set; }
        public object? corporation_id { get; set; }
        public object? counter_currency { get; set; }
        public int coupon_amount { get; set; }
        public string currency_id { get; set; }
        public object date_approved { get; set; }
        public DateTime date_created { get; set; }
        public DateTime date_last_updated { get; set; }
        public DateTime? date_of_expiration { get; set; }
        public object? deduction_schema { get; set; }
        public string description { get; set; }
        public object? differential_pricing_id { get; set; }
        public string external_reference { get; set; }
        public List<object> fee_details { get; set; }
        public long id { get; set; }
        public int installments { get; set; }
        public object integrator_id { get; set; }
        public object issuer_id { get; set; }
        public bool live_mode { get; set; }
        public object marketplace_owner { get; set; }
        public object merchant_account_id { get; set; }
        public object merchant_number { get; set; }
        public Metadata metadata { get; set; }
        public object money_release_date { get; set; }
        public object money_release_schema { get; set; }
        public object money_release_status { get; set; }
        public string notification_url { get; set; }
        public string operation_type { get; set; }
        public Order order { get; set; }
        public Payer payer { get; set; }
        public string payment_method_id { get; set; }
        public string payment_type_id { get; set; }
        public object platform_id { get; set; }
        public PointOfInteraction point_of_interaction { get; set; }
        public object pos_id { get; set; }
        public string processing_mode { get; set; }
        public List<object> refunds { get; set; }
        public int shipping_amount { get; set; }
        public object sponsor_id { get; set; }
        public object statement_descriptor { get; set; }
        public string status { get; set; }
        public string status_detail { get; set; }
        public object store_id { get; set; }
        public int taxes_amount { get; set; }
        public double transaction_amount { get; set; }
        public int transaction_amount_refunded { get; set; }
        public TransactionDetails transaction_details { get; set; }
    }

    public class TransactionData
    {
        public BankInfo bank_info { get; set; }
        public object bank_transfer_id { get; set; }
        public object financial_institution { get; set; }
        public InfringementNotification infringement_notification { get; set; }
        public string qr_code { get; set; }
        public string qr_code_base64 { get; set; }
        public string ticket_url { get; set; }
        public object transaction_id { get; set; }
    }

    public class TransactionDetails
    {
        public object acquirer_reference { get; set; }
        public object bank_transfer_id { get; set; }
        public object external_resource_url { get; set; }
        public string financial_institution { get; set; }
        public double installment_amount { get; set; }
        public double net_received_amount { get; set; }
        public int overpaid_amount { get; set; }
        public object payable_deferral_period { get; set; }
        public object payment_method_reference_id { get; set; }
        public double total_paid_amount { get; set; }
        public object transaction_id { get; set; }
    }

