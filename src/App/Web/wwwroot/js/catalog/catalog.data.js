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

        var getProjectsByUser = function (userId, page, pageSize) {
            var deferred = $q.defer();
            var skip = (page - 1) * pageSize;
            var url = urlProvider.getUserProjectsUrl(userId, skip, pageSize);
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

        var getTechnologies = function () {
            var deferred = $q.defer();
            var url = urlProvider.getTechnologyBaseUrl();
            proxy.get(url)
                .then(function (response) {
                    deferred.resolve(response.data);
                }, function (error) {
                    deferred.reject(error);
                });
            return deferred.promise;
        }

        var getTechnologySuggestions = function (query) {
            var deferred = $q.defer();
            var url = urlProvider.getTechnologyBaseUrl();
            proxy.get(url)
                .then(function (response) {
                    var technologies = _.filter(response.data, function (technology) {
                        return technology.name.toLowerCase().startsWith(query.toLowerCase());
                    });
                    var tags = _.map(technologies, function (technology) {
                        return {
                            text: technology.name,
                            id: technology.id
                        }
                    });
                    deferred.resolve(tags);
                });
            return deferred.promise;
        }

        var addNewProject = function (project) {
            var deferred = $q.defer();
            //Check technologies
            getTechnologies()
                .then(function (technologies) {
                    _.each(project.technologies, function (tech) {
                        var technology = _.findWhere(technologies, { name: tech.name.replace(/-/g, ' ') });
                        tech.id = technology.id;
                        tech.name = technology.name;
                    });
                    //Add the project
                    var url = urlProvider.getProjectBaseUrl();
                    var str = JSON.stringify(project);
                    project.createdOn = new Date();
                    project.lastModifiedOn = new Date();
                    proxy.post(url, project)
                        .then(function (response) {
                            deferred.resolve(response.data);
                        }, function (error) {
                            deferred.reject(error);
                        });
                });
            return deferred.promise;
        }

        return {
            getProjects: getProjects,
            getProjectDetails: getProjectDetails,
            getProjectsByUser: getProjectsByUser,
            addNewProject: addNewProject,
            getTechnologySuggestions: getTechnologySuggestions
        }
    }

    module.factory("catalogData", ["$q", "proxy", "urlProvider", catalogData]);

}(angular.module("catalog")))