///-------------------------------------------------------------------------------------------------
// <copyright file="" company="Signal Genetics Inc.">
// Copyright (c) 2015 Signal Genetics Inc.. All rights reserved.
// </copyright>
// <author></author>
// <date></date>
// <summary></summary>
///-------------------------------------------------------------------------------------------------

module BillingSuiteApp.Controller {
    export class ReconciliationController {

        identifier: string;
        title: string;
        httpServ: ng.IHttpService;
        qServ: ng.IQService;

        util: Service.UtilService;
        payorService: Service.PayorService;
        payorGroupService: Service.PayorGroupService;

        // Toolbar variables
        baiFileTemplate: any;
        baiFromDate: Date;
        baiToDate: Date;

        transactionTemplate: any;
        fromDate: Date;
        toDate: Date;
        fromDateString: string;
        toDateString: string;
        dateType: string;
        apiBody: string = "";

        caseReconciliationTemplate: any;
        caseFromDate: Date;
        caseToDate: Date;
        caseFromDateString: string;
        caseToDateString: string;
        caseListStr: string;
        filterDescription: string;
        payorsList: any;
        payorCodesList: any;
        payorGroupList: any;

        // BAI File List / Grid
        BaiFileUploadOptions: kendo.ui.UploadOptions = undefined;
        BaiFileDataGridSource: kendo.data.DataSource = undefined;
        BaiFileGridOptions: kendo.ui.GridOptions = undefined;
        BaiFileSelection: any;
        BaiFileName: any;
        UploadFailStatus: string = "";
        ServerURLString: string = "";

        // Transaction Grid
        TransactionGridOptions: kendo.ui.GridOptions = undefined;
        TransactionDataGridSource: kendo.data.DataSource = undefined;
        TransactionRowData: any;
        TransactionSelectedRow: any;
        TransactionSelection: any;
        TransactionDescription: any;
        TransactionAmount: any;
        TransactionBalance: any;
        TransactionCaseCount: any;
        TransactionBankFee: number; 
        TransactionLateFee: number; 
        TransactionInterest: number;
        FilterDebits: boolean;
        FilterZBalTransactions: boolean;

        // TransactionData Header Pane
        //  there is already a model for this, not implemented!!
        //TransactionBankFee: any;
        //TransactionLateFee: any;
        //TransactionInterest: any;

        // Case Reconciliation Grid
        CaseTransaction: Model.CaseTransactionModel;
        windowCaseTransactionCreationWait: kendo.ui.Window;
        windowCaseReconciliation: kendo.ui.Window;
        ReconciliationCase: ReconciliationCaseController;
        caseReconciliationToolbarTemplate: any;
        CaseReconciliationGridOptions: kendo.ui.GridOptions = undefined;
        CaseReconciliationDataGridSource: kendo.data.DataSource = undefined;

        // Payment Payor ID 
        paymentPayor: any;

        //Case financials
        caseFinancialsfromDate: Date;
        caseFinancialstoDate: Date;
        caseFinancialsfromDateString: string;
        caseFinancialstoDateString: string;
        caseFinancialsGridOptions: kendo.ui.GridOptions;
        caseFinancialsToolbarTemplate: any;
        caseFinancialsDataSource: kendo.data.DataSource;

        //Case Notes
        caseNotesData: Model.NotesModel;
        windowCaseNotes: kendo.ui.Window;
        caseNotesHandler: CaseNotes;
        caseNotesGridOptions: kendo.ui.GridOptions = undefined;
        caseNotesDataSource: kendo.data.DataSource = undefined;

        enumService: Service.EnumListService;
        enumListReceived: boolean = false;
        casePaymentStatusEnum: any;
        casePaymentStatusList: any;
        casePaymentStatusSelection: string;
        billingClassificationEnum: any;
        billingClassificationList: any;
        billingClassificationSelection: string;

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Initializes a new instance of the ReconciliationController class. </summary>
        ///
        /// <remarks>   Pdelosreyes, 20160115. </remarks>
        ///
        /// <param name="$rootScope">   The $root scope. </param>
        /// <param name="$http">        The $http. </param>
        /// <param name="$q">           The $q. </param>
        ///-------------------------------------------------------------------------------------------------

