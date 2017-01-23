///-------------------------------------------------------------------------------------------------
/// <summary>   A controller for handling billing suite app.s. </summary>
///
/// <remarks>   Rphilavanh, 9/29/2015. </remarks>
///-------------------------------------------------------------------------------------------------


module BillingSuiteApp.Controller {

    export class CaseEditorController {

        caseEditorGridOptions: kendo.ui.GridOptions = undefined;
        httpServ: ng.IHttpService;
        qServ: ng.IQService;

        fromDate: Date;
        toDate: Date;
        fromDateString: string;
        toDateString: string;
        dateType: string;

        identifier: string;
        toolbarTemplate: any;
        detailTemplate: any;
        dataGridSource: kendo.data.DataSource;
        filterCaseNumber: string = "";

        apiBody: string = "&datetype=orderdate";

        getData: Function;

        payorService: Service.PayorService;
        enumService: Service.EnumListService;
        utilService: Service.UtilService;

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

        billTypeEnum: any;
        homePlanEnum: any;
        planTypeEnum: any;
        networkEnum: any;
        insuredRelationshipEnum: any;
        genderEnum: any;

        billTypeList: any;
        homePlanList: any;
        planTypeList: any;
        networkList: any;
        insuredRelationshipList: any;
        genderList: any;

        billTypeSelection: string;
        homePlanSelection: string;
        planTypeSelection: string;
        networkSelection: string;
        insuredRelationshipSelection: string;
        genderSelection: string;

        editRowDataModel: kendo.data.Model;
        dataModel: any;
        exportFlag: boolean = false;

        insuredRelationship1SelectionIsSelf: boolean;
        insuredRelationship2SelectionIsSelf: boolean;
        insuredRelationship1Selection: string;
        insuredRelationship2Selection: string;

        updateActionComment: string;
        caseListStr: string;

        //Case Notes
        caseNotesData: Model.NotesModel;
        windowCaseNotes: kendo.ui.Window;
        caseNotesHandler: CaseNotes;
        caseNotesGridOptions: kendo.ui.GridOptions = undefined;
        caseNotesDataSource: kendo.data.DataSource = undefined;
        
        caseNotesToolbarTemplate: any;

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Constructor. </summary>
        ///
        /// <remarks>   Rphilavanh, 9/29/2015. </remarks>
        ///
        /// <param name="$http">        The $http. </param>
        /// <param name="$q">           The $q. </param>
        /// <param name="PayorService"> The payor service. </param>
        /// <param name="EnumService">  The enum service. </param>
        ///-------------------------------------------------------------------------------------------------

