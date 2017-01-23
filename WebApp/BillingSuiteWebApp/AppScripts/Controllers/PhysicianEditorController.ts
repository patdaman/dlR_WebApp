///-------------------------------------------------------------------------------------------------
// <copyright file="dailyStatusReportController.ts" company="Signal Genetics Inc.">
// Copyright (c) 2015 Signal Genetics Inc.. All rights reserved.
// </copyright>
// <author>DTorres</author>
// <date>20150922</date>
// <summary>Physician editor controller controller class</summary>
///-------------------------------------------------------------------------------------------------

module BillingSuiteApp.Controller {
    export class PhysicianEditorController {

        identifier: string;
        gridOptions: kendo.ui.GridOptions;
        dataGridSource: kendo.data.DataSource;
        toolbarTemplate: any;
        pageSize: number;
        filterNPI: string = "";
        filterFirstName: string = "";
        filterLastName: string = "";
        filterClientList: string = "";


        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Default constructor. </summary>
        ///
        /// <remarks>   Dtorres, 20150922. </remarks>
        ///-------------------------------------------------------------------------------------------------
        constructor($rootScope, $http) {
            this.identifier = $rootScope.AppBuildStatus + "Physician Editor";
            this.toolbarTemplate = $("#toolbarTemplate").html();
            this.pageSize = 50;
            this.dataGridSource = this.initDataGridSource($http);
            this.gridOptions = this.initGridOptions();
            console.log("PhysicianEditorController init complete.");

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
            var filter = {
                logic: "and",
                filters: []
            };
            if (this.filterNPI && this.filterNPI.length > 0)
                filter.filters.push({ field: "NPI", operator: "startswith", value: this.filterNPI });
            if (this.filterFirstName && this.filterFirstName.length > 0)
                filter.filters.push({ field: "FirstName", operator: "startswith", value: this.filterFirstName });
            if (this.filterLastName && this.filterLastName.length > 0)
                filter.filters.push({ field: "LastName", operator: "startswith", value: this.filterLastName });
            if (this.filterClientList && this.filterClientList.length > 0)
                filter.filters.push({ field: "ClientList", operator: "contains", value: this.filterClientList });
            if (filter.filters.length > 0)
                this.dataGridSource.filter([filter]);
            else
                this.dataGridSource.filter([]);


        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Initialises the data grid source. </summary>
        ///
        /// <remarks>   Dtorres, 20150922. </remarks>
        ///
        /// <returns>   A kendo.data.DataSource. </returns>
        ///-------------------------------------------------------------------------------------------------
        private initDataGridSource($http): kendo.data.DataSource {
            var dmodel = kendo.data.Model.define({
                id: "DoctorId",
                fields: {
                    NPI: {
                        editable: true,
                        //validation: {
                        //    NPIvalidation: function (input) {
                        //        if (input.is("[name='NPI']") && input.val() != "") {
                        //            input.attr("data-NPIvalidation-msg", "NPI must be a 10 digit number");
                        //            return /^[0-9]{10}$/.test(input.val());
                        //        }
                        //        return true;
                        //    }
                        //}
                    },
                    FirstName: {
                        editable: false,
                        //validation: {
                        //    FirstNameValidation: function (input) {
                        //        if (input.is("[name='FirstName']") && input.val() != "") {
                        //            input.attr("data-FirstNameValidation-msg", "First Name can only include letters, spaces, and dashes (-)");
                        //            return /^[-\sa-zA-Z]+$/.test(input.val());
                        //        }
                        //        return true;
                        //    }
                        //}
                    },
                    LastName: {
                        editable: false,
                        //validation: {
                        //    LastNameValidation: function (input) {
                        //        if (input.is("[name='LastName']") && input.val() != "") {
                        //            input.attr("data-LastNameValidation-msg", "Last Name can only include letters, spaces, and dashes (-)");
                        //            return /^[-\sa-zA-Z]+$/.test(input.val());
                        //        }
                        //        return true;
                        //    }
                        //}
                    },
                    MiddleName: { editable: false },
                    Title: { editable: false },
                    ClientList: { editable: false },
                    DoctorId: { editable: false },
                }
            });

            var apirelpath = "api:/Physicians";

            var ds: kendo.data.DataSource = CreateGridDataSource($http, 50, dmodel,
                {
                    read: () => {
                        return apirelpath;
                    },
                    update: () => {
                        return apirelpath;
                    }
                });

            return ds;
        }
    


        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Initialises the grid options. </summary>
        ///
        /// <remarks>   Dtorres, 20150922. </remarks>
        ///
        /// <returns>   The kendo.ui.GridOptions. </returns>
        ///-------------------------------------------------------------------------------------------------
        private initGridOptions(): kendo.ui.GridOptions {
            var opts = {
                dataSource: this.dataGridSource,
                scrollable: true,
                sortable: true,
                pageable: {
                    refresh: true,
                    pageSizes: true,
                },
                height: 750,
                autoBind: true,
               // edit: this.onEdit,
                excel: {
                    fileName: moment().year().toString() + moment().month().toString() + moment().day().toString() + "_" + moment().hour().toString() + moment().minute().toString() + moment().second().toString() + "_SignalPhysicians.xlsx",
                    allPages: true
                },
                toolbar: this.toolbarTemplate,
                columns: [
                    { field: "DoctorId", title: "Doctor Id" },
                    { field: "NPI", headerTemplate: "<label for='pnpi'>NPI</label> <input type='text' id='pnpi' style='width: 50px;' ng-model='vm.filterNPI' ng-change='vm.RunFilter(kendoEvent)'/>", pattern: "^\d{10}$" },
                    { field: "Title", title: "Title" },
                    { field: "FirstName", headerTemplate: "<label for='pfname'>First Name</label> <input type='text' id='pfname' style='width: 50px;' ng-model='vm.filterFirstName' ng-change='vm.RunFilter(kendoEvent)'/>" },
                    { field: "MiddleName", title: "Middle Name" },
                    { field: "LastName", headerTemplate: "<label for='plname'>Last Name</label> <input type='text' id='plname' style='width: 50px;' ng-model='vm.filterLastName' ng-change='vm.RunFilter(kendoEvent)'/>" },
                    { field: "ClientList", headerTemplate: "<label for='pclient'>Client List</label> <input type='text' id='pclient' style='width: 50px;' ng-model='vm.filterClientList' ng-change='vm.RunFilter(kendoEvent)'/>" },
                    { command: ["edit"] }
                ],
                editable: "inline"
            };
            return opts;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Executes the edit action.
        ///              </summary>
        ///
        /// <remarks>   Rphilavanh, 10/6/2015. </remarks>
        ///
        /// <param name="e">    The unknown to process. </param>
        ///
        /// <returns>   . </returns>
        ///-------------------------------------------------------------------------------------------------

        public onEdit(e) {

            var model = e.model;
            var pc = e.container.find("input[name='DoctorId']");
            var pc1 = e.container.find("input[name='Title']");
            var pc2 = e.container.find("input[name='FirstName']");
            var pc3 = e.container.find("input[name='MiddleName']");
            var pc4 = e.container.find("input[name='LastName']");
            var pc5 = e.container.find("input[name='ClientList']");
            var isinsertmode = e.model.isNew();

            if (!e.model.isNew()) {
                pc[0].disabled = true;
                pc1[0].disabled = true;
                pc2[0].disabled = true;
                pc3[0].disabled = true;
                pc4[0].disabled = true;
                pc5[0].disabled = true;
            }
            else {
                pc[0].disabled = false;
                pc1[0].disabled = false;
                pc2[0].disabled = false;
                pc3[0].disabled = false;
                pc4[0].disabled = false;
                pc5[0].disabled = false;
            }
        }
    }
}