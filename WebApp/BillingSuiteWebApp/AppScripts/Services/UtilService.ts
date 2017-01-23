///-------------------------------------------------------------------------------------------------
// <copyright file="UtilService.ts" company="Signal Genetics Inc.">
// Copyright (c) 2015 Signal Genetics Inc.. All rights reserved.
// </copyright>
// <author>Ssur</author>
// <date>20151001</date>
// <summary>utility service class</summary>
///-------------------------------------------------------------------------------------------------

module BillingSuiteApp.Service {
    export class UtilService {
        private httpService: ng.IHttpService;
        private qService: ng.IQService;
        private locService: ng.ILocationService;
        private winService: ng.IWindowService;

        constructor(
            $http: ng.IHttpService,
            $q: ng.IQService,
            $location: ng.ILocationService,
            $window: ng.IWindowService) {
            this.httpService = $http;
            this.qService = $q;
            this.locService = $location;
            this.winService = $window;

        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Downloads selected billing event report. </summary>
        ///
        /// <remarks>   Ssur, 20150924. </remarks>
        ///
        /// <param name="billid">   The billid. </param>
        /// <param name="dltype">   The dltype. </param>
        ///
        /// <returns>   . </returns>
        ///-------------------------------------------------------------------------------------------------
        
        public BillingReportFileDownloadService(billid: string, dltype: string) {
            var url: string = this.GetLocalServerHostString()  + "/BillReporter/GetReportDownload?reporttype=" + dltype + "&billingid=" + billid;
            this.winService.location.href = url;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Billing range report file download service. </summary>
        ///
        /// <remarks>   Ssur, 20151028. </remarks>
        ///
        /// <param name="fromdate:string">  The fromdate string. </param>
        /// <param name="todate:string">    The todate string. </param>
        ///
        /// <returns>   . </returns>
        ///-------------------------------------------------------------------------------------------------

        public BillingRangeReportFileDownloadService(fromdate:string, todate:string) {
            var url: string = this.GetLocalServerHostString()  + "/BillReporter/GetRangeSummaryReportDownload?from=" + fromdate + "&to=" + todate;
            this.winService.location.href = url;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Downloads the requisition form described by accid. </summary>
        ///
        /// <remarks>   Ssur, 20151001. </remarks>
        ///
        /// <param name="accid">    The accid. </param>
        ///
        /// <returns>   . </returns>
        ///-------------------------------------------------------------------------------------------------

        public DownloadRequisitionForm(accid: string) {

            var url: string = this.GetLocalServerHostString() + "/CaseEditor/GetRequisitionForm?accid=" + accid;
            this.winService.location.href = url;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets local server host string with port number. </summary>
        ///
        /// <remarks>   Ssur, 20160120. </remarks>
        ///
        /// <returns>   The local server host string. </returns>
        ///-------------------------------------------------------------------------------------------------
        public GetLocalServerHostString() {
            var lhost: string = this.locService.host();
            var lport: number = this.locService.port();
            var prot: string = this.locService.protocol();
            return prot + "://" + lhost + ":" + lport;
        }



    }
}