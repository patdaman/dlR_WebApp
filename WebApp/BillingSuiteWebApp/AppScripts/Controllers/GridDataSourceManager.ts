module BillingSuiteApp.Controller {

    ///-------------------------------------------------------------------------------------------------
    /// <summary>   Creates data source for reading only. </summary>
    ///
    /// <remarks>   Ssur, 20150922. </remarks>
    ///
    /// <param name="fullurl">  The fullurl. </param>
    /// <param name="pagesize"> The pagesize. </param>
    /// <param name="kmodel">   The kmodel. </param>
    /// <param name="paramsFn"> The parameters function. </param>
    ///
    /// <returns>   The new data source. </returns>
    ///-------------------------------------------------------------------------------------------------

    export function CreateDataSource(httpserv: ng.IHttpService, relapipath: string, pagesize: number,
        kmodel: typeof kendo.data.Model,
        paramsFn: Function): kendo.data.DataSource {
        var ds: kendo.data.DataSource = new kendo.data.DataSource(
            {
                transport: {
                    read: function (e) {
                        var pars = paramsFn();
                        httpserv.get(pars)
                            .success(function (data, status, headers, config) {
                                e.success(data);
                            })
                            .error(function (data, status, headers, config) {
                                alert("Error retrieving data");
                                console.log(status);
                            })
                    }

                },

                change: function (e) {
                    var data = this.data();
                    var pageSizes = [
                        { text: "5", value: 5 },
                        { text: "10", value: 10 },
                        { text: "25", value: 25 },
                        { text: "50", value: 50 },
                        { text: "100", value: 100 }
                    ];
                    if (data.length > 0)
                        pageSizes.push({ text: "All", value: data.length });

                    $('.k-pager-sizes select[data-role="dropdownlist"]').data('kendoDropDownList').setDataSource(new kendo.data.DataSource({ data: pageSizes }));

                },
                pageSize: pagesize,
                schema: {

                    model: kmodel,

                }
            }
        )
        return ds;
    } 

   
    ///-------------------------------------------------------------------------------------------------
    /// <summary>   Creates data source for CRUD. </summary>
    ///
    /// <remarks>   Ssur, 20150922. </remarks>
    ///
    /// <param name="fullurl">  The fullurl. </param>
    /// <param name="pagesize"> The pagesize. </param>
    /// <param name="kmodel">   The kmodel. </param>
    /// <param name="paramsFn"> The parameters function. </param>
    ///
    /// <returns>   The new data source. </returns>
    ///-------------------------------------------------------------------------------------------------

    export function CreateGridDataSource(httpserv: ng.IHttpService,
        pagesize: number,
        kmodel: typeof kendo.data.Model,
        transportOptions,
        onChange?: (e: kendo.data.DataSourceChangeEvent) => void): kendo.data.DataSource {

        var dst: kendo.data.DataSourceTransport = {};

        if (transportOptions.read) {
            dst.read = function (e) {
                var pars = transportOptions.read();
                httpserv.get(pars, { withCredentials: true })
                    .success(function (data, status, headers, config) {
                        console.log("GridDataManager: Got Data");
                        e.success(data);
                    })
                    .error(function (data, status, headers, config) {
                        errorHandler("read", data, status, headers, config);
                    })
            }
        }


        if (transportOptions.update) {
            dst.update = function (e) {
                var pars = transportOptions.update();
                httpserv.put(pars, e.data, { withCredentials: true })
                    .success(function (data, status, headers, config) {
                        e.success(data);
                    })
                    .error(function (data, status, headers, config) {
                        errorHandler("update", data, status, headers, config);
                    })
            }
        }

        if (transportOptions.create) {
            dst.create = function (e) {
                var pars = transportOptions.create();
                httpserv.post(pars, e.data, { withCredentials: true })
                    .success(function (data, status, headers, config) {
                        e.success(data);
                    })
                    .error(function (data, status, headers, config) {
                        errorHandler("create", data, status, headers, config);
                    })
            }
        }

        if (transportOptions.destroy) {
            dst.destroy = function (e) {
                var pars = transportOptions.destroy();
                httpserv.delete(pars, { withCredentials: true })
                    .success(function (data, status, headers, config) {
                        e.success(data);
                    })
                    .error(function (data, status, headers, config) {
                        errorHandler("destroy", data, status, headers, config);
                    })
            }
        }




        var ds: kendo.data.DataSource = new kendo.data.DataSource(
            {
                transport: dst,
                change: function (e) {
                    if (onChange) {
                        onChange(e);
                    }
                    var data = this.data();
                    var pageSizes = [
                        { text: "5", value: 5 },
                        { text: "10", value: 10 },
                        { text: "25", value: 25 },
                        { text: "50", value: 50 },
                        { text: "100", value: 100 }
                    ];
                    if (data.length > 0)
                        pageSizes.push({ text: "All", value: data.length });
                    if ($('.k-pager-sizes select[data-role="dropdownlist"]').data('kendoDropDownList'))
                        $('.k-pager-sizes select[data-role="dropdownlist"]').data('kendoDropDownList').setDataSource(new kendo.data.DataSource({ data: pageSizes }));

                },
                pageSize: pagesize,
                schema: {
                    model: kmodel,

                }
            }
        // add transports
        // var transp = new 

        )
        return ds;
    }

    function errorHandler(verbName:string, data: any, status: any, headers: any, config: any): void {
        console.log("DataGrid " + verbName + " error with status: " + status);
        console.log("Exception details: " + data.Message);
        console.log("Stacktrace: " + data.StackTraceString);
        throw { message: "Error doing " + verbName + ". Got Status " + status + ".\n" + data.Message, cause: status };
    }

}