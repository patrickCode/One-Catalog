(function (module) {

    var technologyData = function ($q, proxy, urlProvider) {

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
            var url = urlProvider.getTechnologySuggestionUrl(query);
            proxy.get(url)
                .then(function (response) {
                    var technologies = response.data;
                    var tags = _.map(technologies, function (technology) {
                        return {
                            text: technology.name,
                            description: technology.description,
                            id: technology.id
                        }
                    });
                    deferred.resolve(tags);
                });
            return deferred.promise;
        }

        return {
            getTechnologies: getTechnologies,
            getTechnologySuggestions: getTechnologySuggestions
        }
    }

    module.factory("technologyData", ["$q", "proxy", "urlProvider", technologyData]);

}(angular.module("technology")))