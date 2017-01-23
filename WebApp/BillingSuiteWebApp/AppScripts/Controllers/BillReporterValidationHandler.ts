module BillingSuiteApp.Controller {

    export class BillReporterValidationHandler {

        private httpServ: ng.IHttpService;
        private qServ: ng.IQService;

        //Billing Validation
        billValidationStatusGridOptions: kendo.ui.GridOptions = undefined;
        billingValidationStatusDataSource: kendo.data.DataSource;
        validationToolbarTemplate: any;
        billingToolbarTemplate: any;

        public BillingEventData: Model.BillingEventModel;

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Initializes a new instance of the BillReporterValidationHandler class. </summary>
        ///
        /// <remarks>   Ssur, 20150928. </remarks>
        ///
        /// <param name="http"> The HTTP. </param>
        ///-------------------------------------------------------------------------------------------------

        constructor(http: ng.IHttpService, qserv: ng.IQService) {

            this.httpServ = http;
            this.qServ = qserv;
            this.validationToolbarTemplate = $("#validationToolbarTemplate").html();
            this.billingToolbarTemplate = $("#billingToolbarTemplate").html();

        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Validates the billing aggregate. </summary>
        ///
        /// <remarks>   Ssur, 20151015. </remarks>
        ///
        /// <param name="fromDateString">   from date string. </param>
        /// <param name="toDateString">     to date string. </param>
        /// <param name="billingAggregate"> The billing aggregate. </param>
        ///
        /// <returns>   A ng.IPromise&lt;kendo.data.DataSource&gt; </returns>
        ///-------------------------------------------------------------------------------------------------

        public ValidateBillingAggregate(fromDateString: string, toDateString: string, billingAggregate: string, dateType: string): ng.IPromise<kendo.data.DataSource> {

            return this.getBillValidationData(fromDateString, toDateString, billingAggregate, dateType);

        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Performs the billing action. </summary>
        ///
        /// <remarks>   Ssur, 20151015. </remarks>
        ///
        /// <param name="cases">            The cases. </param>
        /// <param name="billingAggregate"> The billing aggregate. </param>
        /// <param name="billingComment">   The billing comment. </param>
        ///
        /// <returns>   A ng.IPromise&lt;kendo.data.DataSource&gt; </returns>
        ///-------------------------------------------------------------------------------------------------

        public PerformBilling(cases: Array<string>, billingAggregate: string, billingComment: string, xifinCreateAccession: boolean): ng.IPromise<kendo.data.DataSource> {
            var dfo = this.qServ.defer<kendo.data.DataSource>();
            var current: BillReporterValidationHandler = this;
            this.httpServ.put("api:/BillingEvents?billingAggregate=" + billingAggregate +
                "&comment=" + billingComment +
                "&xifinCreateAccession=" + xifinCreateAccession,
                cases,
                { withCredentials: true })
                .success(function (data, status, headers, config) {
                    current.BillingEventData = new Model.BillingEventModel();
                    current.BillingEventData.BillingAggregate = data["BillingAggregate"];
                    current.BillingEventData.AttemptedBilledCaseCount = data["AttemptedBilledCaseCount"];
                    current.BillingEventData.BilledCaseCount = data["BilledCaseCount"];
                    current.BillingEventData.BilledWithErrorCaseCount = data["BilledWithErrorCaseCount"];
                    current.BillingEventData.BillingDate = data["BillingDate"];
                    current.BillingEventData.BillingEventCode = data["BillingEventCode"];
                    current.BillingEventData.Comments = data["Comments"];
                    current.BillingEventData.Id = data["Id"];
                    current.BillingEventData.MessageType = data["MessageType"];
                    current.billingValidationStatusDataSource = new kendo.data.DataSource(
                        {
                            data: data["CaseValidation"],

                        });
                    current.initBillingCompleteGrid();
                    dfo.resolve(current.billingValidationStatusDataSource);
                })
                .error(function (data, status, headers, config) {
                    dfo.reject(data);
                })
            return dfo.promise;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets bill validation data. </summary>
        ///
        /// <remarks>   Ssur, 20151015. </remarks>
        ///
        /// <param name="fromDateString">   from date string. </param>
        /// <param name="toDateString">     to date string. </param>
        /// <param name="billingAggregate"> The billing aggregate. </param>
        ///
        /// <returns>   The bill validation data. </returns>
        ///-------------------------------------------------------------------------------------------------

        private getBillValidationData(fromDateString: string, toDateString: string, billingAggregate: string, dateType: string): ng.IPromise<kendo.data.DataSource> {

            var dfo = this.qServ.defer<kendo.data.DataSource>();

            var current: BillReporterValidationHandler = this;

            var thirdArg: string;
            if (dateType == "Order Date")
                thirdArg = "&datetype=" + "orderdate";
            else
                thirdArg = "&datetype=" + "completeddate";

            this.httpServ.get("api:/BillingEvents?fromDate=" + fromDateString +
                "&toDate=" + moment(toDateString).add(1, 'days').format('MM/DD/YYYY') +
                "&billingAggregate=" + billingAggregate + thirdArg
                , { withCredentials: true })
                .success(function (data, status, headers, config) {
                current.BillingEventData = new Model.BillingEventModel();
                    current.BillingEventData.BillingAggregate = data["BillingAggregate"];
                    current.BillingEventData.AttemptedBilledCaseCount = data["AttemptedBilledCaseCount"];
                    current.BillingEventData.BilledCaseCount = data["BilledCaseCount"];
                    current.BillingEventData.BilledWithErrorCaseCount = data["BilledWithErrorCaseCount"];
                    current.BillingEventData.BillingDate = data["BillingDate"];
                    current.BillingEventData.BillingEventCode = data["BillingEventCode"];
                    current.BillingEventData.Comments = data["Comments"];
                    current.BillingEventData.Id = data["Id"];
                    current.BillingEventData.MessageType = data["MessageType"];
                    current.billingValidationStatusDataSource = new kendo.data.DataSource(
                        {
                            data: data["CaseValidation"],
                         
                        });
                    current.initBillingValidationGrid();
                    dfo.resolve(current.billingValidationStatusDataSource);
                })
                .error(function (data, status, headers, config) {
                    dfo.reject(data);
                })

            return dfo.promise;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Initialises the billing validation grid. </summary>
        ///
        /// <remarks>   Ssur, 20151015. </remarks>
        ///
        /// <returns>   . </returns>
        ///-------------------------------------------------------------------------------------------------

        private initBillingValidationGrid() {
            var current: BillReporterValidationHandler = this;
            this.billValidationStatusGridOptions = {
                dataSource: this.billingValidationStatusDataSource,
                scrollable: true,
                selectable: "multiple, row",
                sortable: true,
                toolbar: this.validationToolbarTemplate,
                excel: {
                    fileName: moment().year().toString() + moment().month().toString() + moment().day().toString() + "_" + moment().hour().toString() + moment().minute().toString() + moment().second().toString() +  + "_SignalBillingValidation.xlsx",
                    allPages: true
                },
                excelExport: function (e) {

                var masterData = e.data;
                    var rows = [{
                        cells: [
                            { value: "IsSelected" },
                            { value: "CaseNumber" },
                            { value: "Status" },
                            { value: "Code" },
                            { value: "Description" },
                            { value: "Notes" },
                            { value: "PayorId" },
                            { value: "FiledErrorMessage" },
                            { value: "Source" }
                        ]
                    }];
                    
                    for (var rowIndex = 0; rowIndex < masterData.length; rowIndex++) {            
                        if (masterData[rowIndex].NonXifinValidationStatus != null) {
                           for (var rowIndex2 = 0; rowIndex2 < masterData[rowIndex].NonXifinValidationStatus.length; rowIndex2++) {
                               rows.push({
                                   cells: [
                                       { value: masterData[rowIndex].IsSelected },
                                       { value: masterData[rowIndex].CaseNumber },
                                       { value: masterData[rowIndex].Status },
                                       { value: masterData[rowIndex].NonXifinValidationStatus[rowIndex2].Code },
                                       { value: masterData[rowIndex].NonXifinValidationStatus[rowIndex2].Description },
                                       { value: null },
                                       { value: null },
                                       { value: masterData[rowIndex].NonXifinValidationStatus[rowIndex2].Notes },
                                       { value: masterData[rowIndex].NonXifinValidationStatus[rowIndex2].Source }
                                   ]
                               })
                           }
                       }
                        if (masterData[rowIndex].XifinValidationStatus != null) {
                           for (var rowIndex2 = 0; rowIndex2 < masterData[rowIndex].XifinValidationStatus.length; rowIndex2++) {
                               rows.push({
                                   cells: [
                                       { value: masterData[rowIndex].IsSelected },
                                       { value: masterData[rowIndex].CaseNumber },
                                       { value: masterData[rowIndex].Status },
                                       { value: masterData[rowIndex].XifinValidationStatus[rowIndex2].Code },
                                       { value: masterData[rowIndex].XifinValidationStatus[rowIndex2].Description },
                                       { value: masterData[rowIndex].XifinValidationStatus[rowIndex2].Notes },
                                       { value: masterData[rowIndex].XifinValidationStatus[rowIndex2].PayorId },
                                       { value: masterData[rowIndex].XifinValidationStatus[rowIndex2].FiledErrorMessage },
                                       { value: masterData[rowIndex].XifinValidationStatus[rowIndex2].Source }
                                   ]
                               })
                           }
                       }
                    }
                    e.preventDefault();
                    var workbook = new kendo.ooxml.Workbook({
                        sheets: [
                            {
                                columns: [ { autoWidth: true }  ],
                                title: "BillingValidation",
                                frozenRows: 1,
                                rows: rows
                            }
                        ]
                    });
                    kendo.saveAs({ dataURI: workbook.toDataURL(), fileName: moment().year().toString() + moment().month().toString() + moment().day().toString() + "_" + moment().hour().toString() + moment().minute().toString() + moment().second().toString() + "_SignalBillingValidation.xlsx" });

                },
                columns: [
                    {
                        field: "IsSelected", title: "Submit",
                        sortable: false,
                        template: '<input type="checkbox" #=IsSelected ? checked="checked":"" #  #= data.IsSelectable ? disabled="":"disabled" # ' +
                        'ng-model="dataItem.IsSelected" ' +
                        '/>',
                        headerTemplate: '<button ng-click="vm.SelectAllValidCases(event)" id="checkAllValids" />All</button>',
                        width: 100
                    },
                    { field: "IsSelected", title: "Selected?", width:75 },
                    { field: "IsSelectable", title: "Can Select?", width:75 },
                    { "field": "CaseNumber", "title": "Case Number" },
                    { "field": "Status", "title": "Status" },

                ],
                editable: false
            }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Initialises the billing complete grid. </summary>
        ///
        /// <remarks>   Ssur, 20151015. </remarks>
        ///
        /// <returns>   . </returns>
        ///-------------------------------------------------------------------------------------------------

        private initBillingCompleteGrid() {
            var current: BillReporterValidationHandler = this;
            this.billValidationStatusGridOptions = {
                dataSource: this.billingValidationStatusDataSource,
                scrollable: true,
                selectable: "multiple, row",
                sortable: true,
                toolbar: this.billingToolbarTemplate,
                excel: {
                    fileName: moment().year().toString() + moment().month().toString() + moment().day().toString() + "_" + moment().hour().toString() + moment().minute().toString() + moment().second().toString() + "_SignalBillingComplete.xlsx",
                    allPages: true
                },
                excelExport: function (e) {

                    var masterData = e.data;
                    var rows = [{
                        cells: [
                            { value: "CaseNumber" },
                            { value: "Status" },
                            { value: "Code" },
                            { value: "Description" },
                            { value: "Notes" },
                            { value: "PayorId" },
                            { value: "FiledErrorMessage" },
                            { value: "Source" }
                        ]
                    }];

                    for (var rowIndex = 0; rowIndex < masterData.length; rowIndex++) {
                        if (masterData[rowIndex].NonXifinValidationStatus != null) {
                            for (var rowIndex2 = 0; rowIndex2 < masterData[rowIndex].NonXifinValidationStatus.length; rowIndex2++) {
                                rows.push({
                                    cells: [
                                        { value: masterData[rowIndex].CaseNumber },
                                        { value: masterData[rowIndex].Status },
                                        { value: masterData[rowIndex].NonXifinValidationStatus[rowIndex2].Code },
                                        { value: masterData[rowIndex].NonXifinValidationStatus[rowIndex2].Description },
                                        { value: null },
                                        { value: null },
                                        { value: masterData[rowIndex].NonXifinValidationStatus[rowIndex2].Notes },
                                        { value: masterData[rowIndex].NonXifinValidationStatus[rowIndex2].Source }
                                    ]
                                })
                            }
                        }
                        if (masterData[rowIndex].XifinValidationStatus != null) {
                            for (var rowIndex2 = 0; rowIndex2 < masterData[rowIndex].XifinValidationStatus.length; rowIndex2++) {
                                rows.push({
                                    cells: [
                                        { value: masterData[rowIndex].CaseNumber },
                                        { value: masterData[rowIndex].Status },
                                        { value: masterData[rowIndex].XifinValidationStatus[rowIndex2].Code },
                                        { value: masterData[rowIndex].XifinValidationStatus[rowIndex2].Description },
                                        { value: masterData[rowIndex].XifinValidationStatus[rowIndex2].Notes },
                                        { value: masterData[rowIndex].XifinValidationStatus[rowIndex2].PayorId },
                                        { value: masterData[rowIndex].XifinValidationStatus[rowIndex2].FiledErrorMessage },
                                        { value: masterData[rowIndex].XifinValidationStatus[rowIndex2].Source }
                                    ]
                                })
                            }
                        }
                    }
                    e.preventDefault();
                    var workbook = new kendo.ooxml.Workbook({
                        sheets: [
                            {
                                columns: [
                                    { autoWidth: true },
                                    { autoWidth: true },
                                    { autoWidth: true },
                                    { autoWidth: true },
                                    { autoWidth: true },
                                    { autoWidth: true },
                                    { autoWidth: true },
                                    { autoWidth: true }
                                ],
                                title: "BillingComplete",
                                frozenRows: 1,
                                rows: rows
                            }
                        ]
                    });
                    kendo.saveAs({ dataURI: workbook.toDataURL(), fileName: moment().year().toString() + moment().month().toString() + moment().day().toString() + "_" + moment().hour().toString() + moment().minute().toString() + moment().second().toString() + "_SignalBillingComplete.xlsx" });

                },
                columns: [                   
                    { "field": "CaseNumber", "title": "Case Number" },
                    { "field": "Status", "title": "Status" }
                ],
                editable: false
            }
        }


    }
}