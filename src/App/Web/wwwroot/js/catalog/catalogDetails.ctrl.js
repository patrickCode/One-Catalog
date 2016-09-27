(function (module) {

    var catalogDetailsController = function ($scope, $rootScope, $stateParams, catalogData) {

        var getDetails = function () {
            $scope.loading = true;
            $scope.errorOcurred = false;
            catalogData.getProjectDetails($scope.id)
                .then(function (data) {
                    $scope.project = data;
                    $scope.loading = false;
                });
            //.error(function (error) {
            //    $scope.errorMessage = "Some error ocurred";
            //    $scope.errorOcurred = true;
            //    $scope.loading = false;
            //});
        }

        var init = function () {
            $scope.id = $stateParams.id;
            getDetails();
        }
        init();
    }

    module.controller("catalogDetailsController", ["$scope", "$rootScope", "$stateParams", "catalogData", catalogDetailsController]);

}(angular.module("catalog")))