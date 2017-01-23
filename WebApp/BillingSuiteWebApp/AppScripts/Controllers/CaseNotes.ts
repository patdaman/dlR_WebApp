module BillingSuiteApp.Controller {

    export class CaseNotes {

        private httpServ: ng.IHttpService;
        private qServ: ng.IQService;

        caseNotesGridOptions: kendo.ui.GridOptions = undefined;
        caseNotesDataSource: kendo.data.DataSource;
        caseNotesToolbarTemplate: any;

        public CaseNotesData: Model.NotesModel;

        constructor(http: ng.IHttpService, qserv: ng.IQService) {

            this.httpServ = http;
            this.qServ = qserv;
            this.caseNotesToolbarTemplate = $("#caseNotesToolbarTemplate").html();

        }

        public GetCaseNotes(caseno: string, noteType: string): ng.IPromise<kendo.data.DataSource> {

            var dfo = this.qServ.defer<kendo.data.DataSource>();

            var current: CaseNotes = this;


            this.httpServ.get("api:/Notes?caseno=" + caseno  
                + "&noteType=" + noteType //Case"
                , { withCredentials: true })
                .success(function (data, status, headers, config) {

                    current.CaseNotesData = new Model.NotesModel();
                    current.CaseNotesData.NoteId = data["NoteId"];
                    current.CaseNotesData.Note = data["Note1"];
                    current.CaseNotesData.NoteType = data["NoteType"];
                    current.CaseNotesData.CreateDate = data["CreateDate"];
                    current.CaseNotesData.UserName = data["UserName"];
                    current.CaseNotesData.Id = data["id"];

                    current.caseNotesDataSource = new kendo.data.DataSource(
                        {
                            data: data,

                        });
                    current.initCaseNotesGrid();
                    dfo.resolve(current.caseNotesDataSource);
                })
                .error(function (data, status, headers, config) {
                    dfo.reject(data);
                })

            return dfo.promise;
        }

        private initCaseNotesGrid() {
            var current: CaseNotes = this;
            this.caseNotesGridOptions = {
                dataSource: this.caseNotesDataSource,
                scrollable: true,
                selectable: "multiple, row",
                sortable: true,
                toolbar: this.caseNotesToolbarTemplate,
                excel: {
                    fileName: moment().year().toString() + moment().month().toString() + moment().day().toString() + "_" + moment().hour().toString() + moment().minute().toString() + moment().second().toString() + "_SignalBillingCaseNotes.xlsx",
                    allPages: true
                },
                columns: [
                    { "field": "Note1", "title": "Note" },
                    { "field": "CreateDate", "title": "Date" },
                    { "field": "UserName", "title": "User" }
                ],
                editable: false
            }
        }


    }
}