        constructor($rootScope, $http, $q,
            PayorService: Service.PayorService,
            PayorGroupService: Service.PayorGroupService,
            EnumService: Service.EnumListService,
            UtilService: Service.UtilService) {
            this.identifier = $rootScope.AppBuildStatus + "Reconciliation";
            this.title = "AR Reconciliation";
            this.ServerURLString = $rootScope.APIPath + "/Bai";

            this.httpServ = $http;
            this.qServ = $q;
            this.util = UtilService;
            this.payorService = PayorService;
            this.payorGroupService = PayorGroupService;
            this.enumService = EnumService;

            // For Bai File Upload Module
            this.BaiFileUploadOptions = this.initBaiFileUploadOptions();

            // For grid view of Bai Files:
            this.BaiFileSelection = "0";
            this.baiFromDate = new Date();
            this.baiToDate = new Date();
            this.baiFileTemplate = $("#baiFileTemplate").html();
            this.BaiFileDataGridSource = this.initBaiFileGrid($http);
            this.BaiFileGridOptions = this.initBaiFileGridOptions();

            // For grid view of transactions:
            this.TransactionSelection = "0";
            this.transactionTemplate = $("#transactionTemplate").html();
            this.toDate = new Date();
            this.fromDate = new Date();
            this.fromDate.setDate(this.toDate.getDate() - 90);
            this.fromDateString = DateToUSString(this.fromDate);
            this.toDateString = DateToUSString(this.toDate);
            this.FilterDebits = false;
            this.FilterZBalTransactions = true;

            this.TransactionDataGridSource = this.InitTransactionGrid($http);
            this.TransactionGridOptions = this.initTransactionGridOptions();

            // For Transaction Data in pop up header pane
            this.TransactionBankFee = 0;
            this.TransactionLateFee = 0;
            this.TransactionInterest = 0;

            // For grid view of cases in pop up:
            this.ReconciliationCase = undefined;

            //Case Financials
            this.caseFinancialstoDate = new Date();
            this.caseFinancialsfromDate = new Date();
            this.caseFinancialsfromDate.setDate(this.caseFinancialstoDate.getDate() - 7);
            this.caseFinancialsfromDateString = DateToUSString(this.caseFinancialsfromDate);
            this.caseFinancialstoDateString = DateToUSString(this.caseFinancialstoDate);
            this.caseFinancialsToolbarTemplate = $("#caseFinancialsToolbarTemplate").html();
            this.caseFinancialsDataSource = this.InitCaseFinancialsGrid($http);
            // this.caseFinancialsGridOptions = this.initCaseFinancialsGridOptions();

            if (this.enumService.EnumServiceReady) {
                this.enumListRecdProc();
            }
            else {
                var current: ReconciliationController = this;
                this.enumService.PopulateEnumListsAsync().then(function (data) {
                    current.enumListRecdProc();
                    current.initCaseFinancialsGridOptions();
                }, function (reason) {
                    console.log("Error loading enums");
                })
            }
            
            this.ReconciliationCase = new ReconciliationCaseController(this, this.httpServ, this.qServ, this.payorService, this.payorGroupService, this.util, this.TransactionSelection);
        }

        private enumListRecdProc() {
            var enumlink: Model.EnumLinkModel;
            enumlink = this.enumService.GetEnumLink("CasePaymentStatus");
            this.casePaymentStatusEnum = enumlink.EnumMap;
            this.casePaymentStatusList = enumlink.EnumList;

            enumlink = this.enumService.GetEnumLink("BillingClassification");
            this.billingClassificationEnum = enumlink.EnumMap;
            this.billingClassificationList = enumlink.EnumList;
            this.enumListReceived = true;
        
            this.caseFinancialsGridOptions = this.initCaseFinancialsGridOptions();
        }


        private payorListRecdProc() {
            this.payorsList = this.payorService.PayorsID2NameMap;
            this.payorCodesList = this.payorService.PayorsID2CodeMap;
        }

