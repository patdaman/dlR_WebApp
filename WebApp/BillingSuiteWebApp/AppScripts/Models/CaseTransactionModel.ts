module BillingSuiteApp.Model {
    export class CaseTransactionModel {
        public CaseNumber: string;
        public TransactionId: number;
        public isSelected: boolean;
        public CompletedDate: Date;
        public BillableStatus: string;
        public Adjustments: number;
        public Payor1: string;
        public Payor1Group: string;
        public Payor2: string;
        public Payor2Group: string;
        public Total: number;
        public PaymentPayorId: number;
        public ListPrice: number;
        public Collections: number;
        public InterestPayment: number;
        public LatePayment: number;
        public NetPaymentApplied: number;
        public RemainingBalance: number;
        public LastModifiedUser: string;
        public LastModifiedDate: Date;
    }
}