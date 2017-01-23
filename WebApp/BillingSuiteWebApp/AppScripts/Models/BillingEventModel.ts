module BillingSuiteApp.Model {
    export class BillingEventModel {
        public Id: number;
        public BillingEventCode: string;
        public BillingDate: Date;
        public Comments: string;
        public BilledCaseCount: number;
        public BilledWithErrorCaseCount: number;
        public AttemptedBilledCaseCount: number;
        public MessageType: string;
        public BillingAggregate: string;
    }
}