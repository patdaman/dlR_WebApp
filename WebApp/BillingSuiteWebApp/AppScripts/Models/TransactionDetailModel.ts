module BillingSuiteApp.Model {
    export class TransactionDetailModel {
        //public Id: number;
        public TransactionId: string;
        public TransactionDate: Date;
        public Description: string;
        public CustomerAccountNumber: number;
        public Comments: string;
        public ReconciledCaseCount: number;
        public Total: number;
        public RemainingBalance: number;
        public MessageType: string;
    }
}