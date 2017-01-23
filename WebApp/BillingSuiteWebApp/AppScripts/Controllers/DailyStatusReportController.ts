///-------------------------------------------------------------------------------------------------
// <copyright file="DSRController.ts" company="Signal Genetics Inc.">
// Copyright (c) 2015 Signal Genetics Inc.. All rights reserved.
// </copyright>
// <author>Ssur</author>
// <date>20150922</date>
// <summary>dsr controller class</summary>
///-------------------------------------------------------------------------------------------------

module BillingSuiteApp.Controller {

    export class DailyStatusReportController {
        fromDate: Date;
        fromDateString: string;
        toDate: Date;
        toDateString: string;
        dataGridSource: kendo.data.DataSource;
        identifier: string;
        toolbarTemplate: any;

        onDateChange: Function;
 
        dailyStatusGridOptions: kendo.ui.GridOptions;
        getData: Function;
        searchCases: Function;
        apiBody: string;
        caseListStr: string;

        payorService: Service.PayorService;
        utilService: Service.UtilService;

        payorsList: any;
        payorCodesList: any;

        fromDateLocal: string;
        toDateLocal: string;

        constructor($rootScope, $http, PayorService: Service.PayorService,
            UtilService: Service.UtilService) {

            // make sure this is undefined so the grid 
            // does not resolve objects till we are ready
            this.payorService = PayorService;
            this.utilService = UtilService;
            this.identifier = $rootScope.AppBuildStatus + "Daily Status Report";
            
            this.toolbarTemplate = $("#toolbarTemplate").html();
            
            this.toDate = new Date();
            this.fromDate = new Date();
            this.fromDate.setDate(this.toDate.getDate() - 1);
            this.fromDateString = DateToUSString(this.fromDate);
            this.toDateString = DateToUSString(this.toDate);

            this.dataGridSource = this.initDataGridSource($http);

            var current: DailyStatusReportController = this;
            if (this.payorService.PayorsID2CodeMap) {
                // note that if we make the promise call (e.g on reload of a page)
                // when the payorlist etc may already be populated the page fails to load
                // this is a workaround till we figure out why this happens

                current.payorsList = PayorService.PayorsID2NameMap;
                current.payorCodesList = PayorService.PayorsID2CodeMap;
                current.initGrid();
            }
            else {
                this.payorService.GetPayorsAsync().then(function (data) {
                    current.payorsList = PayorService.PayorsID2NameMap;
                    current.payorCodesList = PayorService.PayorsID2CodeMap;
                    current.initGrid();
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
        /// <returns>   . </returns>
        ///-------------------------------------------------------------------------------------------------

        private initDataGridSource($http): kendo.data.DataSource{
            var apirelpath = "api:/BillingStatusCases";

            var dmodel = kendo.data.Model.define({
                id: "CaseNumber",
                fields: {
                    "PayorId1": { editable: false },
                    "DateofService": { type: "date", editable: false },
                    "OrderDate": { type: "date", editable: false }
                }
            });

            var ds: kendo.data.DataSource= CreateGridDataSource($http, 20, dmodel,
                {
                    read: () => {
                        return apirelpath + this.apiBody;
                    }
                });
            return ds;

        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Initialises the grid. </summary>
        ///
        /// <remarks>   Rphilavanh, 20151028. </remarks>
        ///
        /// <returns>   . </returns>
        ///-------------------------------------------------------------------------------------------------

        initGrid() {
            this.dailyStatusGridOptions = {
                dataSource: this.dataGridSource,
                scrollable: true,
                sortable: true,
                pageable: {
                    refresh: true,
                    pageSizes: true,
                },
                height: 750,
                autoBind: false,
                excel: {
                    fileName: moment().year().toString() + moment().month().toString() + moment().day().toString() + "_" + moment().hour().toString() + moment().minute().toString() + moment().second().toString() + "_SignalDailyStatusReport.xlsx",
                    allPages: true
                },
                toolbar: this.toolbarTemplate,
                columns: [
                    { "field": "CaseNumber", "title": "CaseNumber" },
                    { "field": "DateofService", "title": "Date of Service", "format": "{0:MM/dd/yyyy}" },
                    { "field": "OrderDate", "title": "OrderDate", "format": "{0:MM/dd/yyyy HH:mm zzz}"},
                    { "field": "ProgramGroup", "title": "ProgramGroup" },
                    { "field": "BillType", "title": "BillType" },
                    { "field": "PayorId1", "title": "Payor 1 Name", values: this.payorsList },
                    { "field": "PayorId1", "title": "Payor 1 Code", values: this.payorCodesList },
                    { "field": "PayorId2", "title": "Payor 2 Name", values: this.payorsList },
                    { "field": "PayorId2", "title": "Payor 2 Code", values: this.payorCodesList },
                    { "field": "PatientMRN", "title": "MRN" },
                    { "field": "ClientName", "title": "ClientName" },
                    { "field": "FacilityName", "title": "FacilityName" },
                    { "field": "ComputedICD10Codes", "title": "ICD 10(comp)" },
                    {
                        field: "CaseNumber", title: "Req Form",
                        template: '<button ng-click="vm.GetReqForm(dataItem.AccessionId)">{{dataItem.CaseNumber}}</button>',
                        width: 100

                    },
                ]

            }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets a data. </summary>
        ///
        /// <remarks>   Rphilavanh, 20151028. </remarks>
        ///
        /// <param name="event">    The event. </param>
        ///
        /// <returns>   The data. </returns>
        ///-------------------------------------------------------------------------------------------------

        public GetData(event) {

            this.apiBody = "?fromDate=" + this.fromDateString + "&toDate=" + moment(this.toDateString).add(1, 'days').format('MM/DD/YYYY') + "&datetype=orderdate";

            this.dataGridSource.read();
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Searches for the list of cases. </summary>
        ///
        /// <remarks>   Rphilavanh, 20151028. </remarks>
        ///
        /// <param name="event">    The event. </param>
        ///
        /// <returns>   The found cases. </returns>
        ///-------------------------------------------------------------------------------------------------

        public SearchCases(event) {
            if (this.caseListStr != null && this.caseListStr != undefined) {
                this.apiBody = "?caseListStr=" + this.caseListStr + "&delim=,";

                this.dataGridSource.read();
            }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets request form. </summary>
        ///
        /// <remarks>   Rphilavanh, 20151028. </remarks>
        ///
        /// <param name="accid">    The accid. </param>
        ///
        /// <returns>   The request form. </returns>
        ///-------------------------------------------------------------------------------------------------

        public GetReqForm(accid: number) {
            this.utilService.DownloadRequisitionForm(accid.toString());
        }
    }
}