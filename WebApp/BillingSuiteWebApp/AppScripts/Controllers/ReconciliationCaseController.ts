///-------------------------------------------------------------------------------------------------
// <copyright file="" company="Signal Genetics Inc.">
// Copyright (c) 2015 Signal Genetics Inc.. All rights reserved.
// </copyright>
// <author>Patrick de los Reyes</author>
// <date>2016-02-10</date>
// <summary></summary>
///-------------------------------------------------------------------------------------------------

module BillingSuiteApp.Controller {
    export class ReconciliationCaseController {

        public httpServ: ng.IHttpService;
        public qServ: ng.IQService;

        identifier: string;
        title: string;

        payorService: Service.PayorService;
        payorGroupService: Service.PayorGroupService;
        UtilService: Service.UtilService;

        rowTemplate: any;
        altRowTemplate: any;
        caseReconciliationTemplate: any;
        caseSearchTemplate: any;
        caseFromDate: Date;
        caseToDate: Date;
        caseFromDateString: string;
        caseToDateString: string;
        caseListStr: string;
        filterDescription: string;
        payorGroupSelection: string;
        payorSelection: string;
        payorsList: any;
        payorCodesList: any;
        payorGroupList: any;
        payorListReceived: boolean = false;
        ServerURLString: string = "";
        TransactionSelection: any;
        TransactionVar: any;
        apiBody: string = "";
        TransactionApiBody: string = "";
        dmodel: any;

        // Transaction Detail 
        public CaseTransaction: Model.CaseTransactionModel;
        TransactionBankFee: number;
        TransactionLateFee: number;
        TransactionInterest: number;

        // Case Reconciliation Grid
        CaseReconciliationGridOptions: kendo.ui.GridOptions;
        CaseReconciliationDataGridSource: kendo.data.DataSource;
        CaseGridOptions: kendo.ui.GridOptions;
        CaseDataGridSource: kendo.data.DataSource;
        CaseNumber: any;
        CaseDetailTemplate: any;
        FilterInternalCases: any;


        reconciliationController: ReconciliationController;

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Initializes a new instance of the ReconciliationController class. </summary>
        ///
        /// <remarks>   Pdelosreyes, 20160115. </remarks>
        ///
        /// <param name="$rootScope">   The $root scope. </param>
        /// <param name="$http">        The $http. </param>
        /// <param name="$q">           The $q. </param>
        ///-------------------------------------------------------------------------------------------------

        constructor(
            parentController: ReconciliationController,
            http: ng.IHttpService,
            qserv: ng.IQService,
            PayorService: Service.PayorService,
            PayorGroupService: Service.PayorGroupService,
            UtilService: Service.UtilService,
            TransactionSelection: any
        ) {

            this.reconciliationController = parentController;

            this.payorService = PayorService;
            this.payorGroupService = PayorGroupService;
            this.httpServ = http;
            this.qServ = qserv;
            this.payorListReceived = false;

            // Grid Row Template for Drag and Drop
            this.rowTemplate = $("#grid-row-template").html();
            this.altRowTemplate = $("#grid-row-template").html();
            this.CaseDetailTemplate = $("#CaseDetailTemplate").html();

            // For grid view of cases:
            this.caseReconciliationTemplate = $("#caseReconciliationTemplate").html();
            this.caseSearchTemplate = $("#caseSearchTemplate").html();
            this.caseToDate = new Date();
            this.caseFromDate = new Date();
            this.caseToDate.setDate(this.caseToDate.getDate() - 30);
            this.caseFromDate.setDate(this.caseFromDate.getDate() - 90);
            this.caseFromDateString = DateToUSString(this.caseFromDate);
            this.caseToDateString = DateToUSString(this.caseToDate);

            this.CaseNumber = "";
            this.FilterInternalCases = true;

            //Initialize grid 
            this.CaseReconciliationDataGridSource = this.initCaseReconciliationDataGridSource(http);
            this.CaseDataGridSource = this.initCaseDataGridSource(http);

            this.TransactionVar = TransactionSelection;
            //Load payors and payor groups 
            if (this.payorService.PayorsID2CodeMap) {
                this.payorListRecdProc();
            }
            else {
                var current: ReconciliationCaseController = this;
                this.payorService.GetPayorsAsync().then(function (data) {
                    current.payorListRecdProc();
                }, function (reason) {
                    console.log("Could not load Payors");
                })
            }

            if (this.payorGroupService.PayorGroupID2NameMap) {
                this.payorGroupListRecdProc();
            }
            else {
                var current2: ReconciliationCaseController = this;
                this.payorGroupService.GetPayorGroupAsync().then(function (data) {
                    current2.payorGroupListRecdProc();
                }, function (reason) {
                    console.log("Could not load Payor Groups");
                })
            }

             

        }

