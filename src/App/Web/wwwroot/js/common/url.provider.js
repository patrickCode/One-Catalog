(function (module) {
    
    var urlProvider = function () {
        var baseUrl = "http://msonecatalogdev.azurewebsites.net/api/";
        var projectBaseUrl = baseUrl + "projects";

        var getProjectDetailsUrl = function (id) {
            return projectBaseUrl + "/" + id;
        }

        var getProjectSearchUrl = function (searchText, technologies, skip, top) {
            var url = projectBaseUrl + "?q=" + searchText;
            url = url + "&skip" + skip + "&top=" + top;
            if (technologies !== undefined && technologies !== null && technologies.length > 0) {
                url = url + "&";
                technologies.forEach(function (technology) {
                    url = url + "technologies=" + technology + "&";
                });
            }
            return url;
        }

        return {
            getProjectDetailsUrl: getProjectDetailsUrl,
            getProjectSearchUrl: getProjectSearchUrl
        }
    }

    module.factory("urlProvider", urlProvider);

}(angular.module("common")))