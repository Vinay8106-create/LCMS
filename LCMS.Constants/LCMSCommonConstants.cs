namespace LCMS.Constants
{
    public static class LCMSCommonConstants
    {
        // Common Reference Number Class For All
        public static class ReferenceNumber
        {
            public const int Id = 1034;
            public const string MetadataName = "ReferenceNumber";
            public const string PrefixName = "PrefixAliceName";
        }

        public static class Currency
        {
            public const int Id = 502;
            public const string CurrencyValue = "VUV";
            public const string InterimInterestRate = "Interim Interest";
        }

        public static class ApplicationRefNo
        {
            public const int ApplicationRefNoId = 1034;

            public const string LastRunningNumber = "LastRunningNumber";
            public const string PrefixAliceName = "PrefixAliceName";
            public const string SuffixAliceName = "SuffixAliceName";
            public const string CashCounter = "CASHC";
            public const string DepositBook = "DPSBK";
            public const string Receipt = "RECPT";
            public const string CreditMemo = "CRDIM";
            public const string RefundRequest = "RFRRN";
            public const string ChangeReceipt = "CHGRC";
            public const string PenaltyWaiverRequest = "PWRRN";
        }

        public static class InformationRefNo
        {
            public const int RefNoId = 7802;   // Change afterwards according to LMS requirement

            public const string LastRunningNumber = "LastRunningNumber";
            public const string Digit = "Digit";
            public const string IsAliceName = "IsAliceName";
            public const string PrefixAliceName = "PrefixAliceName";
            public const string SuffixAliceName = "SuffixAliceName";

            public const string Plan = "PLAN";
            public const string Employer = "EMREF";
            public const string Receipt = "RECPT";
            public const string Invoice = "INVOC";
        }

        public static class WorkFlow
        {
            public const string CashCounterWorkFlow = "CSCWF";
            public const string CashCounter = "CCCNG";
            public const string CreditMemo = "CRTMO";
            public const string RefundRequest = "RFREQ";
            public const string ChangeReceipt = "CHGRC";
            public const string PenaltyWaiverRequest = "PEWAR";
        }

        public static class TransactionType
        {
            public const int AccountsTransactionId = 263;
            public const string PremiumReceiptCancelled = "RECCA";
            public const string PremiumReceipted = "PRREC";
            public const string CreditMemo = "CRDMO";
            public const string Refund = "REFPD";
            public const string PolicyInvoice = "POINV";
            public const string ClaimProcessing = "CLAPR";
            public const string ClaimPayout = "CLPOT";
            public const string PenaltyWaiver = "PENWA";
            public const string CreditMemoCancelled = "CRMCA";
            public const string InvoiceReversal = "INVRV";

            public static class MetaDatas
            {
                public const string TransactionNarration = "TransactionNarration";
                public const string DebitLedgerCode = "DebitLedgerCode";
                public const string CreditLedgerCode = "CreditLedgerCode";
                public const string DebitLedgerCode_Payment = "DebitLedgerCode_Payment";
                public const string CreditTransactionNarration = "CreditTransactionNarration";
                public const string DebitTransactionNarration = "DebitTransactionNarration";

                public const string ReceiptDebitLedgerCode = "ReceiptDebitLedgerCode";
                public const string ReceiptCreditLedgerCode = "ReceiptCreditLedgerCode";
                public const string ReceiptTransactionNarration = "ReceiptTransactionNarration";
                public const string ReceiptDebitTransactionNarration = "ReceiptDebitTransactionNarration";
                public const string ReceiptCreditTransactionNarration = "ReceiptCreditTransactionNarration";

                public const string CreditMemoReceiptTransactionNarration = "CreditMemoReceiptTransactionNarration";
                public const string CreditMemoReceiptDebitLedgerCode = "CreditMemoReceiptDebitLedgerCode";
                public const string CreditMemoReceiptDebitTransactionNarration = "CreditMemoReceiptDebitTransactionNarration";
                public const string CreditMemoReceiptCreditLedgerCode = "CreditMemoReceiptCreditLedgerCode";
                public const string CreditMemoReceiptCreditTransactionNarration = "CreditMemoReceiptCreditTransactionNarration";

                public const string InvoiceTransactionNarration = "InvoiceTransactionNarration";
                public const string InvoiceDebitTransactionNarration = "InvoiceDebitTransactionNarration";
                public const string InvoiceCreditTransactionNarration = "InvoiceCreditTransactionNarration";
                public const string PremiumIncomeLedgerCode = "PremiumIncomeLedgerCode";
                public const string PremiumReceivableLedgerCode = "PremiumReceivableLedgerCode";
                public const string TaxLedgerCode = "TaxLedgerCode";
                public const string PenaltyIncomeLedgerCode = "PenaltyIncomeLedgerCode";
                public const string PenaltyReceivableLedgerCode = "PenaltyReceivableLedgerCode";

                public const string ClaimTransactionNarration = "ClaimTransactionNarration";
                public const string ClaimCustomerLedgerCode = "ClaimCustomerLedgerCode";
                public const string ClaimPayableLedgerCode = "ClaimPayableLedgerCode";
                public const string ClaimTaxLedgerCode = "ClaimTaxLedgerCode";
                public const string ClaimDebitTransactionNarration = "ClaimDebitTransactionNarration";
                public const string ClaimCreditTransactionNarration = "ClaimCreditTransactionNarration";
                public const string ClaimBankLedgerCode = "ClaimBankLedgerCode";
                public const string ClaimPayoutDebitTransactionNarration = "ClaimPayoutDebitTransactionNarration";
                public const string ClaimPayoutCreditTransactionNarration = "ClaimPayoutCreditTransactionNarration";

                public const string PenaltyTransactionNarration = "PenaltyTransactionNarration";
                public const string PenaltyChargesLedgerCode = "PenaltyChargesLedgerCode";
                public const string PenaltyTaxLedgerCode = "PenaltyTaxLedgerCode";
                public const string PenaltyCustomerLedgerCode = "PenaltyCustomerLedgerCode";
                public const string PenaltyDebitTransactionNarration = "PenaltyDebitTransactionNarration";
                public const string PenaltyCreditTransactionNarration = "PenaltyCreditTransactionNarration";

                public const string CreditMemoCancelledTransactionNarration = "CreditMemoCancelledTransactionNarration";
                public const string CreditMemoCustomerLedgerCode = "CreditMemoCustomerLedgerCode";
                public const string CreditMemoLedgerCode = "CreditMemoLedgerCode";
                public const string CreditMemoCancelledDebitTransactionNarration = "CreditMemoDebitTransactionNarration";
                public const string CreditMemoCancelledCreditTransactionNarration = "CreditMemoCreditTransactionNarration";
            }
        }

        public static class Attribute
        {
            public const int Id = 7003;
            public const string TransactionType = "TransactionType";
            public const string ReceiptNo = "RECNO";
            public const string PolicyNo = "POLNO";
            public const string InvoiceNo = "INVON";
            public const string CustomerNo = "CUSNO";
            public const string CreditMemoNo = "CRMEO";
            public const string RefundNo = "REFNO";
            public const string ClaimNo = "CLAIM";
            public const string ClaimPayoutNo = "CLAPO";
            public const string WaiverNo = "PEWAI";
            public const string RemittanceID = "REMID";
            public const string Currency = "CURRY";
            public const string CustomerRefNo = "CUSRN";
            public const string CustomerACId = "CUSAI";
            public const string MemberACNO = "MEMAN";
            public const string InterestYear = "INTYR";
            public const string PaymentNo = "PYMNO";
        }

        public static class SourceType
        {
            public const string Receipt = "RECPT";
            public const string CreditMemo = "CRDMO";
            public const string RefundRequest = "REREQ";
            public const string Invoice = "INVOC";
            public const string ClaimProcessing = "CLAPO";
            public const string ClaimPayout = "CLPOT";
            public const string PenaltyWaiver = "PENWA";
            public const string CreditMemoCancelled = "CRMCA";
        }
    }
}