        private payorGroupListRecdProc() {
            this.payorGroupList = this.payorGroupService.PayorGroupID2NameMap;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Initialises the bai file upload options. </summary>
        ///
        /// <remarks>   Pdelosreyes, 20160120. </remarks>
        ///
        /// <param name="$http">    The $http. </param>
        ///
        /// <returns>   The kendo.ui.UploadOptions. </returns>
        ///-------------------------------------------------------------------------------------------------

        public initBaiFileUploadOptions(): kendo.ui.UploadOptions {

            var tthis = this;
            var baiUploader: kendo.ui.UploadOptions = {
               
                name: "files",
                multiple: false,
                localization: { statusFailed: "File Upload Failed", select: "Select BAI or CSX* File to Upload", retry: "Retry" },
                async: { saveUrl: this.ServerURLString, autoUpload: true },
            }
            return baiUploader;
        } 

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Initialises the bai file grid. </summary>
        ///
        /// <remarks>   Pdelosreyes, 20160115. </remarks>
        ///
        /// <param name="$http">    The $http. </param>
        ///
        /// <returns>   A kendo.data.DataSource. </returns>
        ///-------------------------------------------------------------------------------------------------

        public initBaiFileGrid($http): kendo.data.DataSource {
            var filemodel = kendo.data.Model.define({
                id: "id",
                fields: {
                    "FileName": { editable: false },
                    "StartDate": { editable: false },
                    "EndDate": { editable: false },
                    "UploadUser": { editable: false },
                    "UploadDate": { editable: false }
                }
            });

            var apirelpath = "api:/bai";

            var ds: kendo.data.DataSource = CreateGridDataSource($http, 50, filemodel,
                {
                    read: () => {
                        return apirelpath;
                    }
                });

            return ds;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Initialises the bai file grid options. </summary>
        ///
        /// <remarks>   Pdelosreyes, 20160115. </remarks>
        ///
        /// <returns>   The kendo.ui.GridOptions. </returns>
        ///-------------------------------------------------------------------------------------------------

        public initBaiFileGridOptions(): kendo.ui.GridOptions {
                var Options = {
                    dataSource: this.BaiFileDataGridSource,
                    reorderable: true,
                    resizable: true,
                    scrollable: true,
                    sortable: false,
                    toolbar: this.baiFileTemplate,
                    selectable: true,
                    //change: this.onBaiFileChange,
                    pageable: {
                        refresh: true,
                        pageSizes: true,
                    },
                    height: 750,
                    autoBind: false,
                    columns: [
                        { "field": "FileName", "title": "File Name" },
                        { "field": "StartDate", "title": "Transaction Start Date", "format": "{0:MM/dd/yyyy HH:mm}" },
                        { "field": "EndDate", "title": "Transaction End Date", "format": "{0:MM/dd/yyyy HH:mm}" },
                        { "field": "UploadUser", "title": "Upload User" },
                        { "field": "UploadDate", "title": "Upload Date", "format": "{0:MM/dd/yyyy HH:mm}" },
                    ]
            }
                return Options;
        }

        public FileUploadError(event) {
            var files = event.files;
            if (event.operation == "upload") {
                var resp = JSON.parse(event.XMLHttpRequest.response); //this is a signal exception
                alert("Failed to upload " + files[0].name + "\n Server error: " + resp.Message);
                this.UploadFailStatus = resp.Message;
            }
        }

        public onBaiFileChange(event) {
            var grid = event.sender;
            this.BaiFileName = (grid.dataItem(grid.select())).FileName.toString();
            this.BaiFileSelection = (grid.dataItem(grid.select())).id.toString();
            $("#ReconciliationTabs").data("kendoTabStrip").select("Transactions");
            //$("#ReconciliationTabs").data("kendoTabStrip").select("Transactions");
            this.GetTransactionData(event);
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Loads the BAI File Grid. </summary>
        ///
        /// <remarks>   Pdelosreyes, 20160119. </remarks>
        ///
        /// <param name="event">    The event. </param>
        ///
        /// <returns>   The bai data. </returns>
        ///-------------------------------------------------------------------------------------------------

        public GetBaiData(event) {
            this.BaiFileDataGridSource.read();
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Loads the Transaction grid. </summary>
        ///
        /// <remarks>   Pdelosreyes, 20160115. </remarks>
        ///
        /// <param name="event">    The event. </param>
        ///
        /// <returns>   The data. </returns>
        ///-------------------------------------------------------------------------------------------------
    
        public GetTransactionDataFromBaiFile(event) {
            this.apiBody = "/" + this.BaiFileSelection;
            this.refreshTransactionGrid();
            this.RunFilter(event);
        }

        public GetTransactionData(event) {
            this.apiBody = "?fromDate=" + this.fromDateString
                           + "&toDate=" + moment(this.toDateString).add(1, 'days').format('MM/DD/YYYY')
                           ;
            this.refreshTransactionGrid();
            this.RunFilter(event);
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Loads the Transaction grid with all unreconciled data. </summary>
        ///
        /// <remarks>   Pdelosreyes, 20160115. </remarks>
        ///
        /// <param name="event">    The event. </param>
        ///
        /// <returns>   The data. </returns>
        ///-------------------------------------------------------------------------------------------------

        public GetUnreconciledData(event) {
            this.BaiFileSelection = 0;
            this.InitTransactionGrid(this.httpServ);
            this.refreshTransactionGrid();
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Transaction Grid. </summary>
        ///
        /// <remarks>   Pdelosreyes, 20160105. </remarks>
        ///
        /// <returns>   . </returns>
        ///-------------------------------------------------------------------------------------------------

        public InitTransactionGrid($http): kendo.data.DataSource {
            var apiRelativePath = "api:/Transaction";
            var tmodel = kendo.data.Model.define({
                id: "TransactionId",
                fields: {
                    "AsOfDate": { editable: false, type: "date"  },
                    "AsOfDateModifier": { editable: false },
                    "AsOfTime": { editable: false },
                    "BaiAccountGroupId": { editable: false },
                    "BaiFileId": { editable: false },
                    "Balance": { editable: false },
                    "BankReferenceNumber": { editable: false },
                    "CaseCount": { editable: false },
                    "CurrencyCode": { editable: false },
                    "CustomerAccountNumber": { editable: false },
                    "CustomerReferenceNumber": { editable: false },
                    "DebitCredit": { editable: false },
                    "DepositId": { editable: true },
                    "Description": { editable: false },
                    "GroupStatus": { editable: false },
                    "Interest": { editable: true, type: "number" },
                    "ItemCount": { editable: false },
                    "LastModifiedDate": { editable: false },
                    "LastModifiedUser": { editable: false },
                    "LatePayment": { editable: true, type: "number" },
                    "OriginatorIdentification": { editable: false },
                    "OtherDeposit": { editable: true, type: "number" },
                    "TransactionAmount": { editable: false },
                    "TransactionFundsType": { editable: false },
                    "TransactionId": { editable: false },
                    "TransactionType": { editable: false },
                    "TransactionTypeCode": { editable: false },
                    "TypeCode": { editable: false },
                    "TypeCodeAmount": { editable: false },
                    "TypeCodeDescription": { editable: false },
                    "TypeCodeFundsType": { editable: false },
                    "UltimateReceiverIdentification": { editable: false },
                }
            });
            var current = this; 
            var ds: kendo.data.DataSource = CreateGridDataSource(this.httpServ, 20, tmodel,
                {
                    read: () => {
                        return apiRelativePath + current.apiBody;
                    },
                    update: () => {
                        return apiRelativePath;
                    },
                    destroy: () => {
                        return apiRelativePath + "?transactionId=" + current.TransactionSelection;
                    },
                },
                (e: kendo.data.DataSourceTransportUpdate) => {
                    current.onTransactionUpdate(e);
            });
            return ds;
            }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Initialises the transaction grid. </summary>
        ///
        /// <remarks>   Pdelosreyes, 20160115. </remarks>
        ///
        /// <returns>   . </returns>
        ///-------------------------------------------------------------------------------------------------

        public initTransactionGridOptions(): kendo.ui.GridOptions {
            var current: ReconciliationController = this;
            var Transactions = {
                dataSource: this.TransactionDataGridSource,
                reorderable: true,
                resizable: true,
                scrollable: true,
                filterable: false,
                sortable: true,
                selectable: true,
                toolbar: this.transactionTemplate,
                pageable: {
                    refresh: true,
                    pageSizes: true,
                },
                height: 750,
                autoBind: false,
                columns: [
                    { "field": "TransactionId", "title": "Id", "width": 40   },
                    { "field": "AsOfDate", "title": "Posted Date", "format": "{0:MM/dd/yyyy}" },
                   // { "field": "BankReferenceNumber", "title": "Bank Reference #" },
                    { "field": "CustomerAccountNumber", "title": "Customer Account #", "width": 150  },
                 //   { "field": "TransactionType", "title": "Transaction Type", "width": 150  },
                    { "field": "Description", "title": "Description", "width": 200 },
                    { "field": "DebitCredit", "title": "Type", "width": 80 },
                    { "field": "TransactionAmount", "title": "Amount", "format": "{0:c2}", "width": 80 },
                    { "field": "Balance", "title": "Remaining Balance", "format": "{0:c2}", headerAttributes: { style: "white-space: normal" }  },
                    { "field": "CaseCount", "title": "Case Count" },
                    { "field": "DepositId", "title": "DepositId" },
                    { "field": "Interest", "title": "Interest", "format": "{0:c2}" },
                    { "field": "LatePayment", "title": "Late / Penalty Fee", "format": "{0:c2}", headerAttributes: { style: "white-space: normal" } },
                    { "field": "OtherDeposit", "title": "Bank Fee", "format": "{0:c2}" },
                    { command: ["edit"] },
                    { command: ["destroy"] },
   
                ],
                editable: "inline",
                remove: function (event) {
                    current.TransactionSelection = event.model.TransactionId;
                    alert("Removing transaction " + current.TransactionSelection + " from grid...");

                }
            }
            return Transactions;
        }

        public RunFilter(kendoEvent) {
            var filter = {
                logic: "and",
                filters: []
            };
            if (this.TransactionDataGridSource.data().length > 0) {
                if (this.FilterDebits)
                    filter.filters.push({ field: "DebitCredit", operator: "eq", value: "CR" });
                if (this.FilterZBalTransactions)
                    filter.filters.push({ field: "Balance", operator: "neq", value: "0" });
                //if (this.TransactionBank)
                //    filter.filters.push({ field: "CustomerAccountNumber", operator: "contains", value: this.CustomerAccountNumber });
                if (filter.filters.length > 0)
                    this.TransactionDataGridSource.filter([filter]);
                else
                    this.TransactionDataGridSource.filter([]);
            }
        }

        public onTransactionUpdate(event) {
            var current = this;
            //current.updateCurrentTransaction();
            //current.kendoFastRedrawRow(current.TransactionGridOptions, current.TransactionSelectedRow);
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Executes the transaction change action. </summary>
        ///
        /// <remarks>   Pdelosreyes, 20160302. </remarks>
        ///
        /// <param name="event">    The event. </param>
        ///
        /// <returns>   . </returns>
        ///-------------------------------------------------------------------------------------------------

        public onTransactionChange(event) {
            var grid = event.sender;
            this.TransactionSelectedRow = grid.select();
            this.TransactionRowData = grid.dataItem(this.TransactionSelectedRow);
            this.TransactionSelection = this.TransactionRowData.id.toString();
            this.TransactionAmount = this.TransactionRowData.TransactionAmount.toString();
            this.TransactionBalance = this.TransactionRowData.Balance.toString();
            this.TransactionDescription = this.TransactionRowData.Description.toString();
            this.TransactionBankFee = this.TransactionRowData.OtherDeposit; 
            this.TransactionLateFee = this.TransactionRowData.LatePayment;  
            this.TransactionInterest = this.TransactionRowData.Interest; 

            var caseCount: number = (grid.dataItem(grid.select())).CaseCount;
            if ( caseCount )
                this.TransactionCaseCount = (grid.dataItem(grid.select())).CaseCount.toString();

            this.windowCaseTransactionCreationWait.title('Loading Transaction: ' + this.TransactionSelection);
            this.windowCaseTransactionCreationWait.open();
            this.windowCaseTransactionCreationWait.center();            
            

            this.ReconciliationCase.clearCaseGrid();            
            this.CaseTransaction = this.ReconciliationCase.CaseTransaction;
            this.ReconciliationCase.TransactionSelection = this.TransactionSelection;
            this.windowCaseTransactionCreationWait.close();
            this.windowCaseReconciliation.title('Case Reconciliation against Transaction ' + this.TransactionSelection);
            this.windowCaseReconciliation.open();
            this.windowCaseReconciliation.options.actions.Maximize = true;
            this.windowCaseReconciliation.options.modal = true;

            this.windowCaseReconciliation.maximize();
            //this.windowCaseReconciliation.center();
            
            this.ReconciliationCase.GetCaseTransactionDataFromTransactionId(this.TransactionSelection);
        }
        
        public onPopupClose(Event) {
            this.kendoFastRedrawRow(this.TransactionGridOptions, this.TransactionSelectedRow);
        }

        // Updates a single row in a kendo grid without firing a databound event.
        // This is needed since otherwise the entire grid will be redrawn.
        //  Borrowed from Adam Yaxley
        //  http://stackoverflow.com/questions/13613098/refresh-a-single-kendo-grid-row
        public kendoFastRedrawRow(grid, row) {
            var dataItem = grid.dataItem(row);

            var rowChildren = $(row).children('td[role="gridcell"]');

            for (var i = 0; i < grid.columns.length; i++) {

                var column = grid.columns[i];
                var template = column.template;
                var cell = rowChildren.eq(i);

                if (template !== undefined) {
                    var kendoTemplate = kendo.template(template);

                    // Render using template
                    cell.html(kendoTemplate(dataItem));
                } else {
                    var fieldValue = dataItem[column.field];

                    var format = column.format;
                    var values = column.values;

                    if (values !== undefined && values != null) {
                        // use the text value mappings (for enums)
                        for (var j = 0; j < values.length; j++) {
                            var value = values[j];
                            if (value.value == fieldValue) {
                                cell.html(value.text);
                                break;
                            }
                        }
                    } else if (format !== undefined) {
                        // use the format
                        cell.html(kendo.format(format, fieldValue));
                    } else {
                        // Just dump the plain old value
                        cell.html(fieldValue);
                    }
                }
            }
        }

        //Case financials

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets case financials data. </summary>
        ///
        /// <remarks>   Rphilavanh, 20160212. </remarks>
        ///
        /// <returns>   The case financials data. </returns>
        ///-------------------------------------------------------------------------------------------------

        public GetCaseFinancialsData() {
            this.caseFinancialsDataSource.read();
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Initialises the case financials grid options. </summary>
        ///
        /// <remarks>   Rphilavanh, 20160212. </remarks>
        ///
        /// <returns>   The kendo.ui.GridOptions. </returns>
        ///-------------------------------------------------------------------------------------------------

        private initCaseFinancialsGridOptions(): kendo.ui.GridOptions {
            //  var current: ReconciliationController = this;
      
            var cfGrid = {
                dataSource: this.caseFinancialsDataSource,
              //  reorderable: true,
                resizable: true,
                scrollable: true,
              //  filterable: true,
                sortable: true,
                selectable: true,
                toolbar: this.caseFinancialsToolbarTemplate,
                pageable: {
                    refresh: true,
                    pageSizes: true,
                },
                height: 750,
                autoBind: true,
                excel: {
                    fileName: moment().year().toString() + moment().month().toString() + moment().day().toString() + "_" + moment().hour().toString() + moment().minute().toString() + moment().second().toString() + "_CaseFinancials.xlsx",
                    allPages: true
                },
                edit: this.onEdit,
                columns: [
                    { "field": "CaseNumber", "title": "Case Number", width: "80px", headerAttributes: { style: "white-space: normal" } },
                    { "field": "DateofService", "title": "DOS", format: "{0:MM/dd/yyyy}", width: "80px", headerAttributes: { style: "white-space: normal" } },
                    { "field": "BillingClassification", "title": "Bill Class", width: "80px", headerAttributes: { style: "white-space: normal" } },
                    { "field": "PayorGroupName", "title": "Payor Group", width: "80px", headerAttributes: { style: "white-space: normal" } },
                    { "field": "PayorCode", "title": "Payor Code", width: "80px", headerAttributes: { style: "white-space: normal" } },
                    { "field": "DefaultListPrice", "title": "Default List Price", width: "80px", "format": "{0:c2}", headerAttributes: { style: "white-space: normal" } },
                    { "field": "DefaultContractualAllowance", "title": "Default Contractual Allowance", width: "80px", "format": "{0:c2}", headerAttributes: { style: "white-space: normal" } },
                    { "field": "ListPrice", "title": "List Price", "format": "{0:c2}", width: "80px", headerAttributes: { style: "white-space: normal" } },
                    { "field": "ContractualAllowance", "title": "Contractual Allowance", width: "80px", "format": "{0:c2}", headerAttributes: { style: "white-space: normal" } },
                    { "field": "AllowanceDoubtful", "title": "Allowance Doubtful", width: "80px", "format": "{0:c2}", headerAttributes: { style: "white-space: normal" } },
                    { "field": "ChangeInEstimate", "title": "Change In Estimate", width: "80px", "format": "{0:c2}", headerAttributes: { style: "white-space: normal" } },
                    { "field": "BadDebtExpense", "title": "Bad Debt Expense", width: "80px", "format": "{0:c2}", headerAttributes: { style: "white-space: normal" } },
                    { "field": "Collections", "title": "Collections", width: "80px", "format": "{0:c2}", headerAttributes: { style: "white-space: normal" } },
                    { "field": "RemainingBalance", "title": "AR Balance", width: "80px", "format": "{0:c2}", headerAttributes: { style: "white-space: normal" } },
                    { "field": "AllowableAmount", "title": "Allowable Amount", width: "80px", "format": "{0:c2}", headerAttributes: { style: "white-space: normal" } },
                   //  { "field": "WriteOff", "title": "WriteOff", "format": "{0:c2}", headerAttributes: { style: "white-space: normal" } },
                  //  { "field": "WriteOffReasonCode", "title": "Reason Code", values: this.casePaymentStatusEnum, headerAttributes: { style: "white-space: normal" } },
                    { "field": "InterestPayments", "title": "Interest Payment", width: "80px", "format": "{0:c2}", headerAttributes: { style: "white-space: normal" } },
                    { "field": "LatePayments", "title": "Late Payment", width: "80px", "format": "{0:c2}", headerAttributes: { style: "white-space: normal" } },
                    { "field": "Status", "title": "Open/Closed Status", width: "90px", values: this.casePaymentStatusEnum, headerAttributes: { style: "white-space: normal" } },
                    {
                        "field": "Notes", "title": "Notes", width: "140px", headerAttributes: { style: "white-space: normal" },
                         template: '<button ng-click="::vm.GetCaseNotes(dataItem.CaseNumber)"><strong>View Update History</strong></button>',
                    },
                    { "command": ["edit"], width: "80px"}

                ],
                editable: "inline"
            };
            return cfGrid;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Disables ability to edit Open/Closed status if Remaining Balance is not 0 and current status is open. </summary>
        ///
        /// <remarks>   Rphilavanh, 20160222. </remarks>
        ///
        /// <param name="e">    The unknown to process. </param>
        ///
        /// <returns>   . </returns>
        ///-------------------------------------------------------------------------------------------------

        public onEdit(e) {
            var model = e.model;
            // gets 1st ddl from inline grid
            var pc = e.container.find("[data-role=dropdownlist]").data("kendoDropDownList");
            pc.readonly(true);
            if (e.model.RemainingBalance == 0 || e.model.Status == "Closed") {
                pc.readonly(false);
            }

        }


        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets case notes. </summary>
        ///
        /// <remarks>   Rphilavanh, 20160222. </remarks>
        ///
        /// <param name="caseno">   The caseno. </param>
        ///
        /// <returns>   The case notes. </returns>
        ///-------------------------------------------------------------------------------------------------

        public GetCaseNotes(caseno: string) {

            this.caseNotesHandler = new CaseNotes(this.httpServ, this.qServ);
            var current: ReconciliationController = this;

            this.caseNotesHandler.GetCaseNotes(caseno, "Case Financials")
                .then(function (data) {
                    current.caseNotesData = current.caseNotesHandler.CaseNotesData;
                    current.caseNotesGridOptions = current.caseNotesHandler.caseNotesGridOptions;
                    current.windowCaseNotes.title('Case financials update history for ' + caseno);

                    current.windowCaseNotes.open();
                    current.windowCaseNotes.options.modal = true;
                    current.windowCaseNotes.center();
                })

        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Initialises the case financials grid. </summary>
        ///
        /// <remarks>   Rphilavanh, 20160212. </remarks>
        ///
        /// <param name="$http">    The $http. </param>
        ///
        /// <returns>   A kendo.data.DataSource. </returns>
        ///-------------------------------------------------------------------------------------------------

        private InitCaseFinancialsGrid($http): kendo.data.DataSource {
            var tmodel = kendo.data.Model.define({
                id: "ID",
                fields: {
                    ID: { editable: false, type: "number" },
                    CaseNumber: { editable: false },
                    BillingClassification: { editable: false },
                    PayorGroupName: { editable: false },
                    PayorCode: { editable: false },
                    DateofService: { editable: false, type: "date" },
                    DefaultListPrice: { editable: false },
                    DefaultContractualAllowance: { editable: false },
                    ListPrice: { editable: true },
                    ContractualAllowance: { editable: true },
                    AllowanceDoubtful: { editable: true },
                    ChangeInEstimate: { editable: true },
                    BadDebtExpense: { editable: true },
                    Collections: { editable: false },
                    RemainingBalance: { editable: false },
                    AllowableAmount: { editable: true },
                  //  WriteOff: { editable: true },
                  //  WriteOffReasonCode: { editable: true },
                    InterestPayments: { editable: false },
                    LatePayments: { editable: false },
                    Status: { editable: true },
                    Notes: {
                        editable: true, validation: {
                            required: true
                        }
                    }
                }
            });
            var apirelpath = "api:/CasePayment";
            var ds: kendo.data.DataSource = CreateGridDataSource($http, 20, tmodel,
                {
                    read: () => {
                        return apirelpath + "?fromDate=" + this.caseFinancialsfromDateString +
                            "&toDate=" + moment(this.caseFinancialstoDateString).add(1, 'days').format('MM/DD/YYYY');
                    },
                    update: () => {
                        return apirelpath;
                    }

                });
            return ds;

        }

        public RunCaseFinancialsFilter(kendoEvent) {
            var filter = {
                logic: "and",
                filters: []
            };
            if (this.caseFinancialsDataSource.data().length > 0) {
                if (this.billingClassificationSelection && this.billingClassificationSelection.length > 0)
                    filter.filters.push({ field: "BillingClassification", operator: "eq", value: this.billingClassificationSelection });
                if (this.casePaymentStatusSelection && this.casePaymentStatusSelection.length > 0)
                    filter.filters.push({ field: "Status", operator: "eq", value: this.casePaymentStatusSelection });

                if (filter.filters.length > 0)
                    this.caseFinancialsDataSource.filter([filter]);
                else
                    this.caseFinancialsDataSource.filter([]);
            }
        }

        public updateCurrentTransaction(): void {
            var current = this;

            this.httpServ.get("api:/Transaction/" + this.TransactionSelection, { withCredentials: true })
                .success(function (data, status, headers, config) {
                    console.log("success");
                    var transInfo: any = data;
                    current.TransactionCaseCount = transInfo.CaseCount;
                    current.TransactionBalance = transInfo.RemainingBalance;
                    current.refreshTransactionGrid();
                })
                .error(function error(response) {
                    console.log("updateCurrentTransaction() failed to update transaction.");
                });            
        }

        public refreshTransactionGrid(): void {
            this.TransactionDataGridSource.read(); 
        }
    }
}