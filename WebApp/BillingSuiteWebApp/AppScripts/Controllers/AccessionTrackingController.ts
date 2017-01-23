///-------------------------------------------------------------------------------------------------
// <copyright file="AccessionTrackingController.ts" company="Signal Genetics Inc.">
// Copyright (c) 2016 Signal Genetics Inc.. All rights reserved.
// </copyright>
// <author>Rphilavanh</author>
// <date>20160111</date>
// <summary>accession tracking controller class</summary>
///-------------------------------------------------------------------------------------------------

module BillingSuiteApp.Controller {
    export class AccessionTrackingController {

        accessionTrackingGridOptions: kendo.ui.GridOptions = undefined;
        httpServ: ng.IHttpService;
        qServ: ng.IQService;
        pageSize: number;

        fromDate: Date;
        toDate: Date;
        fromDateString: string;
        toDateString: string;
        dateType: string;
        caseListStr: string;

        identifier: string;
        toolbarTemplate: any;
        detailTemplate: any;
        accessionTrackingDataGridSource: kendo.data.DataSource;

        apiBody: string;
        getData: Function;

        enumService: Service.EnumListService;
        enumListReceived: boolean = false;
        billStatusEnum: any;
        billStatusList: any;
        billStatusSelection: string;
        xifinStatusEnum: any;
        xifinStatusList: any;
        xifinStatusSelection: string;
        statementStatusEnum: any;
        statementStatusList: any;
        statementStatusSelection: string;

        editRowDataModel: kendo.data.Model;
        dataModel: any;

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Initializes a new instance of the AccessionTrackingController class. </summary>
        ///
        /// <remarks>   Rphilavanh, 20160210. </remarks>
        ///
        /// <param name="$rootScope">   The $root scope. </param>
        /// <param name="$http">        The $http. </param>
        /// <param name="$q">           The $q. </param>
        /// <param name="EnumService">  The enum service. </param>
        ///-------------------------------------------------------------------------------------------------

