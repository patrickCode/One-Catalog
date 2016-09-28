(function (module) {

    var userCatalogController = function ($scope, $rootScope, $location, catalogData, adalAuthenticationService) {

        var getUserProjects = function () {
            $scope.loading = true;
            $scope.errorOcurred = false;
            catalogData.getProjectsByUser($scope.userId, $scope.pagination.page, $scope.pagination.pageSize)
                .then(function (data) {
                    $scope.projects = data.results;
                    $scope.count = data.totalCount;
                    $scope.loading = false;
                }, function (error) {
                    alert("Some unexpected error ocurred");
                });
            //.error(function (error) {
            //    $scope.errorMessage = "Some error ocurred";
            //    $scope.errorOcurred = true;
            //    $scope.loading = false;
            //});
        }

        var goToDetails = function (project) {
            $location.path("home/" + project.id + "/details");
        }

        var init = function () {
            $rootScope.breadcrumbs = $rootScope.breadcrumbs || [{}];
            $rootScope.breadcrumbs.pop();
            $rootScope.breadcrumbs.push({
                title: "User Catalog",
                href: "/catalog"
            });

            $scope.userId = adalAuthenticationService.userInfo.userName;

            $scope.pagination = {
                page: 1,
                pageSize: 10
            };
            getUserProjects();
            $scope.goToDetails = goToDetails;
        }
        init();
    }

    module.controller("userCatalogController", ["$scope", "$rootScope", "$location", "catalogData", "adalAuthenticationService", userCatalogController])

}(angular.module("catalog")));