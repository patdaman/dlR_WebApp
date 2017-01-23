module BillingSuiteApp.Model {
    export class CaseFinancialsModel {
        public ID: number;
        public CaseNumber: string;
        public BillingClassification: string;
        public BillingAggregate: string;
        public BillableStatus: string;
        public DateofService: Date;
        public PayorGroupName: string;
        public PayorCode: string;
        public DefaultListPrice: number;
        public DefaultContractualAllowance: number;
        public ListPrice: number;
        public ContractualAllowance: number;
        public AllowanceDoubtful: number;
        public ChangeInEstimate: number;
        public RemainingBalance: number;
        public AllowableAmount: number;
        public BadDebtExpense: number;
        public Collections: number;
        public WriteOff: number;
        public WriteOffReasonCode: string;
        public InterestPayments: number;
        public LatePayments: string;
        public Status: string;
        public LastModifiedUser: string;
        public LastModifiedDate: Date;
        public Notes: string;
    }
}