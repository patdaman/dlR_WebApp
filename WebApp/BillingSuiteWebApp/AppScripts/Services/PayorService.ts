///-------------------------------------------------------------------------------------------------
// <copyright file="PayorService.ts" company="Signal Genetics Inc.">
// Copyright (c) 2015 Signal Genetics Inc.. All rights reserved.
// </copyright>
// <author>Ssur</author>
// <date>20150922</date>
// <summary>payor service class</summary>
///-------------------------------------------------------------------------------------------------

module BillingSuiteApp.Service {
    export class PayorService {
        private httpService: ng.IHttpService;
        private qService: ng.IQService;
        private payorsList: Model.PayorModel[];
        public PayorsID2NameMap: any;
        public PayorsID2CodeMap: any;

        constructor($http: ng.IHttpService, $q: ng.IQService) {
            this.httpService = $http;
            this.payorsList = null;
            this.qService = $q;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Stuff payor list. </summary>
        ///
        /// <remarks>   Ssur, 20150921. </remarks>
        ///
        /// <param name="dresp">    The response from the get. </param>
        ///
        /// <returns>   A Model.PayorModel[]. </returns>
        ///-------------------------------------------------------------------------------------------------

        private stuffPayorList(dresp: any): Model.PayorModel[] {

            this.payorsList = new Array<Model.PayorModel>();
            this.PayorsID2NameMap = [];
            this.PayorsID2CodeMap = [];
            var d = dresp;
            for (var i = 0; i < d.length; i++) {
                var pm = new Model.PayorModel();
                pm.PayorId = d[i].PayorId;
                pm.PayorName = d[i].Name;
                pm.PayorCode = d[i].PayorCode;

                this.PayorsID2NameMap.push({
                    "value": pm.PayorId, "text": pm.PayorName
                });

                this.PayorsID2CodeMap.push({
                    "value": pm.PayorId, "text": pm.PayorCode
                });

                this.payorsList.push(pm);
            }

            this.payorsList.sort();
            this.PayorsID2CodeMap.sort(function (a, b) {
                var textA = a.text.toLowerCase(), textB = b.text.toLowerCase()
                if (textA < textB) //sort string ascending
                    return -1
                if (textA > textB)
                    return 1
                return 0 //default return value (no sorting)
            });
            return this.payorsList;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets payors asynchronous. </summary>
        ///
        /// <remarks>   Ssur, 20150921. </remarks>
        ///
        /// <returns>   A promise for the payors list </returns>
        ///-------------------------------------------------------------------------------------------------

        public GetPayorsAsync(): ng.IPromise<Model.PayorModel[]> {
            var dfo = this.qService.defer<Model.PayorModel[]>();
            var current: any = this;
            
            if (this.payorsList)
                dfo.resolve(this.payorsList);
            else {
                var server: string = "http://devapi/api/";
                this.httpService.get('api:/Payors/',
                    { withCredentials: true })
                    .success(function (response) {
                        current.stuffPayorList(response);
                        dfo.resolve(current.payorsList)
                    })
                    .error(function (error) {
                        dfo.reject('Failed to get PayorsList');
                    });
            }
            return dfo.promise;
        }

      

    }
}