///-------------------------------------------------------------------------------------------------
// <copyright file="BillReporterController.ts" company="Signal Genetics Inc.">
// Copyright (c) 2015 Signal Genetics Inc.. All rights reserved.
// </copyright>
// <author>Ssur</author>
// <date>20150924</date>
// <summary>bill reporter controller class</summary>
///-------------------------------------------------------------------------------------------------
/// fix for Typescript not recognising download
interface HTMLAnchorElement {
    download: string;
}
module BillingSuiteApp.Controller {
    export class BillReporterController {
        billReporterGridOptions: kendo.ui.GridOptions = undefined;

        httpServ: ng.IHttpService;
        qServ: ng.IQService;


        fromDate: Date;
        toDate: Date;
        fromDateString: string;
        toDateString: string;
        dateType: string;

        fromDateSummaryReport: string;
        toDateSummaryReport: string;

        identifier: string;
        toolbarTemplate: any;
        dataGridSource: kendo.data.DataSource;

        apiBody: string = "&datetype=orderdate";

        getData: Function;
        getUnbilled: Function;

        payorService: Service.PayorService;
        enumService: Service.EnumListService;
        utilServ: Service.UtilService;

        payorsList: any;
        payorCodesList: any;

        programGroupEnum: any;
        billingAggregateEnum: any;
        billingClassificationEnum: any;
        billStatusEnum: any;

        programGroupList: any;
        billingAggregateList: any;
        billingClassificationList: any;
        billStatusList: any;

        programGroupSelection: string;
        billingAggregateSelection: string;
        billingClassificationSelection: string;
        billStatusSelection: string;

        payorListReceived: boolean = false;
        enumListReceived: boolean = false;

        // View Panel
        panelBarBillReporter: kendo.ui.PanelBar;

        //Billing report download
        windowDownloadBillReports: kendo.ui.Window;
        dropdownBillEventsOptions: kendo.ui.DropDownListOptions;
        billingEventsList: any;
        billingEventCodeSelection: string;

        //Billing Action
        windowBillingAction: kendo.ui.Window;
        billingActionAggregateSelection: string;
        billingActionGridOptions: kendo.ui.GridOptions;
        billingActionDataGridSource: kendo.data.DataSource;
        billingActionComment: string;
        billingActionDownloadReport: boolean = true;
        notReadytoRunBillingAction: boolean = true;
        billingAggregateSelectionIsNotNone: boolean = false;
        noBillingEventSelectedtoRunReport: boolean = true;
        XifinCreateAccession: boolean = true;
        isNotXifinAggregateSelection: boolean = true;

        //Billing Validation
        billingEventData: Model.BillingEventModel;
        windowBillingCreation: kendo.ui.Window;
        windowBillingValidationWait: kendo.ui.Window;
        windowBillingCreationWait: kendo.ui.Window;
        validationHandler: BillReporterValidationHandler;
        billValidationStatusGridOptions: kendo.ui.GridOptions = undefined;
        billValidationStatusDataSource: kendo.data.DataSource = undefined;
        billValidationUpdateCount: number = 0;
        validationToolbarTemplate: any;
        checked: any;
        checkedrows: any[];
        item: any;
        itemss: any[];
        cases: any;


        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Initializes a new instance of the BillReporterController class. </summary>
        ///
        /// <remarks>   Ssur, 20150924. </remarks>
        ///
        /// <param name="$http">        The $http. </param>
        /// <param name="PayorService"> The payor service. </param>
        /// <param name="EnumService">  The enum service. </param>
        ///-------------------------------------------------------------------------------------------------

