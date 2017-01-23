///-------------------------------------------------------------------------------------------------
// <copyright file="PayorGroupService.ts" company="Signal Genetics Inc.">
// Copyright (c) 2015 Signal Genetics Inc.. All rights reserved.
// </copyright>
// <author>Ssur</author>
// <date>20150922</date>
// <summary>payor group service class</summary>
///-------------------------------------------------------------------------------------------------

module BillingSuiteApp.Service {
    export class PayorGroupService {
        private httpService: ng.IHttpService;
        private qService: ng.IQService;
        private payorGroupList: Model.PayorGroupModel[];
        public PayorGroupID2NameMap: any;
        public PayorGroupID2CodeMap: any;

        constructor($http: ng.IHttpService, $q: ng.IQService) {
            this.httpService = $http;
            this.payorGroupList = null;
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

        private stuffPayorGroupList(dresp: any): Model.PayorGroupModel[] {

            this.payorGroupList = new Array<Model.PayorGroupModel>();
            this.PayorGroupID2NameMap = [];
            var d = dresp;
            for (var i = 0; i < d.length; i++) {
                var pm = new Model.PayorGroupModel();
                pm.PayorGroupId = d[i].PayorGroupId;
                pm.PayorGroupName = d[i].PayorGroupName;

                this.PayorGroupID2NameMap.push({
                    "value": pm.PayorGroupId, "text": pm.PayorGroupName
                });

                this.payorGroupList.push(pm);
            }

            this.payorGroupList.sort();
            this.PayorGroupID2NameMap.sort(function (a, b) {
                var textA = a.text.toLowerCase(), textB = b.text.toLowerCase()
                if (textA < textB) //sort string ascending
                    return -1
                if (textA > textB)
                    return 1
                return 0 //default return value (no sorting)
            });
            return this.payorGroupList;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets payors asynchronous. </summary>
        ///
        /// <remarks>   Ssur, 20150921. </remarks>
        ///
        /// <returns>   A promise for the payors list </returns>
        ///-------------------------------------------------------------------------------------------------

        public GetPayorGroupAsync(): ng.IPromise<Model.PayorGroupModel[]> {
            var dfo = this.qService.defer<Model.PayorGroupModel[]>();
            var current: any = this;

            if (this.payorGroupList)
                dfo.resolve(this.payorGroupList);
            else {
                var server: string = "http://devapi/api/";
                this.httpService.get('api:/PayorGroups/',
                    { withCredentials: true })
                    .success(function (response) {
                        current.stuffPayorGroupList(response);
                        dfo.resolve(current.payorGroupList)
                    })
                    .error(function (error) {
                    dfo.reject('Failed to get PayorGroupList');
                    });
            }
            return dfo.promise;
        }



    }
}