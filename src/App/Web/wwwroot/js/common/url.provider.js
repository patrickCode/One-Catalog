(function (module) {

    var urlProvider = function () {
        //var baseUrl = "http://msonecatalogdev.azurewebsites.net/api/";
        var baseUrl = "http://localhost:8843/api/";
        var projectBaseUrl = baseUrl + "projects";

        var technologyBaseUrl = baseUrl + "technologies";
        var technologySuggestionUrl = technologyBaseUrl + "/suggest?name=";

        var getProjectBaseUrl = function () {
            return projectBaseUrl;
        }

        var getProjectDetailsUrl = function (id) {
            return projectBaseUrl + "/" + id;
        }

        var getProjectSearchUrl = function (searchText, technologies, skip, top) {
            var url = projectBaseUrl + "?q=" + searchText;
            url = url + "&skip=" + skip + "&top=" + top;
            if (technologies !== undefined && technologies !== null && technologies.length > 0) {
                url = url + "&";
                technologies.forEach(function (technology) {
                    url = url + "technologies=" + technology + "&";
                });
            }
            return url;
        }

        var getUserProjectsUrl = function (userId, searchText, skip, top) {
            var url = baseUrl + "users/" + userId + "/projects?q=" + searchText;
            url = url + "&skip=" + skip + "&top=" + top;
            return url;
        }

        var getTechnologyBaseUrl = function () {
            return technologyBaseUrl;
        }

        var getTechnologySuggestionUrl = function (technology) {
            return technologySuggestionUrl + technology;
        }

        return {
            //Project
            getProjectBaseUrl: getProjectBaseUrl,
            getProjectDetailsUrl: getProjectDetailsUrl,
            getProjectSearchUrl: getProjectSearchUrl,
            getUserProjectsUrl: getUserProjectsUrl,

            //Technology
            getTechnologyBaseUrl: getTechnologyBaseUrl,
            getTechnologySuggestionUrl: getTechnologySuggestionUrl
        }
    }

    module.factory("urlProvider", urlProvider);

}(angular.module("common")))