        constructor($rootScope, $http, $q, EnumService: Service.EnumListService) {

            this.identifier = $rootScope.AppBuildStatus + "Billing Tracker";
            this.httpServ = $http;
            this.qServ = $q;

            this.toolbarTemplate = $("#toolbarTemplate").html();
            this.detailTemplate = $("#detailTemplate").html();
            this.pageSize = 50;

            this.enumService = EnumService;

            this.dateType = "Completed Date";
            this.toDate = new Date();
            this.fromDate = new Date();
            this.fromDate.setDate(this.toDate.getDate() - 30);
            this.fromDateString = DateToUSString(this.fromDate);
            this.toDateString = DateToUSString(this.toDate);

            this.accessionTrackingDataGridSource = this.initDataGridSource($http);
            this.accessionTrackingGridOptions = undefined;

            this.enumListReceived = false;

            console.log("AccessionTrackingController init complete.");

            if (this.enumService.EnumServiceReady) {
                this.enumListRecdProc();
            }
            else {
                var current: AccessionTrackingController = this;
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
        /// <remarks>   Rphilavanh, 20160210. </remarks>
        ///
        /// <param name="$http">    The $http. </param>
        ///
        /// <returns>   A kendo.data.DataSource. </returns>
        ///-------------------------------------------------------------------------------------------------

        private initDataGridSource($http): kendo.data.DataSource {
            var apirelpath = "api:/AccessionTracking";

            this.dataModel = kendo.data.Model.define({
                id: "CaseNumber",
                fields: {
                    IsCurrent: { editable: false },
                    SGNLStatus: { editable: false },
                    XifinStatus: { editable: false },
                    StatusChangeCount: { editable: false },
                    OpenErrorCount: { editable: false },
                    CurrentErrors: { editable: false },
                    TotalErrors: { editable: false },
                    StatementStatus: { editable: false },
                    BilledPrice: { editable: false },
                    BalanceDue: { editable: false },
                    DateOfService: { type: "date", editable: false },
                    OrderDate: { type: "date", editable: false },
                    CompletedDate: { type: "date", editable: false },
                    SGNLXifinSubmitDate: { type: "date", editable: false },
                    SGNLXifinSubmitter: { editable: false },
                    //   XifinSubmitDate: { type: "date", editable: false },
                    PricedDate: { type: "date", editable: false },
                    TrackedClosedDate: { type: "date", editable: false },
                    LastTrackingUpdate: { type: "date", editable: false },
                    CurrentErrorsDetail: { editable: false },
                }
            });

            var ds: kendo.data.DataSource = CreateGridDataSource($http, 25, this.dataModel,
                {
                    read: () => {
                        return apirelpath + this.apiBody;
                    },
                    update: () => {
                        return apirelpath;
                    }
                });
            return ds;

        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets a data. </summary>
        ///
        /// <remarks>   Rphilavanh, 20160210. </remarks>
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

            this.accessionTrackingDataGridSource.read();
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Enum list received proc. </summary>
        ///
        /// <remarks>   Rphilavanh, 20160210. </remarks>
        ///
        /// <returns>   . </returns>
        ///-------------------------------------------------------------------------------------------------

        private enumListRecdProc() {
            var enumlink: Model.EnumLinkModel;
            enumlink = this.enumService.GetEnumLink("BillStatus");
            this.billStatusEnum = enumlink.EnumMap;
            this.billStatusList = enumlink.EnumList;

            enumlink = this.enumService.GetEnumLink("XifinStatus");
            this.xifinStatusEnum = enumlink.EnumMap;
            this.xifinStatusList = enumlink.EnumList;

            enumlink = this.enumService.GetEnumLink("StatementStatus");
            this.statementStatusEnum = enumlink.EnumMap;
            this.statementStatusList = enumlink.EnumList;

            this.enumListReceived = true;
            this.CallGridInit();
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Call grid initialise. </summary>
        ///
        /// <remarks>   Rphilavanh, 20160210. </remarks>
        ///
        /// <returns>   . </returns>
        ///-------------------------------------------------------------------------------------------------

        CallGridInit() {
            if (this.enumListReceived) {
                var current: AccessionTrackingController = this;
                current.initGrid();
            }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>  Handles the dropdown detail - copied from CaseEditorController.ts. and modified </summary>
        ///
        /// <remarks>   Rphilavanh, 20160210. </remarks>
        ///
        /// <param name="e">    The unknown to process. </param>
        ///
        /// <returns>   . </returns>
        ///-------------------------------------------------------------------------------------------------

        private handleDetailInit(e) {

            var detailRow = e.detailRow;

            //populate details grid
            detailRow.find(".Error-details").kendoTabStrip({
                animation: {
                    open: { effects: "fadeIn" }
                }
            });

            detailRow.find(".notes").kendoGrid({
                dataSource: this.initContactNotesDetailsGridDataSource(e),
                columns: [
                    { field: "UserId", title: "Contact User", width: "40px" },
                    { field: "ContactDate", title: "Contact Date", width: "50px", format: "{0:MM/dd/yyyy}" },
                    { field: "ContactInfo", title: "Contact Info", width: "100px" },
                    { field: "FollowUpDate", title: "FollowUp Date", width: "50px", format: "{0:MM/dd/yyyy}" },
                    { field: "FollowUpUserId", title: "FollowUp User", width: "30px" },
                    { field: "PrintableNotes", title: "Printable Note", width: "80px" },
                    { field: "PrintOnStatement", title: "Print on STMT", width: "30px" },
                    { field: "FollowUpComplete", title: "FollowUp Complete", width: "40px" },
                    { field: "Voided", title: "Voided", width: "20px" },
                ]
            });

            detailRow.find(".currErr").kendoGrid({
                dataSource: this.initErrorsDetailsGridDataSource(e, false),
                columns: [
                    { field: "ErrorDate", title: "Error Date", width: "50px", format: "{0:MM/dd/yyyy}" },
                    { field: "FixedDate", title: "Fixed Date", width: "50px", format: "{0:MM/dd/yyyy}" },
                    { field: "ReasonCode", title: "Code", width: "50px" },
                    { field: "Description", title: "Description", width: "100px" },
                    { field: "PayorPriority", title: "Payor Priority", width: "50px" },
                    { field: "PayorId", title: "PayorId", width: "60px" },
                    { field: "ErrorGroup", title: "Error Group", width: "40px" },
                    { field: "DetailDescription", title: "Detail Description", width: "240px" },
                    { field: "ErrorNote", title: "Error Note", width: "100px" },
                    { field: "FiledErrorMessage", title: "Filed Error Message", width: "80px" },
                ]
            });

            detailRow.find(".allErr").kendoGrid({
                dataSource: this.initErrorsDetailsGridDataSource(e, true),
                columns: [
                    { field: "ErrorDate", title: "Error Date", width: "50px", format: "{0:MM/dd/yyyy}" },
                    { field: "FixedDate", title: "Fixed Date", width: "50px", format: "{0:MM/dd/yyyy}" },
                    { field: "ReasonCode", title: "Code", width: "50px" },
                    { field: "Description", title: "Description", width: "100px" },
                    { field: "PayorPriority", title: "Payor Priority", width: "50px" },
                    { field: "PayorId", title: "PayorId", width: "60px" },
                    { field: "ErrorGroup", title: "Error Group", width: "40px" },
                    { field: "DetailDescription", title: "Detail Description", width: "240px" },
                    { field: "ErrorNote", title: "Error Note", width: "100px" },
                    { field: "FiledErrorMessage", title: "Filed Error Message", width: "80px" },
                ]
            });



            var masterRow = e.masterRow;
            var originalModel: kendo.data.Model = e.data; //keep reference to the model
            var grid: kendo.ui.Grid = e.sender;
            //var viewCollection: kendo.data.ObservableArray = grid.dataSource.view(); // this does not work because it does not make the datasource dirty
            var datacoll: kendo.data.ObservableArray = grid.dataSource.data();

            var editableModel = new this.dataModel(originalModel.toJSON());
            var selectedModel = originalModel;
            kendo.bind(detailRow, editableModel);
            var ignorelist = ['_events', '_handlers', 'uid', 'dirty'];
            var current = this;

            detailRow.find(".ErrorRefresh > .k-button.save").click(function () {

                //keep expanded row(s) open after refresh
                var expanded = $.map(grid.tbody.children(":has(> .k-hierarchy-cell .k-minus)"), function (row) {
                    return $(row).data("uid");
                });
                grid.one("dataBound", function () {
                    grid.expandRow(grid.tbody.children().filter(function (idx, row) {
                        return $.inArray($(row).data("uid"), expanded) >= 0;
                    }));
                });


                // if (editableModel.dirty) { //Update row even though there is no change in the grid

                var chgidx = datacoll.indexOf(selectedModel);
                var modmodel = datacoll[chgidx];
                var keys = Object.keys(editableModel);
                for (var i = 0; i < keys.length; i++) {
                    var key = keys[i];
                    if (ignorelist.indexOf(key) < 0) {
                        datacoll[chgidx]['dirty'] = true; // little hack to make refresh work
                    }
                }
                //current.updateFromXifin(selectedModel);
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
            );
        }

        public initContactNotesDetailsGridDataSource(e) {

            var dmodel = kendo.data.Model.define({
                id: "id",
                fields: {
                    "ContactDate": { type: "date" },
                    "FollowUpDate": { type: "date" }
                }
            });

            var ds = CreateGridDataSource(this.httpServ, 20, dmodel,
                {
                    read: () => {
                        return "api:/AccessionContactNotes?caseno=" + e.data.CaseNumber;
                    }
                });
            return ds;

        }

        public initErrorsDetailsGridDataSource(e, getAll) {

            var dmodel = kendo.data.Model.define({
                id: "ID",
                fields: {
                    "ErrorDate": { type: "date" },
                    "FixedDate": { type: "date" }
                }
            });

            var ds = CreateGridDataSource(this.httpServ, 20, dmodel,
                {
                    read: () => {
                        return "api:/AccessionErrors?caseno=" + e.data.CaseNumber +
                            "&getAll=" + getAll;
                    }
                });
            return ds;

        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Initialises the grid. </summary>
        ///
        /// <remarks>   Rphilavanh, 20160210. </remarks>
        ///
        /// <returns>   . </returns>
        ///-------------------------------------------------------------------------------------------------

        initGrid() {

            var current: AccessionTrackingController = this;
            this.accessionTrackingGridOptions = {
                dataSource: this.accessionTrackingDataGridSource,
                scrollable: true,
                sortable: true,
                selectable: true,
                pageable: {
                    refresh: true,
                    pageSizes: true,
                },
                height: 750,
                autoBind: false,
                excel: {
                    fileName: moment().year().toString() + moment().month().toString() + moment().day().toString() + "_" + moment().hour().toString() + moment().minute().toString() + moment().second().toString() + "_SignalAccessionTrackingReport.xlsx",
                    allPages: true
                },
                toolbar: this.toolbarTemplate,
                detailTemplate: this.detailTemplate,
                detailInit: function (e) {
                    current.handleDetailInit(e);
                },
                columns: [
                    { field: "CaseNumber", title: "Case Number", width: "100px", headerAttributes: { style: "white-space: normal" } },
                    { field: "SGNLStatus", title: "SGNL Status", width: "100px", headerAttributes: { style: "white-space: normal" } },
                    { field: "XifinStatus", title: "Xifin Status", width: "100px", headerAttributes: { style: "white-space: normal" } },
                    { field: "StatusChangeCount", title: "# Status Changed", width: "60px", headerAttributes: { style: "white-space: normal" } },
                    { field: "OpenErrorCount", title: "Open Errors", width: "60px", headerAttributes: { style: "white-space: normal" } },
                    { field: "CurrentErrors", title: "Current Errors", width: "60px", headerAttributes: { style: "white-space: normal" } },
                    { field: "TotalErrors", title: "Total Errors", width: "60px", headerAttributes: { style: "white-space: normal" } },
                    { field: "StatementStatus", title: "Statement Status", width: "80px", headerAttributes: { style: "white-space: normal" } },
                    { field: "BilledPrice", title: "Billed Price", width: "90px", headerAttributes: { style: "white-space: normal" } },
                    { field: "BalanceDue", title: "Balance Due", width: "90px", headerAttributes: { style: "white-space: normal" } },
                    { field: "DateOfService", title: "Date of Service", "format": "{0:MM/dd/yyyy}", width: "100px", headerAttributes: { style: "white-space: normal" } },
                    { field: "OrderDate", title: "Order Date", "format": "{0:MM/dd/yyyy HH:mm}", width: "100px", headerAttributes: { style: "white-space: normal" } },

                    { field: "CompletedDate", title: "Completed Date", "format": "{0:MM/dd/yyyy HH:mm}", width: "120px", headerAttributes: { style: "white-space: normal" } },
                    { field: "SGNLXifinSubmitDate", title: "Billed On", "format": "{0:MM/dd/yyyy HH:mm}", width: "100px", headerAttributes: { style: "white-space: normal" } },
                    { field: "SGNLXifinSubmitter", title: "Billed By", width: "120px", headerAttributes: { style: "white-space: normal" } },
                    //  { field: "XifinSubmitDate", title: "FnlRptd Date", "format": "{0:MM/dd/yyyy}", width: "100px", headerAttributes: { style: "white-space: normal" }  },
                    { field: "PricedDate", title: "Priced Date", "format": "{0:MM/dd/yyyy}", width: "80px", headerAttributes: { style: "white-space: normal" } },
                    { field: "TrackedClosedDate", title: "ZBal Detected Date", "format": "{0:MM/dd/yyyy}", width: "100px", headerAttributes: { style: "white-space: normal" } },
                    { field: "LastTrackingUpdate", title: "Last Tracking Update", "format": "{0:MM/dd/yyyy HH:mm:ss}", width: "120px", headerAttributes: { style: "white-space: normal" } },
                    { field: "CurrentErrorsDetail", title: "Current Errors Summary", width: "250px", headerAttributes: { style: "white-space: normal" } },
                ],
                editable: "inline"

            }
        }


        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Executes the filter operation. </summary>
        ///
        /// <remarks>   Rphilavanh, 20160211. </remarks>
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
            if (this.accessionTrackingDataGridSource.data().length > 0) {
                if (this.billStatusSelection && this.billStatusSelection.length > 0)
                    filter.filters.push({ field: "SGNLStatus", operator: "eq", value: this.billStatusSelection });
                if (this.xifinStatusSelection && this.xifinStatusSelection.length > 0)
                    filter.filters.push({ field: "XifinStatus", operator: "eq", value: this.xifinStatusSelection });
                if (this.statementStatusSelection && this.statementStatusSelection.length > 0)
                    filter.filters.push({ field: "StatementStatus", operator: "eq", value: this.statementStatusSelection });

                if (filter.filters.length > 0)
                    this.accessionTrackingDataGridSource.filter([filter]);
                else
                    this.accessionTrackingDataGridSource.filter([]);
            }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Searches for the first cases. </summary>
        ///
        /// <remarks>   Rphilavanh, 20160211. </remarks>
        ///
        /// <param name="event">    The event. </param>
        ///
        /// <returns>   The found cases. </returns>
        ///-------------------------------------------------------------------------------------------------

        public SearchCases(event) {
            if (this.caseListStr != null && this.caseListStr != undefined) {
                this.apiBody = "?caseListStr=" + this.caseListStr + "&delim=,";

                this.accessionTrackingDataGridSource.read();
            }
        }

    }
}