        private payorListRecdProc() {
            this.payorsList = this.payorService.PayorsID2NameMap;
            this.payorCodesList = this.payorService.PayorsID2CodeMap;
            this.payorListReceived = true;
            this.CallGridInit();
        }

        private payorGroupListRecdProc() {
            this.payorGroupList = this.payorGroupService.PayorGroupID2NameMap;
        }

        CallGridInit() {
            console.log("PE: Running Grid Init");
            if (this.payorListReceived)  {
                var current: ReconciliationCaseController = this;
                current.payorListReceived = true;
                //Initialize grid 
                current.CaseReconciliationGridOptions = this.initCaseReconciliationGridOptions(this.CaseReconciliationDataGridSource);
                this.CaseGridOptions = this.initCaseGridOptions(this.CaseDataGridSource);

                this.RunFilter(null, true);
            }
        }
        
        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Initializes the data grid source. (TOP GRID) </summary>
        ///
        /// <remarks>   Dtorres, 20160216. </remarks>
        ///
        /// <param name="http"> The HTTP. </param>
        ///
        /// <returns>   A kendo.data.DataSource. </returns>
        ///-------------------------------------------------------------------------------------------------

        private initCaseReconciliationDataGridSource(http: ng.IHttpService): kendo.data.DataSource {
            var apiRelativePath = "api:/CaseTransaction";
            this.dmodel = kendo.data.Model.define({
                id: "CaseNumber",
                fields: {
                    "CaseNumber": { editable: false },
                    "TransactionId": { editable: false, type: "number" },
                    "IsSelected": { editable: true, type: "boolean" },
                    "IsSelectable": { editable: false, type: "boolean" },
                    "CompletedDate": { editable: false, type: "date" },
                    "Payor1Group": { editable: false },
                    "Payor1": { editable: false },
                    "Payor2Group": { editable: false },
                    "Payor2": { editable: false },
                    "ListPrice": { editable: false },
                    "Collections": { editable: false },
                    "PaymentPayorId": { editable: true, type: "number"  },
                    "BillableStatus": { editable: false },
                    "NetPaymentApplied": { editable: true },
                    "InterestPayment": { editable: true },
                    "LatePayment": { editable: true },
                    "Total": { editable: false},
                    "RemainingBalance": { editable: false },
                    "LastModifiedUser": { editable: false },
                    "LastModifiedDate": { editable: false },
                }
            });

            var current = this;
            var ds: kendo.data.DataSource = CreateGridDataSource(http, 20, this.dmodel,
                {
                    read: () => {
                        return apiRelativePath + current.TransactionApiBody;
                    },
                    create: () => {
                        return apiRelativePath;
                    },
                    update: () => {
                        return apiRelativePath;
                    },
                    destroy: () => {
                        return apiRelativePath + "?transactionId=" + this.TransactionSelection + "&caseNumber=" + this.CaseNumber;
                    }
                },
                (e: kendo.data.DataSourceChangeEvent) => {
                    current.onCaseReconDataGridChanged(e);
                }
            );
            return ds;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Executes the edit case recon action. </summary>
        ///
        /// <remarks>   Pdelosreyes, 20160225. </remarks>
        ///
        /// <param name="event">  NOT FOR DOUGHNUT RELEASE  The event. </param>
        ///
        /// <returns>   . </returns>
        ///-------------------------------------------------------------------------------------------------

        //public onEditCaseRecon(event) {
        //    var grid = event.sender;
        //    var payorCode = (grid.dataItem(grid.select())).Payor.toString();
        //    this.PaymentPayorData = [
        //        //{ PaymentPayorId: "" },
        //        { PaymentPayorId: payorCode },
        //        { PaymentPayorId: "Self" }
        //    ];
        //    var gridData = $("#CaseReconciliationGrid").data("kendoGrid");
        //    var container = event.container;
        //    var input = $('<input id="PaymentPayorId" name="PaymentPayorId">');
        //    input.appendTo(container);
        //    input.kendoDropDownList({
        //        dataTextField: "PaymentPayorId",
        //        dataValueField: "PaymentPayorId",
        //        dataSource: this.PaymentPayorOptions
        //    }).appendTo(container);
        //}

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Initialises the case reconciliation grid options. (TOP GRID) </summary>
        ///
        /// <remarks>   Dtorres, 20160222. </remarks>
        ///
        /// <param name="dataSource">   The data source. </param>
        ///
        /// <returns>   The kendo.ui.GridOptions. </returns>
        ///-------------------------------------------------------------------------------------------------

        private initCaseReconciliationGridOptions(dataSource: kendo.data.DataSource): kendo.ui.GridOptions {

            var current: ReconciliationCaseController = this;
            var options = {
                dataSource: dataSource,
             //   edit: this.onEditCaseRecon,
                reorderable: true,
                resizable: true,
                selectable: "row",
                scrollable: true,
                sortable: true,
                //drop: this.onCaseReconciliationDrop,
                toolbar: this.caseReconciliationTemplate,
                pageable: {
                    refresh: true,                    
                },
                //detailTemplate: this.CaseDetailTemplate,
                //detailInit: function (e) {
                //    current.handleDetailInit(e);
                //},
                columns: [
                    { "field": "IsSelected", "width": "1px", hide: true },
                    { "field": "IsSelectable", "width": "1px", hide: true },
                    { "field": "CaseNumber", "title": "CaseNumber" },
                    { "field": "TransactionId", "width": "1px", hide: true },
                    { "field": "CompletedDate", "title": "Completed Date", "format": "{0:MM/dd/yyyy}" },
                    { "field": "Payor1Group", "title": "Payor1 Group" },
                    { "field": "Payor1", "title": "Payor1 Code" },
                    { "field": "Payor1Group", "title": "Payor2 Group" },
                    { "field": "Payor2", "title": "Payor2 Code" },
                    {
                        "field": "PaymentPayorId", "title": "Payment Payor Id", "values": this.payorCodesList, "width": "200px"
                    },
                    { "field": "BillableStatus", "title": "Bill Status" },
                    { "field": "NetPaymentApplied", "title": "Amount", "format": "{0:c2}" },
                    { "field": "InterestPayment", "title": "Interest Payment", "format": "{0:c2}" },
                    { "field": "LatePayment", "title": "Late Payment", "format": "{0:c2}" },
                    { "field": "Total", "title": "Total", "format": "{0:c2}" },
                    { "field": "RemainingBalance", "title": "Remaining Bal", "format": "{0:c2}" },
                    { command: [{ name: "edit", text: 'edit' }], "width": "90px" },
                    {
                        command: [{ template: "<span class='k-button' ng-click='vm.ReconciliationCase.removeCase($event)'>Remove</span>", width: "75px" }]
                    }
                ],
                editable: "inline",
            };
            return options;
        }

        public removeCase($event) {
            var grid = $("#CaseReconciliationGrid").data("kendoGrid");
            var tr = $($event.target).closest("tr"); // get the current table row (tr)
            // get the data bound to the current table row
            var data: any = grid.dataItem(tr);
            var current = this;
            this.httpServ.delete(
                "api:/CaseTransaction/?transactionId=" + data.TransactionId + "&caseNumber=" + data.CaseNumber,
                { withCredentials: true })
                .success(function (data, status, headers, config) {
                    current.CaseReconciliationDataGridSource.read();
                    current.reconciliationController.updateCurrentTransaction();
                    console.log("Delete succeeded.")
                })
                .error(function (data, status, headers, config) {
                    current.errorHandler("destroy", data, status, headers, config);
                });

        }

        public onCaseReconciliationDrop(event) {
            var current: ReconciliationCaseController = this.reconciliationController.ReconciliationCase;
            var grid = event.sender;
            var dataItem = current.CaseDataGridSource.getByUid(event.draggable.currentTarget.data("uid"));
            //current.CaseDataGridSource.pushUpdate(dataItem);
            current.CaseDataGridSource.add(dataItem);
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Initialises the case data grid source. (BOTTOM GRID) </summary>
        ///
        /// <remarks>   Dtorres, 20160222. </remarks>
        ///
        /// <param name="http"> The HTTP. </param>
        ///
        /// <returns>   A kendo.data.DataSource. </returns>
        ///-------------------------------------------------------------------------------------------------

        private initCaseDataGridSource(http: ng.IHttpService): kendo.data.DataSource {
            var apiRelativePath = "api:/CaseTransaction";
            this.dmodel = kendo.data.Model.define({
                id: "CaseNumber",
                fields: {
                    "CaseNumber": { editable: false },
                    "TransactionId": { editable: false, type: "number" },
                    "IsSelected": { editable: false, type: "boolean" },
                    "IsSelectable": { editable: false, type: "boolean" },
                    "CompletedDate": { editable: false, type: "date" },
                    "Payor1Group": { editable: false },
                    "Payor1": { editable: false },
                    "Payor2Group": { editable: false },
                    "Payor2": { editable: false },
                    "ListPrice": { editable: false },
                    "Collections": { editable: false },
                    "PaymentPayorId": { editable: true, type: "number"  },
                    "BillableStatus": { editable: false },
                    "NetPaymentApplied": { editable: true },
                    "InterestPayment": { editable: true },
                    "LatePayment": { editable: true},
                    "Total": { editable: false },
                    "RemainingBalance": { editable: false },
                    "LastModifiedUser": { editable: false },
                    "LastModifiedDate": { editable: false },
                }
            });

            var current = this;
            var ds: kendo.data.DataSource = CreateGridDataSource(http, 20, this.dmodel,
                {
                    read: () => {
                        return apiRelativePath + current.apiBody;
                    },
                    update: () => {
                        (e: kendo.data.DataSourceTransportUpdate) => {
                            current.onCaseDataDataSourceChanged(e);
                        }
                        return apiRelativePath;
                    },
                    create: () => {
                        (e: kendo.data.DataSourceChangeEvent) => {
                            current.onCaseDataDataSourceChanged(e);
                        }
                    }
                }
            );
            return ds;

        }

        public RunFilter(kendoEvent, init: boolean = false) {

            var current: ReconciliationCaseController = null;
            if (init) {
                current = this;
            }
            else {
                current = this.reconciliationController.ReconciliationCase;
            }
            var filter = {
                logic: "and",
                filters: []
            };
            if (init || current.CaseDataGridSource.data().length > 0) {
                filter.filters.push({ field: "IsSelectable", operator: "eq", value: "true" });
                if (current.FilterInternalCases)
                    filter.filters.push({ field: "CaseNumber", operator: "contains", value: "CL" });
                current.CaseDataGridSource.filter([filter]);
            }
        }

        public removeCaseFilter() {
            //var current = this.reconciliationController.ReconciliationCase;
            //current.CaseDataGridSource.filter([]);
            //var filter = {
            //    logic: "and",
            //    filters: []
            //};
            //if (current.CaseDataGridSource.data().length > 0) {
            //    filter.filters.push({ field: "IsSelected", operator: "eq", value: "false" });
            //}
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Initialises the case grid options. (BOTTOM GRID) </summary>
        ///
        /// <remarks>   Dtorres, 20160222. </remarks>
        ///
        /// <param name="dataSource">   The data source. </param>
        ///
        /// <returns>   The kendo.ui.GridOptions. </returns>
        ///-------------------------------------------------------------------------------------------------

        private initCaseGridOptions(dataSource: kendo.data.DataSource): kendo.ui.GridOptions {
            var current: ReconciliationCaseController = this;
            var options = {
                dataSource: dataSource,
                autoBind: false,
                reorderable: true,
                resizable: true,
                selectable: "row",
                scrollable: true,
                sortable: true,
                toolbar: this.caseSearchTemplate,
                pageable: {
                    refresh: true,
                    pageSizes: true,
                },
                //detailTemplate: this.CaseDetailTemplate,
                //detailInit: function (e) {
                //    current.handleDetailInit(e);
                //},
                height: 400,
                columns: [
                    { "field": "IsSelected", "width": "1px", hide: true },
                    { "field": "IsSelectable", "width": "1px", hide: true },
                    { "field": "CaseNumber", "title": "CaseNumber" },
                    { "field": "TransactionId", "width": "1px", hide: true },
                    { "field": "CompletedDate", "title": "Completed Date", "format": "{0:MM/dd/yyyy}" },
                    { "field": "Payor1Group", "title": "Payor1 Group" },
                    { "field": "Payor1", "title": "Payor1 Code" },
                    { "field": "Payor2Group", "title": "Payor2 Group" },
                    { "field": "Payor2", "title": "Payor2 Code" },
                    {
                        "field": "PaymentPayorId", "title": "Payment Payor Id", "values": this.payorCodesList, "width": "200px" 
                    },
                    { "field": "BillableStatus", "title": "Bill Status" },
                    { "field": "NetPaymentApplied", "title": "Amount", "format": "{0:c2}" },
                    { "field": "InterestPayment", "title": "Interest Payment", "format": "{0:c2}" },
                    { "field": "LatePayment", "title": "Late Payment", "format": "{0:c2}" },
                    { "field": "Total", "title": "Total", "format": "{0:c2}" },
                    { "field": "RemainingBalance", "title": "Remaining Bal", "format": "{0:c2}" },
                    { command: [{ name: "edit", text: 'Add' }], "width": "90px" }
                ],
                editable: "inline"

            };
            return options;
        }

        public onCaseSelect(event) {
            var current: ReconciliationCaseController = this.reconciliationController.ReconciliationCase;

            var grid = event.sender;
            var reconGrid = $("#CaseReconciliationGrid").data("kendoGrid");
            this.CaseNumber = (grid.dataItem(grid.select())).CaseNumber.toString();
            this.TransactionVar = (grid.dataItem(grid.select())).TransactionId;
            //this.PaymentPayorData = [{ PaymentPayorId: "" },
            //    { PaymentPayorId: (grid.dataItem(grid.select())).Payor1.toString() },
            //    { PaymentPayorId: "Self" }];
            //grid.table.kendoDraggable({
            //    filter: "tbody tr",
            //    group: "caseGroup",
            //    hint: function (item) {
            //        return item.clone();
            //    },
            //});
            //reconGrid.table.kendoDropTarget({
            //    group: "caseGroup",
            //    drop: function (e) {
            //        var dataItem = current.CaseDataGridSource.getByUid(event.draggable.currentTarget.data("uid"));
            //        current.CaseDataGridSource.add(dataItem);
            //    }
            //});
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Executes the data source changed action. </summary>
        ///
        /// <remarks>   Dtorres, 20160222. </remarks>
        ///
        /// <param name="event">    The event. </param>
        ///
        /// <returns>   . </returns>
        ///-------------------------------------------------------------------------------------------------

        private onCaseReconDataGridChanged(event: kendo.data.DataSourceTransportUpdate) {
            this.reconciliationController.updateCurrentTransaction();
        }


        public onCaseDataDataSourceChanged(event: kendo.data.DataSourceChangeEvent) {
            this.CaseReconciliationDataGridSource.read();
            this.reconciliationController.updateCurrentTransaction();
        }

        public GetCaseTransactionDataFromTransactionId(TransactionSelection) {
            this.TransactionApiBody = "/" + TransactionSelection;
            this.CaseReconciliationDataGridSource.read();
        }

        public GetCaseDataFromCaseNumber() {
            this.apiBody = "?CaseNumber=" + this.caseListStr
                + "&NewTransId=" + this.TransactionSelection
                + "&TransactionId=";
            this.CaseDataGridSource.read();
        }

        //public GetCaseDataFromDates() {
        //    this.apiBody = "?fromDate=" + this.caseFromDateString
        //        + "&toDate=" + moment(this.caseToDateString).add(1, 'days').format('MM/DD/YYYY');
        //    this.CaseDataGridSource.read();
        //}

        public GetCaseDataForGrid() {
            this.apiBody = "?fromDate=" + moment(this.caseFromDateString).format('MM/DD/YYYY')
                + "&toDate=" + moment(this.caseToDateString).format('MM/DD/YYYY')
                + "&TransactionId=" + this.TransactionSelection
                + "&CaseNumber=" + ""  // this.caseListStr
                + "&Payor=" + this.payorSelection
                + "&PayorGroup=" + this.payorGroupSelection
                + "&newTransId=" + this.TransactionVar;
            this.CaseDataGridSource.read();
        }

        public clearCaseGrid(): void {
            this.CaseDataGridSource.data([]);
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Initialises the case detail grid. (DETAIL GRID) NOT CURRENTLY USED </summary>
        ///
        /// <remarks>   Dtorres, 20160222. </remarks>
        ///
        /// <param name="$http">    The $http. </param>
        ///
        /// <returns>   A kendo.data.DataSource. </returns>
        ///-------------------------------------------------------------------------------------------------
        private InitCaseDetailGrid($http): kendo.data.DataSource {
            var tmodel = kendo.data.Model.define({
                id: "ID",
                fields: {
                    ID: { editable: false, type: "number" },
                    CaseNumber: { editable: false },
                    PayorGroupName: { editable: false },
                    PayorCode: { editable: false },
                    DateofService: { editable: false, type: "date" },
                    DefaultListPrice: { editable: false },
                    DefaultContractualAllowance: { editable: false },
                    ListPrice: { editable: false },
                    ContractualAllowance: { editable: true },
                    AllowanceDoubtful: { editable: true },
                    ChangeInEstimate: { editable: true },
                    RemainingBalance: { editable: false },
                    AllowableAmount: { editable: true },
                    Collections: { editable: false },
                    WriteOff: { editable: true },
                    WriteOffReasonCode: { editable: true },
                    InterestPayments: { editable: true },
                    LatePayments: { editable: true },
                }
            });
            var apirelpath = "api:/CasePayment";
            var ds: kendo.data.DataSource = CreateGridDataSource($http, 20, tmodel,
                {
                    read: () => {
                        return apirelpath + "/";
                        //return apirelpath + "?CaseNumber=" ;
                    },
                    update: () => {
                        return "api:/CasePayment/";
                    }

                });
            return ds;
        }

        private handleDetailInit(e) {

            var detailRow = e.detailRow;

            detailRow.find(".caseDetail").kendoGrid({
                dataSource: this.InitCaseDetailGrid(e),
                columns: [
                    //{ "field": "CaseNumber", "title": "CaseNumber", headerAttributes: { style: "white-space: normal" } },
                    { "field": "DateofService", "title": "DOS", format: "{0:MM/dd/yyyy}", headerAttributes: { style: "white-space: normal" } },
                    //{ "field": "PayorGroupName", "title": "Payor Group", headerAttributes: { style: "white-space: normal" } },
                    //{ "field": "PayorCode", "title": "Payor Code", headerAttributes: { style: "white-space: normal" } },
                    { "field": "DefaultContractualAllowance", "title": "Original Contractual Allowance", "format": "{0:c2}", headerAttributes: { style: "white-space: normal" } },
                    { "field": "ListPrice", "title": "List Price", "format": "{0:c2}", headerAttributes: { style: "white-space: normal" } },
                    { "field": "ContractualAllowance", "title": "Contractual Allowance", "format": "{0:c2}", headerAttributes: { style: "white-space: normal" } },
                    { "field": "AllowanceDoubtful", "title": "Allowance Doubtful", "format": "{0:c2}", headerAttributes: { style: "white-space: normal" } },
                    { "field": "ChangeInEstimate", "title": "Change In Estimate", "format": "{0:c2}", headerAttributes: { style: "white-space: normal" } },
                    //{ "field": "RemainingBalance", "title": "AR Balance", "format": "{0:c2}", headerAttributes: { style: "white-space: normal" } },
                    { "field": "AllowableAmount", "title": "Allowable Amount", "format": "{0:c2}", headerAttributes: { style: "white-space: normal" } },
                    { "field": "Collections", "title": "Collections", "format": "{0:c2}", headerAttributes: { style: "white-space: normal" } },
                    { "field": "WriteOff", "title": "WriteOff", "format": "{0:c2}", headerAttributes: { style: "white-space: normal" } },
                    //{ "field": "WriteOffReasonCode", "title": "Reason Code", values: this.writeOffReasonEnum, headerAttributes: { style: "white-space: normal" } },
                    { "field": "InterestPayments", "title": "Interest Payment", "format": "{0:c2}", headerAttributes: { style: "white-space: normal" } },
                    { "field": "LatePayments", "title": "Late Payment", "format": "{0:c2}", headerAttributes: { style: "white-space: normal" } },
                    { "command": ["edit"] }
                ]
            });
            var masterRow = e.masterRow;
            var originalModel: kendo.data.Model = e.data; //keep reference to the model
            var grid: kendo.ui.Grid = e.sender;
            var datacoll: kendo.data.ObservableArray = grid.dataSource.data();

            var editableModel = new this.dmodel(originalModel.toJSON());
            var selectedModel = originalModel;
            kendo.bind(detailRow, editableModel);
            var ignorelist = ['_events', '_handlers', 'uid', 'dirty'];
            var current = this;

            detailRow.find(".case-details > .k-button.save").click(function () {
                if (editableModel.dirty) {

                    var chgidx = datacoll.indexOf(selectedModel);
                    var modmodel = datacoll[chgidx];
                    var keys = Object.keys(editableModel);

                    for (var i = 0; i < keys.length; i++) {
                        var key = keys[i];
                        if (ignorelist.indexOf(key) < 0) {
                            if (editableModel[key] != datacoll[chgidx][key]) {
                                datacoll[chgidx][key] = editableModel[key];
                                datacoll[chgidx]['dirty'] = true;

                            }
                        }
                    }

                    //datacoll.splice(datacoll.indexOf(selectedModel), 1, editableModel);
                    try {
                        grid.dataSource.sync();
                    }
                    catch (ex) {
                        // we will typically get an error because of not having a destroy transport
                        // that does not really matter. So we suppress it here
                        if (ex.message != "Unable to get property 'call' of undefined or null reference") {
                            // try to get the original data back
                            datacoll.splice(datacoll.indexOf(selectedModel), 1, originalModel);
                            throw (ex);
                        }
                    }


                }
            });
            detailRow.find(".case-details > .k-button.cancel").click(function () {
                if (editableModel.dirty) {
                    editableModel = new kendo.data.Model(originalModel.toJSON());
                    kendo.bind(detailRow, editableModel);
                }
                grid.collapseRow(masterRow);
            });
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Case detail grid options. (DETAIL GRID) NOT CURRENTLY USED</summary>
        ///
        /// <remarks>   Dtorres, 20160222. </remarks>
        ///
        /// <param name="dataItem"> The data item. </param>
        ///
        /// <returns>   The kendo.ui.GridOptions. </returns>
        ///-------------------------------------------------------------------------------------------------

        public caseDetailGridOptions(dataItem: any): kendo.ui.GridOptions {
            return {
                dataSource: new kendo.data.DataSource({
                    //data: dataItem.CaseNumber.toJSON(),
                    transport: {
                        read: () => {
                            return "api:/CasePayment?CaseNumber=" + dataItem.CaseNumber.toString();
                        },
                        update: () => {
                            return "api:/CasePayment";
                        }
                    }
                }),
                columns: [
                    //{ "field": "CaseNumber", "title": "CaseNumber", headerAttributes: { style: "white-space: normal" } },
                    { "field": "DateofService", "title": "DOS", format: "{0:MM/dd/yyyy}", headerAttributes: { style: "white-space: normal" } },
                    //{ "field": "PayorGroupName", "title": "Payor Group", headerAttributes: { style: "white-space: normal" } },
                    //{ "field": "PayorCode", "title": "Payor Code", headerAttributes: { style: "white-space: normal" } },
                    { "field": "DefaultContractualAllowance", "title": "Original Contractual Allowance", "format": "{0:c2}", headerAttributes: { style: "white-space: normal" } },
                    { "field": "ListPrice", "title": "List Price", "format": "{0:c2}", headerAttributes: { style: "white-space: normal" } },
                    { "field": "ContractualAllowance", "title": "Contractual Allowance", "format": "{0:c2}", headerAttributes: { style: "white-space: normal" } },
                    { "field": "AllowanceDoubtful", "title": "Allowance Doubtful", "format": "{0:c2}", headerAttributes: { style: "white-space: normal" } },
                    { "field": "ChangeInEstimate", "title": "Change In Estimate", "format": "{0:c2}", headerAttributes: { style: "white-space: normal" } },
                    //{ "field": "RemainingBalance", "title": "AR Balance", "format": "{0:c2}", headerAttributes: { style: "white-space: normal" } },
                    { "field": "AllowableAmount", "title": "Allowable Amount", "format": "{0:c2}", headerAttributes: { style: "white-space: normal" } },
                    { "field": "Collections", "title": "Collections", "format": "{0:c2}", headerAttributes: { style: "white-space: normal" } },
                    { "field": "WriteOff", "title": "WriteOff", "format": "{0:c2}", headerAttributes: { style: "white-space: normal" } },
                    //{ "field": "WriteOffReasonCode", "title": "Reason Code", values: this.writeOffReasonEnum, headerAttributes: { style: "white-space: normal" } },
                    { "field": "InterestPayments", "title": "Interest Payment", "format": "{0:c2}", headerAttributes: { style: "white-space: normal" } },
                    { "field": "LatePayments", "title": "Late Payment", "format": "{0:c2}", headerAttributes: { style: "white-space: normal" } },
                    { "command": ["edit"] }
                ]
            };
        }


        private errorHandler(verbName: string, data: any, status: any, headers: any, config: any): void {
            console.log("DataGrid " + verbName + " error with status: " + status);
            console.log("Exception details: " + data.Message);
            console.log("Stacktrace: " + data.StackTraceString);
            throw { message: "Error doing " + verbName + ". Got Status " + status + ".\n" + data.Message, cause: status };
        }





    }
}