(function (module) {

    var shellController = function ($scope, $rootScope, $stateParams, adalAuthenticationService) {
        
        var goTo = function (page) {

        }

        var search = function () {
            alert($scope.catalog.searchtext);
        }

        var init = function () {
            $rootScope.breadcrumbs = [{
                title: "",
                href: ""
            }];
            $scope.userInfo = adalAuthenticationService.userInfo;

            $scope.goTo = goTo;
            $scope.search = search;
            $scope.catalog = {
                searchtext: ""
            }
        }
        init();
    }

    module.controller("shellController", ['$scope', '$rootScope', '$stateParams', 'adalAuthenticationService', shellController])

}(angular.module("shell")))