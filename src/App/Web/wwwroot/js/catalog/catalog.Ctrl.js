(function (module) {

    var catalogController = function ($scope, $rootScope, $location, catalogData) {

        var search = function () {
            $scope.errorOcurred = false;
            $scope.loading = true;
            catalogData.getProjects($scope.searchText, $scope.selectedFacets.technologies, $scope.pagination.page, $scope.pagination.pageSize)
                .then(function (data) {
                    $scope.projects = data.results;
                    $scope.count = data.totalCount;
                    $scope.loading = false;
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
                title: "Catalog",
                href: "/common/catalog"
            });
            
            $scope.pagination = {
                page: 1,
                pageSize: 10
            };

            $scope.facets = {
                technologies: ["ASP.NET Core 1.0"]
            }

            $scope.selectedFacets = {
                technologies: []
            }

            $scope.search = search;
            $scope.goToDetails = goToDetails;
            search();
        }
        init();
    }

    module.controller("catalogController", ["$scope", "$rootScope", "$location", "catalogData", catalogController]);

}(angular.module("catalog")))