(function (module) {

    var proxy = function ($http) {

        var call = function (url, method, data) {
            if (method === undefined || method === null || method === "GET") {
                return $http.get(url);
            }
            if (method === "DELETE") {
                return $http.delete(url);
            } else {
                return $http({
                    method: method,
                    url: url,
                    data: data
                });
            }
        }

        var get = function (url) {
            return call(url, "GET");
        }

        var post = function (url, data) {
            return call(url, "POST", data);
        }

        var put = function (url, data) {
            return call(url, "PUT", data);
        }

        var patch = function (url, data) {
            return call(url, "PATCH", data);
        }

        var del = function (url) {
            return call(url, "DELETE");
        }

        return {
            get: get,
            post: post,
            put: put,
            patch: patch,
            del: del
        };
    }

    module.factory("proxy", proxy);

}(angular.module("common")))