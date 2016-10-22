(function (module) {

    var userService = function ($q, proxy) {
        var getUserDetail = function () {
            var deferred = $q.defer();
            var url = "https://graph.windows.net/microsoft.com/users?$filter=displayName+eq+'Pratik+Bhattacharya'&api-version=1.6";
            //var url = "https://graph.windows.net/microsoft.com/me&api-version=1.6";
            proxy.get(url)
                .then(function (data) {
                    var user = data.data;
                    deferred.resolve(user);
                }, function (error) {
                    deferred.reject(error);
                });
            return deferred.promise;
        }

        return {
            getUserDetail: getUserDetail
        }
    }

    module.factory("userService", userService);

}(angular.module("user")))