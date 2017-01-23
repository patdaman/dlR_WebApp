///-------------------------------------------------------------------------------------------------
// <copyright file="PayorEditorController.ts" company="Signal Genetics Inc.">
// Copyright (c) 2015 Signal Genetics Inc.. All rights reserved.
// </copyright>
// <author>PdelosReyes</author>
// <date>20150922</date>
// <summary>Payor editor controller class</summary>
///-------------------------------------------------------------------------------------------------

module BillingSuiteApp.Controller {
    export class PayorEditorController {

        identifier: string;
        gridOptions: kendo.ui.GridOptions;
        dataGridSource: kendo.data.DataSource;

        httpServ: ng.IHttpService;
        qServ: ng.IQService;
        payorGroupService: Service.PayorGroupService;
        enumService: Service.EnumListService;

        payorGroupList: any;
        payorGroupListReceived: boolean = false;
        enumListReceived: boolean = false;

        placeOfServiceEnum: any;
        placeOfServiceList: any;
        placeOfServiceSelection: string;

        toolbarTemplate: any;
        pageSize: number;
        filterPayorCode: string = "";
        filterPayorName: string = "";
        filterPayorGroup: string = "";


        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Default constructor. </summary>
        ///
        /// <remarks>   PdelosReyes, 20150929. </remarks>
        ///-------------------------------------------------------------------------------------------------
        constructor($rootScope, $http, $q,
            PayorGroupService: Service.PayorGroupService,
            EnumService: Service.EnumListService) {

            this.httpServ = $http;
            this.qServ = $q;
            this.gridOptions = undefined;
            this.payorGroupService = PayorGroupService;
            this.enumService = EnumService;

            this.payorGroupListReceived = false;
            this.enumListReceived = false;

            this.identifier = $rootScope.AppBuildStatus + "Payor Editor";
            this.toolbarTemplate = $("#toolbarTemplate").html();
            this.pageSize = 50;

            console.log("PE: Setting datasource");
            this.dataGridSource = this.initDataGridSource($http);

            if (this.payorGroupService.PayorGroupID2NameMap) {
                this.payorGroupListRecdProc();
            }
            else {
                var current: PayorEditorController = this;
                this.payorGroupService.GetPayorGroupAsync().then(function (data) {
                    current.payorGroupListRecdProc();
                }, function (reason) {
                    console.log("Went to hell in a handbasket");
                })
            }

            if (this.enumService.EnumServiceReady) {
                this.enumListRecdProc();
            }
            else {
                var current: PayorEditorController = this;
                this.enumService.PopulateEnumListsAsync().then(function (data) {
                    current.enumListRecdProc();
                }, function (reason) {
                    console.log("Error loading enums");
                })
            }
        }

        private enumListRecdProc() {
            var enumlink: Model.EnumLinkModel;
            enumlink = this.enumService.GetEnumLink("PlaceOfService");
            this.placeOfServiceEnum = enumlink.EnumMap;
            this.placeOfServiceList = enumlink.EnumList;

            this.enumListReceived = true;
            this.CallGridInit();
        }
                       
        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Executes the filter operation. </summary>
        ///
        /// <remarks>   Ssur, 20150923. </remarks>
        ///
        /// <param name="kendoEvent">   The kendo event. </param>
        ///
        /// <returns>   . </returns>
        ///-------------------------------------------------------------------------------------------------

        public RunFilter(kendoEvent) {
            //alert("Running Filters");
            var filter = {
                logic: "and",
                filters: [{
                    "field": "Name",
                    "operator": "neq",
                    "value": "Unassigned"
                }]
            };
            if (this.filterPayorCode && this.filterPayorCode.length > 0)
                filter.filters.push({ field: "PayorCode", operator: "startswith", value: this.filterPayorCode });
            if (this.filterPayorName && this.filterPayorName.length > 0)
                filter.filters.push({ field: "Name", operator: "startswith", value: this.filterPayorName });
            if (this.filterPayorGroup && this.filterPayorGroup.length > 0)
                filter.filters.push({ field: "PayorGroup", operator: "startswith", value: this.filterPayorGroup });
            if (filter.filters.length > 0)
                this.dataGridSource.filter([filter]);
            else
                this.dataGridSource.filter([]);

        }

