(function (module) {

    var catalogData = function ($q, proxy, urlProvider) {

        var getProjects = function (searchText, technologies, page, pageSize) {
            var deferred = $q.defer();
            if (searchText === undefined || searchText === null || searchText === "")
                searchText = "*";
            var skip = (page - 1) * pageSize;
            var url = urlProvider.getProjectSearchUrl(searchText, technologies, skip, pageSize);
            proxy.get(url)
                .then(function (response) {
                    deferred.resolve(response.data);
                }, function (error) {
                    deferred.reject(error);
                });
            return deferred.promise;
        }

        var getProjectDetails = function (projectId) {
            var deferred = $q.defer();
            var url = urlProvider.getProjectDetailsUrl(projectId);
            proxy.get(url)
                .then(function (response) {
                    deferred.resolve(response.data);
                }, function (error) {
                    deferred.reject(error);
                });
            return deferred.promise;
        }

        return {
            getProjects: getProjects,
            getProjectDetails: getProjectDetails
        }
    }

    module.factory("catalogData", ["$q", "proxy", "urlProvider", catalogData]);

}(angular.module("catalog")))