        constructor($rootScope, $http, $q,
            PayorService: Service.PayorService,
            EnumService: Service.EnumListService,
            UtilService: Service.UtilService) {

            this.billReporterGridOptions = undefined;

            this.httpServ = $http;
            this.qServ = $q;
            this.utilServ = UtilService;

            this.payorService = PayorService;
            this.enumService = EnumService;
            this.identifier = $rootScope.AppBuildStatus + "Bill Reporter";

            this.toolbarTemplate = $("#toolbarTemplate").html();

            this.dateType = "Completed Date";

            this.toDate = new Date();
            this.fromDate = new Date();
            this.fromDate.setDate(this.toDate.getDate() - 7);

            var tda = new Date();
            tda.setMonth(this.toDate.getMonth() - 1);


            this.fromDateString = DateToUSString(this.fromDate);
            this.fromDateSummaryReport = DateToUSString(tda);

            this.toDateString = DateToUSString(this.toDate);
            this.toDateSummaryReport = this.toDateString;

            this.payorListReceived = false;
            this.enumListReceived = false;

            this.dataGridSource = this.initDataGridSource($http);

            if (this.payorService.PayorsID2CodeMap) {
                this.payorListRecdProc();
            }
            else {
                var current: BillReporterController = this;
                this.payorService.GetPayorsAsync().then(function (data) {
                    current.payorListRecdProc();
                }, function (reason) {
                    throw reason;
                })
            }


            if (this.enumService.EnumServiceReady) {
                this.enumListRecdProc();
            }
            else {
                var current: BillReporterController = this;
                this.enumService.PopulateEnumListsAsync().then(function (data) {
                    current.enumListRecdProc();
                }, function (reason) {
                    throw reason;
                })
            }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Initialises the data grid source. </summary>
        ///
        /// <remarks>   Ssur, 20150925. </remarks>
        ///
        /// <param name="$http">    The $http. </param>
        ///
        /// <returns>   A kendo.data.DataSource. </returns>
        ///-------------------------------------------------------------------------------------------------

        private initDataGridSource($http): kendo.data.DataSource {
            var apirelpath = "api:/BillingStatusCases";


            var dmodel = kendo.data.Model.define({
                id: "CaseNumber",
                fields: {
                    "PayorId1": { editable: false },
                    "OrderDate": { type: "date", editable: false },
                    "DateofService": { type: "date", editable: false },
                    "CompletedDate": { type: "date", editable: false }
                }
            });

            var ds: kendo.data.DataSource = CreateGridDataSource($http, 20, dmodel,
                {
                    read: () => {
                        return apirelpath + this.apiBody;
                    }
                });
            return ds;

        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets a data. </summary>
        ///
        /// <remarks>   Ssur, 20150925. </remarks>
        ///
        /// <param name="event">    The event. </param>
        ///
        /// <returns>   The data. </returns>
        ///-------------------------------------------------------------------------------------------------

        public GetData(event) {
            var thirdArg: string;
            if (this.dateType == "Order Date")
                thirdArg = "&datetype=" + "orderdate";

            else
                thirdArg = "&datetype=" + "completeddate";

            this.apiBody = "?fromDate=" + this.fromDateString + "&toDate=" + moment(this.toDateString).add(1, 'days').format('MM/DD/YYYY') + thirdArg;

            this.dataGridSource.read();
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets an unbilled. </summary>
        ///
        /// <remarks>   Ssur, 20150925. </remarks>
        ///
        /// <param name="event">    The event. </param>
        ///
        /// <returns>   The unbilled. </returns>
        ///-------------------------------------------------------------------------------------------------

        public GetUnbilled(event) {
            this.apiBody = "?filter=unbilled";
            this.dataGridSource.read();
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Payor list received proc. </summary>
        ///
        /// <remarks>   Ssur, 20150925. </remarks>
        ///
        /// <returns>   . </returns>
        ///-------------------------------------------------------------------------------------------------

        private payorListRecdProc() {
            this.payorsList = this.payorService.PayorsID2NameMap;
            this.payorCodesList = this.payorService.PayorsID2CodeMap;
            this.payorListReceived = true;
            this.CallGridInit();
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Enum list received proc. </summary>
        ///
        /// <remarks>   Ssur, 20150925. </remarks>
        ///
        /// <returns>   . </returns>
        ///-------------------------------------------------------------------------------------------------

        private enumListRecdProc() {
            var enumlink: Model.EnumLinkModel;
            enumlink = this.enumService.GetEnumLink("BillStatus");
            this.billStatusEnum = enumlink.EnumMap;
            this.billStatusList = enumlink.EnumList;

            enumlink = this.enumService.GetEnumLink("ProgramGroup");
            this.programGroupEnum = enumlink.EnumMap;
            this.programGroupList = enumlink.EnumList;

            enumlink = this.enumService.GetEnumLink("BillingClassification");
            this.billingClassificationEnum = enumlink.EnumMap;
            this.billingClassificationList = enumlink.EnumList;


            enumlink = this.enumService.GetEnumLink("BillingAggregate");
            this.billingAggregateEnum = enumlink.EnumMap;
            this.billingAggregateList = enumlink.EnumList;

            this.enumListReceived = true;
            this.CallGridInit();
        }

      

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Call grid initialization: checks for all drop downs first </summary>
        ///
        /// <remarks>   Ssur, 20150924. </remarks>
        ///
        /// <returns>   . </returns>
        ///-------------------------------------------------------------------------------------------------

        CallGridInit() {
            if (this.payorListReceived && this.enumListReceived) {
                var current: BillReporterController = this;
                current.payorListReceived = true;
                current.initGrid();
            }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Initialises the grid. </summary>
        ///
        /// <remarks>   Ssur, 20150924. </remarks>
        ///
        /// <returns>   . </returns>
        ///-------------------------------------------------------------------------------------------------

        initGrid() {

            this.billReporterGridOptions = {
                dataSource: this.dataGridSource,
                scrollable: true,
                sortable: true,
                pageable: {
                    refresh: true,
                    pageSizes: true,
                },
                height: 600,
                autoBind: false,
                excel: {
                    fileName: moment().year().toString() + moment().month().toString() + moment().day().toString() + "_" + moment().hour().toString() + moment().minute().toString() + moment().second().toString() + "_SignalBillReporter.xlsx",
                    allPages: true
                },
                toolbar: this.toolbarTemplate,
                columns: [
                    { "field": "CaseNumber", "title": "CaseNumber" },
                    { "field": "ProgramGroup", "title": "Program" },
                    { "field": "BillingAggregate", "title": "Aggregate" },
                    { "field": "BillingClassification", "title": "Bill Class" },
                    { "field": "BillableStatus", "title": "Bill Status" },
                    { "field": "DateofService", "title": "Date of Service", "format": "{0:MM/dd/yyyy}" },
                    { "field": "OrderDate", "title": "Order Date", "format": "{0:MM/dd/yyyy HH:mm zzz}" },
                    { "field": "CompletedDate", "title": "Completed Date", "format": "{0:MM/dd/yyyy HH:mm zzz}" },
                    { "field": "ClientName", "title": "Client" },
                    { "field": "PayorId1", "title": "Payor 1", values: this.payorsList },
                    { "field": "PayorId2", "title": "Payor 2", values: this.payorsList },
                    { "field": "DoctorId", "title": "Physician", "template": '<span> {{dataItem.DoctorLastName}}</span>,<span> {{dataItem.DoctorFirstName}} </span>' },
                    { "field": "QNS", "title": "QNS" },
                    { "field": "QNSReason", "title": "QNSReason" },

                ]

            }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Executes the filter operation. </summary>
        ///
        /// <remarks>   Ssur, 20150924. </remarks>
        ///
        /// <param name="kendoEvent">   The kendo event. </param>
        ///
        /// <returns>   . </returns>
        ///-------------------------------------------------------------------------------------------------

        public RunFilter(kendoEvent) {
            var filter = {
                logic: "and",
                filters: []
            };
            if (this.dataGridSource.data().length > 0) {
                if (this.programGroupSelection && this.programGroupSelection.length > 0)
                    filter.filters.push({ field: "ProgramGroup", operator: "eq", value: this.programGroupSelection });
                if (this.billingClassificationSelection && this.billingClassificationSelection.length > 0)
                    filter.filters.push({ field: "BillingClassification", operator: "eq", value: this.billingClassificationSelection });
                if (this.billingAggregateSelection && this.billingAggregateSelection.length > 0)
                    filter.filters.push({ field: "BillingAggregate", operator: "eq", value: this.billingAggregateSelection });
                if (this.billStatusSelection && this.billStatusSelection.length > 0)
                    filter.filters.push({ field: "BillableStatus", operator: "eq", value: this.billStatusSelection });

                if (filter.filters.length > 0)
                    this.dataGridSource.filter([filter]);
                else
                    this.dataGridSource.filter([]);
            }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Throws up Popup for Billing report download. </summary>
        ///
        /// <remarks>   Ssur, 20150924. </remarks>
        ///
        /// <returns>   . </returns>
        ///-------------------------------------------------------------------------------------------------

        public BillingReportDownload() {
            this.billingEventsList = undefined;
            this.dropdownBillEventsOptions = undefined;
            var current: BillReporterController = this;
            this.httpServ.get("api:/BillingEvents", { withCredentials: true })
                .success(function (data, status, headers, config) {
                    current.billingEventsList = data;
                    current.billingEventCodeSelection = null;
                    current.noBillingEventSelectedtoRunReport = true;
                    current.dropdownBillEventsOptions = {
                        dataSource: new kendo.data.DataSource({ data: current.billingEventsList }),
                        dataTextField: "BillingEventCode",
                        dataValueField: "BillingEventCode",
                        index: 0,
                        optionLabel: "Select...",
                        headerTemplate: '<div class="dropdown-header k-widget k-header">' +
                        '<span>Billing Aggregate      </span>' +
                        '<span>Billing Event Code   </span>' +
                        '<span>Comments</span>' +
                        '</div>',
                        valueTemplate: '<span style="color:blue"> Event:</span> <span>{{dataItem.BillingEventCode}}</span> <span style="color:blue">Billing Aggregate:</span> <span class="selected-value"> {{dataItem.BillingAggregate}}</span> ',
                        template: ' <span style="color:blue"> Event:</span> <span>{{dataItem.BillingEventCode}}</span> <span style="color:blue">Billing Aggregate:</span> <span class="selected-value"> {{dataItem.BillingAggregate}}</span>  <span style="color:blue"> Comments:</span>  <span>{{dataItem.Comments}}</span>',

                    }
                    current.windowDownloadBillReports.title("Select a Billing Event Report to Download");
                    current.windowDownloadBillReports.open();
                    current.windowDownloadBillReports.center();
                })
                .error(function (data, status, headers, config) {
                    throw (data);
                });



        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Billing panel manager. </summary>
        ///
        /// <remarks>   Ssur, 20151028. </remarks>
        ///
        /// <param name="event">    The event. </param>
        ///
        /// <returns>   . </returns>
        ///-------------------------------------------------------------------------------------------------

        public BillingPanelManager(event) {

            if (event.item.id == "paneBillReviewer") {
            }
            else if (event.item.id == "paneBillValidate") {
                this.createBillingValidationView();
                //$("#panelBarBillReporter").kendoPanelBar().data("panelBillValidate").expand(">li:first");
            }
            //else if (event.item.id == "paneBillAction") {
            //    this.CreateBillingActionView();
            //}
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Billing action popup. </summary>
        ///
        /// <remarks>   Ssur, 20150925. </remarks>
        ///
        /// <returns>   . </returns>
        ///-------------------------------------------------------------------------------------------------

        public createBillingValidationView() {

            if (!this.billingActionDataGridSource) {
                var dmodel = kendo.data.Model.define({
                    id: "CaseNumber",
                    fields: {
                        "PayorId1": { editable: false },
                        "OrderDate": { type: "date", editable: false },
                        "DateofService": { type: "date", editable: false },
                        "CompletedDate": { type: "date", editable: false }
                    }
                });
                var current: BillReporterController = this;
                this.billingActionDataGridSource = CreateGridDataSource(this.httpServ, 20, dmodel,
                    {
                        read: () => {
                            var thirdArg: string;
                            if (current.dateType == "Order Date")
                                thirdArg = "&datetype=" + "orderdate";

                            else
                                thirdArg = "&datetype=" + "completeddate";

                            return "api:/BillingStatusCases?fromDate=" + current.fromDateString +
                                "&toDate=" + moment(current.toDateString).add(1, 'days').format('MM/DD/YYYY') +
                                "&billingAggregate=" + current.billingActionAggregateSelection + thirdArg;
                        }
                    });


                this.initBillingActionGrid();
            }

            //this.windowBillingAction.title("Create Billing Action");
            //this.windowBillingAction.open();
            //this.windowBillingAction.center();
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Fills billing action grid. </summary>
        ///
        /// <remarks>   Ssur, 20150925. </remarks>
        ///
        /// <param name="event">    The event. </param>
        ///
        /// <returns>   . </returns>
        ///-------------------------------------------------------------------------------------------------

        public FillBillingActionGrid(event) {
            this.isNotXifinAggregateSelection = true;
            if (this.billingActionAggregateSelection && this.billingActionAggregateSelection.length > 0) {
                if (this.billingActionAggregateSelection == "Xifin") {
                    this.isNotXifinAggregateSelection = false;
                }
                this.billingAggregateSelectionIsNotNone = true;
                this.billingActionDataGridSource.read();
                this.CheckRunBillingActionPossible();
            }
            else {
                this.billingAggregateSelectionIsNotNone = false;
                if (this.billingActionDataGridSource)
                    this.billingActionDataGridSource.data([]);
                this.CheckRunBillingActionPossible()
            }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Check run billing action possible. </summary>
        ///
        /// <remarks>   Checks if the billing action is possible </remarks>
        ///
        /// <returns>   . </returns>
        ///-------------------------------------------------------------------------------------------------

        public CheckRunBillingActionPossible() {
            this.notReadytoRunBillingAction = true;
            if (this.billingAggregateSelectionIsNotNone)
                if (this.billingActionComment && this.billingActionComment.trim().length > 0)
                    this.notReadytoRunBillingAction = false;

        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Check if a billing event is selected during Billing Report.
        ///              </summary>
        ///
        /// <remarks>   Rphilavanh, 20151021. </remarks>
        ///
        /// <returns>   . </returns>
        ///-------------------------------------------------------------------------------------------------

        public CheckBillingEventSelected() {
            this.noBillingEventSelectedtoRunReport = true;
            if (this.billingEventCodeSelection != null && this.billingEventCodeSelection != undefined)
                this.noBillingEventSelectedtoRunReport = false;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Initialises the billing action grid. </summary>
        ///
        /// <remarks>   Ssur, 20150925. </remarks>
        ///
        /// <returns>   . </returns>
        ///-------------------------------------------------------------------------------------------------

        initBillingActionGrid() {
            this.billingActionGridOptions = {
                dataSource: this.billingActionDataGridSource,
                scrollable: true,
                sortable: true,
                pageable: {
                    refresh: true,
                    pageSizes: true,
                },
                height: 300,
                autoBind: false,
                columns: [
                    { "field": "CaseNumber", "title": "CaseNumber" },
                    { "field": "ProgramGroup", "title": "Program" },
                    { "field": "BillingClassification", "title": "Bill Class" },
                    { "field": "BillableStatus", "title": "Bill Status" },
                    { "field": "DateofService", "title": "Date of Service", "format": "{0:MM/dd/yyyy}" },
                    { "field": "OrderDate", "title": "Order Date", "format": "{0:MM/dd/yyyy HH:mm}" },
                    { "field": "CompletedDate", "title": "Completed Date", "format": "{0:MM/dd/yyyy HH:mm}" },
                    { "field": "ClientName", "title": "Client" },
                    { "field": "PayorId1", "title": "Payor 1", values: this.payorsList },
                    { "field": "PayorId2", "title": "Payor 2", values: this.payorsList },
                    { "field": "DoctorId", "title": "Physician", "template": '<span> {{dataItem.DoctorLastName}}</span>,<span> {{dataItem.DoctorFirstName}} </span>' },
                    { "field": "QNS", "title": "QNS" },
                    { "field": "QNSReason", "title": "QNSReason" },

                ]

            }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Executes the billing action validation operation. </summary>
        ///
        /// <remarks>   Ssur, 20150930. </remarks>
        ///
        /// <returns>   . </returns>
        ///-------------------------------------------------------------------------------------------------

        public RunBillingActionValidation() {
            this.validationHandler = new BillReporterValidationHandler(this.httpServ, this.qServ);
            this.windowBillingValidationWait.title('Please wait for validation: ' + this.billingActionAggregateSelection);
            this.windowBillingValidationWait.open();
            this.windowBillingValidationWait.center();
            var current: BillReporterController = this;

            this.validationHandler.ValidateBillingAggregate(this.fromDateString, this.toDateString, this.billingActionAggregateSelection, this.dateType)
                .then(function (data) {
                    current.billingEventData = current.validationHandler.BillingEventData;

                    current.billValidationStatusGridOptions = current.validationHandler.billValidationStatusGridOptions;
                    current.billValidationStatusDataSource = current.validationHandler.billingValidationStatusDataSource;
                    current.billValidationUpdateCount++; // trap this as a single event for a rebind
                    current.windowBillingValidationWait.close();
                    var panelBar = $("#panelBarBillReporter").data("kendoPanelBar");
                    panelBar.expand("#paneBillAction", true);
                    panelBar.select().find("#paneBillAction").addClass("k-state-selected").addClass("k-state-focused");
                }, function (reason) {
                    current.windowBillingValidationWait.close();
                    throw reason;
                })
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Select all valid cases. </summary>
        ///
        /// <remarks>   Ssur, 20151015. </remarks>
        ///
        /// <param name="event">    The event. </param>
        ///
        /// <returns>   . </returns>
        ///-------------------------------------------------------------------------------------------------

        public SelectAllValidCases(event: any) {
            var data = this.billValidationStatusDataSource.data();
            var cl: Array<string> = [];
            for (var i = 0; i < data.length; i++) {
                if (data[i].IsSelectable)
                    data[i].IsSelected = true;
                else
                    data[i].IsSelected = false;
            }

        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Executes the billing action submit operation. </summary>
        ///
        /// <remarks>   Ssur, 20151015. </remarks>
        ///
        /// <returns>   . </returns>
        ///-------------------------------------------------------------------------------------------------

        public RunBillingActionSubmit() {
            this.validationHandler = new BillReporterValidationHandler(this.httpServ, this.qServ);
            this.windowBillingCreationWait.title('Wait for bill submission: ' + this.billingEventData.BillingAggregate);
            this.windowBillingCreationWait.open();
            this.windowBillingCreationWait.center();
            var current: BillReporterController = this;

            var data = this.billValidationStatusDataSource.data();
            var cl: Array<string> = [];
            for (var i = 0; i < data.length; i++) {
                if (data[i].IsSelected && data[i].IsSelectable)
                    cl.push(data[i].CaseNumber);
            }


            this.validationHandler.PerformBilling(cl, this.billingEventData.BillingAggregate, this.billingActionComment, this.XifinCreateAccession)
                .then(function (data) {
                    current.billingEventData = current.validationHandler.BillingEventData;
                    current.billValidationStatusGridOptions = current.validationHandler.billValidationStatusGridOptions;
                    current.windowBillingCreationWait.close();
                    current.windowBillingCreation.title('Billing Action Report for Billing Code:' + current.billingEventData.BillingEventCode);

                    current.windowBillingCreation.open();
                    current.windowBillingCreation.options.modal = true;
                    current.windowBillingCreation.center();
                   
                    //current.SelectedBillingReportDownload(current.billingEventData.BillingEventCode, "Excel");

                }, function (reason) {
                    current.windowBillingCreationWait.close();
                    throw reason;
                })
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Downloads selected report </summary>
        ///
        /// <remarks>   Ssur, 20150924. </remarks>
        ///
        /// <returns>   . </returns>
        ///-------------------------------------------------------------------------------------------------

        public BillingReportFileDownload(billid: string, dltype: string) {

            this.utilServ.BillingReportFileDownloadService(billid, dltype);
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Billing summary report file download. </summary>
        ///
        /// <remarks>   Ssur, 20151028. </remarks>
        ///
        /// <returns>   . </returns>
        ///-------------------------------------------------------------------------------------------------

        public BillingSummaryReportFileDownload() {
            this.utilServ.BillingRangeReportFileDownloadService(this.fromDateSummaryReport, this.toDateSummaryReport);
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Specified the grid options for the detail grids within the Billing Action panel. </summary>
        ///
        /// <remarks>   Dtorres, 20151014. </remarks>
        ///
        /// <param name="dataItem"> The data item. </param>
        ///
        /// <returns>   kendo.ui.GridOptions </returns>
        ///-------------------------------------------------------------------------------------------------

        public detailGridOptions(dataItem: any): kendo.ui.GridOptions {           

            if (dataItem["BillingTarget"] == "Xifin") {
                return {
                    dataSource: new kendo.data.DataSource({
                        data: dataItem.XifinValidationStatus.toJSON()
                    }),
                    columns: [
                        { field: "Code", title: "Code", width: "56px" },
                        { field: "Description", title: "Description", width: "110px" },
                        { field: "ErrorNote", title: "Notes", width: "110px" },
                        { field: "PayorId", title: "PayorId", width: "110px" },
                        { field: "FiledErrorMessage", title: "Error Message", width: "110px" },
                        { field: "Source", title: "Source", width: "56px" }
                    ]
                };
            }
            else {
                return {
                    dataSource: new kendo.data.DataSource({
                        data: dataItem.NonXifinValidationStatus.toJSON()
                    }),
                    columns: [
                        { field: "Code", title: "Code", width: "56px" },
                        { field: "Description", title: "Description", width: "110px" },
                        { field: "ErrorNote", title: "Notes", width: "110px" },
                        { field: "Source", title: "Source", width: "56px" }
                    ]
                };
            }
        }
        
    }
}