        constructor($rootScope, $http, $q,
            PayorService: Service.PayorService,
            EnumService: Service.EnumListService,
            UtilService: Service.UtilService) {

            this.caseEditorGridOptions = undefined;


            this.httpServ = $http;
            this.qServ = $q;

            this.payorService = PayorService;
            this.enumService = EnumService;
            this.utilService = UtilService;

            this.identifier = $rootScope.AppBuildStatus + "Case Editor";

            this.toolbarTemplate = $("#toolbarTemplate").html();
            this.detailTemplate = $("#detailTemplate").html();

            this.dateType = "Completed Date";

            this.toDate = new Date();
            this.fromDate = new Date();
            this.fromDate.setDate(this.toDate.getDate() - 7);

            this.fromDateString = DateToUSString(this.fromDate);
            this.toDateString = DateToUSString(this.toDate);

            this.payorListReceived = false;
            this.enumListReceived = false;

            this.dataGridSource = this.initDataGridSource($http);

            if (this.payorService.PayorsID2CodeMap) {
                this.payorListRecdProc();
            }
            else {
                var current: CaseEditorController = this;
                this.payorService.GetPayorsAsync().then(function (data) {
                    current.payorListRecdProc();
                }, function (reason) {
                    console.log("Went to hell in a handbasket");
                })
            }

            if (this.enumService.EnumServiceReady) {
                this.enumListRecdProc();
            }
            else {
                var current: CaseEditorController = this;
                this.enumService.PopulateEnumListsAsync().then(function (data) {
                    current.enumListRecdProc();
                }, function (reason) {
                    console.log("Error loading enums");
                })
            }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Initialises the data grid source. </summary>
        ///
        /// <remarks>   Rphilavanh, 9/29/2015. </remarks>
        ///
        /// <param name="$http">    The $http. </param>
        ///
        /// <returns>   A kendo.data.DataSource. </returns>
        ///-------------------------------------------------------------------------------------------------

        private initDataGridSource($http): kendo.data.DataSource {
            var apirelpath = "api:/BillingStatusCases";

            this.dataModel = kendo.data.Model.define({
                id: "CaseNumber",
                fields: {
                    CaseNumber: { editable: false },
                    ProgramGroup: { editable: false },
                    ComputedBillingClassification: { editable: false },
                    BillingAggregate: { editable: false },
                    BillableStatus: { editable: true },
                    Deficiencies: { editable: false },
                    ICD9Codes: { editable: false },
                    QNS: { editable: false },
                    ClientName: { editable: false },
                    DoctorNPI: { editable: false },
                    PatientMRN: { editable: false },
                    PayorGroup1: { editable: false },
                    PayorGroup2: { editable: false },
                    DateofService: { editable: false, type: "date" },
                    OrderDate: { editable: false, type: "date" },
                    CompletedDate: { editable: false, type: "date" },

                    BillingClassification: { editable: true },
                    BillType: { editable: true },
                    PayorId1: { editable: true },
                    HomePlan1: { editable: true },
                    PlanType1: { editable: true },
                    Network1: { editable: true },
                    InsuredRelationship1: { editable: true },
                    PayorGroupNumber1: { editable: true },
                    PayorPolicyNumber1: { editable: true },
                    PayorId2: { editable: true },
                    HomePlan2: { editable: true },
                    PlanType2: { editable: true },
                    Network2: { editable: true },
                    InsuredRelationship2: { editable: true },
                    PayorGroupNumber2: { editable: true },
                    PayorPolicyNumber2: { editable: true },

                    PlaceOfService1: { editable: false },
                    PlaceOfService2: { editable: false },

                    Subscriber1FirstName: { editable: true },
                    Subscriber1MiddleName: { editable: true },
                    Subscriber1LastName: { editable: true },
                    Subscriber1DateOfBirth: { editable: true, type: "date" },
                    Subscriber1Gender: { editable: true },
                    Subscriber1Address1: { editable: true },
                    Subscriber1Address2: { editable: true },
                    Subscriber1City: { editable: true },
                    Subscriber1StateProvince: { editable: true },
                    Subscriber1PostalCode: { editable: true },
                    Subscriber1Country: { editable: true },
                    Subscriber1PhoneNumber: { editable: true },

                    Subscriber2FirstName: { editable: true },
                    Subscriber2MiddleName: { editable: true },
                    Subscriber2LastName: { editable: true },
                    Subscriber2DateOfBirth: { editable: true, type: "date" },
                    Subscriber2Gender: { editable: true },
                    Subscriber2Address1: { editable: true },
                    Subscriber2Address2: { editable: true },
                    Subscriber2City: { editable: true },
                    Subscriber2StateProvince: { editable: true },
                    Subscriber2PostalCode: { editable: true },
                    Subscriber2Country: { editable: true },
                    Subscriber2PhoneNumber: { editable: true },

                    LastCaseEdit: { editable: false, type: "date" },
                    LastCaseEditUser: { editable: false },
                }

            });


            var ds: kendo.data.DataSource = CreateGridDataSource($http, 20, this.dataModel,
                {
                    read: () => {
                        this.updateActionComment = null;
                        return apirelpath + this.apiBody;
                    },
                    update: () => {
                        this.updateActionComment = null;
                        return apirelpath;
                    }

                });

            return ds;

        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets a data. </summary>
        ///
        /// <remarks>   Rphilavanh, 9/29/2015. </remarks>
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


        public CancelUpdate(event) {
            this.dataGridSource.cancelChanges();
        }

        private payorListRecdProc() {
            this.payorsList = this.payorService.PayorsID2NameMap;
            this.payorCodesList = this.payorService.PayorsID2CodeMap;
            this.payorListReceived = true;
            this.CallGridInit();
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Enum list received proc. </summary>
        ///
        /// <remarks>   Rphilavanh, 9/29/2015. </remarks>
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

            enumlink = this.enumService.GetEnumLink("BillType");
            this.billTypeEnum = enumlink.EnumMap;
            this.billTypeList = enumlink.EnumList;

            enumlink = this.enumService.GetEnumLink("HomePlan");
            this.homePlanEnum = enumlink.EnumMap;
            this.homePlanList = enumlink.EnumList;

            enumlink = this.enumService.GetEnumLink("PlanType");
            this.planTypeEnum = enumlink.EnumMap;
            this.planTypeList = enumlink.EnumList;

            enumlink = this.enumService.GetEnumLink("Network");
            this.networkEnum = enumlink.EnumMap;
            this.networkList = enumlink.EnumList;

            enumlink = this.enumService.GetEnumLink("InsuredRelationship");
            this.insuredRelationshipEnum = enumlink.EnumMap;
            this.insuredRelationshipList = enumlink.EnumList;

            enumlink = this.enumService.GetEnumLink("Gender");
            this.genderEnum = enumlink.EnumMap;
            this.genderList = enumlink.EnumList;

            this.enumListReceived = true;
            this.CallGridInit();
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Call grid initialise. </summary>
        ///
        /// <remarks>   Rphilavanh, 9/29/2015. </remarks>
        ///
        /// <returns>   . </returns>
        ///-------------------------------------------------------------------------------------------------

        CallGridInit() {
            if (this.payorListReceived && this.enumListReceived) {
                var current: CaseEditorController = this;
                current.payorListReceived = true;
                //current.initGrid();
                current.initGridAlt();
            }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Initialises the grid. </summary>
        ///
        /// <remarks>   Rphilavanh, 9/29/2015. </remarks>
        ///
        /// <returns>   . </returns>
        ///-------------------------------------------------------------------------------------------------

        initGrid() {

            this.caseEditorGridOptions = {
                dataSource: this.dataGridSource,
                scrollable: true,
                sortable: true,
                pageable: {
                    refresh: true,
                    pageSizes: true,
                },
                height: 750,
                autoBind: false,
                toolbar: this.toolbarTemplate,
                columns: [
                    { command: ["edit"] },
                    { "field": "CaseNumber", headerTemplate: "<label for='pnpi'>Case #</label> <input type='text' id='pnpi' style='width: 50px;' ng-model='vm.filterCaseNumber' ng-change='vm.RunFilter(kendoEvent)'/>" },
                    { "field": "ProgramGroup", "title": "Program" },
                    { "field": "BillType", "title": "Bill Type" },
                    { "field": "BillingClassification", "title": "Bill Class" },
                    { "field": "ComputedBillingClassification", "title": "Computed Bill Class" },
                    { "field": "Deficiencies", "title": "Deficiencies" },
                    { "field": "ComputedICD10Codes", "title": "ICD10" },
                    { "field": "QNS", "title": "QNS" },
                    { "field": "RepeatCount", "title": "Repeat Count" },
                    { "field": "ClientName", "title": "Client" },
                    { "field": "DoctorNPI", "title": "Doctor NPI" },
                    { "field": "PatientMRN", "title": "MRN" },

                    { "field": "PayorId1", "title": "Payor 1", values: this.payorCodesList },
                    { "field": "PayorGroup1", "title": "Payor Group" },
                    { "field": "HomePlan1", "title": "Home Plan", values: this.homePlanList },
                    { "field": "PlanType1", "title": "Plan Type", values: this.planTypeList },
                    { "field": "InsuredRelationship1", "title": "Relationship", values: this.insuredRelationshipEnum },
                    { "field": "PayorGroupPolicyNumber1", "title": "Group #" },
                    { "field": "PayorPolicyNumber1", "title": "Policy #" },

                    { "field": "InsuredFirstName1", "title": "Insured First Name" },
                    { "field": "InsuredMiddleName1", "title": "Insured Middle Name" },
                    { "field": "InsuredLastName1", "title": "Insured Last Name" },
                    { "field": "InsuredDOB1", "title": "Insured DOB" },
                    { "field": "InsuredGender1", "title": "Insured Gender" },
                    { "field": "InsuredAddress1_1:", "title": "Insured Address1" },
                    { "field": "InsuredAddress2_1", "title": "Insured Address2" },
                    { "field": "InsuredCity1", "title": "Insured City" },
                    { "field": "InsuredState1", "title": "Insured State" },
                    { "field": "InsuredPostalCode1", "title": "Insured ZIP" },
                    { "field": "InsuredCountryCode1", "title": "Insured Country" },
                    { "field": "InsuredPhone1", "title": "Insured Phone" },

                    { "field": "PayorId2", "title": "Payor 2", values: this.payorCodesList },
                    { "field": "PayorGroup2", "title": "Payor Group" },
                    { "field": "HomePlan2", "title": "Home Plan", values: this.homePlanList },
                    { "field": "PlanType2", "title": "Plan Type", values: this.planTypeList },
                    { "field": "InsuredRelationship2", "title": "Relationship", values: this.insuredRelationshipList },
                    { "field": "PayorGroupPolicyNumber2", "title": "Group #" },
                    { "field": "PayorPolicyNumber2", "title": "Policy #" },

                    { "field": "InsuredFirstName2", "title": "Insured First Name" },
                    { "field": "InsuredMiddleName2", "title": "Insured Middle Name" },
                    { "field": "InsuredLastName2", "title": "Insured Last Name" },
                    { "field": "InsuredDOB2", "title": "Insured DOB" },
                    { "field": "InsuredGender2", "title": "Insured Gender" },
                    { "field": "InsuredAddress1_2:", "title": "Insured Address1" },
                    { "field": "InsuredAddress2_2", "title": "Insured Address2" },
                    { "field": "InsuredCity2", "title": "Insured City" },
                    { "field": "InsuredState2", "title": "Insured State" },
                    { "field": "InsuredPostalCode2", "title": "Insured ZIP" },
                    { "field": "InsuredCountryCode2", "title": "Insured Country" },
                    { "field": "InsuredPhone2", "title": "Insured Phone" },

                ],
                editable: "inline"
            }

        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Handles the dropdown detail </summary>
        ///
        /// <remarks>   Ssur, 20151001. DO NOT CHANGE WITHOUT ASKING ME</remarks>
        ///
        /// <param name="e">    The unknown to process. </param>
        ///
        /// <returns>   . </returns>
        ///-------------------------------------------------------------------------------------------------
       
        private handleDetailInit(e) {
        var detailRow = e.detailRow;
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

        detailRow.find(".case-details > .k-button.save").click(function () {
            if (editableModel.dirty) {
               
                var chgidx = datacoll.indexOf(selectedModel);
                var modmodel = datacoll[chgidx];
                var keys = Object.keys(editableModel);

                for (var i = 0; i < keys.length; i++) {
                    var key = keys[i];
                    if (ignorelist.indexOf(key) < 0){
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
        public TruncateString(val: any): string {
            if (val == undefined)
                return "";
            return val.substring(0,100);
        }
        public DateStringer(val: any): string {
        if (val == "" || val == undefined)
            return "";
        return val.toLocaleDateString();
    }
        public DateTimeStringer(val: any): string {
        if (val == "" || val == undefined)
            return "";
        return moment(val).format("MM/DD/YYYY HH:mm Z")
    }
        public EmptyUser(val: any): string {
        if (val == "" || val == undefined)
            return "<Nobody>";
        return val;
    }
        public AddCommaIfNotEmpty(val: any): string {
        if (val == "" || val == undefined)
            return "";
        return val + ",";
    }
        public CheckDisallowBillStatusChange(billStatus: string): boolean {
        if (billStatus == 'Billed' || billStatus == 'Closed')
            return true;
        return false;

    }

    ///-------------------------------------------------------------------------------------------------
    /// <summary>   Initialises the grid alternate. </summary>
    ///
    /// <remarks>   Rphilavanh, 20151112. </remarks>
    ///
    /// <returns>   . </returns>
    ///-------------------------------------------------------------------------------------------------

        initGridAlt() {

        var current: CaseEditorController = this;
        this.caseEditorGridOptions = {
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
                fileName: moment().year().toString() + moment().month().toString() + moment().day().toString() + "_" + moment().hour().toString() + moment().minute().toString() + moment().second().toString() + "_SignalCases.xlsx",
                allPages: true
            },
            excelExport:

            function (e) {

                if (!this.exportFlag) {
                    e.sender.hideColumn(1);
                    e.sender.hideColumn(2);
                    e.sender.hideColumn(3);
                    e.sender.showColumn(4);
                    e.sender.showColumn(5);
                    e.sender.showColumn(6);
                    e.sender.showColumn(7);
                    e.sender.showColumn(8);
                    e.sender.showColumn(9);
                    e.sender.showColumn(10);
                    e.sender.showColumn(11);
                    e.sender.showColumn(12);
                    e.sender.showColumn(13);
                    e.sender.showColumn(14);
                    e.sender.showColumn(15);
                    e.sender.showColumn(16);
                    e.sender.showColumn(17);
                    e.sender.showColumn(18);
                    e.sender.showColumn(19);
                    e.sender.showColumn(20);
                    e.sender.showColumn(21);
                    e.sender.showColumn(22);
                    e.sender.showColumn(23);
                    e.sender.showColumn(24);
                    e.sender.showColumn(25);
                    e.sender.showColumn(26);
                    e.sender.showColumn(27);
                    e.sender.showColumn(28);
                    e.sender.showColumn(29);
                    e.sender.showColumn(30);
                    e.sender.showColumn(31);
                    e.sender.showColumn(32);
                    e.sender.showColumn(33);

                    e.sender.showColumn(34);
                    e.sender.showColumn(35);
                    e.sender.showColumn(36);
                    e.sender.showColumn(37);
                    e.sender.showColumn(38);
                    e.sender.showColumn(39);
                    e.sender.showColumn(40);
                    e.sender.showColumn(41);
                    e.sender.showColumn(42);
                    e.sender.showColumn(43);
                    e.sender.showColumn(44);
                    e.sender.showColumn(45);
                    e.sender.showColumn(46);
                    e.sender.showColumn(47);
                    e.sender.showColumn(48);
                    e.sender.showColumn(49);
                    e.sender.showColumn(50);
                    e.sender.showColumn(51);
                    e.sender.showColumn(52);
                    e.sender.showColumn(53);
                    e.sender.showColumn(54);
                    e.sender.showColumn(55);
                    e.sender.showColumn(56);
                    e.sender.showColumn(57);
                    e.sender.showColumn(58);
                    e.sender.showColumn(59);
                    e.sender.showColumn(60);
                    e.sender.showColumn(61);
                    e.sender.showColumn(62);
                    e.sender.showColumn(63);
                    e.sender.showColumn(64);
                    e.sender.showColumn(65);
                    e.sender.showColumn(66);
                    e.preventDefault();
                    this.exportFlag = true;
                    setTimeout(function () {
                        e.sender.saveAsExcel();
                    });
                } else {
                    e.sender.showColumn(1);
                    e.sender.showColumn(2);
                    e.sender.showColumn(3);
                    e.sender.hideColumn(4);
                    e.sender.hideColumn(5);
                    e.sender.hideColumn(6);
                    e.sender.hideColumn(7);
                    e.sender.hideColumn(8);
                    e.sender.hideColumn(9);
                    e.sender.hideColumn(10);
                    e.sender.hideColumn(11);
                    e.sender.hideColumn(12);
                    e.sender.hideColumn(13);
                    e.sender.hideColumn(14);
                    e.sender.hideColumn(15);
                    e.sender.hideColumn(16);
                    e.sender.hideColumn(17);
                    e.sender.hideColumn(18);
                    e.sender.hideColumn(19);
                    e.sender.hideColumn(20);
                    e.sender.hideColumn(21);
                    e.sender.hideColumn(22);
                    e.sender.hideColumn(23);
                    e.sender.hideColumn(24);
                    e.sender.hideColumn(25);
                    e.sender.hideColumn(26);
                    e.sender.hideColumn(27);
                    e.sender.hideColumn(28);
                    e.sender.hideColumn(29);
                    e.sender.hideColumn(30);
                    e.sender.hideColumn(31);
                    e.sender.hideColumn(32);
                    e.sender.hideColumn(33);

                    e.sender.hideColumn(34);
                    e.sender.hideColumn(35);
                    e.sender.hideColumn(36);
                    e.sender.hideColumn(37);
                    e.sender.hideColumn(38);
                    e.sender.hideColumn(39);
                    e.sender.hideColumn(40);
                    e.sender.hideColumn(41);
                    e.sender.hideColumn(42);
                    e.sender.hideColumn(43);
                    e.sender.hideColumn(44);
                    e.sender.hideColumn(45);
                    e.sender.hideColumn(46);
                    e.sender.hideColumn(47);
                    e.sender.hideColumn(48);
                    e.sender.hideColumn(49);
                    e.sender.hideColumn(50);
                    e.sender.hideColumn(51);
                    e.sender.hideColumn(52);
                    e.sender.hideColumn(53);
                    e.sender.hideColumn(54);
                    e.sender.hideColumn(55);
                    e.sender.hideColumn(56);
                    e.sender.hideColumn(57);
                    e.sender.hideColumn(58);
                    e.sender.hideColumn(59);
                    e.sender.hideColumn(60);
                    e.sender.hideColumn(61);
                    e.sender.hideColumn(62);
                    e.sender.hideColumn(63);
                    e.sender.hideColumn(64);
                    e.sender.hideColumn(65);
                    e.sender.hideColumn(66);
                    this.exportFlag = false;
                }
            },
            toolbar: this.toolbarTemplate,
            detailTemplate: this.detailTemplate,
            detailInit: function (e) {
                current.handleDetailInit(e);
                // alert(e.action + e.index);
            },
            //detailExpand: this.SetRowToEditMode,
            columns: [
                //{ command: ["edit"]  },
                {
                    "field": "CaseNumber",
                    "width": 100,
                    template: '<div style="font-size:large">{{::dataItem.CaseNumber}}</div> <br /><br/>' +
                    '<div style="float:left; margin-right:5px; width:100%;">Last updated by <strong>{{::vm.EmptyUser(dataItem.LastCaseEditUser)}}</strong> on <strong>{{::vm.DateTimeStringer(dataItem.LastCaseEdit)}}</strong> </div><br/><br/><br/>' +

                    '<div style="float:left; margin-right:5px; width:100%;">Last Update Note:  <strong>{{::vm.TruncateString(dataItem.LastCaseNote)}}</strong> </strong> </div><br/><br/><br/>' +
                    '<button ng-click="::vm.GetCaseNotes(dataItem.CaseNumber)"><strong>View Update History</strong></button>',
                    headerTemplate: "<label for='pnpi'>Case #</label> <input type='text' id='pnpi' style='width: 80px;' ng-model='::vm.filterCaseNumber' ng-change='::vm.RunFilter(kendoEvent)'/>"

                },
                {
                    "field": "CaseNumber", "title": "Non Editable Details", "width": 400,
                    "template": '<label class="sgnl-columnitemlabel"> ' +

                    '<div style="float:left; margin-right:5px; width:100%;">Order Date: <strong>{{::vm.DateTimeStringer(dataItem.OrderDate)}}</strong>  </div>' +
                    '<div style="float:left; margin-right:5px; width:100%;">Completed Date: <strong>{{::vm.DateTimeStringer(dataItem.CompletedDate)}}</strong>  </div><br/><br/><br/>' +

                    '<div style="float:left; margin-right:5px; width:40%;">Bill Status: <strong>{{::dataItem.BillableStatus }}</strong>  </div>' +
                    '<div style="float:left; margin-right:5px; width:55%;">Computed Bill Class: <strong>{{::dataItem.ComputedBillingClassification}}</strong>  </div>' +

                    '<div style="float:left; margin-right:5px; width:40%;">Program: <strong>{{::dataItem.ProgramGroup }}</strong>  </div>' +
                    '<div style="float:left; margin-right:5px; width:55%;">Aggregate: <strong>{{::dataItem.BillingAggregate }}</strong> </div><br/><br/><br/>' +

                    '<div style="float:left; margin-right:5px; width:40%;">Date Of Service: <strong>{{::vm.DateStringer(dataItem.DateofService)}}</strong>  </div>' +
                    '<div style="float:left; margin-right:5px; width:55%;">Ordering Physician: <strong>{{::vm.AddCommaIfNotEmpty(dataItem.DoctorLastName)}} {{::dataItem.DoctorFirstName}} {{::dataItem.DoctorMiddleName }}</strong>  </div>' +

                    '<div style="float:left; margin-right:5px; width:40%;">Repeat Count: <strong>{{::dataItem.RepeatCount}}</strong>  </div>' +
                    '<div style="float:left; margin-right:5px; width:55%;">Patient: <strong>{{::vm.AddCommaIfNotEmpty(dataItem.PatientLastName)}} {{::dataItem.PatientFirstName}} {{::dataItem.PatientMiddleName }}</strong>  </div>' +

                    '<div style="float:left; margin-right:5px; width:40%;">QNS: <strong>{{::dataItem.QNS}}</strong>  </div>' +
                    '<div style="float:left; margin-right:5px; width:55%;">ICD10: <strong>{{::dataItem.ComputedICD10Codes}}</strong> </div><br/><br/><br/>' +

                    '<div style="float:left; margin-right:5px; width:100%;">Client Name: <strong>{{::dataItem.ClientName}}</strong>  </div>' +
                    '<div style="float:left; margin-right:5px; width:100%;">Deficiencies: <strong>{{::dataItem.Deficiencies}}</strong>  </div>' +
                    '</label>'
                },
                {
                    "field": "CaseNumber", "title": "Editable Details", "width": 600,
                    "template": '<label class="sgnl-columnitemlabel"> ' +
                    '<div style="float:left; margin-right:5px; width:45%;">Bill Class:</strong>  <strong>{{::dataItem.BillingClassification}}</strong> </div>' +
                    '<div style="float:left; margin-right:5px; width:45%;">Bill Type: <strong>{{::dataItem.BillType}}</strong>  </div><br />' +

                    '<table><tr><td><div style="float:left; margin-right:5px; width:100%;">Primary Payor: <strong>{{::dataItem.PayorCode1}}</strong> </div>' +
                    '<div style="float:left; margin-right:5px; width:100%;">Place Of Service: <strong>{{::dataItem.PlaceOfService1}}</strong> </div>' +
                    '<div style="float:left; margin-right:5px; width:48%;">Network 1: <strong>{{::dataItem.Payor1Network1}}</strong> </div>' +
                    '<div style="float:left; margin-right:5px; width:48%;">Network 2: <strong>{{::dataItem.Payor1Network2}}</strong> </div>' +
                    '<div style="float:left; margin-right:5px; width:48%;">Home Plan: <strong>{{::dataItem.HomePlan1}}</strong> </div>' +
                    '<div style="float:left; margin-right:5px; width:48%;">Plan Type: <strong>{{::dataItem.PlanType1}}</strong> </div>' +
                    '<div style="float:left; margin-right:5px; width:48%;">Group No: <strong>{{::dataItem.PayorGroupNumber1}}</strong> </div>' +
                    '<div style="float:left; margin-right:5px; width:48%;">Policy No: <strong>{{::dataItem.PayorPolicyNumber1}}</strong> </div>' +
                    '<div style="float:left; margin-right:5px; width:100%;">Relationship: <strong>{{::dataItem.InsuredRelationship1}}</strong> </div>' +

                    '<div style="float:left; margin-right:5px; width:100%;">Insured Name: <strong>{{::vm.AddCommaIfNotEmpty(dataItem.Subscriber1LastName)}} {{::dataItem.Subscriber1FirstName}} {{::dataItem.Subscriber1MiddleName}}</strong> </div>' +
                    '<div style="float:left; margin-right:5px; width:48%;">Insured DOB: <strong>{{::vm.DateStringer(dataItem.Subscriber1DateOfBirth)}}</strong> </div>' +
                    '<div style="float:left; margin-right:5px; width:48%;">Insured Gender: <strong>{{::dataItem.Subscriber1Gender}}</strong> </div>' +
                    '<div style="float:left; margin-right:5px; width:100%;">Insured Phone: <strong>{{::dataItem.Subscriber1PhoneNumber}}</strong> </div> ' +
                    '<div style="float:left; margin-right:5px; width:100%;">Insured Address: <strong>{{::dataItem.Subscriber1Address1}} {{::dataItem.Subscriber1Address2}} {{::vm.AddCommaIfNotEmpty(dataItem.Subscriber1City)}} {{::dataItem.Subscriber1StateProvince}} {{::dataItem.Subscriber1PostalCode}} {{::dataItem.Subscriber1Country}}</strong> </div></td>' +


                        '<td><div style="float:left; margin-right:5px; width:100%;">Secondary Payor: <strong>{{::dataItem.PayorCode2}}</strong> </div>' +
                    '<div style="float:left; margin-right:5px; width:100%;">Place Of Service: <strong>{{::dataItem.PlaceOfService2}}</strong> </div>' +
                    '<div style="float:left; margin-right:5px; width:48%;">Network 1: <strong>{{::dataItem.Payor2Network1}}</strong> </div>' +
                    '<div style="float:left; margin-right:5px; width:48%;">Network 2: <strong>{{::dataItem.Payor2Network2}}</strong> </div>' +
                    '<div style="float:left; margin-right:5px; width:48%;">Home Plan: <strong>{{::dataItem.HomePlan2}}</strong> </div>' +
                    '<div style="float:left; margin-right:5px; width:48%;">Plan Type: <strong>{{::dataItem.PlanType2}}</strong> </div>' +
                    '<div style="float:left; margin-right:5px; width:48%;">Group No: <strong>{{::dataItem.PayorGroupNumber2}}</strong> </div>' +
                    '<div style="float:left; margin-right:5px; width:48%;">Policy No: <strong>{{::dataItem.PayorPolicyNumber2}}</strong> </div>' +
                    '<div style="float:left; margin-right:5px; width:100%;">Relationship: <strong>{{::dataItem.InsuredRelationship2}}</strong> </div>' +
                    '<div style="float:left; margin-right:5px; width:100%;">Insured Name: <strong>{{::vm.AddCommaIfNotEmpty(dataItem.Subscriber2LastName)}} {{::dataItem.Subscriber2FirstName}} {{::dataItem.Subscriber2MiddleName}}</strong> </div>' +
                    '<div style="float:left; margin-right:5px; width:48%;">Insured DOB: <strong>{{::vm.DateStringer(dataItem.Subscriber2DateOfBirth)}}</strong> </div>' +
                    '<div style="float:left; margin-right:5px; width:48%;">Insured Gender: <strong>{{::dataItem.Subscriber2Gender}}</strong> </div>' +
                        '<div style="float:left; margin-right:5px; width:100%;">Insured Phone: <strong>{{::dataItem.Subscriber2PhoneNumber}}</strong> </div> ' +
                    '<div style="float:left; margin-right:5px; width:100%;">Insured Address: <strong>{{::dataItem.Subscriber2Address1}} {{::dataItem.Subscriber2Address2}} {{::vm.AddCommaIfNotEmpty(dataItem.Subscriber2City)}} {{::dataItem.Subscriber2StateProvince}} {{::dataItem.Subscriber2PostalCode}} {{::dataItem.Subscriber2Country}}</strong> </div></td></tr></table>' +

                    '</label>'
                },

                {
                    field: "CaseNumber", title: "Req Form",
                    template: '<button ng-click="::vm.GetReqForm(dataItem.AccessionId)">{{::dataItem.CaseNumber}}</button>',
                    width: 100

                },
                { title: "Bill Status", field: "BillableStatus", hidden: true },
                { title: "Computed Bill Class", field: "ComputedBillingClassification", hidden: true },

                { title: "Date Of Service", field: "DateofService", hidden: true },
                { title: "Order Date", field: "OrderDate", hidden: true },
                { title: "Completed Date", field: "CompletedDate", hidden: true },
                { title: "Client Name", field: "ClientName", hidden: true },

                { title: "Program", field: "ProgramGroup", hidden: true },
                { title: "Deficiencies", field: "Deficiencies", hidden: true },
                { title: "ICD10", field: "ComputedICD10Codes", hidden: true },
                { title: "Aggregate", field: "BillingAggregate", hidden: true },
                { title: "Patient Last Name", field: "PatientLastName", hidden: true },
                { title: "Patient First Name", field: "PatientFirstName", hidden: true },
                { title: "Patient Middle Name", field: "PatientMiddleName", hidden: true },
                { title: "Repeat Count", field: "RepeatCount", hidden: true },

                { title: "Bill Type", field: "BillType", hidden: true },
                { title: "Ordering Physician Last Name", field: "DoctorLastName", hidden: true },
                { title: "Ordering Physician First Name", field: "DoctorFirstName", hidden: true },
                { title: "Ordering Physician Middle Name", field: "DoctorMiddleName", hidden: true },
                { title: "QNS", field: "QNS", hidden: true },
                { title: "Bill Class", field: "BillingClassification", hidden: true },

                { title: "Primary Payor", field: "PayorCode1", hidden: true },
                { title: "Place Of Service", field: "PlaceOfService1", hidden: true },
                { title: "Home Plan", field: "HomePlan1", hidden: true },
                { title: "Plan Type", field: "PlanType1", hidden: true },
                { title: "Network1", field: "Payor1Network1", hidden: true },
                { title: "Network2", field: "Payor1Network2", hidden: true },
                { title: "Relationship", field: "InsuredRelationship1", hidden: true },
                { title: "Group No.", field: "PayorGroupNumber1", hidden: true },
                { title: "Policy No.", field: "PayorPolicyNumber1", hidden: true },

                { field: "Subscriber1FirstName", title: "Insured First Name", hidden: true },
                { field: "Subscriber1MiddleName", title: "Insured Middle Name", hidden: true },
                { field: "Subscriber1LastName", title: "Insured Last Name", hidden: true },
                { field: "Subscriber1DateOfBirth", title: "Insured DOB", hidden: true },
                { field: "Subscriber1Gender", title: "Insured Gender", hidden: true },
                { field: "Subscriber1Address1", title: "Insured Address1", hidden: true },
                { field: "Subscriber1Address2", title: "Insured Address2", hidden: true },
                { field: "Subscriber1City", title: "Insured City", hidden: true },
                { field: "Subscriber1StateProvince", title: "Insured State", hidden: true },
                { field: "Subscriber1PostalCode", title: "Insured ZIP", hidden: true },
                { field: "Subscriber1Country", title: "Insured Country", hidden: true },
                { field: "Subscriber1PhoneNumber", title: "Insured Phone", hidden: true },

                { title: "Secondary Payor", field: "PayorCode2", hidden: true },
                { title: "Place Of Service", field: "PlaceOfService2", hidden: true },
                { title: "Home Plan", field: "HomePlan2", hidden: true },
                { title: "Plan Type", field: "PlanType2", hidden: true },
                { title: "Network1", field: "Payor2Network1", hidden: true },
                { title: "Network2", field: "Payor2Network2", hidden: true },
                { title: "Relationship", field: "InsuredRelationship2", hidden: true },
                { title: "Group No.", field: "PayorGroupNumber2", hidden: true },
                { title: "Policy No.", field: "PayorPolicyNumber2", hidden: true },

                { field: "Subscriber2FirstName", title: "Insured First Name", hidden: true },
                { field: "Subscriber2MiddleName", title: "Insured Middle Name", hidden: true },
                { field: "Subscriber2LastName", title: "Insured Last Name", hidden: true },
                { field: "Subscriber2DateOfBirth", title: "Insured DOB", hidden: true },
                { field: "Subscriber2Gender", title: "Insured Gender", hidden: true },
                { field: "Subscriber2Address1", title: "Insured Address1", hidden: true },
                { field: "Subscriber2Address2", title: "Insured Address2", hidden: true },
                { field: "Subscriber2City", title: "Insured City", hidden: true },
                { field: "Subscriber2StateProvince", title: "Insured State", hidden: true },
                { field: "Subscriber2PostalCode", title: "Insured ZIP", hidden: true },
                { field: "Subscriber2Country", title: "Insured Country", hidden: true },
                { field: "Subscriber2PhoneNumber", title: "Insured Phone", hidden: true },

                { field: "LastCaseEditUser", title: "Last Updated By", hidden: true },
                { field: "LastCaseEdit", title: "Last Updated Date", hidden: true },
                //{
                //    field: "CaseNumber",
                //    title: "Edit",
                //    template: '<a class="k-button k-button-icontext k-grid-edit" href="\#"><span class="k-icon k-edit"></span>Edit</a>',
                //    width: 100
                //}

            ],
            editable: "inline"
        }
    }
    
        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Executes the filter operation. </summary>
        ///
        /// <remarks>   Rphilavanh, 9/29/2015. </remarks>
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
            if (this.filterCaseNumber && this.filterCaseNumber.length > 0)
                filter.filters.push({ field: "CaseNumber", operator: "contains", value: this.filterCaseNumber });


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

        public SearchCases(event) {
            if (this.caseListStr != null && this.caseListStr != undefined) {
                this.apiBody = "?caseListStr=" + this.caseListStr + "&delim=,";

                this.dataGridSource.read();
            }
        }

    public GetReqForm(accid: number) {
        this.utilService.DownloadRequisitionForm(accid.toString());
    }

        public GetCaseNotes(caseno: string) {

            this.caseNotesHandler = new CaseNotes(this.httpServ, this.qServ);
            var current: CaseEditorController = this;

            this.caseNotesHandler.GetCaseNotes(caseno, "Case")
                .then(function (data) {
                    current.caseNotesData = current.caseNotesHandler.CaseNotesData;
                    current.caseNotesGridOptions = current.caseNotesHandler.caseNotesGridOptions;
                    current.windowCaseNotes.title('Case update history for ' + caseno);

                    current.windowCaseNotes.open();
                    current.windowCaseNotes.options.modal = true;
                    current.windowCaseNotes.center();
                })

        }

        //public CheckInsuredRelationship1IsSelf() {
        //    this.insuredRelationship1SelectionIsSelf = false;
        //    if (this.insuredRelationship1Selection == "SELF") {
        //        this.insuredRelationship1SelectionIsSelf = true;
        //    }
        //}
        //public CheckInsuredRelationship2IsSelf() {
        //    this.insuredRelationship2SelectionIsSelf = false;
        //    if (this.insuredRelationship2Selection == "SELF") {
        //        this.insuredRelationship2SelectionIsSelf = true;
        //  }
        //}

    }
}