        private payorGroupListRecdProc() {
            this.payorGroupList = this.payorGroupService.PayorGroupID2NameMap;
            this.payorGroupListReceived = true;
            this.CallGridInit();
        }


        CallGridInit() {
            console.log("PE: Running Grid Init");
            if (this.payorGroupListReceived && this.enumListReceived) {
                var current: PayorEditorController = this;
                current.payorGroupListReceived = true;
                current.initGridOptions();
            }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Initialises the data grid source. </summary>
        ///
        /// <remarks>   PdelosReyes, 20150929. </remarks>
        ///
        /// <returns>   A kendo.data.DataSource. </returns>
        ///-------------------------------------------------------------------------------------------------
        private initDataGridSource($http): kendo.data.DataSource {
            var dmodel = kendo.data.Model.define({
                id: "PayorId",
                fields: {
                    PayorCode: { editable: true },
                    Name: { editable: true },
                    ContactPhone: { editable: true },
                    ContactAddr_1: { editable: true },
                    ContactAddr_2: { editable: true },
                    ContactCity: { editable: true },
                    ContactState: { editable: true },
                    ContactZipCode: { editable: true },
                    PayorGroupId: { editable: true },
                    PlaceOfService: { editable: true }
                }
            });

            var apirelpath = "api:/Payors";

            var ds: kendo.data.DataSource = CreateGridDataSource($http, 50, dmodel,
                {
                    read: () => {
                        return apirelpath;
                    },
                    update: () => {
                        return apirelpath;
                    },
                    create: () => {
                        return apirelpath;
                    }
                });
            ds.filter({
                    "logic": "and",
                    "filters": [{
                        "field": "Name",
                        "operator": "neq",
                        "value": "Unassigned"
                    }]
                });
            return ds;
        }
    


        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Initialises the grid options. </summary>
        ///
        /// <remarks>   PdelosReyes, 20150929. </remarks>
        ///
        /// <returns>   The kendo.ui.GridOptions. </returns>
        ///-------------------------------------------------------------------------------------------------
        private initGridOptions() {
            
            this.gridOptions = {
                dataSource: this.dataGridSource,
                scrollable: true,
                sortable: true,
                pageable: {
                    refresh: true,
                    pageSizes: true,
                },
                height: 750,
                autoBind: true,
                edit: this.onEdit,
                toolbar: ["create", "excel"],
                excel: {
                    fileName: moment().year().toString() + moment().month().toString() + moment().day().toString() + "_" + moment().hour().toString() + moment().minute().toString() + moment().second().toString() + "_SignalPayors.xlsx",
                    filterable: true,
                    allPages: true
                },
                columns: [
                    //{ "field": "PayorId", "title": "Payor Id", "hidden": "true" },
                    { "field": "PayorCode", headerTemplate: "<label for='pcode'>Payor Code</label> <input type='text' id='pcode' style='width: 50px;' ng-model='vm.filterPayorCode' ng-change='vm.RunFilter(kendoEvent)'/>" },
                    { "field": "Name", headerTemplate: "<label for='pname'>Name</label> <input type='text' id='pname' style='width: 50px;' ng-model='vm.filterPayorName' ng-change='vm.RunFilter(kendoEvent)'/>" },
                    { "field": "PayorGroupId", "title": "Payor Group", values: this.payorGroupList },
                    { "field": "ContactPhone", "title": "Phone" },
                    { "field": "ContactAddr_1", "title": "Address 1" },
                    { "field": "ContactAddr_2", "title": "Address 2" },
                    { "field": "ContactCity", "title": "City" },
                    { "field": "ContactState", "title": "State" },
                    { "field": "ContactZipCode", "title": "Zip Code" },
                    { "field": "PlaceOfService", "title": "Place Of Service", values: this.placeOfServiceEnum },
                    { command: ["edit"] }
                ],
                editable: "inline"
            };
        }



        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Executes the edit action. </summary>
        ///
        /// <remarks>   Ssur, 20150930. </remarks>
        ///
        /// <param name="e">    The unknown to process. </param>
        ///
        /// <returns>   . </returns>
        ///-------------------------------------------------------------------------------------------------

        public onEdit(e) {

            var model = e.model;
            var pc = e.container.find("input[name='PayorCode']");
            var isinsertmode = e.model.isNew();

            if (!e.model.isNew()) {
                pc[0].disabled = true;
            }
            else {
                pc[0].disabled = false;
            }
        }

    }
}